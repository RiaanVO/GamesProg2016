using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class SteeringOutput
    {
        #region Fields
        Vector3 velocity;
        float rotation;
        #endregion

        #region Properties
        public Vector3 Velocity {
            get { return velocity; }
            set { velocity = value; }
        }

        public float Rotation {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector3 Linear
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float Angular
        {
            get { return rotation; }
            set { rotation = value; }
        }
        #endregion
        public SteeringOutput() {
            velocity = Vector3.Zero;
            rotation = 0;
        }
    }
}
