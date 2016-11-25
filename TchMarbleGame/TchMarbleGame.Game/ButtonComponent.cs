using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;

namespace TchMarbleGame
{
    public class ButtonComponent : AsyncScript
    {
        public RigidbodyComponent GroundPlateRigidbody { get; set; }

        public override async Task Execute()
        {
            var rigidbody = Entity.Get<RigidbodyComponent>();
            var physicsSimulation = this.GetSimulation();
            
            var constraint = (SliderConstraint)Simulation.CreateConstraint(ConstraintTypes.Slider, rigidbody, GroundPlateRigidbody, Matrix.Translation(0, 1, 0), Matrix.Translation(0, 1, 0), true);
            constraint.LowerAngularLimit = 0;
            constraint.UpperAngularLimit = 0;
            constraint.LowerLinearLimit = 0;
            constraint.UpperLinearLimit = 2;

            physicsSimulation.AddConstraint(constraint);
        
            while(Game.IsRunning)
            {
                rigidbody.ApplyForce(new Vector3(0, 10, 0));
                // Do stuff every new frame
                await Script.NextFrame();
            }
        }
    }
}
