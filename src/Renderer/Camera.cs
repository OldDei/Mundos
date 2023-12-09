using Arch.Core.Extensions;
using OpenTK.Mathematics;
using System;

// Camera class from https://learnopengl.com/Getting-started/Camera
namespace Mundos
{
    // This is the camera class as it could be set up after the tutorials on the website.
    // It is important to note there are a few ways you could have set up this camera.
    // For example, you could have also managed the player input inside the camera class,
    // and a lot of the properties could have been made into functions.

    // TL;DR: This is just one of many ways in which we could have set up the camera.
    // Check out the web version if you don't know why we are doing a specific thing or want to know more about the code.
    internal class Camera
    {
        public int entityID; // ID of entity this component is attached to

        // Those vectors are directions pointing outwards from the camera to define how it rotated.
        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;

        // Rotation around the X axis (radians)
        private float _pitch;

        // Rotation around the Y axis (radians)
        private float _yaw = -MathHelper.PiOver2; // Without this, you would be started rotated 90 degrees right.

        // The field of view of the camera (radians)
        private float _fov = MathHelper.DegreesToRadians(45f);
        public bool Locked = false;

        public Camera(int entityID, float aspectRatio, bool isPrimaryCamera = false)
        {
            this.AspectRatio = aspectRatio;
            this.entityID = entityID;
            if (isPrimaryCamera)
                WorldManager.SetActiveCamera(this);
        }

        // This is simply the aspect ratio of the viewport, used for the projection matrix.
        public float AspectRatio { private get; set; }

        public Vector3 Front => _front;

        public Vector3 Up => _up;

        public Vector3 Right => _right;

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                var angle = MathHelper.Clamp(value, (-89f), (89f));
                if (!Locked) _pitch = MathHelper.DegreesToRadians(angle);
            }
        }

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                if (!Locked) _yaw = MathHelper.DegreesToRadians(value);
            }
        }

        // The field of view (FOV) is the vertical angle of the camera view.
        // This has been discussed more in depth in a previous tutorial,
        // but in this tutorial, you have also learned how we can use this to simulate a zoom feature.
        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 90f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
        public Matrix4 GetViewMatrix()
        {
            Vector3 position = EntityManager.GetEntity(entityID).Get<Position>().position;
            UpdateVectors();
            return Matrix4.LookAt(position, position + _front, _up);
        }

        // Get the projection matrix using the same method we have used up until this point
        public Matrix4 GetProjectionMatrixPerspective()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        }

        public Matrix4 GetProjectionMatrixOrthographic()
        {
            return Matrix4.CreateOrthographic(AspectRatio * 2, 2, 0.01f, 100f);
        }

        // This function is going to update the direction vertices using some of the math learned in the web tutorials.
        private void UpdateVectors()
        {
            Vector3 rotation = EntityManager.GetEntity(entityID).Get<Rotation>().rotation;
            Pitch = rotation.X;
            Yaw = rotation.Y;

            // First, the front matrix is calculated using some basic trigonometry.
            _front.X = MathF.Cos(-_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(-_pitch);
            _front.Z = MathF.Cos(-_pitch) * MathF.Sin(_yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
            _front = Vector3.Normalize(_front);

            // Calculate both the right and the up vector using cross product.
            // Note that we are calculating the right from the global up; this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up =    Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }
}