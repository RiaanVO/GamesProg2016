using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class KinematicArrive : SteeringBehavior
    {
        #region Fields
        GameObject gameObject;
        Vector3 target;

        float maxSpeed;
        float arriveRadius;
        float timeToTarget;
        #endregion

        #region Properties
        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }

        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
            set { gameObject = value; }
        }

        public float ArriveRadius {
            get { return arriveRadius; }
            set { arriveRadius = value; }
        }

        public float TimeToTarget
        {
            get { return timeToTarget; }
            set { timeToTarget = value; }
        }
        #endregion

        #region Initialization
        public KinematicArrive() : this(Vector3.Zero, null, 0f, 1f, 1f) { }

        public KinematicArrive(Vector3 target, GameObject gameObject, float maxSpeed, float arriveRadius, float timeToTarget)
        {
            this.target = target;
            this.gameObject = gameObject;
            this.maxSpeed = maxSpeed;
            this.arriveRadius = arriveRadius;
            this.timeToTarget = timeToTarget;
        }
        #endregion

        #region Public Methods
        public override SteeringOutput getSteering()
        {
            SteeringOutput steering = new SteeringOutput();
            Vector3 velocity = target - gameObject.Position;

            if (velocity.Length() < arriveRadius)
                return null;

            velocity /= timeToTarget;

            if (velocity.Length() > maxSpeed)
                velocity = Vector3.Normalize(velocity) * maxSpeed;

            GameObject.Orientation = getNewOrientation(gameObject.Orientation, velocity);

            steering.Velocity = velocity;
            steering.Rotation = 0;
            return steering;
        }
        #endregion
    }
}
