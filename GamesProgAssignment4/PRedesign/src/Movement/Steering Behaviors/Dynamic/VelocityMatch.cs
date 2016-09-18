using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class VelocityMatch : SteeringBehavior
    {
        #region Fields
        GameObject gameObject;
        Vector3 targetVelocity;
        float maxAcceleration;
        
        float timeToTarget;
        #endregion

        #region Properties
        public Vector3 TargetVelocity
        {
            get { return targetVelocity; }
            set { targetVelocity = value; }
        }

        public float MaxAcceleration
        {
            get { return maxAcceleration; }
            set { maxAcceleration = value; }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
            set { gameObject = value; }
        }

        public float TimeToTarget
        {
            get { return timeToTarget; }
            set { timeToTarget = value; }
        }
        #endregion


        #region Initialization
        public VelocityMatch(GameObject gameObject, Vector3 targetVelocity, float maxAcceleration, float timeToTarget)
        {
            this.gameObject = gameObject;
            this.targetVelocity = targetVelocity;
            this.maxAcceleration = maxAcceleration;
            this.timeToTarget = timeToTarget;
        }
        #endregion

        #region Public Methods

        public override SteeringOutput getSteering()
        {
            if (gameObject == null)
                return null;

            SteeringOutput steering = new SteeringOutput();
            Vector3 newVelocity = targetVelocity - gameObject.Velocity;
            newVelocity /= timeToTarget;

            if (newVelocity.Length() > maxAcceleration)
                newVelocity = Vector3.Normalize(newVelocity) * maxAcceleration;

            steering.Linear = newVelocity;
            steering.Angular = 0;
            return steering;
        }

        #endregion
    }
}
