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

        public Renderer(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            _deltaTime = 0.0; // Time between current frame and last frame
            _camera = new Camera(Vector3.Zero, Size.X / (float)Size.Y); // Create a camera object at the origin
            _shaderDefault = new Shader("src/Renderer/Shaders/shaderDefault.vert", "src/Renderer/Shaders/shaderDefault.frag"); // Create a default shader object
        }

        protected override void OnLoad()
        {
            base.OnLoad();

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _deltaTime += e.Time;

            // Physics update

            // Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            Vector3 defaultColor = new Vector3(0.5f, 0.2f, 0.2f);
            _shaderDefault.Use();
            _shaderDefault.SetVector3("defaultColor", defaultColor);

            var model = Matrix4.Identity;

            DrawScene();

            SwapBuffers();
        }

        private void DrawScene()
        {
            // throw new NotImplementedException();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            _camera.AspectRatio = Size.X / (float)Size.Y;
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

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vertexArrayObject);
        }
    }
}