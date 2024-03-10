using System.Reflection;
using System.Runtime.CompilerServices;
using Arch.Core;
using Arch.Core.Extensions;

namespace Mundos {

    public static class ScriptManager {
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
            if (script_types != null && script_types.ContainsKey(script)) {
                MundosScript? script_instance = (MundosScript?)Activator.CreateInstance(script_types[script]);
                if (script_instance == null) {
                    Log.Error($"ScriptManager: Failed to create instance of script {script}");
                    return;
                }
                entity.Add(new Script(entity, script_instance));
            } else {
                Log.Error($"ScriptManager: Script {script} not found");
            }
        }

        public static void UpdateScripts() {
            QueryDescription queryDesc = new QueryDescription().WithAll<Script>();
            foreach (var chunk in WorldManager.World.Query(queryDesc)) {
                var scriptArray = chunk.GetArray<Script>();
                foreach (var script in scriptArray) {
                    if (script.enabled) {
                        script.MundosScriptRef.Update();
                    }
                }
            }
        }
    }
}