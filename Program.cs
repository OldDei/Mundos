using System;
using OpenTK.Graphics.ES20;
using OpenTK.Mathematics;

namespace Mundos
{
    class Program
    {
        static void Main(string[] args)
        {
            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (Renderer renderer = new Renderer(1366, 768, "Mundos"))
            {
                renderer.SetScene(SceneManager.GetScene(0));
                renderer.Run();
            }
        }
    }
}