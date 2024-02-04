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
            groundEntity.Set(new UUID(groundEntity.Id, Guid.NewGuid()), new Position(groundEntity.Id,  0, -1f, 0), new Rotation(groundEntity.Id, 0, 0, 0), new Scale(groundEntity.Id, 5, 1, 5), new Mesh(groundEntity.Id, 1, 1));

            Entity wallEntity = EntityManager.Create(EntityManager.ArchetypeType.Model, "Wall", groundEntity);
            wallEntity.Set(new UUID(groundEntity.Id, Guid.NewGuid()), new Position(wallEntity.Id, 0, 0.5f, -0.5f), new Rotation(wallEntity.Id, 0, 0, 0), new Scale(wallEntity.Id, 1, 1, 1), new Mesh(wallEntity.Id, 0, 0));

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
            Entity parent = EntityManager.EntityParents[entity];
            while (parent != EntityManager.Root) {
                modelMatrix *= GetModelMatrix(parent);
                parent = EntityManager.EntityParents[parent];
            }

            return modelMatrix;
        }

        public static World World => _world;
    }
}