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
    class TetraDoor : BasicModel
    {
        Player player;

        bool isUnlocked;
        BoxCollider collider;
        //bool keyRelocated = false;
        //Key needs  to have a door related with it
        AudioEmitterComponent audioEmitter;
        List<Collider> currentCollisions;
        private float deltaTime;

        //Unlock animation variables
        private float diamondMaxDistance = 2.5f; //how far the diamond parts can move
        private float currentDistance; //how far the diamond parts are currently
        private float diamondMovementSpeed = 3f; //how quickly the diamond parts move
        private bool opening;
        private bool unlockComplete;

        // Disc rotate animation variables
        private float activateDistance = 40f; //distance between player and object to animate open
        private bool discRotated;
        private float discRotation;
        private float rotationalSpeed = 2f; //speed of rotation

        //Door open animation variables
        private bool doorsOpen;
        private float doorMaxDistance;
        private float currentDoorDistance;
        private float doorOpenSpeed = 1f;

        public TetraDoor(Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(startPosition, camera, model)
        {
            this.player = player;
            isUnlocked = false; //false
            orientation = 0f;
            scale = 0.15f;
            //translationMatrix = Matrix.CreateTranslation(startPosition) + Matrix.CreateTranslation(50f, 50f, 17f);
            //translationMatrix = Matrix.CreateTranslation(startPosition + new Vector3(LevelManager.TileSize / 2, LevelManager.TileSize / 2, LevelManager.TileSize / 4));
            scaleMatrix = Matrix.CreateScale(scale);

            //Collision code. Will the door tag work correctly? Should we start this off being an obstacle and then make it a door tag?
            //collider = new SphereCollider(this, ObjectTag.obstacle, 3f);
            collider = new BoxCollider(this, ObjectTag.obstacle, new Vector3(LevelManager.TileSize, LevelManager.TileSize, LevelManager.TileSize * 0.5f));
            collider.DrawColour = Color.Yellow;

            CollisionManager.ForceTreeConstruction();

            audioEmitter = new AudioEmitterComponent(this);
            //audioEmitter.addSoundEffect("pickup", game.Content.Load<SoundEffect>(@"Sounds/key"));

            //Set up animation variables
            discRotated = false;
            opening = false;
            unlockComplete = false;
            doorsOpen = false;
            currentDistance = 0f;

            //Disc rotation
            discRotation = 0f;

            //Door animation variables init
            doorMaxDistance = 1.25f;
            currentDoorDistance = 0f;

            hasLighting = true;
        }

        public override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            //Doesn't do anything out of ordinary if locked
            if (isUnlocked)
            {
                currentCollisions = collider.getCollisions();
                if (currentCollisions.Count() > 0)
                {
                    foreach (Collider col in currentCollisions)
                    {
                        if (col.Tag == ObjectTag.player)
                        {
                            //Game is over
                            LevelManager.NextLevel();
                            return;
                        }
                    }
                }

                if (Vector3.Distance(this.position, player.Position) < activateDistance)
                    opening = true;
                //Doens't close again.

                //Animate the unlocking sequence
                unlockAnimation();
                openAnimation();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Opens the door to be able to pass through.
        /// </summary>
        public void unlockDoor()
        {
            collider.Remove();
            collider = new BoxCollider(this, ObjectTag.exit, new Vector3(LevelManager.TileSize, LevelManager.TileSize, LevelManager.TileSize * 0.5f));
            isUnlocked = true;
        }

        private void unlockAnimation()
        {
            if (!unlockComplete)
            {
                if (currentDistance <= diamondMaxDistance)
                {
                    currentDistance += diamondMovementSpeed * deltaTime;
                    model.Bones["diamond_top_geo"].Transform = Matrix.CreateTranslation(0f, currentDistance, 0f);
                    model.Bones["diamond_bot_geo"].Transform = Matrix.CreateTranslation(0f, -currentDistance, 0f);
                }
                else
                {
                    //unlocked fully, stop animating
                    unlockComplete = true;
                }
            }
        }

        private void openAnimation()
        {
            if (opening && unlockComplete)
            {
                if (!discRotated)
                {
                    //Continue animating the unlock sequence (rotation) until unlocked
                    if (discRotation < MathHelper.Pi)
                    {
                        discRotation += rotationalSpeed * deltaTime;
                    }
                    else
                    {
                        discRotated = true;
                    }
                    //This will change depending on orientation, or will it work still?
                    //NEEDS TESTING when whole model is rotated 90 degrees
                    model.Bones["disk_geo"].Transform = Matrix.CreateTranslation(new Vector3(-50, -50, -17)) *  Matrix.CreateRotationZ(discRotation) * Matrix.CreateTranslation(new Vector3(50, 50, 17));
                    model.Bones["diamond_top_geo"].Transform = Matrix.CreateTranslation(new Vector3(-50, -50, -17)) * Matrix.CreateRotationZ(discRotation) * Matrix.CreateTranslation(new Vector3(50, 50, 17));
                    model.Bones["diamond_mid_geo"].Transform = Matrix.CreateTranslation(new Vector3(-50, -50, -17)) * Matrix.CreateRotationZ(discRotation) * Matrix.CreateTranslation(new Vector3(50, 50, 17));
                    model.Bones["diamond_bot_geo"].Transform = Matrix.CreateTranslation(new Vector3(-50, -50, -17)) * Matrix.CreateRotationZ(discRotation) * Matrix.CreateTranslation(new Vector3(50, 50, 17));
                }
                else if (!doorsOpen)
                {
                    //Open the doors until they're open
                    if (currentDoorDistance < doorMaxDistance)
                    {
                        currentDoorDistance += doorOpenSpeed * deltaTime;
                        model.Bones["door_R_geo"].Transform *= Matrix.CreateTranslation(currentDoorDistance, 0f, 0f);
                        model.Bones["disk_geo"].Transform *= Matrix.CreateTranslation(currentDoorDistance, 0f, 0f);
                        model.Bones["diamond_top_geo"].Transform *= Matrix.CreateTranslation(currentDoorDistance, 0f, 0f);
                        model.Bones["diamond_mid_geo"].Transform *= Matrix.CreateTranslation(currentDoorDistance, 0f, 0f);
                        model.Bones["diamond_bot_geo"].Transform *= Matrix.CreateTranslation(currentDoorDistance, 0f, 0f);
                        model.Bones["door_L_geo "].Transform *= Matrix.CreateTranslation(-currentDoorDistance, 0f, 0f);
                    }
                    else
                    {
                        doorsOpen = true;
                    }
                }
            }
        }

        public override Matrix GetWorld()
        {
            //translationMatrix = Matrix.CreateTranslation(position);
            return scaleMatrix * rotationMatrix * translationMatrix;
        }
    }
}

