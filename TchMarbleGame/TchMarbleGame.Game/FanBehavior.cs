using System.Collections.Generic;
using System.Threading.Tasks;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Core.Mathematics;

namespace TchMarbleGame
{
    public class FanBehavior : SyncScript
    {
        private readonly HashSet<RigidbodyComponent> _entitiesInRange = new HashSet<RigidbodyComponent>();

        /// <summary>
        /// The collider that describes the shape in which the fan blows entities away.
        /// </summary>
        public StaticColliderComponent ImpactArea { get; set; }

        /// <summary>
        /// The force with which the fan blows entities away.
        /// </summary>
        public float AppliedForce { get; set; } = 3;

        /// <summary>
        /// Use this to activate and deactivate the fan.
        /// </summary>
        public bool IsEnabled { get; set; }

        public override void Start()
        {
            Script.AddTask(HandleNewCollisions);
            Script.AddTask(HandleEndOfCollisions);
        }

        private async Task HandleEndOfCollisions()
        {
            while (Game.IsRunning)
            {
                var collision = await ImpactArea.CollisionEnded();
                var collider =
                    collision.ColliderA as RigidbodyComponent ??
                    collision.ColliderB as RigidbodyComponent;

                _entitiesInRange.Remove(collider);
            }
        }

        private async Task HandleNewCollisions()
        {
            while (Game.IsRunning)
            {
                var collision = await ImpactArea.NewCollision();
                var collider =
                    collision.ColliderA as RigidbodyComponent ??
                    collision.ColliderB as RigidbodyComponent;

                _entitiesInRange.Add(collider);
            }
        }

        public override void Update()
        {
            if (!IsEnabled)
                return;

            // Calculate direction of fan
            var fanDirection = Vector3.UnitY;
            Vector3 worldPosition, worldScale;
            Quaternion worldRotation;
            ImpactArea.Entity.Transform.WorldMatrix.Decompose(out worldPosition, out worldRotation, out worldScale);
            worldRotation.Rotate(ref fanDirection);

            foreach (var rigidbody in _entitiesInRange)
            {
                var distance = Vector3.Distance(Entity.Transform.Position, rigidbody.Entity.Transform.Position);
                var force = AppliedForce * (10 - distance);
                rigidbody.ApplyForce(force * fanDirection);
            }

            Entity.Get<RigidbodyComponent>().ApplyTorque(fanDirection);
        }
    }
}
