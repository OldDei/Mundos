using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using OpenTK.Mathematics;

namespace Mundos
{
    public static class WorldManager
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
        public static bool LoadWorld(string v)
        {
            // TODO: Load world from file
            _world = World.Create();

            Entity groundEntity = EntityManager.Create(EntityManager.ArchetypeType.Model, "Ground");
            groundEntity.Set(new Position(groundEntity.Id,  0, -1f, 0), new Rotation(groundEntity.Id, 0, 0, 0), new Scale(groundEntity.Id, 5, 1, 5), new Mesh(groundEntity.Id, 1, 1));

            Entity wallEntity = EntityManager.Create(EntityManager.ArchetypeType.Model, "Wall", groundEntity);
            wallEntity.Set(new Position(wallEntity.Id, 0, 0.5f, -2.5f), new Rotation(wallEntity.Id, 0, 0, 0), new Scale(wallEntity.Id, 1, 1, 1), new Mesh(wallEntity.Id, 0, 0));

            return true;
        }

        /// <summary>
        /// Saves the current world to a file with the specified name.
        /// </summary>
        /// <param name="v">The name of the file to save the world to.</param>
        /// <returns>True if the world was saved successfully, otherwise false.</returns>
        public static bool SaveWorld(string v)
        {
            // TODO: Save world to file
            return false;
        }

        internal static void SetActiveCamera(Camera camera) => _primaryCamera = camera;
        internal static void GetActiveCamera(out Camera? camera) => camera = _primaryCamera;
        internal static Camera? GetActiveCamera() => _primaryCamera;

        internal static Vector3 GetEntityWorldPosition(Entity entity)
        {
            Vector3 position = entity.Get<Position>().position;
            Entity parent = EntityManager.EntityParents[entity];
            while (parent != EntityManager.Root)
            {
                Position parentPosition = parent.Get<Position>();
                position += parentPosition.position;
                parent = EntityManager.EntityParents[parent];
            }
            return position;
        }

        internal static Vector3 GetEntityWorldRotation(Entity entity)
        {
            Vector3 rotation = entity.Get<Rotation>().rotation;
            Entity parent = EntityManager.EntityParents[entity];
            while (parent != EntityManager.Root)
            {
                Rotation parentRotation = parent.Get<Rotation>();
                rotation += parentRotation.rotation;
                parent = EntityManager.EntityParents[parent];
            }
            return rotation;
        }

        internal static Vector3 GetEntityWorldScale(Entity entity)
        {
            Vector3 scale = entity.Get<Scale>().scale;
            Entity parent = EntityManager.EntityParents[entity];
            while (parent != EntityManager.Root)
            {
                Scale parentScale = parent.Get<Scale>();
                scale *= parentScale.scale;
                parent = EntityManager.EntityParents[parent];
            }
            return scale;
        }

        public static World World => _world;
    }
}