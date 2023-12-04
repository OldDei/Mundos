using OpenTK.Mathematics;

namespace Mundos
{
    public class Model : Node
    {
        Mesh _mesh;
        Shader _shader;

        public Model(Node? parent, string name, Vector3 position, Vector3 rotation, Vector3 scale) : base(parent, name, position, rotation, scale)
        {
            _mesh = new Mesh(new float[0], new uint[0], new List<Texture>(), this);
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
            base.Draw(renderer);
            _mesh.Draw(renderer); // Draw the mesh with the shader
        }

        public Matrix4 GetModelMatrix()
        {
            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateScale(_scale.X, _scale.Y, _scale.Z);
            model *= Matrix4.CreateRotationX(_rotation.X);
            model *= Matrix4.CreateRotationY(_rotation.Y);
            model *= Matrix4.CreateRotationZ(_rotation.Z);
            model *= Matrix4.CreateTranslation(_position.X, _position.Y, _position.Z);
            return model;
        }
    }
}