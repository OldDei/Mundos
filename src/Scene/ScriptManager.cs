using System.Reflection;
using Arch.Core;
using Arch.Core.Extensions;

namespace Mundos {

    public static class ScriptManager {
        private static Dictionary<string, Script> loaded_scripts = new Dictionary<string, Script>();
        public static Dictionary<string, Type> script_types = new Dictionary<string, Type>();

        static ScriptManager() {
            // Load all scripts from the application assembly
            var scripts = Util.GetInheritedClasses(typeof(MundosScript), Assembly.GetEntryAssembly());
            foreach (var script in scripts) {
                script_types.Add(script.Name, script);
                Console.WriteLine($"ScriptManager: Loaded script {script.Name}");
            }

            // Load all scripts from the core assembly
            scripts = Util.GetInheritedClasses(typeof(MundosScript), Assembly.GetExecutingAssembly());
            foreach (var script in scripts) {
                script_types.Add(script.Name, script);
                Console.WriteLine($"ScriptManager: Loaded script {script.Name}");
            }
        }

        public static void AddScript(Entity entity, MundosScript script) {
            var scriptComponent = new Script(entity, script);
            loaded_scripts.Add(script.GetType().Name, scriptComponent);
            entity.Set(scriptComponent);
        }

        public static void RemoveScript(Entity entity, Script script) {
            if (script.enabled) script.MundosScriptRef.OnDestroy();

            entity.Remove<Script>();

            loaded_scripts.Remove(script.MundosScriptRef.GetType().Name);
        }

        public static void UpdateScripts() {
            foreach (var script in loaded_scripts) {
                if (script.Value.enabled) {
                    script.Value.MundosScriptRef.OnUpdate();
                }
            }
        }

        public static void EnableScript(Script script) {
            script.MundosScriptRef.OnEnable();
            script.enabled = true;
        }

        public static void DisableScript(Script script) {
            script.MundosScriptRef.OnDisable();
            script.enabled = false;
        }
    }
}