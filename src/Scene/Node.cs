using OpenTK.Mathematics;

namespace Mundos
{
    public class Node
    {
        Vector3 _position;
        Vector3 _rotation;
        Vector3 _scale;

        readonly Node? _parent;
        readonly List<Node> _children;

        public Node(Node? parent, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _parent = parent;
            _position = position;
            _rotation = rotation;
            _scale = scale;

            _children = new List<Node>();
        }

        public void AddChild(Node child)
        {
            _children.Add(child);
        }
    }
}