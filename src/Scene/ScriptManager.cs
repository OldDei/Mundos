using System.Reflection;
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
                var script_instance = (MundosScript?)Activator.CreateInstance(script_types[script]);
                if (script_instance == null) {
                    Log.Error($"ScriptManager: Failed to create instance of script {script}");
                    return;
                }
                Script script_ref = new Script(entity, script_instance);
                entity.Add(script_ref);
                Log.Debug($"ScriptManager: Added script {script} to entity {entity.Get<UUID>().UniversalUniqueID}");
                Log.Debug($"ScriptManager: Added script {script} to entity {entity.Id}");
            } else {
                Log.Error($"ScriptManager: Script {script} not found");
            }
        }

        public static void UpdateScripts() {
            var queryDesc = new QueryDescription().WithAll<Script>();

            foreach(Chunk chunk in WorldManager.World.Query(queryDesc))
            {
                Script[] scripts = chunk.GetArray<Script>();

                Parallel.For(0, chunk.Size, i => {if (scripts[i].enabled) scripts[i].MundosScriptRef.Update();});
            }
        }
    }
}