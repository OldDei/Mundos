using System.Threading.Tasks.Dataflow;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Mundos {
    public class CameraMove : MundosScript {
        public override void OnUpdate() {

            float speed = 1f;
            Console.WriteLine("Position OnUpdate1: " + position);

            // Update camera position based on input
            if (Input.IsKeyDown(Keys.W)) {
                position.Z -= speed * Time.deltaTimef;
            }
            if (Input.IsKeyDown(Keys.S)) {
                position.Z += speed * Time.deltaTimef;
            }
            if (Input.IsKeyDown(Keys.A)) {
                position.X -= speed * Time.deltaTimef;
            }
            if (Input.IsKeyDown(Keys.D)) {
                position.X += speed * Time.deltaTimef;
            }
            if (Input.IsKeyDown(Keys.Space)) {
                position.Y += speed * Time.deltaTimef;
            }
            if (Input.IsKeyDown(Keys.LeftControl)) {
                position.Y -= speed * Time.deltaTimef;
            }
            if (Input.IsKeyDown(Keys.LeftShift)) {
                speed = 2f;
            }
            else {
                speed = 1f;
            }
            Console.WriteLine("Position OnUpdate2: " + position);
            base.OnUpdate();
            Console.WriteLine("Position OnUpdate3: " + position);
        }
    }
}