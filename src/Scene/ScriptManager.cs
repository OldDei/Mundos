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
                Log.Debug($"ScriptManager: Loaded script {script.Name}");
            }

            // Load all scripts from the core assembly
            scripts = Util.GetInheritedClasses(typeof(MundosScript), Assembly.GetExecutingAssembly());
            foreach (var script in scripts) {
                script_types.Add(script.Name, script);
                Log.Debug($"ScriptManager: Loaded script {script.Name}");
            }

            Log.Info($"ScriptManager: Loaded {script_types.Count} scripts");
        }

        public static void AddScript(Entity entity, string script) {
            if (script_types.ContainsKey(script)) {
                AddScript(entity, (MundosScript)Activator.CreateInstance(script_types[script]));
            } else {
                Log.Error($"ScriptManager: Script {script} not found");
            }
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