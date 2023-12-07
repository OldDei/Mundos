using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using OpenTK.Mathematics;

namespace Mundos
{
    public static class WorldManager
    {
        private static World _world;

        static WorldManager()
        {
            // WorldManager will always have at least an empty world
            _world = World.Create();
        }


        internal static World World => _world;

        /// <summary>
        /// Loads a world with the specified name.
        /// </summary>
        /// <param name="v">The name of the world to load.</param>
        /// <returns>True if the world was loaded successfully, otherwise false.</returns>
        internal static bool LoadWorld(string v)
        {
            // TODO: Load world from file
            _world = World.Create();

            _world.Create(componentArchetypes[ArchetypeType.Model]).Set(new Position(-0.25f, 0, 0), new Rotation(0, 0, 0), new Scale(1, 1, 1), new Mesh(0));
            _world.Create(componentArchetypes[ArchetypeType.Model]).Set(new Position( 0.25f, 0, 0), new Rotation(0, 0, 0), new Scale(1, 1, 1), new Mesh(0));
            return true;
        }

        /// <summary>
        /// Saves the current world to a file with the specified name.
        /// </summary>
        /// <param name="v">The name of the file to save the world to.</param>
        /// <returns>True if the world was saved successfully, otherwise false.</returns>
        internal static bool SaveWorld(string v)
        {
            return false;
        }

        public static Dictionary<ArchetypeType, ComponentType[]> componentArchetypes = new Dictionary<ArchetypeType, ComponentType[]>()
        {
            { ArchetypeType.EmptyNode,  new ComponentType[]{ typeof(Position), typeof(Rotation), typeof(Scale)               } },
            { ArchetypeType.Model,      new ComponentType[]{ typeof(Position), typeof(Rotation), typeof(Scale), typeof(Mesh) } }
        };

        public enum ArchetypeType
        {
            EmptyNode,
            Model
        }
    }
}