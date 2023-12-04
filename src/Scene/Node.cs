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
        private Scene _scene;
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
            ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick | (SceneManager.GetSelectedNode() == this ? ImGuiTreeNodeFlags.Selected : 0);
            if (ImGui.TreeNodeEx(_name, nodeFlags))
            {
                if (ImGui.IsItemClicked()){
                    SceneManager.SetSelectedNode(this);
                    Console.WriteLine("Selected node: " + _name);
                }

                foreach (Node child in _children)
                {
                    child.DrawTreeNode();
                }
                ImGui.TreePop();
            }
        }
    }
}