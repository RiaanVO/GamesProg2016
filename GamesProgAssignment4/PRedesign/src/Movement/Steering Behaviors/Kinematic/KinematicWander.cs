using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class KinematicWander : SteeringBehavior
    {
        #region Fields
        GameObject gameObject;
        float maxSpeed;
        float maxRotation;
        Random random;
        #endregion

        #region Properties
        public GameObject GameObject {
            get { return gameObject; }
            set { gameObject = value; }
        }

        public float MaxSpeed {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        public float MaxRotation
        {
            get { return maxRotation; }
            set { maxRotation = value; }
        }
        #endregion

        #region Initialize
        public KinematicWander(GameObject gameObject, float maxSpeed, float maxRotation) {
            this.gameObject = gameObject;
            this.maxSpeed = maxSpeed;
            this.maxRotation = maxRotation;
            random = new Random();
        }

        #endregion

        #region Public Methods
        public override SteeringOutput getSteering()
        {
            if (gameObject == null) 
                return null;
            SteeringOutput steering = new SteeringOutput();

            steering.Velocity = maxSpeed * orientationVector(gameObject.Orientation);
            steering.Rotation = maxRotation * randomBinomial();
            return steering;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Converts an orientation into a vector
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        private Vector3 orientationVector(float orientation) {
            return Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(orientation));
        }

        /// <summary>
        ///Return a float in the range of -1 - 1
        /// </summary>
        /// <returns>float</returns>
        private float randomBinomial() {
            return (float)(random.NextDouble() - random.NextDouble());
        }
        #endregion
    }
}
