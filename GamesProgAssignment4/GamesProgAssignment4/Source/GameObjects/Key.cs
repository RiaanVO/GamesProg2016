using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamesProgAssignment4
{
    class Key : BasicModel
    {
        Player player;

        float rotationalSpeed = 2f;
        float orientation;

        bool hasBeenCollected;

        public Key(Game game, ObjectManager objectManager, Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(game, objectManager, startPosition, model, camera)
        {
            this.player = player;

            position = startPosition;
            hasBeenCollected = false; //false
            orientation = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            //temporary work-around to show key has been collected:
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                hasBeenCollected = true;
                player.setHasKey(hasBeenCollected);
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
