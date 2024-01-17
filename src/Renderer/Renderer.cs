using Arch.Core;
using Arch.Core.Extensions;
using CommunityToolkit.HighPerformance;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace Mundos
{
    /// <summary>
    /// The Renderer class is responsible for rendering the game window and drawing the world.
    /// It inherits from the GameWindow class and provides methods for updating and rendering the game.
    /// </summary>
    public class Renderer : GameWindow
    {
        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private ImGuiController _controller;

        public Renderer(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            _controller = new ImGuiController(width, height);

            // Create a camera object at the origin TODO: Make a separate camera for the editor not in ECS
            // Entity camera = EntityManager.Create(EntityManager.ArchetypeType.Camera, "Editor Camera");
            // camera.Add(new Camera(camera.Id, Size.X / (float)Size.Y), new Position(camera.Id, 0, 0, 3f), new Rotation(camera.Id, 0, -90f, 0), new Scale(camera.Id, 1, 1, 1));

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

            // We enable depth testing here
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // update delta times
            Time.deltaTime = e.Time;
            Time.deltaTimef = (float)e.Time;
            _controller.Update(this, (float)e.Time);

            // Input update
            Input.UpdateState(this.KeyboardState, this.MouseState);

            // Camera lock
            EntityManager.GetActiveCamera(out Camera? camera);
            if (camera != null && camera.Locked)
            {
                CursorState = CursorState.Grabbed;
            }
            else
            {
                CursorState = CursorState.Normal;
            }

            // World update
            UpdateWorld();

            // Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Draw the world
            DrawWorld();

            // ImGui update
            UpdateImGui();
            _controller.Render();

            ImGuiController.CheckGLError("End of frame");

            // Swap the front and back buffers of the window
            SwapBuffers();
        }

        /// <summary>
        /// Updates registered ImGui functions.
        /// </summary>
        public void UpdateImGui() {
            foreach (Action function in _imGuiRenderFunctions)
            {
                function();
            }
        }

        private List<Action> _imGuiRenderFunctions = new List<Action>();
        public void RegisterImGuiRenderFunction(Action function)
        {
            _imGuiRenderFunctions.Add(function);
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

                foreach (int i in chunk)
                {
                    Matrix4 model = Matrix4.Identity;
                    Vector3 _rotation = WorldManager.GetEntityWorldRotation(chunk.Entities[i]);

                    model *= Matrix4.CreateScale(WorldManager.GetEntityWorldScale(chunk.Entities[i]));
                    model *= Matrix4.CreateRotationX(_rotation.X);
                    model *= Matrix4.CreateRotationY(_rotation.Y);
                    model *= Matrix4.CreateRotationZ(_rotation.Z);
                    model *= Matrix4.CreateTranslation(WorldManager.GetEntityWorldPosition(chunk.Entities[i]));

                    DrawMesh(meshes[i], model); // TODO: Instead submit the mesh to the renderer and let it draw it
                }
            }
        }

        private void UpdateWorld()
        {
            var queryDesc = new QueryDescription().WithAny<Script>();
            foreach(Chunk chunk in WorldManager.World.Query(queryDesc))
            {
                Script[] scripts = chunk.GetArray<Script>();
                Parallel.For(0, chunk.Size, i => scripts[i].MundosScriptRef.Update());
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            EntityManager.GetActiveCamera(out Camera? camera);
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

            EntityManager.GetActiveCamera(out Camera? camera); // Get the active camera from the world
            if (camera == null) // If there's no camera, we can't draw anything
                return;

            ShaderManager.GetShader(mesh.shaderIndex).SetMatrix4("model", modelMatrix);
            ShaderManager.GetShader(mesh.shaderIndex).SetMatrix4("view", camera.GetViewMatrix());
            ShaderManager.GetShader(mesh.shaderIndex).SetMatrix4("projection", camera.GetProjectionMatrixPerspective());
            ShaderManager.GetShader(mesh.shaderIndex).Use();

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        public void SetVSync(bool enabled)
        {
            if (enabled)
            {
                Context.SwapInterval = 1;
            }
            else
            {
                Context.SwapInterval = 0;
            }
        }
    }
}