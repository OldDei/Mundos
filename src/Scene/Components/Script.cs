/// <summary>
/// Represents a script attached to an entity
/// </summary>
public struct Script
{
    /// <summary>
    /// The ID of the entity this component is attached to.
    /// </summary>
    public int entityID;

    /// <summary>
    /// Reference to the MundosScript object.
    /// </summary>
    public MundosScript MundosScriptRef;

    /// <summary>
    /// Initializes a new instance of the <see cref="Script"/> struct.
    /// </summary>
    /// <param name="entityID">The ID of the entity to which the script is attached.</param>
    /// <param name="script">The script object.</param>
    public Script(int entityID, MundosScript script)
    {
        MundosScriptRef = script;
        MundosScriptRef.entityID = entityID;
        MundosScriptRef.OnCreate();
    }
}
