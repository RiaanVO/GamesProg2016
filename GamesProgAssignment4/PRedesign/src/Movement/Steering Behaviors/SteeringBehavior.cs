using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    abstract class SteeringBehavior
    {
        public abstract SteeringOutput getSteering();

        #region Helper Methods
        protected float getNewOrientation(float currentOrientation, Vector3 velocity)
        {
            if (velocity.Length() > 0)
            {
                return (float)Math.Atan2(velocity.X, velocity.Z);
            }
            return currentOrientation;
        }
        #endregion
    }
}
