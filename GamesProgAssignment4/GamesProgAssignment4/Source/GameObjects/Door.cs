using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace GamesProgAssignment4
{
    class Door : BasicModel
    {
        Player player;

        //bool hasBeenCollected;
        BoxCollider collider;

        //AudioEmitterComponent audioEmitter;

        public Door(Game game, ObjectManager objectManager, Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(game, objectManager, startPosition, model, camera)
        {
            this.player = player;
            //Estimating door dimensions
            collider = new BoxCollider(game, this, objectTag.door, true, false, startPosition, startPosition + new Vector3(21f, 20f, 5f));
            scale = Matrix.CreateScale(16f);
            rotation = Matrix.CreateRotationY(MathHelper.Pi + MathHelper.PiOver2);
            //audioEmitter = new AudioEmitterComponent(game, this);
            //audioEmitter.addSoundEffect("pickup", game.Content.Load<SoundEffect>(@"Sounds/scaryscream"));
        }

        public override void Update(GameTime gameTime)
        {
            if (collider.collidingWith.Count != 0 && player.getHasKey())
            {
                foreach (Collider col in collider.collidingWith)
                {
                    if (col.tag == objectTag.player)
                    {
                        //Game ends
                        (game as Game1).gameEnd();
                    }
                }
            }

            base.Update(gameTime);
        }

        public override Matrix GetWorld()
        {
            return scale * rotation * translation;
        }
    }
}
