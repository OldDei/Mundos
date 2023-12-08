using System.Threading.Tasks.Dataflow;
using Arch.Core.Extensions;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Mundos {
    public class CameraMove : MundosScript {
        public override void OnUpdate() {

            float moveSpeed = 1f;
            float mouseSensitivity = 30f;

            Camera cam = EntityManager.GetEntity(entityID).Get<Camera>();

            // Update camera position based on input
            if (Input.IsKeyDown(Keys.W)) {
                position += moveSpeed * Time.deltaTimef * cam.Front;
            }
            if (Input.IsKeyDown(Keys.S)) {
                position += moveSpeed * Time.deltaTimef * -cam.Front;
            }
            if (Input.IsKeyDown(Keys.D)) {
                position += moveSpeed * Time.deltaTimef * cam.Right;
            }
            if (Input.IsKeyDown(Keys.A)) {
                position += moveSpeed * Time.deltaTimef * -cam.Right;
            }
            if (Input.IsKeyDown(Keys.Space)) {
                position += moveSpeed * Time.deltaTimef * cam.Up;
            }
            if (Input.IsKeyDown(Keys.LeftControl)) {
                position += moveSpeed * Time.deltaTimef * -cam.Up;
            }
            if (Input.IsKeyDown(Keys.LeftShift)) {
                moveSpeed = 2f;
            }
            else {
                moveSpeed = 1f;
            }

            Vector2 mouseMoveDelta = Input.MouseMoveDelta();
            rotation.X += mouseMoveDelta.Y * mouseSensitivity * Time.deltaTimef; rotation.X = MathHelper.Clamp(rotation.X, -89f, 89f);
            rotation.Y += mouseMoveDelta.X * mouseSensitivity * Time.deltaTimef;

            base.OnUpdate();
        }
    }
}