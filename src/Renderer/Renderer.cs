using Arch.Core;
using Arch.Core.Extensions;
using CommunityToolkit.HighPerformance;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Mundos
{
    internal class Renderer : GameWindow
    {
        private double _deltaTime;
        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private Shader _shaderDefault;
        private World _world;
        ImGuiController _controller;

        public Renderer(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            _world = WorldManager.World;

            // Create a camera object at the origin TODO: only do this if there's no camera in the world already
            Entity camera = EntityManager.Create(EntityManager.ArchetypeType.Camera);
            camera.Set(new Camera(camera.Id, new Vector3(0.0f, 0.0f, 3.0f), Size.X / (float)Size.Y, true), new Position(camera.Id, 0, 0, 0), new Rotation(camera.Id, 0, 0, 0), new Scale(camera.Id, 1, 1, 1));
            camera.Set(new Script(camera.Id, new CameraMove()));

            _shaderDefault = new Shader(); // Create a default shader object
            _deltaTime = 0.0; // Time between current frame and last frame

            _controller = new ImGuiController(width, height);

            Debug.WriteLine("Renderer initialized.");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Set the clear color to a dark grey
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            Vector3 defaultColor = new Vector3(0.5f, 0.2f, 0.2f);
            // _shaderDefault.SetVector3("defaultColor", defaultColor);

            // We enable depth testing here
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // update delta time
            _deltaTime = e.Time;
            _controller.Update(this, (float)e.Time);

            // Input update
            Input.UpdateKeyboardState(this.KeyboardState);

            // World update
            UpdateWorld();

            // Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Draw the world
            DrawWorld();

            // ImGui update
            // UpdateImGui();
            _controller.Render();

            ImGuiController.CheckGLError("End of frame");

            // Swap the front and back buffers of the window
            SwapBuffers();
        }

        /// <summary>
        /// Updates the ImGui user interface.
        /// TODO: This should probably be moved to a separate class.
        /// </summary>
        private void UpdateImGui()
        {
            // Debug window
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
                _world = WorldManager.World;
            }

            // World tree view tab
            if (ImGui.BeginTabBar("Mundos"))
            {
                ImGui.BeginTabItem("World");
                {
                    if (_world != null){}
                        // _world.DrawWorldTree();
                    else
                        ImGui.Text("No world loaded.");
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }

            ImGui.End();

            // Store current frame time
            _frameTimes[_frameIndex] = _deltaTime;
            _frameIndex = (_frameIndex + 1) % MaxFrameCount;
        }

        private const int MaxFrameCount = 120; // Number of frames to consider for average calculation
        private double[] _frameTimes = new double[MaxFrameCount];
        private int _frameIndex = 0;
        private double FPSAverage()
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

        /// <summary>
        /// Draws the current world.
        /// If no world is loaded, nothing is drawn.
        /// </summary>
        private void DrawWorld()
        {
            var queryDesc = new QueryDescription().WithAll<Position, Rotation, Scale, Mesh>();
            foreach(Chunk chunk in WorldManager.World.Query(queryDesc))
            {
                Position[]  positions = chunk.GetArray<Position>();
                Rotation[]  rotations = chunk.GetArray<Rotation>();
                Scale[]     scales = chunk.GetArray<Scale>();
                Mesh[]      meshes = chunk.GetArray<Mesh>();


                Matrix4 model = Matrix4.Identity;
                Vector3 _position;
                Vector3 _rotation;
                Vector3 _scale;

                foreach (int i in chunk)
                {
                    _position = positions[i].position;
                    _rotation = rotations[i].rotation;
                    _scale = scales[i].scale;

                    model *= Matrix4.CreateScale(_scale.X, _scale.Y, _scale.Z);
                    model *= Matrix4.CreateRotationX(_rotation.X);
                    model *= Matrix4.CreateRotationY(_rotation.Y);
                    model *= Matrix4.CreateRotationZ(_rotation.Z);
                    model *= Matrix4.CreateTranslation(_position.X, _position.Y, _position.Z);

                    DrawMesh(meshes[i], model); // TODO: Instead submit the mesh to the renderer and let it draw it
                }
            }
        }

        private void UpdateWorld()
        {
            var queryDesc = new QueryDescription().WithAll<Script>();
            foreach(Chunk chunk in WorldManager.World.Query(queryDesc))
            {
                Script[] scripts = chunk.GetArray<Script>();
                Parallel.For(0, chunk.Size, i => scripts[i].MundosScriptRef.OnUpdate());
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            WorldManager.GetActiveCamera(out Camera? camera);
            if (camera != null)
                camera.AspectRatio = Size.X / (float)Size.Y;

            _controller.WindowResized(ClientSize.X, ClientSize.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);

            _controller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _controller.MouseScroll(e.Offset);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vertexArrayObject);
        }

        /// <summary>
        /// Draws a mesh on the screen.
        /// </summary>
        /// <param name="mesh">The mesh to be drawn.</param>
        public void DrawMesh(Mesh mesh, Matrix4 modelMatrix)
        {
            MeshManager.GetMesh(mesh.meshIndex, out float[] vertices, out float[] normals, out float[] texCoords, out uint[] indices);

            // Get the data from the model
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            WorldManager.GetActiveCamera(out Camera? camera); // Get the active camera from the world
            if (camera == null) // If there's no camera, we can't draw anything
                return;

            _shaderDefault.SetMatrix4("model", modelMatrix);
            _shaderDefault.SetMatrix4("view", camera.GetViewMatrix());
            _shaderDefault.SetMatrix4("projection", camera.GetProjectionMatrixPerspective());
            _shaderDefault.Use();

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }
    }
}