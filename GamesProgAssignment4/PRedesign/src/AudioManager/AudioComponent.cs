using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace PRedesign
{
    abstract class AudioComponent
    {
        #region Fields
        protected GameObject gameObject;
        #endregion

        #region Properties
        public GameObject GameObject {
            get { return gameObject; }
        }
        #endregion

        #region Initialization
        public AudioComponent(GameObject gameObject) {
            this.gameObject = gameObject;
        }
        #endregion
    }
}
