using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PRedesign.src.Objects.Testing
{
    class Pickup : BasicModel
    {
        float rotationalSpeed = 2f;
        bool hasBeenCollected;
        SphereCollider collider;
        AudioEmitterComponent audioEmitter;

        public Pickup(Vector3 startPosition, Model model) : base(startPosition, model)
        {
            hasBeenCollected = false; //false
            scale = 0.03f;
            scaleMatrix = Matrix.CreateScale(scale);

            //Collision code
            collider = new SphereCollider(this, ObjectTag.pickup, 3f);
            collider.DrawColour = Color.Yellow;
            
            //Audio code
            //audioEmitter = new AudioEmitterComponent(game, this);
            //audioEmitter.addSoundEffect("pickup", game.Content.Load<SoundEffect>(@"Sounds/key"));

            //Animation code
            orientation = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            //temporary work-around to show key has been collected:
            if (!hasBeenCollected)
            {
                List<Collider> currentCollisions = collider.getCollisions();
                if (currentCollisions.Count() > 0)
                {
                    foreach (Collider col in currentCollisions)
                    {
                        if (col.Tag == ObjectTag.player)
                        {
                            pickedUp();
                        }
                    }
                }

                rotateKey(gameTime);
                base.Update(gameTime);
            }
        }

        private void pickedUp()
        {
            hasBeenCollected = true;
        }

        private void rotateKey(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            orientation += rotationalSpeed * deltaTime;
            rotationMatrix = Matrix.CreateRotationY(orientation);
        }

        public override Matrix GetWorld()
        {
            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!hasBeenCollected) base.Draw(gameTime);
        }
    }
}
