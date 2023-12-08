using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using OpenTK.Mathematics;

namespace Mundos
{
    internal static class WorldManager
    {
        private static World _world;
        private static Camera? _primaryCamera;

        static WorldManager()
        {
            // WorldManager will always have at least an empty world
            _world = World.Create();
        }



        /// <summary>
        /// Loads a world with the specified name.
        /// </summary>
        /// <param name="v">The name of the world to load.</param>
        /// <returns>True if the world was loaded successfully, otherwise false.</returns>
        internal static bool LoadWorld(string v)
        {
            // TODO: Load world from file
            _world = World.Create();

            Entity entity1 = EntityManager.Create(EntityManager.ArchetypeType.Model);
            entity1.Set(new Position(entity1.Id, -0.5f, 0, 0), new Rotation(entity1.Id, 0, 0, 0), new Scale(entity1.Id, 1, 1, 1), new Mesh(entity1.Id, 0));

            Entity entity2 = EntityManager.Create(EntityManager.ArchetypeType.Model);
            entity2.Set(new Position(entity1.Id,  0.5f, 0, 0), new Rotation(entity1.Id, 0, 0, 0), new Scale(entity1.Id, 1, 1, 1), new Mesh(entity1.Id, 0));

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

        internal static void SetActiveCamera(Camera camera) => _primaryCamera = camera;
        internal static void GetActiveCamera(out Camera? camera) => camera = _primaryCamera;
        internal static World World => _world;
    }
}