using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System.Threading.Tasks;

namespace TchMarbleGame
{
    public class SmoothEntityFollower : AsyncScript
    {
        /// <summary>
        /// The entity to be followed.
        /// </summary>
        public Entity Target { get; set; }
        
        public override async Task Execute()
        {
            // At the beginning, determine the distance to the target entity.            
            var offsetFromTarget = Entity.Transform.Position - Target.Transform.Position;

            // In each frame, move this entity towards the position of the target entity,
            // always keeping the distance determined at the beginning.
            while (Game.IsRunning)
            {
                var targetPosition = Target.Transform.Position + offsetFromTarget;
                Entity.Transform.Position = Vector3.Lerp(Entity.Transform.Position, targetPosition, 5 * (float)Game.DrawTime.Elapsed.TotalSeconds);
                await Script.NextFrame();
            }
        }
    }
}
