using OpenTK.Mathematics;

namespace Mundos
{
    public class SceneManager
    {
        readonly List<Scene> _scenes;

        public SceneManager()
        {
            _scenes = new List<Scene>();
            _scenes.Add(new Scene());
        }

        public void AddScene(Scene scene)
        {
            _scenes.Add(scene);
        }

        public void DestroyScene(Scene scene)
        {
            _scenes.Remove(scene);
        }

        public Scene? GetScene(int index)
        {
            if (index < 0 || index >= _scenes.Count)
                return null;
            return _scenes[index];
        }
    }

    public class Scene
    {
        readonly List<Node> _nodes;

        public Scene()
        {
            _nodes = new List<Node>();
            _nodes.Add(new Node(null, "Root Node", Vector3.Zero, Vector3.Zero, Vector3.One));
        }

        public void AddNode(Node node)
        {
            _nodes.Add(node);
        }

        public void DestroyNode(Node node)
        {
            _nodes.Remove(node);
        }

        /// <summary>
        /// Retrieves a node with the specified name.
        /// </summary>
        /// <param name="name">The name of the node to retrieve.</param>
        /// <returns>The node with the specified name, or null if no node is found.</returns>
        public Node? GetNode(string name)
        {
            foreach (Node node in _nodes)
            {
                if (node._name == name)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieves the node at the specified index. To get the root node, use index 0.
        /// </summary>
        /// <param name="index">The index of the node to retrieve.</param>
        /// <returns>The node at the specified index.</returns>
        public Node? GetNode(int index)
        {
            if (index < 0 || index >= _nodes.Count)
                return null;
            return _nodes[index];
        }

        /// <summary>
        /// Draws all the nodes in the scene.
        /// </summary>
        public void Draw(Renderer renderer)
        {
            foreach (Node node in _nodes)
            {
                node.Draw(renderer);
            }
        }
    }
}