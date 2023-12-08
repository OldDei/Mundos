using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Mundos
{
    public static class Input
    {
        private static KeyboardState? input; // keybaord state from renderer

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

        internal static void UpdateKeyboardState(KeyboardState input)
        {
            Input.input = input;
        }
    }
}