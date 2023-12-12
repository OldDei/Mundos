using Arch.Core;
using Arch.Core.Extensions;
using ImGuiNET;

namespace Mundos {
    public static class ImGuiWindows {
        private static Entity? lastSelectedEntity = null;
        private static ImGuiTreeNodeFlags baseFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.DefaultOpen;

        public static void DrawComponentWindow() {
            ImGui.Begin("Components");
            if (lastSelectedEntity != null)
            {
                Entity entity = (Entity)lastSelectedEntity;
                ImGui.Text($"Entity: {EntityManager.EntityNames[entity]}");
                ImGui.Separator();

                object[] components = WorldManager.World.GetAllComponents(entity);

                foreach (object component in components)
                {
                    ImGui.Text($"{component.GetType().Name}");
                    DrawComponent(component, entity);
                }
            }
            else
                ImGui.Text("No entity selected.");

            ImGui.End();
        }

        private static void DrawComponent(object component, Entity entity)
        {
            if (component.GetType() == typeof(Position))
            {
                ImGui.Indent();
                Position Position = (Position)component;
                System.Numerics.Vector3 vec = new System.Numerics.Vector3(Position.position.X, Position.position.Y, Position.position.Z);
                ImGui.DragFloat3("Position", ref vec, 0.1f);
                ImGui.Unindent();
                if (vec != new System.Numerics.Vector3(Position.position.X, Position.position.Y, Position.position.Z))
                {
                    Position.position.X = vec.X;
                    Position.position.Y = vec.Y;
                    Position.position.Z = vec.Z;
                    entity.Set(Position);
                }
            }

            if (component.GetType() == typeof(Rotation))
            {
                ImGui.Indent();
                Rotation Rotation = (Rotation)component;
                System.Numerics.Vector3 vec = new System.Numerics.Vector3(Rotation.rotation.X, Rotation.rotation.Y, Rotation.rotation.Z);
                ImGui.DragFloat3("Rotation", ref vec, 0.1f);
                ImGui.Unindent();
                if (vec != new System.Numerics.Vector3(Rotation.rotation.X, Rotation.rotation.Y, Rotation.rotation.Z))
                {
                    Rotation.rotation.X = vec.X;
                    Rotation.rotation.Y = vec.Y;
                    Rotation.rotation.Z = vec.Z;
                    entity.Set(Rotation);
                }
            }

            if (component.GetType() == typeof(Scale))
            {
                ImGui.Indent();
                Scale Scale = (Scale)component;
                System.Numerics.Vector3 vec = new System.Numerics.Vector3(Scale.scale.X, Scale.scale.Y, Scale.scale.Z);
                ImGui.DragFloat3("Scale", ref vec, 0.1f);
                ImGui.Unindent();
                if (vec != new System.Numerics.Vector3(Scale.scale.X, Scale.scale.Y, Scale.scale.Z))
                {
                    Scale.scale.X = vec.X;
                    Scale.scale.Y = vec.Y;
                    Scale.scale.Z = vec.Z;
                    entity.Set(Scale);
                }
            }

            if (component.GetType() == typeof(Mesh))
            {
                ImGui.Indent();
                Mesh Mesh = (Mesh)component;
                // TODO: Make this a dropdown menu with all available meshes and shaders
                ImGui.InputInt("Mesh ID", ref Mesh.meshIndex);
                ImGui.InputInt("Shader ID", ref Mesh.shaderIndex);
                ImGui.Unindent();
                if (Mesh.meshIndex != ((Mesh)component).meshIndex || Mesh.shaderIndex != ((Mesh)component).shaderIndex)
                {
                    entity.Set(Mesh);
                }
            }

            if (component.GetType() == typeof(Camera))
            {
                ImGui.Indent();
                Camera Camera = (Camera)component;
                float Fov = Camera.Fov;
                float Yaw = Camera.Yaw;
                float Pitch = Camera.Pitch;
                bool Locked = Camera.Locked;
                ImGui.DragFloat("Fov", ref Fov, 0.1f);
                ImGui.Checkbox("Locked", ref Locked);
                ImGui.Unindent();
                if (Fov != ((Camera)component).Fov || Locked != ((Camera)component).Locked)
                {
                    Camera.Fov = Fov;
                    Camera.Locked = Locked;
                    entity.Set(Camera);
                }
            }

            if (component.GetType() == typeof(Script))
            {
                ImGui.Indent();
                Script Script = (Script)component;
                ImGui.Text($"Script: {Script.MundosScriptRef.GetType().Name}");
                ImGui.Unindent();
            }
        }

        public static void DrawEntityTreeWindow() {
            ImGui.Begin("Debug");
            double fps = Math.Round(FPSAverage());
            ImGui.Text($"FPS: {fps}");
            ImGui.Separator();

            // World management buttons
            if (ImGui.Button("Save world"))
            {
                WorldManager.SaveWorld("world.xml");
            }
            ImGui.SameLine();
            if (ImGui.Button("Load world"))
            {
                WorldManager.LoadWorld("world.xml");
            }

            // World tree view tab
            if (ImGui.BeginTabBar("Mundos"))
            {
                ImGui.BeginTabItem("World");
                {
                    if (WorldManager.World != null)
                    {
                        if (ImGui.TreeNodeEx(EntityManager.EntityNames[EntityManager.Root], baseFlags))
                        {
                            DrawChildren(EntityManager.Root); // Draw the children of the root node
                            ImGui.TreePop();
                        }
                    }
                    else
                        ImGui.Text("No world loaded.");
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }

            ImGui.End();

            // Store current frame time for FPS calculation
            _frameTimes[_frameIndex] = Time.deltaTime;
            _frameIndex = (_frameIndex + 1) % MaxFrameCount;
        }

        private static void DrawChildren(Entity entity)
        {
            if (!EntityManager.EntityChildren.ContainsKey(entity)) return; // If this entity doesn't have any children, return

            foreach (Entity child in EntityManager.EntityChildren[entity]) // Iterate through all children of this entity
            {
                bool selected = false;
                if (child == lastSelectedEntity) selected = true;
                if (ImGui.TreeNodeEx(EntityManager.EntityNames[child], baseFlags | (selected ? ImGuiTreeNodeFlags.Selected : 0)))
                {
                    if (ImGui.IsItemClicked()) lastSelectedEntity = child; // If this entity is clicked, select it
                    DrawChildren(child); // Draw the children of this child
                    ImGui.TreePop();
                }
            }
        }

        private static int MaxFrameCount = 120; // Number of frames to consider for average calculation
        private static double[] _frameTimes = new double[MaxFrameCount];
        private static int _frameIndex = 0;
        private static double FPSAverage()
        {
            double frameTimeSum = 0;
            int frameCount = Math.Min(_frameIndex, MaxFrameCount);
            for (int i = 0; i < frameCount; i++)
            {
                frameTimeSum += _frameTimes[i];
            }
            double averageFrameTime = frameTimeSum / frameCount;
            double averageFPS = 1 / averageFrameTime;
            return averageFPS;
        }
    }
}