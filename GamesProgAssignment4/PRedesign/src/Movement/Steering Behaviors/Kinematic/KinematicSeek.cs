using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class KinematicSeek : SteeringBehavior
    {
        #region Fields
        Vector3 target;
        float maxSpeed;
        GameObject gameObject;
        #endregion

        #region properties
        public Vector3 Target {
            get { return target; }
            set { target = value; }
        }

        public float MaxSpeed {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        public GameObject GameObject {
            get { return gameObject; }
            set { gameObject = value; }
        }
        #endregion

        #region Initialize
        public KinematicSeek() : this(Vector3.Zero, null, 0f) { }

        public KinematicSeek(Vector3 target, GameObject gameObject, float maxSpeed) {
            this.target = target;
            this.gameObject = gameObject;
            this.maxSpeed = maxSpeed;
        }
        #endregion

        #region Public Methods
        public override SteeringOutput getSteering()
        {
            SteeringOutput steering = new SteeringOutput();

            if (gameObject == null)
                return null;

            //Calculate the velocity
            Vector3 velocity = target - gameObject.Position ;
            if (velocity != Vector3.Zero)
                velocity.Normalize();
            velocity *= maxSpeed;
            steering.Velocity = velocity;

            //Face the direction of travel
            gameObject.Orientation = getNewOrientation(gameObject.Orientation, velocity);

            steering.Rotation = 0;
            return steering;
        }

        #endregion
    }
}
