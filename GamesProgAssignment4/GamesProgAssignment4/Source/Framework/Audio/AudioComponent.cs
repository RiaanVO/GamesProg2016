using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GamesProgAssignment4
{
    abstract class AudioComponent
    {
        protected GameObject gameObject;
        protected Game game;

        public AudioComponent(Game game, GameObject gameObject) {
            this.gameObject = gameObject;
            this.game = game;
        }

        //Not needed as manager will do it
        ///public abstract void Update();
    }
}
