using Arch.Core;
using Arch.Core.Extensions;
using OpenTK.Mathematics;

/// <summary>
/// Is a base class for all game scripts.
/// </summary>
public abstract class MundosScript
{
#pragma warning disable CS0169 // Remove unused private members
    public int entityID { get; set;} = 0; // ID of entity this component is attached to
    private Vector3 EntityPosition {
        get => Mundos.EntityManager.GetEntity(entityID).Get<Position>().position;
        set => Mundos.EntityManager.GetEntity(entityID).Set(new Position(entityID, value));
    }
    protected Vector3 position;
    private Vector3 EntityRotation {
        get => Mundos.EntityManager.GetEntity(entityID).Get<Rotation>().rotation;
        set => Mundos.EntityManager.GetEntity(entityID).Set(new Rotation(entityID, value));
    }
    protected Vector3 rotation;
    private Vector3 EntityScale {
        get => Mundos.EntityManager.GetEntity(entityID).Get<Scale>().scale;
        set => Mundos.EntityManager.GetEntity(entityID).Set(new Scale(entityID, value));
    }
    protected Vector3 scale;
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