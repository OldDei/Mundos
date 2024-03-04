using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using OpenTK.Mathematics;
using ImGuiNET;

namespace Mundos {
    /// <summary>
    /// The EntityManager class is responsible for managing entities in the scene graph.
    /// It provides methods for creating, retrieving, and destroying entities, as well as setting and getting the active camera.
    /// </summary>
    public static class EntityManager {
        private static Dictionary<UUID, Entity> _entities = new Dictionary<UUID, Entity>(); // Contains all IDs and entities in the scene
        private static Dictionary<Entity, string> _entityNames = new Dictionary<Entity, string>(); // Contains all entities and their names
        private static Dictionary<Entity, Entity> _entityParents = new Dictionary<Entity, Entity>(); // Contains all entities and their parents
        private static Dictionary<Entity, List<Entity>> _entityChildren = new Dictionary<Entity, List<Entity>>(); // Contains all entities and their children
        private static Entity _root = WorldManager.World.Create(); // Root node of the scene
        private static Camera? _primaryCamera; // The primary camera of the scene

        static EntityManager() {
            // EntityManager will create a root node for the scene
            _root.Add(new UUID(_root, Guid.NewGuid())); // Set a new UUID for the root node
            _entities.Add(_root.Get<UUID>(), _root); // Add the root node to the entities list
            _entityNames.Add(_root, "Root"); // Add the root node to the names list
            _entityParents.Add(_root, _entities[_root.Get<UUID>()]); // Add the root node to the parents list, the root node is its own parent
            _entityChildren.Add(_root, new List<Entity>()); // Add the root node to the children list

            Log.Info("EntityManager: EntityManager initialized.");
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
            entity.Set(new UUID(entity, Guid.NewGuid())); // Set a new UUID for this entity
            _entities.Add(entity.Get<UUID>(), entity); // Add this entity to the entities list
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
            entity.Set(new UUID(entity, Guid.NewGuid())); // Set a new UUID for this entity
            _entities.Add(entity.Get<UUID>(), entity); // Add this entity to the entities list
            _entityNames.Add(entity, name); // Add this entity to the names list
            return entity;
        }

        /// <summary>
        /// Retrieves the entity with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity with the specified ID.</returns>
        public static Entity GetEntity(UUID id) {
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
        public static void DestroyEntity(UUID id) {
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
            _entities.Remove(entity.Get<UUID>()); // Remove this entity from the entities list
            WorldManager.World.Destroy(entity); // Destroy this entity
        }

        /// <summary>
        /// Calculates the model matrix for the specified entity.
        /// The model matrix is a transformation matrix that represents the position, rotation and scale of the entity.
        /// The model matrix is calculated by multiplying the position, rotation and scale matrices together.
        /// The resulting matrix includes all transformations due to rotation and scale of parents as well.
        /// </summary>
        /// <param name="entity">The entity for which to calculate the model matrix.</param>
        /// <returns>The model matrix for the specified entity.</returns>
        internal static Matrix4 GetModelMatrix(Entity entity)
        {
            Matrix4 modelMatrix = Matrix4.Identity;

            // Apply own transformations
            modelMatrix *= Matrix4.CreateScale(entity.Get<Scale>().scale);
            modelMatrix *= Matrix4.CreateFromQuaternion(entity.Get<Rotation>().rotationQuaternion);
            modelMatrix *= Matrix4.CreateTranslation(entity.Get<Position>().position);

            // Apply parent transformations
            Entity parent = EntityParents[entity];
            while (parent != Root) {
                modelMatrix *= GetModelMatrix(parent);
                parent = EntityParents[parent];
            }

            return modelMatrix;
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
        public static Dictionary<UUID, Entity> Entities { get => _entities; }

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