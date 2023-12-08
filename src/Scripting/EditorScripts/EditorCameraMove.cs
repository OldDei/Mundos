using System.Threading.Tasks.Dataflow;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Mundos {
    public class CameraMove : MundosScript {
        public override void OnUpdate() {

            // float speed = 0.8f;
            Console.WriteLine("Position of camera is: " + position);

            // Update camera position based on input
            if (Input.IsKeyDown(Keys.W)) {
            }
            if (Input.IsKeyDown(Keys.S)) {
            }
            if (Input.IsKeyDown(Keys.A)) {
            }
            if (Input.IsKeyDown(Keys.D)) {
            }
            if (Input.IsKeyDown(Keys.Space)) {
            }
            if (Input.IsKeyDown(Keys.LeftControl)) {
            }
            if (Input.IsKeyDown(Keys.LeftShift)) {
                // TODO: Increase camera speed
            }
        }
    }
}