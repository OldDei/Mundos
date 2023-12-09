using Arch.Core;
using Arch.Core.Utils;
using ImGuiNET;

namespace Mundos {
    internal static class EntityManager {
        private static Dictionary<int, Entity> _entities = new Dictionary<int, Entity>(); // Contains all IDs and entities in the scene
        private static Dictionary<Entity, string> _entityNames = new Dictionary<Entity, string>(); // Contains all entities and their names
        private static Dictionary<Entity, Entity> _entityParents = new Dictionary<Entity, Entity>(); // Contains all entities and their parents
        private static Dictionary<Entity, List<Entity>> _entityChildren = new Dictionary<Entity, List<Entity>>(); // Contains all entities and their children
        private static Entity _root = WorldManager.World.Create(); // Root node of the scene

        static EntityManager() {
            // EntityManager will create a root node for the scene
            _entities.Add(_root.Id, _root); // Add the root node to the entities list
            _entityNames.Add(_root, "Root"); // Add the root node to the names list
            _entityParents.Add(_root, _entities[0]); // Add the root node to the parents list, the root node is its own parent
            _entityChildren.Add(_root, new List<Entity>()); // Add the root node to the children list
        }

        internal static void DrawEntityTree()
        {
            ImGuiTreeNodeFlags baseFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.DefaultOpen;
            if (ImGui.TreeNodeEx(_entityNames[_root], baseFlags))
            {
                foreach (Entity entity in _entityChildren[_root]) // Iterate through all children of the root node
                {
                    if (ImGui.TreeNodeEx(_entityNames[entity], baseFlags))
                    {
                        // foreach (Entity child in _entityChildren[entity]) // Iterate through all children of this entity
                        // {
                        //     if (ImGui.TreeNodeEx(_entityNames[child], baseFlags))
                        //     {
                        //         ImGui.TreePop();
                        //     }
                        // }
                        ImGui.TreePop();
                    }
                }
                ImGui.TreePop();
            }
        }

        internal static Entity Create(ArchetypeType archetype, string name, Entity? parent = null) {
            Entity entity = WorldManager.World.Create(componentArchetypes[archetype]);
            Entity parentEntity = parent ?? _root; // If no parent is specified, use the root node
            _entities.Add(entity.Id, entity); // Add this entity to the entities list
            _entityNames.Add(entity, name); // Add this entity to the names list
            _entityParents.Add(entity, parentEntity); // Add this entity to the parents list
            if (!_entityChildren.ContainsKey(parentEntity)) _entityChildren.Add(parentEntity, new List<Entity>()); // If the parent entity doesn't have any children, add an empty list
            _entityChildren[parentEntity].Add(entity); // Add this entity to the children list of its parent
            return entity;
        }

        internal static Entity GetEntity(int id) {
            return _entities[id];
        }

        internal static Entity GetEntity(string name) {
            return _entityNames.FirstOrDefault(x => x.Value == name).Key;
        }

        internal static void DestroyEntity(int id) {
            DestroyEntity(_entities[id]);
        }

        internal static void DestroyEntity(Entity entity) {
            _entityChildren[entity].ForEach(child => DestroyEntity(child)); // Destroy all children of this entity and their children
            _entityChildren.Remove(entity); // Remove this entity from the children list of its parent
            _entityNames.Remove(entity); // Remove this entity from the names list
            _entityParents.Remove(entity); // Remove this entity from the parents list
            _entities.Remove(entity.Id); // Remove this entity from the entities list
            WorldManager.World.Destroy(entity); // Destroy this entity
        }

        public static Dictionary<ArchetypeType, ComponentType[]> componentArchetypes = new Dictionary<ArchetypeType, ComponentType[]>()
        {
            { ArchetypeType.EmptyNode,  new ComponentType[]{ typeof(Position), typeof(Rotation), typeof(Scale)                  } },
            { ArchetypeType.Model,      new ComponentType[]{ typeof(Position), typeof(Rotation), typeof(Scale), typeof(Mesh)    } },
            { ArchetypeType.Camera,     new ComponentType[]{ typeof(Position), typeof(Rotation), typeof(Scale), typeof(Camera)  } },
            { ArchetypeType.Script,     new ComponentType[]{ typeof(Script)                                                     } }
        };

        public enum ArchetypeType
        {
            EmptyNode,
            Model,
            Camera,
            Script
        }
    }
}