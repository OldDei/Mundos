using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Mundos
{
    /// <summary>
    /// Represents a mesh object that contains vertex and index data, as well as textures and a parent node.
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// Represents a vertex in a mesh.
        /// </summary>
        public struct Vertex
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Vertex"/> struct.
            /// </summary>
            /// <param name="position">The position of the vertex.</param>
            /// <param name="normal">The normal vector of the vertex.</param>
            /// <param name="texCoords">The texture coordinates of the vertex.</param>
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

        private float[] vertices;
        private uint[] indices;
        private List<Texture> textures;
        private Node parentNode;

        public Mesh(float[] vertices, uint[] indices, List<Texture> textures, Node parentNode)
        {
            this.parentNode = parentNode;

            this.vertices = vertices;
            this.indices = indices;
            this.textures = textures;

            this.vertices = new float[]{
                0.5f,  0.5f, 0.0f,  // top right
                0.5f, -0.5f, 0.0f,  // bottom right
                -0.5f, -0.5f, 0.0f,  // bottom left
                -0.5f,  0.75f, 0.0f   // top left
            };
            this.indices = new uint[]{  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };
        }

        /// <summary>
        /// Draws the mesh using the specified renderer.
        /// TODO: Is the mesh visible? Culling or something?
        /// </summary>
        /// <param name="renderer">The renderer to use for drawing.</param>
        public void Draw(Renderer renderer)
        {
            renderer.DrawMesh(this);
        }

        /// <summary>
        /// Retrieves the draw data of the mesh.
        /// </summary>
        /// <param name="vertices">The array of vertices.</param>
        /// <param name="indices">The array of indices.</param>
        /// <param name="textures">The list of textures.</param>
        public void GetDrawData(out float[] vertices, out uint[] indices, out List<Texture> textures)
        {
            vertices = this.vertices;
            indices = this.indices;
            textures = this.textures;
        }

        /// <summary>
        /// Returns a 4x4 matrix used for transformations in rendering.
        /// TODO: Optimize by caching the matrix and only updating it when necessary.
        /// </summary>
        internal Matrix4 GetMeshTransformMatrix()
        {
            Matrix4 model = Matrix4.Identity;

            // Get the parent node's position, rotation, and scale
            Vector3 _position = parentNode.GetPosition();
            Vector3 _rotation = parentNode.GetRotation();
            Vector3 _scale = parentNode.GetScale();

            model *= Matrix4.CreateScale(_scale.X, _scale.Y, _scale.Z);
            model *= Matrix4.CreateRotationX(_rotation.X);
            model *= Matrix4.CreateRotationY(_rotation.Y);
            model *= Matrix4.CreateRotationZ(_rotation.Z);
            model *= Matrix4.CreateTranslation(_position.X, _position.Y, _position.Z);

            return model;
        }
    }
}