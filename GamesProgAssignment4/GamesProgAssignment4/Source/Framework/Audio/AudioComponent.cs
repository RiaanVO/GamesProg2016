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

        public abstract void Update();
    }
}
