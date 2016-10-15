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
    class ExitPortal : BasicModel
    {
        Player player;

        bool isUnlocked;
        SphereCollider collider;
        //bool keyRelocated = false;
        //Key needs  to have a door related with it
        AudioEmitterComponent audioEmitter;

        // animation variables
        private float deltaTime;

        // hover animation variables
        private float hoverLimit = 0.8f;
        private float hoverHeight;
        private float originalYPosition;
        private float hoverSpeed = 0.8f;

        public ExitPortal(Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(startPosition, camera, model)
        {
            this.player = player;
            isUnlocked = false; //false
            orientation = 0f;
            scale = 0.03f;
            scaleMatrix = Matrix.CreateScale(scale);

            //Collision code. Will the door tag work correctly? Should we start this off being an obstacle and then make it a door tag?
            collider = new SphereCollider(this, ObjectTag.exit, 3f);
            collider.DrawColour = Color.Yellow;

            CollisionManager.ForceTreeConstruction();

            audioEmitter = new AudioEmitterComponent(this);
            //audioEmitter.addSoundEffect("pickup", game.Content.Load<SoundEffect>(@"Sounds/key"));

            hoverHeight = 0f;
            originalYPosition = startPosition.Y;
            hasLighting = true;
        }

        public override void Update(GameTime gameTime)
        {
            //Doesn't do anything out of ordinary if locked
            //Exit portal should just stay open.
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            List<Collider> currentCollisions = collider.getCollisions();
            if (currentCollisions.Count() > 0)
            {
                foreach (Collider col in currentCollisions)
                {
                    if (col.Tag == ObjectTag.player)
                    {
                        //Game is over
                        LevelManager.ReloadLevel();
                        return;
                    }
                }
            }

            //Animate the unlocking sequence if needed
            //Animate opening if player is close
            hoverAnimation();

            base.Update(gameTime);
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

            position.Y = originalYPosition + (float)(Math.Sin(hoverHeight) * hoverLimit);
        }

        public override Matrix GetWorld()
        {
            translationMatrix = Matrix.CreateTranslation(position);
            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!isUnlocked)
                base.Draw(gameTime);
        }
    }
}
