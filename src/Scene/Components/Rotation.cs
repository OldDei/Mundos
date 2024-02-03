using OpenTK.Mathematics;

/// <summary>
/// Represents the rotation component of an entity in a scene.
/// </summary>
public struct Rotation
{
    /// <summary>
    /// The ID of the entity this component is attached to.
    /// </summary>
    public int entityID;

    /// <summary>
    /// The rotation vector.
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// The rotation value as a quaternion.
    /// </summary>
    public Quaternion rotationQuaternion => Quaternion.FromEulerAngles(rotation);

    /// <summary>
    /// The rotation value as a matrix.
    /// </summary>
    public Matrix4 rotationMatrix => Matrix4.CreateFromQuaternion(rotationQuaternion);

    /// <summary>
    /// The rotation value as Euler angles.
    /// </summary>
    public Vector3 rotationEulerAngles => rotation;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotation"/> struct.
    /// </summary>
    /// <param name="entityID">The ID of the entity.</param>
    /// <param name="x">The x-axis rotation value.</param>
    /// <param name="y">The y-axis rotation value.</param>
    /// <param name="z">The z-axis rotation value.</param>
    public Rotation(int entityID, float x, float y, float z)
    {
        this.rotation = new Vector3(x, y, z);
        this.entityID = entityID;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotation"/> struct.
    /// </summary>
    /// <param name="entityID">The ID of the entity.</param>
    /// <param name="rotation">The rotation vector.</param>
    public Rotation(int entityID, Vector3 rotation)
    {
        this.rotation = rotation;
        this.entityID = entityID;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotation"/> struct.
    /// </summary>
    /// <param name="entityID">The ID of the entity.</param>
    /// <param name="rotation">The rotation component.</param>
    public Rotation(int entityID, Rotation rotation)
    {
        this.rotation = rotation.rotation;
        this.entityID = entityID;
    }
}
