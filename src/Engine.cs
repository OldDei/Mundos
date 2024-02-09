namespace Mundos
{
    public class Engine
    {
        public struct RuntimeSettings
        {
            public RuntimeSettings() {}

            public string title = "Mundos App";
            public bool debug = false;
            public bool vSync = true;
            public bool maximize = false;
            public int width = 800;
            public int height = 600;
            public string? scriptsFolder = null;
        }
        public RuntimeSettings settings;

        public Renderer renderer;

        internal ScriptManager scriptManager;

        public Engine(RuntimeSettings? settings)
        {
            if (settings == null) this.settings = new RuntimeSettings();
            else this.settings = settings.Value;
            renderer = new Renderer(this.settings.width, this.settings.height, this.settings.title);
            scriptManager = new ScriptManager(this.settings.scriptsFolder);
            renderer.SetVSync(true);
            renderer.MaximizeWindow(true);
            Console.WriteLine("Engine initialized.");
        }

        public void Run()
        {
            Console.WriteLine("Engine running.");
            renderer.Run();
        }
    }
}