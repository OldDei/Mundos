using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;

namespace Mundos
{
    public static class WorldManager
    {
        private static World _world;

        static WorldManager()
        {
            // WorldManager will always have at least an empty world
            _world = World.Create();

            Console.WriteLine("WorldManager initialized.");;
        }

        /// <summary>
        /// Loads a world with the specified name.
        /// </summary>
        /// <param name="v">The name of the world to load.</param>
        /// <returns>True if the world was loaded successfully, otherwise false.</returns>
        public static bool LoadWorld(string v)
        {
            // TODO: Load world from file
            _world = World.Create();

            Entity groundEntity = EntityManager.Create(EntityManager.ArchetypeType.Model, "Ground");
            groundEntity.Set(new UUID(groundEntity, Guid.NewGuid()), new Position(groundEntity,  0, -1f, 0), new Rotation(groundEntity, 0, 0, 0), new Scale(groundEntity, 5, 1, 5), new Mesh(groundEntity, 1, 1));

            Entity wallEntity = EntityManager.Create(EntityManager.ArchetypeType.Model, "Wall", groundEntity);
            wallEntity.Set(new UUID(wallEntity, Guid.NewGuid()), new Position(wallEntity, 0, 0.5f, -0.5f), new Rotation(wallEntity, 0, 0, 0), new Scale(wallEntity, 1, 1, 1), new Mesh(wallEntity, 0, 0));

            return true;
        }

        /// <summary>
        /// Saves the current world to a file with the specified name.
        /// </summary>
        /// <param name="v">The name of the file to save the world to.</param>
        /// <returns>True if the world was saved successfully, otherwise false.</returns>
        public static bool SaveWorld(string v)
        {
            Serializer.SaveWorld(v, _world);
            return false;
        }

        public static World World => _world;
    }
}