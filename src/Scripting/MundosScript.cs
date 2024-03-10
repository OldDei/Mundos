using Arch.Core;
using Arch.Core.Extensions;
using Mundos;
using OpenTK.Mathematics;

/// <summary>
/// Is a base class for all game scripts.
/// </summary>
public abstract class MundosScript
{
    public Entity parentEntity { get; set; } // Entity this component is attached to
    private Vector3 EntityPosition {
        get => parentEntity.Get<Position>().position;
        set => parentEntity.Set(new Position(parentEntity, value));
    }
    protected Vector3 position; // Position of the entity this script is attached to
    private Vector3 EntityRotation {
        get => parentEntity.Get<Rotation>().rotation;
        set => parentEntity.Set(new Rotation(parentEntity, value));
    }
    protected Vector3 rotation; // Rotation of the entity this script is attached to
    private Vector3 EntityScale {
        get => parentEntity.Get<Scale>().scale;
        set => parentEntity.Set(new Scale(parentEntity, value));
    }
    protected Vector3 scale; // Scale of the entity this script is attached to

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
    /// This function updates this script.
    /// </summary>
    internal void Update() {
        position = EntityPosition;
        rotation = EntityRotation;
        scale = EntityScale;
        OnUpdate();
        EntityPosition = position;
        EntityRotation = rotation;
        EntityScale = scale;
    }
    /// <summary>
    /// Called when the script is updated.
    /// </summary>
    public virtual void OnUpdate() { }
}