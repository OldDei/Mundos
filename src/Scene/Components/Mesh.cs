/// <summary>
/// Mesh component containing index of mesh in MeshManager
/// </summary>
public struct Mesh
{
    public int entityID; // ID of entity this component is attached to
    public int meshIndex; // Index of mesh in MeshManager
    public int shaderIndex; // Index of shader in ShaderManager
    public Mesh(int entityID, int meshIndex, int shaderIndex)
    {
        this.meshIndex = meshIndex;
        this.entityID = entityID;
        this.shaderIndex = shaderIndex;
    }
    public Mesh(int entityID, Mesh mesh, int shaderIndex)
    {
        this.meshIndex = mesh.meshIndex;
        this.entityID = entityID;
        this.shaderIndex = shaderIndex;
    }
}