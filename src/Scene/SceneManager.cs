using OpenTK.Mathematics;

namespace Mundos
{
    public static class SceneManager
    {
        readonly static List<Scene> _scenes;
        static Node _selectedNode;

        static SceneManager()
        {
            _scenes = new List<Scene>();
            _scenes.Add(new Scene());
            _selectedNode = _scenes[0].GetRootNode();
        }

        public static void AddScene(Scene scene)
        {
            _scenes.Add(scene);
        }

        public static void DestroyScene(Scene scene)
        {
            _scenes.Remove(scene);
        }

        public static Scene? GetScene(int index)
        {
            if (index < 0 || index >= _scenes.Count)
                return null;
            return _scenes[index];
        }

        public static Node GetSelectedNode()
        {
            return _selectedNode;
        }

        public static void SetSelectedNode(Node node)
        {
            _selectedNode = node;
        }

        /// <summary>
        /// Loads a scene from the specified path.
        /// TODO: Actually implement this
        /// </summary>
        /// <param name="path">The path of the scene to load.</param>
        internal static void LoadScene(string path)
        {
            Scene? scene = GetScene(0); // Create a scene object

            if (scene == null)
                return;

            scene.AddNode(new Model(scene.GetRootNode(), "Model Test Node 1", Vector3.Zero, Vector3.Zero, Vector3.One)); // Create a model object
            scene.AddNode(new Model(scene.GetRootNode(), "Model Test Node 2", Vector3.Zero, Vector3.Zero, Vector3.One)); // Create a model object
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
        /// TODO: Actually implement this
        /// </summary>
        /// <param name="name">The name of the node to retrieve.</param>
        /// <returns>The node with the specified name, or null if no node is found.</returns>
        public Node? GetNode(string name)
        {
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

        /// <summary>
        /// Draws the scene tree by calling the DrawTreeNode method on the root node.
        /// The Draw call will then propagate to all the children of the root node.
        /// </summary>
        public void DrawSceneTree()
        {
            _rootNode.DrawTreeNode();
        }
    }
}