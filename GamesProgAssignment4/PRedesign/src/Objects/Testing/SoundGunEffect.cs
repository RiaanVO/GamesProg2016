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
        SphereCollider collider;
        private float soundRadius = 40f;
        AudioEmitterComponent audioEmitter;

        //Animation variables
        private float deltaTime;
        private bool active;
        private float maxAnimRadius = 70f; //Should be roughly double the soundRadius
        private float currentAnimRadius;
        private float expansionSpeed;
        private float minExpansionSpeed = 60f;

        //Fading variables
        private float maxAlpha = 0.5f;
        private float alphaFadeSpeed = 0.04f;

        public SoundGunEffect(Vector3 startPosition, Model model) : base(startPosition, model)
        {
            Scale = 1f;

            //Game logic variables
            active = false;

            //Audio code
            audioEmitter = new AudioEmitterComponent(this);
            audioEmitter.createSoundEffectInstance("laser", ContentStore.loadedSounds["laser"], true, false, false, 0.5f);

            //Animation code
            currentAnimRadius = 0.5f;

            hasLighting = true;
            useAlpha = true;
            alpha = 0.5f;
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
            collider = new SphereCollider(this, ObjectTag.sound, soundRadius);
            collider.DrawColour = Color.Green;
            //collider.updateColliderPos(effectPosition);

            //Reset Animation variables
            active = true;
            currentAnimRadius = 0.5f;
            alpha = maxAlpha;
            expansionSpeed = minExpansionSpeed;
            //Resets animation variables
            //Sets position?
            //etc
            audioEmitter.playSoundEffect("laser", 1f);


        }

        private void deactivateEffect()
        {
            active = false;
            collider.Remove();
        }

        private void expandAnimation()
        {
            //Expands outwards until it hits the max
            if (currentAnimRadius < maxAnimRadius)
            {
                currentAnimRadius += deltaTime * expansionSpeed;
                //speeds it up as it goes
                expansionSpeed += 2f;
            }
            else
            {
                //Linked here
                deactivateEffect();
            }
            scale = currentAnimRadius;
            //Advanced - fades out as it goes
            //alpha -= maxAlpha * alphaFadeSpeed;
            alpha =  maxAnimRadius / currentAnimRadius * alphaFadeSpeed;
        }

        public override Matrix GetWorld()
        {
            scaleMatrix = Matrix.CreateScale(scale);
            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            ObjectManager.GraphicsDevice.RasterizerState = rs;

            if (active) base.Draw(gameTime);

            rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            ObjectManager.GraphicsDevice.RasterizerState = rs;
        }
        
    }
}
