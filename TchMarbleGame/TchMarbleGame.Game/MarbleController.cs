using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Core.Diagnostics;
using SiliconStudio.Xenko.Games;
using System;
using System.Diagnostics;
using SiliconStudio.Xenko.Rendering;

namespace TchMarbleGame
{
    public class MarbleController : SyncScript
    {
        private RigidbodyComponent _rigidbody;

        public CameraComponent Camera { get; set; }
        
        public override void Start()
        {
            base.Start();

            Profiler.Enable(GameProfilingKeys.GameDrawFPS);
            
            _rigidbody = Entity.Get<RigidbodyComponent>();

            VirtualButtonGroup b1, b2;
            
            Input.VirtualButtonConfigSet = new VirtualButtonConfigSet
            {
                new VirtualButtonConfig
                {
                    new VirtualButtonBinding("Horizontal", b1 = new VirtualButtonGroup
                    {
                        new VirtualButtonTwoWay(VirtualButton.Keyboard.Left, VirtualButton.Keyboard.Right),
                        VirtualButton.GamePad.LeftThumbAxisX
                    }),
                    new VirtualButtonBinding("Vertical", b2 = new VirtualButtonGroup
                    {
                        new VirtualButtonTwoWay(VirtualButton.Keyboard.Down, VirtualButton.Keyboard.Up),
                        VirtualButton.GamePad.LeftThumbAxisY
                    })
                }
            };

            b1.IsDisjunction = true;
            b2.IsDisjunction = true;
        }

        public override void Update()
        {
            var horizontal = -Input.GetVirtualButton(0, "Horizontal");
            var vertical = Input.GetVirtualButton(0, "Vertical");

            var cameraLookDirection = Vector3.UnitZ;
            Camera.Entity.Transform.Rotation.Rotate(ref cameraLookDirection);
            cameraLookDirection.Y = 0;
            cameraLookDirection.Normalize();

            var torqueHorizontal = cameraLookDirection * 2 * horizontal;
            var torqueVertical = Vector3.Cross(cameraLookDirection, Vector3.UnitY) * 2 * vertical;

            _rigidbody.ApplyTorque(torqueHorizontal + torqueVertical);
        }
    }
}
