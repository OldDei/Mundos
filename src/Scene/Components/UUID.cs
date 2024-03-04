using Arch.Core;
using Mundos;

/// <summary>
/// Represents a unique UUID attached to an entity
/// </summary>
public struct UUID
{
    /// <summary>
    /// The ID of the entity this component is attached to.
    /// </summary>
    public Entity ComponentEntity;

    /// <summary>
    /// UUID of the entity.
    /// </summary>
    public Guid UniversalUniqueID;

    /// <summary>
    /// Initializes a new instance of the <see cref="UUID"/> struct.
    /// </summary>
    /// <param name="ComponentEntity">The entity this component is attached to.</param>
    /// <param name="UUID">The UUID of the entity.</param>
    public UUID(Entity ComponentEntity, Guid UUID)
    {
        this.ComponentEntity = ComponentEntity;
        this.UniversalUniqueID = UUID;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UUID"/> struct.
    /// </summary>
    /// <param name="ComponentEntity">The entity this component is attached to.</param>
    public UUID(Entity ComponentEntity)
    {
        this.ComponentEntity = ComponentEntity;
        this.UniversalUniqueID = Guid.NewGuid();
    }
}
