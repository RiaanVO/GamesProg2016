using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class FollowPath : SteeringBehavior
    {
        #region Fields
        GameObject gameObject;
        Vector3[] targets;
        int currentTargetIndex;
        bool isLooping;
        float changeRadius;

        float maxAcceleration;
        float maxSpeed;

        float targetRadius;
        float slowRadius;
        float timeToTarget;

        float maxAngularAcceleration;
        float maxRotation;

        Seek seekBehavior;
        Arrive arriveBehavior;
        Face faceBehavior;
        #endregion

        #region Properties
        /// <summary>
        /// For following
        /// </summary>
        public float ChangeRadius {
            get { return changeRadius; }
            set { changeRadius = value; }
        }

        public Vector3[] Targets
        {
            get { return targets; }
            set {
                targets = value;
                currentTargetIndex = 0;
            }
        }

        public int CurrentTargetIndex {
            get { return currentTargetIndex; }
            set { currentTargetIndex = value; }
        }

        public bool IsLooping {
            get { return isLooping; }
            set { isLooping = value; }
        }

        /// <summary>
        /// For the sub behaviours
        /// </summary>
        public GameObject GameObject
        {
            get { return gameObject; }
            set {
                gameObject = value;
                seekBehavior.GameObject = gameObject;
                arriveBehavior.GameObject = gameObject;
                faceBehavior.GameObject = gameObject;
            }
        }

        public float MaxAcceleration
        {
            get { return maxAcceleration; }
            set {
                maxAcceleration = value;
                seekBehavior.MaxAcceleration = maxAcceleration;
                arriveBehavior.MaxAcceleration = maxAcceleration;
            }
        }
        
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set {
                maxSpeed = value;
                arriveBehavior.MaxSpeed = MaxSpeed;
            }
        }

        public float TargetRadius
        {
            get { return targetRadius; }
            set { targetRadius = value;
                arriveBehavior.TargetRadius = targetRadius;
                faceBehavior.TargetRadius = targetRadius;
            }
        }

        public float SlowRadius
        {
            get { return slowRadius; }
            set {
                slowRadius = value;
                arriveBehavior.SlowRadius = slowRadius;
                faceBehavior.SlowRadius = slowRadius;
            }
        }

        public float TimeToTarget
        {
            get { return timeToTarget; }
            set {
                timeToTarget = value;
                arriveBehavior.TimeToTarget = timeToTarget;
                faceBehavior.TimeToTarget = timeToTarget;
            }
        }
        
        public float MaxAngularAcceleration
        {
            get { return MaxAngularAcceleration; }
            set
            {
                maxAngularAcceleration = value;
                faceBehavior.MaxAngularAcceleration = maxAngularAcceleration;
            }
        }
        
        public float MaxRotation {
            get { return maxRotation; }
            set {
                maxRotation = value;
                faceBehavior.MaxRotation = maxRotation;
            }
        }

        #endregion

        #region Initialization
        public FollowPath(GameObject gameObject, Vector3[] targets, int currentTargetIndex, bool isLooping, float changeRadius)
        {
            this.targets = targets;
            this.currentTargetIndex = currentTargetIndex;
            this.isLooping = isLooping;
            this.changeRadius = changeRadius;

            seekBehavior = new Seek();
            arriveBehavior = new Arrive();
            faceBehavior = new Face();

            GameObject = gameObject;
            MaxAcceleration = 10;
            MaxSpeed = 2;
            TargetRadius = 1;
            SlowRadius = 2;
            TimeToTarget = 0.25f;
            MaxAngularAcceleration = MathHelper.PiOver2;
            MaxRotation = MathHelper.PiOver4;
        }
        #endregion

        #region Public Methods

        public override SteeringOutput getSteering()
        {
            if (gameObject == null || targets == null)
                return null;
            if (targets.Count() == 0)
                return null;


            SteeringOutput movementSteering = new SteeringOutput();
            if (isLooping){
                movementSteering = applySeek();
            } else {
                if (currentTargetIndex >= targets.Length - 1)
                {
                    movementSteering = applyArrive();
                } else {
                    movementSteering = applySeek();
                }
            }

            //Calculate the looking components
            SteeringOutput orientationSteering = new SteeringOutput();
            faceBehavior.Target = targets[currentTargetIndex];
            //orientationSteering = faceBehavior.getSteering();
            if(orientationSteering != null && movementSteering != null)
                movementSteering.Angular = orientationSteering.Angular;

            return movementSteering;
        }

        #endregion

        #region helper Methods
        private SteeringOutput applySeek() {
            seekBehavior.Target = targets[currentTargetIndex];
            SteeringOutput steering = seekBehavior.getSteering();

            if((targets[currentTargetIndex] - gameObject.Position).Length() < changeRadius)
                currentTargetIndex++;

            if (currentTargetIndex >= targets.Length)
                currentTargetIndex = 0;

            return steering;
        }

        private SteeringOutput applyArrive() {
            arriveBehavior.Target = targets[currentTargetIndex];
            SteeringOutput steering = arriveBehavior.getSteering();
            return steering;
        }
        #endregion
    }
}
