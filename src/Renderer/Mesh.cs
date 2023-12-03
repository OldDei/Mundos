using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Mundos
{
    public class Mesh
    {
        public struct Vertex
        {
            public Vertex(Vector3 position, Vector3 normal, Vector2 texCoords)
            {
                Position = position;
                Normal = normal;
                TexCoords = texCoords;
            }

            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 TexCoords;
        }

        private List<Vertex> vertices = new List<Vertex>();
        private List<uint> indices = new List<uint>();
        private List<Texture> textures = new List<Texture>();

        public Mesh(List<Vertex> vertices, List<uint> indices, List<Texture> textures)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.textures = textures;

            vertices.Add(new Vertex(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 1.0f)));
            vertices.Add(new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(1.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 0.0f)));
            vertices.Add(new Vertex(new Vector3(-0.5f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f), new Vector2(0.0f, 1.0f)));

            indices.Add(0);
            indices.Add(1);
            indices.Add(3);

            indices.Add(1);
            indices.Add(2);
            indices.Add(3);
        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawMesh(this);
        }

        public void GetDrawData(out List<Vertex> vertices, out List<uint> indices, out List<Texture> textures)
        {
            vertices = this.vertices;
            indices = this.indices;
            textures = this.textures;
        }
    }
}