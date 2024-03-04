using Arch.Core;

/// <summary>
/// Represents a script attached to an entity
/// </summary>
public struct Script
{
    /// <summary>
    /// The ID of the entity this component is attached to.
    /// </summary>
    public Entity ComponentEntity;

    /// <summary>
    /// Reference to the MundosScript object.
    /// </summary>
    public MundosScript MundosScriptRef;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Script"/> is enabled.
    /// </summary>
    public bool enabled
    {
        get => isEnabled;
        set {
            isEnabled = value;
            if (value) MundosScriptRef.OnEnable(); else MundosScriptRef.OnDisable();
        }
    }

    private bool isEnabled = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="Script"/> struct.
    /// </summary>
    /// <param name="entityID">The ID of the entity to which the script is attached.</param>
    /// <param name="script">The script object.</param>
    public Script(Entity ComponentEntity, MundosScript script)
    {
        this.ComponentEntity = ComponentEntity;
        MundosScriptRef = script;
        MundosScriptRef.parentEntity = ComponentEntity;
        MundosScriptRef.OnCreate();
    }
}
