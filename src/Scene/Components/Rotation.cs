using OpenTK.Mathematics;

public struct Rotation
{
    public  int entityID; // ID of entity this component is attached to
    public Vector3 rotation;
    public Rotation(int entityID, float x, float y, float z)
    {
        this.rotation = new Vector3(x, y, z);
        this.entityID = entityID;
    }
    public Rotation(int entityID, Vector3 rotation)
    {
        this.rotation = rotation;
        this.entityID = entityID;

    }
    public Rotation(int entityID, Rotation rotation)
    {
        this.rotation = rotation.rotation;
        this.entityID = entityID;
    }
}
