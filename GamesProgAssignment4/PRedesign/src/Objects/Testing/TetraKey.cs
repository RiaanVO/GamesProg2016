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

        bool hasBeenCollected;
        SphereCollider collider;
        //bool keyRelocated = false;
        //Key needs  to have a door related with it
        AudioEmitterComponent audioEmitter;
        TetraDoor door;

        // animation variables
        private bool isSplit;
        private float deltaTime;
        private float rotationalSpeed = 1f; //speed of rotation
        private float activateDistance = 35f; //distance between player and object to activate
        private float outerMaxDistance = 25f; //how far the outer parts can move
        private float currentDistance; //how far the outers are currently
        private float outerMovementSpeed = 90f; //how quickly the outer parts move
        
        // hover animation variables
        private float hoverHeight;
        private float originalYPosition;
        private float hoverSpeed = 0.8f;

        public TetraKey(Vector3 startPosition, Model model, Player player, TetraDoor door) : base(startPosition, model)
        {
            this.player = player;
            hasBeenCollected = false; //false
            orientation = 0f;
            scale = 0.03f;
            scaleMatrix = Matrix.CreateScale(scale);
            this.door = door;

            //Collision code
            collider = new SphereCollider(this, ObjectTag.pickup, 3f);
            collider.DrawColour = Color.Yellow;

            //Audio code
            audioEmitter = new AudioEmitterComponent(this);
            audioEmitter.createSoundEffectInstance("key", ContentStore.loadedSounds["key"], false, false, false, 1f);
            //audioEmitter.addSoundEffect("pickup", game.Content.Load<SoundEffect>(@"Sounds/key"));

            //Animation code
            isSplit = false;
            currentDistance = 0f;
            hoverHeight = 0f;
            originalYPosition = startPosition.Y;

            hasLighting = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!hasBeenCollected)
            {
                //Check collisions
                List<Collider> currentCollisions = collider.getCollisions();
                if (currentCollisions.Count() > 0)
                {
                    foreach (Collider col in currentCollisions)
                    {
                        if (col.Tag == ObjectTag.player)
                        {
                            keyPickedUp();
                        }
                    }
                }

                rotateKey(gameTime);

                //Replace with proper state machine behaviour?
                if (Vector3.Distance(this.position, player.Position) < activateDistance)
                    isSplit = true;
                else
                    isSplit = false;

                splitAnimation();
                hoverAnimation();
                base.Update(gameTime);
            }
        }

        private void keyPickedUp()
        {
            //Unlocks the linked door
            hasBeenCollected = true;
            audioEmitter.setInstancePlayback("key", true);
            collider.Remove();
            door.unlockDoor();
        }

        private void rotateKey(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            
            orientation += rotationalSpeed * deltaTime;
            //resets to zero + overlap
            if (orientation > MathHelper.TwoPi)
            {
                float overlap = orientation - MathHelper.TwoPi;
                orientation = 0f + overlap;
            }
            //Otherwise rotate the outside counterclockwise
            if (isSplit)
            {
                rotationMatrix = Matrix.CreateRotationY(-orientation);
                model.Bones["top_geo"].Transform = Matrix.CreateRotationY(orientation * 2);
                model.Bones["bot_geo"].Transform = Matrix.CreateRotationY(orientation * 2);
            }
            else
            {
                rotationMatrix = Matrix.CreateRotationY(orientation);
                model.Bones["top_geo"].Transform = Matrix.CreateRotationY(0f);
                model.Bones["bot_geo"].Transform = Matrix.CreateRotationY(0f);
            }
        }

        private void splitAnimation()
        {
            //Moves them outwards or inwards
            //Needs to accelerate and decelerate as they go past? or just move it out until it hits the limit
            if (isSplit && currentDistance <= outerMaxDistance)
            {
                currentDistance += outerMovementSpeed * deltaTime;
            }
            if (!isSplit && currentDistance > 0)
            {
                currentDistance -= outerMovementSpeed * deltaTime;
            }
            //currentDistance += (isSplit ? outerMovementSpeed : -outerMovementSpeed);
            model.Bones["top_geo"].Transform *= Matrix.CreateTranslation(0f, currentDistance, 0f);
            model.Bones["bot_geo"].Transform *= Matrix.CreateTranslation(0f, -currentDistance, 0f);

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
            translationMatrix = Matrix.CreateTranslation(position);
            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!hasBeenCollected)
                base.Draw(gameTime);
        }
    }
}

