using OpenTK.Mathematics;

namespace Mundos {

    internal static class MeshManager {

        static List<float[]> vertexPositions;
        static List<float[]> vertexNormals;
        static List<float[]> vertexTexCoords;
        static List<uint[]> indices;

        static MeshManager() {
            vertexPositions = new List<float[]>();
            vertexNormals = new List<float[]>();
            vertexTexCoords = new List<float[]>();
            indices = new List<uint[]>();

            // Debug square vertical
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

        internal static void GetMesh(int id, out float[] vertexPositions, out float[] vertexNormals, out float[] vertexTexCoords, out uint[] indices) {
            vertexPositions = MeshManager.vertexPositions[id];
            vertexNormals = MeshManager.vertexNormals[id];
            vertexTexCoords = MeshManager.vertexTexCoords[id];
            indices = MeshManager.indices[id];
        }

        internal static void LoadMesh(string path) {

        }
    }
}