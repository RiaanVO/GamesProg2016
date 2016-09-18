using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class Arrive : SteeringBehavior
    {
        #region Fields
        GameObject gameObject;
        Vector3 target;
        float maxAcceleration;
        float maxSpeed;

        float targetRadius;
        float slowRadius;
        float timeToTarget;
        #endregion

        #region Properties
        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
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

        public float MaxSpeed {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        public float TargetRadius
        {
            get { return targetRadius; }
            set { targetRadius = value; }
        }

        public float SlowRadius
        {
            get { return slowRadius; }
            set { slowRadius = value; }
        }

        public float TimeToTarget
        {
            get { return timeToTarget; }
            set { timeToTarget = value; }
        }
        #endregion


        #region Initialization
        /// <summary>
        /// Need to set everything from properties
        /// </summary>
        public Arrive() { }

        public Arrive(GameObject gameObject, Vector3 target, float maxAcceleration, float maxSpeed, float targetRadius, float slowRadius, float timeToTarget)
        {
            this.gameObject = gameObject;
            this.target = target;
            this.maxAcceleration = maxAcceleration;
            this.maxSpeed = maxSpeed;
            this.targetRadius = targetRadius;
            this.slowRadius = slowRadius;
            this.timeToTarget = timeToTarget;
        }
        #endregion

        #region Public Methods

        public override SteeringOutput getSteering()
        {
            if (gameObject == null)
                return null;

            SteeringOutput steering = new SteeringOutput();

            Vector3 direction = target - gameObject.Position;
            float distance = direction.Length();
            float targetSpeed;
            Vector3 targetVelocity = Vector3.Zero;

            if (distance < targetRadius)
                return null;

            if (distance > slowRadius)
                targetSpeed = maxSpeed;
            else
                targetSpeed = maxSpeed * distance / slowRadius;

            if(direction != Vector3.Zero)
                targetVelocity = Vector3.Normalize(direction) * targetSpeed;

            targetVelocity -= gameObject.Velocity;
            targetVelocity /= timeToTarget;

            if (targetVelocity.Length() > maxAcceleration)
                targetVelocity = Vector3.Normalize(targetVelocity) * maxAcceleration;

            steering.Linear = targetVelocity;
            steering.Angular = 0;
            return steering;
        }

        #endregion
    }
}
