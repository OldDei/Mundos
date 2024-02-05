using Arch.Core;
using OpenTK.Mathematics;

/// <summary>
/// Represents the scale component
/// </summary>
public struct Scale
{
    /// <summary>
    /// The ID of the entity this component is attached to.
    /// </summary>
    public Entity ComponentEntity;

    /// <summary>
    /// The scale vector.
    /// </summary>
    public Vector3 scale;

    /// <summary>
    /// Initializes a new instance of the <see cref="Scale"/> struct with the specified entity ID and scale values.
    /// </summary>
    /// <param name="entityID">The ID of the entity this component is attached to.</param>
    /// <param name="x">The X scale value.</param>
    /// <param name="y">The Y scale value.</param>
    /// <param name="z">The Z scale value.</param>
    public Scale(Entity ComponentEntity, float x, float y, float z)
    {
        this.scale = new Vector3(x, y, z);
        this.ComponentEntity = ComponentEntity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Scale"/> struct with the specified entity ID and scale vector.
    /// </summary>
    /// <param name="entityID">The ID of the entity this component is attached to.</param>
    /// <param name="scale">The scale vector.</param>
    public Scale(Entity ComponentEntity, Vector3 scale)
    {
        this.scale = scale;
        this.ComponentEntity = ComponentEntity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Scale"/> struct with the specified entity ID and another scale component.
    /// </summary>
    /// <param name="entityID">The ID of the entity this component is attached to.</param>
    /// <param name="scale">The other scale component.</param>
    public Scale(Entity ComponentEntity, Scale scale)
    {
        this.scale = scale.scale;
        this.ComponentEntity = ComponentEntity;
    }
}