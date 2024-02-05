using Arch.Core;
using OpenTK.Mathematics;

/// <summary>
/// Represents the position of an entity in 3D space.
/// </summary>
public struct Position
{
    /// <summary>
    /// The ID of the entity this position component is attached to.
    /// </summary>
    public Entity ComponentEntity;

    /// <summary>
    /// The position vector in 3D space.
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> struct with the specified entity ID and position coordinates.
    /// </summary>
    /// <param name="ComponentEntity">The entity this position component is attached to.</param>
    /// <param name="x">The x-coordinate of the position.</param>
    /// <param name="y">The y-coordinate of the position.</param>
    /// <param name="z">The z-coordinate of the position.</param>
    public Position(Entity ComponentEntity, float x, float y, float z)
    {
        this.position = new Vector3(x, y, z);
        this.ComponentEntity = ComponentEntity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> struct with the specified entity ID and position vector.
    /// </summary>
    /// <param name="ComponentEntity">The entity this position component is attached to.</param>
    /// <param name="position">The position vector.</param>
    public Position(Entity ComponentEntity, Vector3 position)
    {
        this.position = position;
        this.ComponentEntity = ComponentEntity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> struct with the specified entity ID and position component.
    /// </summary>
    /// <param name="ComponentEntity">The entity this position component is attached to.</param>
    /// <param name="position">The position component.</param>
    public Position(Entity ComponentEntity, Position position)
    {
        this.position = position.position;
        this.ComponentEntity = ComponentEntity;
    }
}