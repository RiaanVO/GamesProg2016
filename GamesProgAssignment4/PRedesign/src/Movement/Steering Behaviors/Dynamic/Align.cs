using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class Align : SteeringBehavior
    {
        #region Fields
        protected GameObject gameObject;
        protected float targetOrientation;
        protected float maxAngularAcceleration;
        protected float maxRotation;

        protected float targetRadius;
        protected float slowRadius;
        protected float timeToTarget;
        #endregion

        #region Properties
        public float TargetOrientation
        {
            get { return targetOrientation; }
            set { targetOrientation = value; }
        }

        public float MaxAngularAcceleration
        {
            get { return maxAngularAcceleration; }
            set { maxAngularAcceleration = value; }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
            set { gameObject = value; }
        }

        public float MaxRotation
        {
            get { return maxRotation; }
            set { maxRotation = value; }
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
        /// Dangours to use as you need to manulay set all the values
        /// </summary>
        public Align() { }

        public Align(GameObject gameObject, float targetOrientation, float maxAngularAcceleration, float maxRotation, float targetRadius, float slowRadius, float timeToTarget)
        {
            this.gameObject = gameObject;
            this.targetOrientation = targetOrientation;
            this.maxAngularAcceleration = maxAngularAcceleration;
            this.maxRotation = maxRotation;
            this.targetRadius = targetRadius;
            this.slowRadius = slowRadius;
            this.timeToTarget = timeToTarget;
        }
        #endregion

        #region Public Methods
        
        //Needs implementation
        public override SteeringOutput getSteering()
        {
            if (gameObject == null)
                return null;

            SteeringOutput steering = new SteeringOutput();
            float targetRotation;
            float rotation = targetOrientation - gameObject.Orientation;
            rotation = MathHelper.WrapAngle(rotation);
            float rotationSize = Math.Abs(rotation);
            if (rotationSize < targetRadius)
                return null;

            if (rotationSize > slowRadius)
                targetRotation = maxRotation;
            else
                targetRotation = maxRotation * rotationSize / slowRadius;

            targetRotation *= rotation / rotationSize;
            targetRotation -= gameObject.Orientation;
            targetRotation /= timeToTarget;

            float angularAcceleration = Math.Abs(targetRotation);
            if (angularAcceleration > maxAngularAcceleration)
                targetRotation = targetRotation / angularAcceleration * maxAngularAcceleration;

            steering.Angular = targetRotation;
            steering.Linear = Vector3.Zero;
            return steering;
        }

        #endregion

        #region Helper Methods
       
        #endregion
    }
}
