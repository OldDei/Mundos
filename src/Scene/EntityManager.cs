using Arch.Core;
using Arch.Core.Utils;
using CommunityToolkit.HighPerformance;

namespace Mundos {
    internal static class EntityManager {
        private static Dictionary<int, Entity> _entities = new Dictionary<int, Entity>();

        internal static Entity Create(ArchetypeType archetype) {
            Entity entity = WorldManager.World.Create(componentArchetypes[archetype]);
            _entities.Add(entity.Id, entity);
            return entity;
        }

        internal static Entity GetEntity(int id) {
            return _entities[id];
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