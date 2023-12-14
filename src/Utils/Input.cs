using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Mundos
{
    public static class Input
    {
        private static KeyboardState? input; // keybaord state from renderer
        private static MouseState? mouse; // mouse state from renderer

        public static bool IsKeyDown(Keys key)
        {
            if (input == null)
                return false;
            if (input.IsAnyKeyDown)
                return input.IsKeyDown(key);
            return false;
        }

        public static bool IsKeyPressed(Keys key)
        {
            if (input == null)
                return false;
            if (input.IsAnyKeyDown)
                return input.IsKeyPressed(key);
            return false;
        }

        internal static void UpdateState(KeyboardState input, MouseState mouse)
        {
            Input.input = input;
            Input.mouse = mouse;
        }

        public static Vector2 MouseMoveDelta()
        {
            if (mouse == null)
                return Vector2.Zero;
            return mouse.Delta;
        }
    }
}