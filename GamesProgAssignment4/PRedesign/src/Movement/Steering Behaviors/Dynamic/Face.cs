using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class Face : Align
    {
        #region Fields
        Vector3 target;
        #endregion

        #region Properties
        public Vector3 Target {
            get { return target; }
            set { target = value; }
        }
        #endregion
        /// <summary>
        /// Set everthing through properties
        /// </summary>
        public Face() { }

        public Face(GameObject gameObject, Vector3 target, float maxAngularAcceleration, float maxRotation, float targetRadius, float slowRadius, float timeToTarget)
        {
            this.gameObject = gameObject;
            this.target = target;
            this.maxAngularAcceleration = maxAngularAcceleration;
            this.maxRotation = maxRotation;
            this.targetRadius = targetRadius;
            this.slowRadius = slowRadius;
            this.timeToTarget = timeToTarget;
        }

        public override SteeringOutput getSteering()
        {
            if (gameObject == null)
                return null;
            SteeringOutput steering = new SteeringOutput();

            Vector3 direction = target - gameObject.Position;
            if (direction == Vector3.Zero)
                return steering;

            TargetOrientation = (float)Math.Atan2(direction.X, direction.Z);

            return base.getSteering();
        }
    }
}
