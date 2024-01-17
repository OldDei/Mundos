using OpenTK.Mathematics;

/// <summary>
/// Represents the position of an entity in 3D space.
/// </summary>
public struct Position
{
    /// <summary>
    /// The ID of the entity this position component is attached to.
    /// </summary>
    public int entityID;

    /// <summary>
    /// The position vector in 3D space.
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> struct with the specified entity ID and position coordinates.
    /// </summary>
    /// <param name="entityID">The ID of the entity.</param>
    /// <param name="x">The x-coordinate of the position.</param>
    /// <param name="y">The y-coordinate of the position.</param>
    /// <param name="z">The z-coordinate of the position.</param>
    public Position(int entityID, float x, float y, float z)
    {
        this.position = new Vector3(x, y, z);
        this.entityID = entityID;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> struct with the specified entity ID and position vector.
    /// </summary>
    /// <param name="entityID">The ID of the entity.</param>
    /// <param name="position">The position vector.</param>
    public Position(int entityID, Vector3 position)
    {
        this.position = position;
        this.entityID = entityID;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> struct with the specified entity ID and position component.
    /// </summary>
    /// <param name="entityID">The ID of the entity.</param>
    /// <param name="position">The position component.</param>
    public Position(int entityID, Position position)
    {
        this.position = position.position;
        this.entityID = entityID;
    }
}