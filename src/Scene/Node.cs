using OpenTK.Mathematics;

namespace Mundos
{
    public class Node
    {
        public string _name;

        Vector3 _position;
        Vector3 _rotation;
        Vector3 _scale;

        readonly Node? _parent;
        readonly List<Node> _children;

        public Node(Node? parent, string name, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _parent = parent;
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _name = name;

            _children = new List<Node>();
        }

        public void AddChild(Node child)
        {
            _children.Add(child);
        }

        public void DestroyChild(Node child)
        {
            _children.Remove(child);
        }

        /// <summary>
        /// Draws the node and its children.
        /// </summary>
        virtual public void Draw(Renderer renderer)
        {
            // If the note has anything to draw, it should be implemented here

            foreach (Node child in _children)
            {
                child.Draw(renderer); // Draw the children
            }
        }
    }
}