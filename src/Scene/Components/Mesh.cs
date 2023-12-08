/// <summary>
/// Mesh component containing index of mesh in MeshManager
/// </summary>
public struct Mesh
{
    public int entityID; // ID of entity this component is attached to
    public int meshIndex; // Index of mesh in MeshManager
    public Mesh(int entityID, int meshIndex)
    {
        this.meshIndex = meshIndex;
        this.entityID = entityID;
    }
    public Mesh(int entityID, Mesh mesh)
    {
        this.meshIndex = mesh.meshIndex;
        this.entityID = entityID;
    }
}