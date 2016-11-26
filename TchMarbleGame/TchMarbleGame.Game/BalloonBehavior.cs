using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Core;

namespace TchMarbleGame
{
    /// <summary>
    /// Enables an entity to smoothly scale up and scale down in size,
    /// like inflating and deflating a balloon.
    /// </summary>
    public class BalloonBehavior : SyncScript
    {
        private Vector3 _initialScale;
        private ColliderShape _colliderShape;

        /// <summary>
        /// The time in seconds it takes for the entity to inflate or deflate.
        /// </summary>
        public float InflationDuration { get; set; }

        /// <summary>
        /// The factor by which the scale of the entity is multiplied on inflation.
        /// </summary>
        public float InflationFactor { get; set; } = 2;

        /// <summary>
        /// Toggle this property to inflate and deflate the entity.
        /// </summary>
        [DataMemberIgnore]
        public bool IsInflated { get; set; }

        public override void Start()
        {
            _initialScale = Entity.Transform.Scale;
            _colliderShape = Entity.Get<PhysicsComponent>().ColliderShape;
        }

        public override void Update()
        {
            var targetScale = _initialScale * (IsInflated ? InflationFactor : 1);
            var scale = Vector3.Lerp(Entity.Transform.Scale, targetScale, .2f);

            // We have to scale both, the graphical shape and the physical shape
            Entity.Transform.Scale = scale;
            if (_colliderShape != null)
                _colliderShape.Scaling = scale;
        }
    }
}
