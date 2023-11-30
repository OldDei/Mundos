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
    }

    public class Scene
    {
        readonly List<Node> _nodes;

        public Scene()
        {
            _nodes = new List<Node>();
            _nodes.Add(new Node(null, Vector3.Zero, Vector3.Zero, Vector3.One));
        }
    }
}