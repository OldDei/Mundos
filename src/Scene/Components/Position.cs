using OpenTK.Mathematics;

public struct Position
{
    public int entityID; // ID of entity this component is attached to
    public Vector3 position;
    public Position(int entityID, float x, float y, float z)
    {
        this.position = new Vector3(x, y, z);
        this.entityID = entityID;
    }
    public Position(int entityID, Vector3 position)
    {
        this.position = position;
        this.entityID = entityID;
    }
    public Position(int entityID, Position position)
    {
        this.position = position.position;
        this.entityID = entityID;
    }
}