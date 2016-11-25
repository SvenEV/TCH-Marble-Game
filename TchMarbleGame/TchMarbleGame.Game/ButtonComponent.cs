using System;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Xenko.Rendering.Materials;
using System.Diagnostics;

namespace TchMarbleGame
{
    /// <summary>
    /// Controls a button that can be physically pushed and released.
    /// </summary>
    public class ButtonComponent : AsyncScript
    {
        private bool _isPressed = false;

        public RigidbodyComponent MovablePartRigidbody { get; set; }

        public RigidbodyComponent GroundPlateRigidbody { get; set; }

        /// <summary>
        /// This event is raised when the button is pressed.
        /// </summary>
        public event Action<ButtonComponent> Pressed;

        /// <summary>
        /// This event is raised when the button is released.
        /// </summary>
        public event Action<ButtonComponent> Released;

        /// <summary>
        /// Indicates whether the button is currently pressed.
        /// </summary>
        public bool IsPressed
        {
            get { return _isPressed; }
            private set
            {
                if (value != _isPressed)
                {
                    _isPressed = value;
                    (_isPressed ? Pressed : Released)?.Invoke(this);

                    MovablePartRigidbody.Entity.Get<ModelComponent>()
                        .GetMaterial(0).Parameters.Set(MaterialKeys.DiffuseValue,
                            _isPressed ? Color.OrangeRed : Color.Red);

                    Debug.WriteLine(value);
                }
            }
        }

        public override async Task Execute()
        {
            // Set up a physical constraint so that the movable part of the button
            // can only be moved up and down
            var constraint = (Generic6DoFConstraint)Simulation.CreateConstraint(ConstraintTypes.Generic6DoF,
                GroundPlateRigidbody,
                MovablePartRigidbody,
                Matrix.Identity,
                Matrix.Identity,
                true);

            constraint.AngularLowerLimit = Vector3.Zero;
            constraint.AngularUpperLimit = Vector3.Zero;

            constraint.LinearLowerLimit = new Vector3(0, .02f, 0);
            constraint.LinearUpperLimit = new Vector3(0, .18f, 0);

            var physicsSimulation = this.GetSimulation();
            physicsSimulation.AddConstraint(constraint);

            Script.AddTask(HandleCollisions);
            Script.AddTask(HandleEndOfCollisions);

            while (Game.IsRunning)
            {
                MovablePartRigidbody.Entity.Transform.Rotation = Quaternion.Identity;
                await Script.NextFrame();
            }
        }

        private async Task HandleCollisions()
        {
            while (Game.IsRunning)
            {
                var collision = await GroundPlateRigidbody.NewCollision();

                if (collision.ColliderA == MovablePartRigidbody ||
                    collision.ColliderB == MovablePartRigidbody)
                {
                    IsPressed = true;
                }
            }
        }

        private async Task HandleEndOfCollisions()
        {
            while (Game.IsRunning)
            {
                var collision = await GroundPlateRigidbody.CollisionEnded();

                if (collision.ColliderA == MovablePartRigidbody ||
                    collision.ColliderB == MovablePartRigidbody)
                {
                    IsPressed = false;
                }
            }
        }
    }
}
