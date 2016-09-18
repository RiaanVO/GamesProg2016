using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class Seek : SteeringBehavior
    {

        #region Fields
        GameObject gameObject;
        Vector3 target;
        float maxAcceleration;
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
        #endregion

        #region Initialization

        /// <summary>
        /// need to set everyting throught properties
        /// </summary>
        public Seek() { }

        public Seek(GameObject gameObject, Vector3 target, float maxAcceleration) {
            this.gameObject = gameObject;
            this.target = target;
            this.maxAcceleration = maxAcceleration;
        }
        #endregion

        #region Public Methods

        public override SteeringOutput getSteering()
        {
            if (gameObject == null)
                return null;

            SteeringOutput steering = new SteeringOutput();
            Vector3 acceleration = target - gameObject.Position;
            if(acceleration != Vector3.Zero)
                acceleration = Vector3.Normalize(acceleration) * maxAcceleration;
            steering.Linear = acceleration;
            steering.Angular = 0;
            return steering;
        }

        #endregion

    }
}
