using ImGuiNET;
using OpenTK.Mathematics;

namespace Mundos
{
    /// <summary>
    /// Represents a node in a scene hierarchy.
    /// </summary>
    public class Node
    {
        public string _name;

        public System.Numerics.Vector3 _position;
        public System.Numerics.Vector3 _rotation;
        public System.Numerics.Vector3 _scale;
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

        /// <summary>
        /// Adds a child node to the current node.
        /// </summary>
        /// <param name="child">The child node to add.</param>
        public void AddChild(Node child)
        {
            _children.Add(child);
        }

        /// <summary>
        /// Destroys a child node by removing it from the list of children.
        /// </summary>
        /// <param name="child">The child node to destroy.</param>
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

        /// <summary>
        /// Draws the tree node for the current node in the scene.
        /// TODO: Move to a separate class?
        /// </summary>
        virtual public void DrawTreeNode()
        {
            ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick | (SceneManager.GetSelectedNode() == this ? ImGuiTreeNodeFlags.Selected : 0);
            if (ImGui.TreeNodeEx(_name, nodeFlags))
            {
                // Every node has a name, position, rotation, and scale
                ImGui.DragFloat3("Position", ref _position);
                ImGui.DragFloat3("Rotation", ref _rotation);
                ImGui.DragFloat3("Scale",    ref _scale);

                if (ImGui.IsItemClicked()){
                    SceneManager.SetSelectedNode(this);
                }

                foreach (Node child in _children)
                {
                    child.DrawTreeNode();
                }
                ImGui.TreePop();
            }
        }

        internal Vector3 GetScale()
        {
            return new Vector3(_scale.X, _scale.Y, _scale.Z) + _parent?.GetScale() ?? Vector3.Zero;
        }

        internal Vector3 GetRotation()
        {
            return new Vector3(_rotation.X, _rotation.Y, _rotation.Z) + _parent?.GetRotation() ?? Vector3.Zero;
        }

        internal Vector3 GetPosition()
        {
            return new Vector3(_position.X, _position.Y, _position.Z) + _parent?.GetPosition() ?? Vector3.Zero;
        }
    }
}