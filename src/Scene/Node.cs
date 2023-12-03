using ImGuiNET;
using OpenTK.Mathematics;

namespace Mundos
{
    public class Node
    {
        public string _name;

        System.Numerics.Vector3 _position;
        System.Numerics.Vector3 _rotation;
        System.Numerics.Vector3 _scale;

        readonly Node? _parent;
        readonly List<Node> _children;

        public Node(Node? parent, string name, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _parent = parent;
            _position = new System.Numerics.Vector3(position.X, position.Y, position.Z);
            _rotation = new System.Numerics.Vector3(rotation.X, rotation.Y, rotation.Z);
            _scale = new System.Numerics.Vector3(scale.X, scale.Y, scale.Z);
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

        virtual public void DrawTreeNode()
        {
            if (ImGui.TreeNode(_name))
            {
                // Every node has a name, position, rotation, and scale
                ImGui.DragFloat3("Position", ref _position);
                ImGui.DragFloat3("Rotation", ref _rotation);
                ImGui.DragFloat3("Scale",    ref _scale);

                foreach (Node child in _children)
                {
                    child.DrawTreeNode();
                }
                ImGui.TreePop();
            }
        }
    }
}