namespace Mundos
{
    class Program
    {
        static void Main(string[] args)
        {
            WorldManager.LoadWorld("world.xml");

            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (Renderer renderer = new Renderer(1920, 1080, "Mundos"))
            {
                renderer.Run();
            }
        }
    }
}