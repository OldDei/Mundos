using OpenTK.Mathematics;

namespace Mundos {

    internal static class ShaderManager {
        static List<Shader> shaders = new List<Shader>();

        static ShaderManager() {
            // Create default Red
            int shaderR = NewShader("res/Shaders/shaderDefault.vert", "res/Shaders/shaderDefault.frag");
            shaders[shaderR].SetVector4("defaultColor", new Vector4(0.5f, 0.2f, 0.2f, 1.0f));

            // Create default Green
            int shaderG = NewShader("res/Shaders/shaderDefault.vert", "res/Shaders/shaderDefault.frag");
            shaders[shaderG].SetVector4("defaultColor", new Vector4(0.2f, 0.8f, 0.2f, 1.0f));

            Console.WriteLine("ShaderManager initialized.");
        }

        internal static Shader GetShader(int id) {
            return shaders[id];
        }

        internal static int NewShader(string vertPath, string fragPath) {
            shaders.Add(new Shader(vertPath, fragPath));
            return shaders.Count - 1;
        }
    }
}