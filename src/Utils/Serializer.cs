using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Core.Events;
using Arch.Core;
using Arch.Core.Extensions;
using System.ComponentModel;
using Arch.Core.Utils;
using System.Reflection;

namespace Mundos
{
    public static class Serializer
    {
        /// <summary>
        /// Saves the specified <see cref="World"/> to a file.
        /// </summary>
        /// <param name="v">The file path where the world will be saved.</param>
        /// <param name="world">The world object to be saved.</param>
        /// <returns>True if the <see cref="World"/> was successfully saved; otherwise, false.</returns>
        public static bool SaveWorld(string v, World world)
        {
            // Open the file at the specified path
            TextWriter file = File.CreateText(v);

            // Create a new instance of the YamlSerializer
            var serializer = new SerializerBuilder().Build();
            List<Dictionary<string, object>> entitiesData = new List<Dictionary<string, object>>();

            foreach (Entity entity in EntityManager.Entities.Values)
            {
                if (entity == EntityManager.Root) continue; // Skip the root node

                Dictionary<string, object> entityData = new Dictionary<string, object>
                {
                    { "Name", EntityManager.EntityNames[entity] }
                };

                object[] components = entity.GetAllComponents();
                foreach (object component in components)
                {
                    Dictionary<string, object> componentData = new Dictionary<string, object>(); // some components have more than one field
                    switch (component.GetType().Name)
                    {
                        case "UUID":
                            entityData.Add("UUID", ((UUID)component).UniversalUniqueID.ToString());
                            break;
                        case "Position":
                            entityData.Add("Position", ((Position)component).position.X.ToString() + ";" + ((Position)component).position.Y.ToString() + ";" + ((Position)component).position.Z.ToString());
                            break;
                        case "Rotation":
                            entityData.Add("Rotation", ((Rotation)component).rotation.X.ToString() + ";" + ((Rotation)component).rotation.Y.ToString() + ";" + ((Rotation)component).rotation.Z.ToString());
                            break;
                        case "Scale":
                            entityData.Add("Scale", ((Scale)component).scale.X.ToString() + ";" + ((Scale)component).scale.Y.ToString() + ";" + ((Scale)component).scale.Z.ToString());
                            break;
                        case "Mesh":
                            componentData.Add("Mesh", ((Mesh)component).meshIndex);
                            componentData.Add("Shader", ((Mesh)component).shaderIndex);
                            entityData.Add("Mesh", componentData);
                            break;
                        case "Script":
                            string? scriptName = ((Script)component).MundosScriptRef.GetType().Name;
                            if (scriptName != null)
                                entityData.Add("Script", scriptName);
                            else
                                entityData.Add("Script", "null");
                            break;
                    }
                }

                entitiesData.Add(entityData);
            }

            // Serialize the list of entities to a YAML string
            string yaml = serializer.Serialize(entitiesData);
            file.WriteLine(yaml);

            // Close the file
            file.Close();

            return false;
        }

        /// <summary>
        /// Loads a <see cref="World"/> from the specified file.
        /// </summary>
        /// <param name="v">The file path from where the world will be loaded.</param>
        /// <returns>The <see cref="World"/> object that was loaded from the file.</returns>
        public static World LoadWorld(string v)
        {
            World newWorld = World.Create();

            // Open the file at the specified path
            TextReader file = File.OpenText(v);

            // Create a new instance of the YamlDeserializer
            var deserializer = new DeserializerBuilder().Build();

            // Deserialize the YAML string to a list of entities
            List<Dictionary<string, object>> entitiesData = deserializer.Deserialize<List<Dictionary<string, object>>>(file.ReadToEnd());

            // Close the file
            file.Close();

            // Create the entities from the data
            foreach (Dictionary<string, object> entityData in entitiesData)
            {
                Entity entity = newWorld.Create();

                foreach (KeyValuePair<string, object> componentData in entityData)
                {
                    switch (componentData.Key)
                    {
                        case "UUID":
                            entity.Add(new UUID(entity, Guid.Parse((string)componentData.Value)));
                            break;
                        case "Position":
                            string[] position = ((string)componentData.Value).Split(';');
                            entity.Add(new Position(entity, float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2])));
                            break;
                        case "Rotation":
                            string[] rotation = ((string)componentData.Value).Split(';');
                            entity.Add(new Rotation(entity, float.Parse(rotation[0]), float.Parse(rotation[1]), float.Parse(rotation[2])));
                            break;
                        case "Scale":
                            string[] scale = ((string)componentData.Value).Split(';');
                            entity.Add(new Scale(entity, float.Parse(scale[0]), float.Parse(scale[1]), float.Parse(scale[2])));
                            break;
                        case "Mesh":
                            Dictionary<string, object> meshData = (Dictionary<string, object>)componentData.Value;
                            entity.Add(new Mesh(entity, (int)meshData["Mesh"], (int)meshData["Shader"]));
                            break;
                        case "Script":
                            if ((string)componentData.Value != "null") {
                                Type? scriptType = Type.GetType("Mundos." + (string)componentData.Value);
                                TypeCode typeCode = Type.GetTypeCode(scriptType);
                                // See if the Script manager knows about this script
                                // TODO: make a script manager

                            }
                            break;
                    }
                }
            }

            return newWorld;
        }
    }
}
