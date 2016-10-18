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
        private Vector3 offset = new Vector3(0f, 1.0f, 0f); //Vector offset of the final gun position
        private float xOffsetMod = -2f; //Multiplies the cross vector to move the gun to the side more
        private float zOffsetMod = 5f; //Multiplies the look vector to move gun in and out more
        private Vector3 playerLookDirection;
        private Matrix tiltMatrix = Matrix.CreateRotationY(0.2f) * Matrix.CreateRotationX(-0.1f);

        //Firing variables
        private float maxSoundGunCooldown = 1f; //Should be in seconds?
        private float currentSoundGunCooldown;
        private SoundGunEffect soundGunEffect;

        //Fire animimation variables
        private float deltaTime;
        private float startFireAnim = 0f;
        private float endFireAnim = MathHelper.Pi;//private float endFireAnim = MathHelper.PiOver2 * 3f;
        private float currentFireAnim = 0f;
        private float currentFireAngle = 0f;
        private float animSpeed = 8f;

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
            fireAnimation();
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
                currentFireAnim = startFireAnim; //Starts the firing animation
                currentFireAngle = startFireAnim;
            }
        }

        private void fireAnimation()
        {
            if (currentFireAnim < endFireAnim)
            {
                currentFireAnim += deltaTime * animSpeed;
            }
            //Have to move the thing according to sine
            currentFireAngle = -(float)Math.Sin(currentFireAnim) / 2f;
        }
        
        /// <summary>
        /// Updates the gun with the correct position and orientation so it can display correctly.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="orientation"></param>
        public void updateMatrices(Vector3 pos, Vector3 lookDirection, float yaw, float pitch)
        {
            lookDirection.Normalize();
            //Position = pos;
            Position = pos + (lookDirection * (zOffsetMod + currentFireAngle*2)) + (Vector3.Cross(Vector3.Up, lookDirection) * xOffsetMod);
            Orientation = yaw;
            pitchOrientation = pitch;
            //playerLookDirection = lookDirection;
            playerLookDirection.Normalize();
        }

        public override Matrix GetWorld()
        {
            //Rotates/tilts on the spot to make it look good
            //Then translates out along the player's look direction, 
            //and sideways relative to the look vector.
            //translationMatrix = Matrix.CreateTranslation(position + (playerLookDirection * zOffsetMod) - (Vector3.Cross(Vector3.Up, playerLookDirection) * xOffsetMod) + offset);
            rotationMatrix = Matrix.CreateRotationX(pitchOrientation + currentFireAngle) * Matrix.CreateRotationY(orientation);
            return scaleMatrix * tiltMatrix * rotationMatrix * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (isVisible)
                base.Draw(gameTime);
        }
    }
}
