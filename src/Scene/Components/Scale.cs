using OpenTK.Mathematics;

public struct Scale
{
    public int entityID; // ID of entity this component is attached to
    public Vector3 scale;
    public Scale(int entityID, float x, float y, float z)
    {
        this.scale = new Vector3(x, y, z);
        this.entityID = entityID;
    }
    public Scale(int entityID, Vector3 scale)
    {
        this.scale = scale;
        this.entityID = entityID;
    }
    public Scale(int entityID, Scale scale)
    {
        this.scale = scale.scale;
        this.entityID = entityID;
    }
}