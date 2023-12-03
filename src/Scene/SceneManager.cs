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
        Node _rootNode;

        public Scene()
        {
            _rootNode = new Node(null, "Root Node", Vector3.Zero, Vector3.Zero, Vector3.One);
        }

        public void AddNode(Node node)
        {
            _rootNode.AddChild(node);
        }

        /// <summary>
        /// Retrieves a node with the specified name.
        /// </summary>
        /// <param name="name">The name of the node to retrieve.</param>
        /// <returns>The node with the specified name, or null if no node is found.</returns>
        public Node? GetNode(string name)
        {
            // TODO: Implement this

            return null;
        }

        public Node GetRootNode()
        {
            return _rootNode;
        }

        /// <summary>
        /// Draws all the nodes in the scene.
        /// </summary>
        public void Draw(Renderer renderer)
        {
            _rootNode.Draw(renderer);
        }

        public void DrawSceneTree()
        {
            _rootNode.DrawTreeNode();
        }
    }
}