using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Xenko.Animations;

namespace TchMarbleGame
{
    public class BalloonBehavior : AsyncScript
    {
        // Declared public member fields and properties will show in the game studio
        
        public ComputeAnimationCurve<float> InflationCurve { get; set; }
        
        /// <summary>
        /// The time in seconds it takes for the entity to inflate or deflate.
        /// </summary>
        public float InflationDuration { get; set; }

        public override async Task Execute()
        {
            this.GetSimulation().ColliderShapesRendering = true;
        
            var isInflated = false;
            var colliderShape = Entity.Get<RigidbodyComponent>()?.ColliderShape;             
                    
            while(Game.IsRunning)
            {
                if (Input.IsKeyPressed(Keys.Space))
                {
                    var sw = Stopwatch.StartNew();
                    
                    var startScale = Entity.Transform.Scale;
                    var endScale = (isInflated ? .5f : 2) * Entity.Transform.Scale;
                    
                    while (sw.Elapsed.TotalSeconds < InflationDuration)
                    { 
                        var t = (float)sw.Elapsed.TotalSeconds / InflationDuration;
                        var scale = Vector3.Lerp(startScale, endScale, t /*InflationCurve.Evaluate(t)*/);
                        
                        Entity.Transform.Scale = scale;
                        
                        if (colliderShape != null)
                            colliderShape.Scaling = scale;

                        await Script.NextFrame();
                    }
                    
                    isInflated = !isInflated;
                }
            
                // Do stuff every new frame
                await Script.NextFrame();
            }
        }
    }
}
