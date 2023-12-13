using System.Threading.Tasks.Dataflow;
using Arch.Core.Extensions;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

// TODO: Move this to the editor project
namespace Mundos {
    public class EditorCameraMove : MundosScript {
        float moveSpeed;
        float mouseSensitivity = 45f;

        public override void OnUpdate() {
            // Get the camera component
            Camera cam = EntityManager.GetEntity(entityID).Get<Camera>();

            // Increase move speed if shift is held
            if (Input.IsKeyDown(Keys.LeftShift)) {
                moveSpeed = 4f;
            }
            else {
                moveSpeed = 1f;
            }

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
                position += moveSpeed * Time.deltaTimef * Vector3.UnitY; // Up
            }
            if (Input.IsKeyDown(Keys.LeftControl)) {
                position += moveSpeed * Time.deltaTimef * -Vector3.UnitY; // Down
            }

            // Lock/unlock the camera
            if (Input.IsKeyPressed(Keys.R)) {
                cam.Locked = !cam.Locked;
            }

            // Only update rotation if the camera is locked to cursor (otherwise the mouse will jump when we unlock it)
            if (cam.Locked) {
                Vector2 mouseMoveDelta = Input.MouseMoveDelta();
                rotation.X += mouseMoveDelta.Y * mouseSensitivity * Time.deltaTimef; rotation.X = MathHelper.Clamp(rotation.X, -89f, 89f);
                rotation.Y += mouseMoveDelta.X * mouseSensitivity * Time.deltaTimef;
            }

            base.OnUpdate();
        }
    }
}