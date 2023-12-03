using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace Mundos
{
    public class Renderer : GameWindow
    {
        private double _deltaTime;
        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private Shader _shaderDefault;
        private Camera _camera;
        private Scene? _scene;
        ImGuiController _controller;

        public Renderer(int width, int height, string title, SceneManager sceneManager) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            _deltaTime = 0.0; // Time between current frame and last frame
            _camera = new Camera(new Vector3(0.0f, 0.0f, 3.0f), Size.X / (float)Size.Y); // Create a camera object at the origin
            _shaderDefault = new Shader(); // Create a default shader object

            LoadScene(sceneManager);

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

            // Physics update

            // Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Draw the scene
            DrawScene();

            // ImGui update
            UpdateImGui();
            _controller.Render();

            ImGuiController.CheckGLError("End of frame");

            // Swap the front and back buffers of the window
            SwapBuffers();
        }

        private void UpdateImGui()
        {
            // Debug window
            ImGui.Begin("Debug");
            double fps = Math.Round(FPSAverage());
            ImGui.Text($"FPS: {fps}");

            // Scene tree view tab
            if (ImGui.BeginTabBar("Mundos"))
            {
                ImGui.BeginTabItem("Scene");
                {
                    if (_scene != null)
                        _scene.DrawSceneTree();
                    else
                        ImGui.Text("No scene loaded.");
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

        private void DrawScene()
        {
            if (_scene != null)
                _scene.Draw(this);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            _camera.AspectRatio = Size.X / (float)Size.Y;

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

        public void DrawMesh(Mesh model)
        {
            // Data to retrieve
            // List<Mesh.Vertex> vertices;
            // List<uint> indices;
            // List<Texture> textures;

            // Get the data from the model
            // model.GetDrawData(out vertices, out indices, out textures);

            float[] vertices = {
                0.5f,  0.5f, 0.0f,  // top right
                0.5f, -0.5f, 0.0f,  // bottom right
                -0.5f, -0.5f, 0.0f,  // bottom left
                -0.5f,  0.5f, 0.0f   // top left
            };
            uint[] indices = {  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            _shaderDefault.SetMatrix4("model", Matrix4.Identity);
            _shaderDefault.SetMatrix4("view", _camera.GetViewMatrix());
            _shaderDefault.SetMatrix4("projection", _camera.GetProjectionMatrixPerspective());
            _shaderDefault.Use();

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        public void SetScene(Scene? scene)
        {
            _scene = scene;
        }

        private void LoadScene(SceneManager sceneManager)
        {
            _scene = sceneManager.GetScene(0); // Create a scene object

            if (_scene == null)
                return;

            _scene.AddNode(new Model(_scene.GetRootNode(), "Model Test Node", Vector3.Zero, Vector3.Zero, Vector3.One)); // Create a model object
        }
    }
}