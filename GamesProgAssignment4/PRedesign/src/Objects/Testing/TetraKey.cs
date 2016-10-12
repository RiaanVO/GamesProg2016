using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PRedesign
{
    class TetraKey : BasicModel
    {
        Player player;

        float rotationalSpeed = 2f;

        bool hasBeenCollected;
        //SphereCollider collider;
        bool keyRelocated = false;

        AudioEmitterComponent audioEmitter;
        private float activateDistance;

        //Split animation variables
        private bool isSplit;
        private float outerDistance = 2f;
        private float currentDistance = 0f;
        private float movementSpeed = 0.1f;
        private Matrix innerRotation;
        private Matrix outerRotation;

        public TetraKey(Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(startPosition, camera, model)
        {
            this.player = player;
            hasBeenCollected = false; //false
            orientation = 0f;
            //collider = new SphereCollider(game, this, objectTag.pickup, true, false, 6f);
            scaleMatrix = Matrix.CreateScale(0.5f);
            audioEmitter = new AudioEmitterComponent(this);
            //audioEmitter.addSoundEffect("pickup", game.Content.Load<SoundEffect>(@"Sounds/key"));
            activateDistance = 20f;

            //Set up animation variables
            isSplit = false;
        }

        public override void Update(GameTime gameTime)
        {
            //temporary work-around to show key has been collected:
            /*
            if (collider.collidingWith.Count != 0 && !hasBeenCollected)
            {
                foreach (Collider col in collider.collidingWith) {
                    if(col.tag == objectTag.player)
                    {
                        hasBeenCollected = true;
                        player.setHasKey(hasBeenCollected);
                        audioEmitter.playSoundEffect("pickup", 0.1f);
                    }
                }
            }
            */

            //Note: Likely not needed any more as we have an editor?
            /*
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.K) && !keyRelocated)
            {
                position = new Vector3(450, 6f, 340);
                translationMatrix = Matrix.CreateTranslation(position);
                collider.updatePos();
                collider.updateColliderPos();
                keyRelocated = true;
            }
            */

            if (!hasBeenCollected)
            {
                rotateKey(gameTime);

                //Replace with proper state machine behaviour?
                /*if (Vector3.Distance(this.position, player.Position) < activateDistance)
                {
                    splitAnimation();
                }*/

                base.Update(gameTime);
            }
            //split key?
        }

        private void rotateKey(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            
            orientation += rotationalSpeed * deltaTime;
            //resets to zero + overlap
            if (orientation > MathHelper.TwoPi)
            {
                float overlap = orientation - MathHelper.TwoPi;
                orientation = 0f + overlap;
            }

            //Angle in radians
            rotationMatrix = Matrix.CreateRotationY(orientation);

            //If not split:
            //all rotate together (don't have to change anything)
            
            //Otherwise rotate the outside counterclockwise
            if (isSplit)
            {
                model.Bones["top_jnt"].Transform *= -outerRotation;
                model.Bones["bot_jnt"].Transform *= -outerRotation;
            }
        }

        private void splitAnimation()
        {

            //Rotates them - doesn't need to now
            outerRotation = Matrix.CreateRotationY(0.01f);
            model.Bones["top_jnt"].Transform *= outerRotation;
            model.Bones["bot_jnt"].Transform *= outerRotation;

            model.Bones["mid_jnt"].Transform *= outerRotation;

            //Moves them outwards
            //Needs to accelerate and decelerate as they go past? or just move it out until it hits the limit
            if (currentDistance <= outerDistance)
            {
                currentDistance += movementSpeed;
                model.Bones["top_jnt"].Transform *= Matrix.CreateTranslation(0f, currentDistance, 0f);
                model.Bones["bot_jnt"].Transform *= Matrix.CreateTranslation(0f, currentDistance, 0f);
            }
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

