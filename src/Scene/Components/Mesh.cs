/// <summary>
/// Represents a mesh component.
/// </summary>
public struct Mesh
{
    /// <summary>
    /// The ID of the entity this component is attached to.
    /// </summary>
    public int entityID;

    /// <summary>
    /// The index of the mesh in the MeshManager.
    /// </summary>
    public int meshIndex;

    /// <summary>
    /// The index of the shader in the ShaderManager.
    /// </summary>
    public int shaderIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mesh"/> struct.
    /// </summary>
    /// <param name="entityID">The ID of the entity.</param>
    /// <param name="meshIndex">The index of the mesh.</param>
    /// <param name="shaderIndex">The index of the shader.</param>
    public Mesh(int entityID, int meshIndex, int shaderIndex)
    {
        this.meshIndex = meshIndex;
        this.entityID = entityID;
        this.shaderIndex = shaderIndex;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mesh"/> struct.
    /// </summary>
    /// <param name="entityID">The ID of the entity.</param>
    /// <param name="mesh">The mesh to copy the index from.</param>
    /// <param name="shaderIndex">The index of the shader.</param>
    public Mesh(int entityID, Mesh mesh, int shaderIndex)
    {
        this.meshIndex = mesh.meshIndex;
        this.entityID = entityID;
        this.shaderIndex = shaderIndex;
    }
}