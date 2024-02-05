using Arch.Core;

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
    /// <param name="UUID">The UUID of the entity.</param>
    /// <param name="script">The script object.</param>
    public UUID(Entity ComponentEntity, Guid UUID)
    {
        this.ComponentEntity = ComponentEntity;
        this.UniversalUniqueID = UUID;
    }
}
