using OpenTK.Mathematics;

namespace Mundos {

    /// <summary>
    /// Manages the creation and retrieval of mesh data.
    /// </summary>
    internal static class MeshManager {

        /// <summary>
        /// List of float arrays representing the vertex positions.
        /// </summary>
        static List<float[]> vertexPositions;

        /// <summary>
        /// List of float arrays representing the vertex normals.
        /// </summary>
        static List<float[]> vertexNormals;

        /// <summary>
        /// List of float arrays representing the vertex texture coordinates.
        /// </summary>
        static List<float[]> vertexTexCoords;

        /// <summary>
        /// List of uint arrays representing the indices.
        /// </summary>
        static List<uint[]> indices;

        /// <summary>
        /// The MeshManager class is responsible for managing meshes in the scene.
        /// </summary>
        static MeshManager() {
            vertexPositions = new List<float[]>();
            vertexNormals = new List<float[]>();
            vertexTexCoords = new List<float[]>();
            indices = new List<uint[]>();

            // Debug square vertical TODO: remove
            vertexPositions.Add(new float[] {
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                 0.5f,  0.5f, 0.0f,
                -0.5f,  0.5f, 0.0f
            });
            vertexNormals.Add(new float[] {
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f,  0.0f, 1.0f,
                0.0f,  0.0f, 1.0f
            });
            vertexTexCoords.Add(new float[] {
                0.0f, 0.0f,
                1.0f, 0.0f,
                1.0f,  1.0f,
                0.0f,  1.0f
            });
            indices.Add(new uint[] {
                0, 1, 2,
                2, 3, 0
            });
            // Debug square horizontal
            vertexPositions.Add(new float[] {
                -0.5f,  0.0f, -0.5f,
                 0.5f,  0.0f, -0.5f,
                 0.5f,  0.0f,  0.5f,
                -0.5f,  0.0f,  0.5f
            });
            vertexNormals.Add(new float[] {
                0.0f, 1.0f, 0.0f,
                0.0f,  1.0f, 0.0f,
                0.0f,  1.0f, 0.0f,
                0.0f,  1.0f, 0.0f
            });
            vertexTexCoords.Add(new float[] {
                0.0f, 0.0f,
                1.0f, 0.0f,
                1.0f,  1.0f,
                0.0f,  1.0f
            });
            indices.Add(new uint[] {
                0, 1, 2,
                2, 3, 0
            });
        }

        /// <summary>
        /// Retrieves the mesh data for a given ID.
        /// </summary>
        /// <param name="id">The ID of the mesh.</param>
        /// <param name="vertexPositions">The array of vertex positions.</param>
        /// <param name="vertexNormals">The array of vertex normals.</param>
        /// <param name="vertexTexCoords">The array of vertex texture coordinates.</param>
        /// <param name="indices">The array of indices.</param>
        internal static void GetMesh(int id, out float[] vertexPositions, out float[] vertexNormals, out float[] vertexTexCoords, out uint[] indices) {
            vertexPositions = MeshManager.vertexPositions[id];
            vertexNormals = MeshManager.vertexNormals[id];
            vertexTexCoords = MeshManager.vertexTexCoords[id];
            indices = MeshManager.indices[id];
        }

        /// <summary>
        /// Loads a mesh from the specified path.
        /// </summary>
        /// <param name="path">The path of the mesh file.</param>
        internal static void LoadMesh(string path) {
            // TODO: implement
        }
    }
}