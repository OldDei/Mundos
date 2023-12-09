using OpenTK.Mathematics;

namespace Mundos {

    internal static class ShaderManager {
        static List<Shader> shaders = new List<Shader>();

        static ShaderManager() {
            // Create default shader at index 0
            shaders.Add(new Shader("src/Renderer/Shaders/shaderDefault.vert", "src/Renderer/Shaders/shaderDefault.frag"));

            // Set default color for default shader
            Vector4 defaultColorR = new Vector4(0.5f, 0.2f, 0.2f, 1.0f);
            shaders[0].SetVector4("defaultColor", defaultColorR);

            // Create default shader at index 0
            shaders.Add(new Shader("src/Renderer/Shaders/shaderDefault.vert", "src/Renderer/Shaders/shaderDefault.frag"));

            // Set default color for default shader
            Vector4 defaultColorG = new Vector4(0.2f, 0.8f, 0.2f, 1.0f);
            shaders[1].SetVector4("defaultColor", defaultColorG);
        }

        internal static Shader GetShader(int id) {
            return shaders[id];
        }

        internal static void NewShader(string vertPath, string fragPath) {
            shaders.Add(new Shader(vertPath, fragPath));
        }
    }
}