using OpenTK.Mathematics;

namespace Mundos
{
    public class Model : Node
    {
        Mesh _mesh;
        Shader _shader;

        public Model(Node? parent, string name, Vector3 position, Vector3 rotation, Vector3 scale) : base(parent, name, position, rotation, scale)
        {
            _mesh = new Mesh(new List<Mesh.Vertex>(), new List<uint>(), new List<Texture>());
            _shader = new Shader();
        }

        public void SetMesh(Mesh mesh)
        {
            _mesh = mesh;
        }

        public void SetShader(Shader shader)
        {
            _shader = shader;
        }

        override public void Draw(Renderer renderer)
        {
            _mesh.Draw(renderer); // Draw the mesh with the shader
        }
    }
}