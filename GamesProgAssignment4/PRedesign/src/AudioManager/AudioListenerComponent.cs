using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace PRedesign
{
    class AudioListenerComponent : AudioComponent
    {
        #region Fields
        AudioListener listener;
        #endregion

        #region Properties
        public AudioListener AudioListener {
            get { return listener; }
        }
        public bool HasMoved {
            get { return listener.Position != gameObject.Position; }
        }
        #endregion

        #region Initialization
        public AudioListenerComponent(GameObject gameObject) : base(gameObject) {
            listener = new AudioListener();
            listener.Position = gameObject.Position;

            //Self register
            AudioManager.setListenerCompoent(this);
        }
        #endregion

        #region Public Methods
        public void UpdatePosition() {
            listener.Position = gameObject.Position;
        }
        #endregion
    }
}
