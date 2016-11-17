using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace PRedesign
{
    class GunPickup : BasicModel
    {
        bool hasBeenCollected;
        SphereCollider collider;
        //BasicModel itemToPickUp //Holds the actual item that the pickup represents
        AudioEmitterComponent audioEmitter;

        //Animation variables
        float deltaTime;
        float rotationalSpeed = 2f;

        // hover animation variables
        private float hoverHeight;
        private float originalYPosition;
        private float hoverSpeed = 2f;

        //Tilt effect variables
        private Matrix tiltMatrix = Matrix.CreateRotationX(-0.5f);

        public GunPickup(Vector3 startPosition, Model model) : base(startPosition, model)
        {
            hasBeenCollected = false; //false
            Scale = 0.08f;

            //Collision code
            collider = new SphereCollider(this, ObjectTag.gun, 3f);
            collider.DrawColour = Color.Yellow;
            
            //Audio code
            audioEmitter = new AudioEmitterComponent(this);
            //audioEmitter.addSoundEffect("pickup", ContentStore.loadedSounds["choir"]);
            audioEmitter.createSoundEffectInstance("pickup", ContentStore.loadedSounds["choir"], true, true, true, 1f);
            audioEmitter.setVolume("pickup", 1f);
            //Animation code
            orientation = 0f;
            hoverHeight = 0f;
            originalYPosition = startPosition.Y;

            hasLighting = true;
        }

        public override void Update(GameTime gameTime)
        {
            //temporary work-around to show key has been collected:
            if (!hasBeenCollected)
            {
                deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
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

                rotateKey();
                hoverAnimation();
                base.Update(gameTime);
            }
        }

        private void pickedUp()
        {
            hasBeenCollected = true;
            collider.Remove();
        }

        private void rotateKey()
        {
            orientation += rotationalSpeed * deltaTime;
            rotationMatrix = tiltMatrix * Matrix.CreateRotationY(orientation);
            audioEmitter.stopAll();
        }

        private void hoverAnimation()
        {
            hoverHeight += hoverSpeed * deltaTime;
            //resets to zero + overlap
            if (hoverHeight > MathHelper.TwoPi)
            {
                float overlap = hoverHeight - MathHelper.TwoPi;
                hoverHeight = 0f + overlap;
            }

            position.Y = originalYPosition + (float)Math.Sin(hoverHeight);
        }

        public override Matrix GetWorld()
        {
            translationMatrix = Matrix.CreateTranslation(position); //Handles the hovering
            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!hasBeenCollected) base.Draw(gameTime);
        }
    }
}
