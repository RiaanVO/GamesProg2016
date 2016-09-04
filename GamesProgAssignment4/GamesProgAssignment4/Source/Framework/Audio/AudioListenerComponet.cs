using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GamesProgAssignment4
{
    class AudioListenerComponet : AudioComponent
    {
        public AudioListener listener { get; protected set; }
        public bool hasMoved { get; protected set; }

        public AudioListenerComponet(Game game, GameObject gameObject) : base(game, gameObject)
        {
            listener = new AudioListener();
            listener.Position = gameObject.position;
            game.Services.GetService<AudioManager>().setListenerComponent(this);
        }

        public override void Update()
        {
            if(checkHasMoved()) {
                listener.Position = gameObject.position;
            }
        }

        /// <summary>
        /// Updates the position of the listener.
        /// </summary>
        public void UpdatePosition() {
            listener.Position = gameObject.position;
        }

        /// <summary>
        /// Checks if the game object has moved from the listeners position
        /// </summary>
        /// <returns></returns>
        public bool checkHasMoved() {
            return hasMoved = listener.Position != gameObject.position;
        }
    }
}
