using System;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Core.Mathematics;

namespace TchMarbleGame
{
    public class MarbleController : SyncScript
    {
        private RigidbodyComponent _rigidbody;

        public CameraComponent Camera { get; set; }

        public override void Start()
        {
            _rigidbody = Entity.Get<RigidbodyComponent>();
        }

        public override void Update()
        {
            var horizontal = 0f;
            var vertical = 0f;

            if (Input.IsKeyDown(Keys.Right)) horizontal--;
            if (Input.IsKeyDown(Keys.Left)) horizontal++;

            if (Input.IsKeyDown(Keys.Down)) vertical--;
            if (Input.IsKeyDown(Keys.Up)) vertical++;

            var cameraLookDirection = Vector3.UnitZ;
            Camera.Entity.Transform.Rotation.Rotate(ref cameraLookDirection);
            cameraLookDirection.Y = 0;
            cameraLookDirection.Normalize();

            var torqueHorizontal = cameraLookDirection * 10 * horizontal;
            var torqueVertical = Vector3.Cross(cameraLookDirection, Vector3.UnitY) * 10 * vertical;

            _rigidbody.ApplyTorque(torqueHorizontal + torqueVertical);
        }
    }
}
