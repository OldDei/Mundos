namespace Mundos
{
    public static class Engine
    {
        public struct RuntimeSettings
        {
            public RuntimeSettings() {}

            public string title = "Mundos App";
            public bool debug = false;
            public int width = 800;
            public int height = 600;
        }
        public static RuntimeSettings? settings = null;

        public static Renderer renderer { get; }

        static Engine()
        {
            if (settings == null) settings = new RuntimeSettings();
            renderer = new Renderer(settings.Value.width, settings.Value.height, settings.Value.title);
            Console.WriteLine("Engine initialized.");
        }

        public static void Run()
        {
            Console.WriteLine("Engine running.");
            renderer.Run();
        }
    }
}