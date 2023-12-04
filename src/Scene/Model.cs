using OpenTK.Mathematics;

namespace Mundos
{
    /// <summary>
    /// Represents a model in the Mundos scene.
    /// </summary>
    public class Model : Node
    {
        Mesh _mesh;
        Shader _shader;

        public Model(Node? parent, string name, Vector3 position, Vector3 rotation, Vector3 scale) : base(parent, name, position, rotation, scale)
        {
            _mesh = new Mesh(new float[0], new uint[0], new List<Texture>(), this);
            _shader = new Shader();
        }

        /// <summary>
        /// Sets a new mesh for the model.
        /// </summary>
        /// <param name="mesh">The mesh to set.</param>
        public void SetMesh(Mesh mesh)
        {
            _mesh = mesh;
        }

        /// <summary>
        /// Sets the shader for the model.
        /// TODO: Actually implement using the shader?
        /// </summary>
        /// <param name="shader">The shader to set.</param>
        public void SetShader(Shader shader)
        {
            _shader = shader;
        }

        /// <summary>
        /// Draws the mesh of the model using the specified renderer.
        /// </summary>
        /// <param name="renderer">The renderer used to draw the model.</param>
        override public void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            _mesh.Draw(renderer); // Draw the mesh with the shader
        }
    }
}