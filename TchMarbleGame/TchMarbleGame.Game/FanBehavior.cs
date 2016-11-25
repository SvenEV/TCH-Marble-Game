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
            foreach (var rigidbody in _entitiesInRange)
            {
                var distance = Vector3.Distance(Entity.Transform.Position, rigidbody.Entity.Transform.Position);
                var force = 3 * (10 - distance);
                rigidbody.ApplyForce(force * Vector3.UnitY);
            }

            Entity.Transform.Rotation *= Quaternion.RotationY(1);
        }
    }
}
