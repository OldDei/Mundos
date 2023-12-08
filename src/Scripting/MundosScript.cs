using Arch.Core;
using Arch.Core.Extensions;
/// <summary>
/// Is a base class for all game scripts.
/// </summary>
public abstract class MundosScript
{
#pragma warning disable CS0169 // Remove unused private members
    public int entityID { get; set;} = 0; // ID of entity this component is attached to
    internal Position position {
        get => Mundos.EntityManager.GetEntity(entityID).Get<Position>();
        set => Mundos.EntityManager.GetEntity(entityID).Set(value);
    }
    internal Rotation rotation {
        get => Mundos.EntityManager.GetEntity(entityID).Get<Rotation>();
        set => Mundos.EntityManager.GetEntity(entityID).Set(value);
    }
    internal Scale scale {
        get => Mundos.EntityManager.GetEntity(entityID).Get<Scale>();
        set => Mundos.EntityManager.GetEntity(entityID).Set(value);
    }
#pragma warning restore CS0169 // Remove unused private members
    /// <summary>
    /// Called when the script is created.
    /// </summary>
    public virtual void OnCreate() { }
    /// <summary>
    /// Called when the script is destroyed.
    /// </summary>
    public virtual void OnDestroy() { }
    /// <summary>
    /// Called when the script is enabled.
    /// </summary>
    public virtual void OnEnable() { }
    /// <summary>
    /// Called when the script is disabled.
    /// </summary>
    public virtual void OnDisable() { }
    /// <summary>
    /// Called when the script is updated.
    /// </summary>
    public virtual void OnUpdate() { }
}