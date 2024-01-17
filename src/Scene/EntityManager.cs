using Arch.Core;
using Arch.Core.Utils;
using ImGuiNET;

namespace Mundos {
    public static class EntityManager {
        private static Dictionary<int, Entity> _entities = new Dictionary<int, Entity>(); // Contains all IDs and entities in the scene
        private static Dictionary<Entity, string> _entityNames = new Dictionary<Entity, string>(); // Contains all entities and their names
        private static Dictionary<Entity, Entity> _entityParents = new Dictionary<Entity, Entity>(); // Contains all entities and their parents
        private static Dictionary<Entity, List<Entity>> _entityChildren = new Dictionary<Entity, List<Entity>>(); // Contains all entities and their children
        private static Entity _root = WorldManager.World.Create(); // Root node of the scene
        private static Camera? _primaryCamera; // The primary camera of the scene

        static EntityManager() {
            // EntityManager will create a root node for the scene
            _entities.Add(_root.Id, _root); // Add the root node to the entities list
            _entityNames.Add(_root, "Root"); // Add the root node to the names list
            _entityParents.Add(_root, _entities[0]); // Add the root node to the parents list, the root node is its own parent
            _entityChildren.Add(_root, new List<Entity>()); // Add the root node to the children list
        }

        /// <summary>
        /// Creates a new entity with the specified archetype and adds it to the scene graph.
        /// If no parent is specified, the entity will be added to the root node.
        /// </summary>
        /// <param name="archetype">The archetype type of the entity.</param>
        /// <param name="name">The name of the entity.</param>
        /// <returns>The created entity.</returns>
        public static Entity Create(ArchetypeType archetype, string name, Entity? parent = null) {
            Entity entity = WorldManager.World.Create(componentArchetypes[archetype]); // Create a new entity with the specified archetype
            Entity parentEntity = parent ?? _root; // If no parent is specified, use the root node
            _entities.Add(entity.Id, entity); // Add this entity to the entities list
            _entityNames.Add(entity, name); // Add this entity to the names list
            _entityParents.Add(entity, parentEntity); // Add this entity to the parents list
            if (!_entityChildren.ContainsKey(parentEntity)) _entityChildren.Add(parentEntity, new List<Entity>()); // If the parent entity doesn't have any children, add an empty list
            _entityChildren[parentEntity].Add(entity); // Add this entity to the children list of its parent
            return entity;
        }

        /// <summary>
        /// Creates a new entity with the specified archetype, but does not add it to the scene graph.
        /// </summary>
        /// <param name="archetype">The archetype type of the entity.</param>
        /// <param name="name">The name of the entity.</param>
        /// <returns>The created entity.</returns>
        public static Entity CreateNoRelation(ArchetypeType archetype, string name) {
            Entity entity = WorldManager.World.Create(componentArchetypes[archetype]); // Create a new entity with the specified archetype
            _entities.Add(entity.Id, entity); // Add this entity to the entities list
            _entityNames.Add(entity, name); // Add this entity to the names list
            return entity;
        }

        /// <summary>
        /// Retrieves the entity with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity with the specified ID.</returns>
        public static Entity GetEntity(int id) {
            return _entities[id];
        }

        /// <summary>
        /// Retrieves an entity by its name.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <returns>The entity with the specified name.</returns>
        public static Entity GetEntity(string name) {
            return _entityNames.FirstOrDefault(x => x.Value == name).Key;
        }

        /// <summary>
        /// Destroys an entity with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the entity to destroy.</param>
        public static void DestroyEntity(int id) {
            DestroyEntity(_entities[id]);
        }

        /// <summary>
        /// Destroys an entity and all its children recursively.
        /// </summary>
        /// <param name="entity">The entity to destroy.</param>
        public static void DestroyEntity(Entity entity) {
            _entityChildren[entity].ForEach(child => DestroyEntity(child)); // Destroy all children of this entity and their children
            _entityChildren.Remove(entity); // Remove this entity from the children list of its parent
            _entityNames.Remove(entity); // Remove this entity from the names list
            _entityParents.Remove(entity); // Remove this entity from the parents list
            _entities.Remove(entity.Id); // Remove this entity from the entities list
            WorldManager.World.Destroy(entity); // Destroy this entity
        }

        /// <summary>
        /// Sets the active camera for the entity manager to use.
        /// </summary>
        /// <param name="camera">The camera to set as active.</param>
        public static void SetActiveCamera(Camera camera) => _primaryCamera = camera;

        /// <summary>
        /// Retrieves the active camera of the scene.
        /// </summary>
        /// <param name="camera">The active camera.</param>
        internal static void GetActiveCamera(out Camera? camera) => camera = _primaryCamera;

        /// <summary>
        /// Gets the active camera of the scene.
        /// </summary>
        /// <returns>The active camera, or null if there is no active camera.</returns>
        internal static Camera? GetActiveCamera() => _primaryCamera;

        /// <summary>
        /// Dictionary that contains all IDs and entities in the scene.
        /// </summary>
        public static Dictionary<int, Entity> Entities { get => _entities; }

        /// <summary>
        /// Dictionary that contains all entities and their names.
        /// </summary>
        public static Dictionary<Entity, string> EntityNames { get => _entityNames; }

        /// <summary>
        /// Dictionary that contains all entities and their parents.
        /// </summary>
        public static Dictionary<Entity, Entity> EntityParents { get => _entityParents; }

        /// <summary>
        /// Dictionary that contains all entities and their children.
        /// </summary>
        public static Dictionary<Entity, List<Entity>> EntityChildren { get => _entityChildren; }

        /// <summary>
        /// The root node of the scene.
        /// </summary>
        public static Entity Root { get => _root; }

        /// <summary>
        /// Dictionary that maps ArchetypeType to an array of ComponentType.
        /// Engine supports custom archetypes, but these are the default ones.
        /// </summary>
        public static Dictionary<ArchetypeType, ComponentType[]> componentArchetypes = new Dictionary<ArchetypeType, ComponentType[]>()
        {
            { ArchetypeType.EmptyNode,  new ComponentType[]{ typeof(Position), typeof(Rotation), typeof(Scale)                                  } },
            { ArchetypeType.Model,      new ComponentType[]{ typeof(Position), typeof(Rotation), typeof(Scale), typeof(Mesh)                    } },
            { ArchetypeType.Camera,     new ComponentType[]{ typeof(Position), typeof(Rotation), typeof(Scale), typeof(Camera), typeof(Script)  } },
            { ArchetypeType.Script,     new ComponentType[]{ typeof(Script)                                                                     } }
        };

        /// <summary>
        /// Represents the types of archetypes that can be used in the entity manager.
        /// </summary>
        public enum ArchetypeType
        {
            EmptyNode,
            Model,
            Camera,
            Script
        }
    }
}