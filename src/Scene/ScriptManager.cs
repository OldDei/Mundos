using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Mundos {

    internal class ScriptManager {
        private List<Script> scripts = new List<Script>();

        public ScriptManager(string scriptsFolder) {
            // Compile all scripts in the scripts folder
            CompileScripts(scriptsFolder);

            // Read all scripts from the scripts folder and folders inside it
            var scriptFiles = Directory.GetFiles(scriptsFolder, "*.dll", SearchOption.AllDirectories);

        }

        private void CompileScripts(string scriptsFolder)
        {
            Console.WriteLine("Compiling scripts...");

            var scriptFiles = Directory.GetFiles(scriptsFolder, "*.cs", SearchOption.AllDirectories);
            foreach (var scriptFile in scriptFiles)
            {
                var script = CSharpScript.Create(File.ReadAllText(scriptFile));
                var compilation = script.GetCompilation();
                var result = compilation.Emit(Path.Combine(scriptsFolder, "Scripts.dll"));
                if (!result.Success)
                {
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }
                }
            }

            Console.WriteLine("Scripts compiled.");
        }

        public void AddScript(Script script) {
            script.MundosScriptRef.OnCreate();
            scripts.Add(script);
        }

        public void RemoveScript(Script script) {
            script.MundosScriptRef.OnDestroy();
            scripts.Remove(script);
        }

        public void UpdateScripts() {
            foreach (var script in scripts) {
                if (script.enabled) script.MundosScriptRef.OnUpdate();
            }
        }

        public void EnableScript(Script script) {
            script.MundosScriptRef.OnEnable();
            script.enabled = true;
        }

        public void DisableScript(Script script) {
            script.MundosScriptRef.OnDisable();
            script.enabled = false;
        }
    }
}