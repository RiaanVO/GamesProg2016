using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class SoundGunEffect : BasicModel
    {
        SphereCollider soundCollider;
        private float soundRadius = 30f;
        //BasicModel itemToPickUp //Holds the actual item that the pickup represents
        AudioEmitterComponent audioEmitter;

        //Animation variables
        private float deltaTime;
        private bool active;
        private float maxAnimRadius = 30f;
        private float currentAnimRadius;

        public SoundGunEffect(Vector3 startPosition, Model model) : base(startPosition, model)
        {
            Scale = 0.08f;

            //Game logic variables
            active = false;

            //Audio code
            audioEmitter = new AudioEmitterComponent(this);
            audioEmitter.addSoundEffect("pickup", ContentStore.loadedSounds["choir"]);

            //Animation code
            currentAnimRadius = 0f;

            //hasLighting = true;
            //Alpha = 
        }

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            if (active)
            {
                //Updating animation
                //Should we tie the animation with the actual collider? Probably, user should see the effect properly
                //This will mean we need to update the collider radius as it expands
                
                expandAnimation();

                //Check if the effect is still going, otherwise deactivate the effect
                base.Update(gameTime);
            }
        }

        public void activateEffect(Vector3 effectPosition)
        {
            //Colliders
            Position = effectPosition;
            soundCollider = new SphereCollider(this, ObjectTag.sound, soundRadius);
            soundCollider.updateColliderPos(effectPosition);

            //Animation
            active = true;
            currentAnimRadius = 5f;
            //Enables collider
            //Resets animation variables
            //Sets position?
            //etc
        }

        private void deactivateEffect()
        {
            active = false;
            soundCollider.Remove();
        }

        private void expandAnimation()
        {
            //Expands outwards until it hits the max
            if (currentAnimRadius < maxAnimRadius)
            {
                //currentAnimRadius += deltaTime;
                Scale = currentAnimRadius;
            }

            //Advanced - fades out as it goes

            
        }

        public override Matrix GetWorld()
        {
            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!active) base.Draw(gameTime);
        }
        
    }
}
