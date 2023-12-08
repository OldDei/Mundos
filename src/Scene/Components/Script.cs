/// <summary>
/// Represents a script component.
/// </summary>
public struct Script
{
    public int entityID; // ID of entity this component is attached to
    public MundosScript MundosScriptRef;
    public Script(int entityID, MundosScript script)
    {
        MundosScriptRef = script;
        MundosScriptRef.entityID = entityID;
        MundosScriptRef.OnCreate();
    }
}
