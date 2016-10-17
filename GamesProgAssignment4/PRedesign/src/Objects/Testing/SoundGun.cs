using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class SoundGun : BasicModel
    {
        public bool isVisible;
        private float pitchOrientation;
        //Right, up, -forwards
        //private Vector3 offset = new Vector3(0f, 0.0f, -3.5f);
        private Vector3 offset = new Vector3(0f, 0.0f, 0f);
        private Matrix tiltMatrix = Matrix.CreateRotationY(0.2f) * Matrix.CreateRotationX(0.1f);

        //Firing variables
        SphereCollider soundCollider;
        private float maxSoundRadius = 10f;
        private float maxSoundGunCooldown = 1f; //Should be in seconds?
        private float currentSoundGunCooldown;

        //Fire animimation variables
        private SoundGunEffect soundGunEffect;
        private float maxOpacity = 0.5f;
        private float currentOpacity;
        private float currentSoundRadius;
        private float deltaTime;

        public SoundGun(Vector3 startPosition, Model model) : base(startPosition, model)
        {
            Scale = 0.045f;
            pitchOrientation = 0f;
            hasLighting = true;

            isVisible = true;

            //Gameplay variables
            currentSoundGunCooldown = 0f;

            //Should take the soundGunEffect as an argument or just make it's own?
            //Temporary:
            soundGunEffect = new SoundGunEffect(startPosition, ContentStore.loadedModels["sphere"]);
        }

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            //Update cooldown if not needed
            if (currentSoundGunCooldown > 0f)
            {
                currentSoundGunCooldown -= deltaTime;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Enables the gun to be seen etc
        /// </summary>
        public void Enable()
        {
            isVisible = true;
        }

        /// <summary>
        /// Fires the gun
        /// </summary>
        public void Fire(Vector3 target)
        {
            //Does not fire if the gun is on cool down
            if (currentSoundGunCooldown <= 0f)
            {
                //Create a sphere collider at the target
                soundGunEffect.activateEffect(target);
                //Update cooldown
                currentSoundGunCooldown = maxSoundGunCooldown;
            }
        }

        /// <summary>
        /// Updates the gun with the correct position and orientation so it can display correctly.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="orientation"></param>
        public void updateMatrices(Vector3 pos, float yaw, float pitch)
        {
            Position = pos + offset;
            Orientation = yaw;
            pitchOrientation = pitch;
        }

        public override Matrix GetWorld()
        {
            //Rotates/tilts on the spot to make it look good
            //Then translates out on the 
            //THEN rotates around the player into the correct position. Should add the offset here instead of to the translate?
            translationMatrix = Matrix.CreateTranslation(position);
            rotationMatrix = Matrix.CreateRotationX(pitchOrientation)* Matrix.CreateRotationY(orientation);
            Matrix yawMatrix = Matrix.CreateRotationY(-orientation * 0.01f);
            Matrix pitchMatrix = Matrix.CreateRotationX(pitchOrientation * 0.01f) ;
            //Matrix pitchRotationMatrix = ;
            //return scaleMatrix * rotationMatrix * tiltMatrix * translationMatrix * yawMatrix;
            //Ned to reverse Y rotation orientation
            return Matrix.Invert(ObjectManager.Camera.View) * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (isVisible)
                base.Draw(gameTime);
        }


        //Fields
        //
        /*
        private Vector3 offset = new Vector3(-2f,0f,-2f);
        private Matrix tiltMatrix = Matrix.CreateRotationY(0.2f) * Matrix.CreateRotationX(0.2f);

        public SoundGun (Vector3 startPosition, Model model) : base(startPosition, model)
        {
            //isVisible = false;
            //isVisible = true;

            scale = 0.08f;
            scaleMatrix = Matrix.CreateScale(scale);

            //hasLighting = true;
        }


        /// <summary>
        /// Updates the gun with the correct position and orientation so it can display correctly.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="orientation"></param>
        public void updateMatrices(Vector3 pos, float orientation)
        {
            //Position = pos + offset;
            Orientation = orientation;
        }


        
        public override Matrix GetWorld()
        {
            //Rotate around the Y axis with the player
            //Possibly also tilt up in a good direction sort of thing
            //Then translate out by offset
            //Then rotate around the player, should be same angle as previous rotation
            //return scaleMatrix * rotationMatrix * translationMatrix;
            //rotationMatrix = Matrix.CreateRotationY(orientation);
            //tiltMatrix = Matrix.CreateRotation(x,y,z);
            //translationMatrix = Matrix.CreateTranslation(position + offset);

            return scaleMatrix * rotationMatrix * tiltMatrix * translationMatrix * rotationMatrix;
        }
    

        public override void Draw(GameTime gameTime)
        {
            //if (isVisible)
                base.Draw(gameTime);
        }
        
        */
    }
}
