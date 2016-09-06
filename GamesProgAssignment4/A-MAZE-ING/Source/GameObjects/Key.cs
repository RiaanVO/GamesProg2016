﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace A_MAZE_ING
{
    class Key : BasicModel
    {
        Player player;

        float rotationalSpeed = 2f;
        float orientation;

        bool hasBeenCollected;
        SphereCollider collider;
        bool keyRelocated = false;

        AudioEmitterComponent audioEmitter;

        public Key(Game game, ObjectManager objectManager, Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(game, objectManager, startPosition, model, camera)
        {
            this.player = player;
            hasBeenCollected = false; //false
            orientation = 0f;
            collider = new SphereCollider(game, this, objectTag.pickup, true, false, 6f);
            scale = Matrix.CreateScale(0.5f);
            audioEmitter = new AudioEmitterComponent(game, this);
            audioEmitter.addSoundEffect("pickup", game.Content.Load<SoundEffect>(@"Sounds/key"));
        }

        public override void Update(GameTime gameTime)
        {
            //temporary work-around to show key has been collected:
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


            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.K) && !keyRelocated)
            {
                position = new Vector3(450, 6f, 340);
                translation = Matrix.CreateTranslation(position);
                collider.updatePos();
                collider.updateColliderPos();
                keyRelocated = true;
            }
            

            if (!hasBeenCollected)
            {
                rotateKey(gameTime);
                base.Update(gameTime);
            }
        }

        private void rotateKey(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            orientation += rotationalSpeed * deltaTime;
            rotation = Matrix.CreateRotationY(orientation);
        }

        public override Matrix GetWorld()
        {
            return scale * rotation * translation;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!hasBeenCollected) base.Draw(gameTime);
        }
    }
}

