using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class Key : BasicModel
    {
        float rotationalSpeed = 2f;
        float orientation;

        bool hasBeenCollected;

        public Key(Game game, ObjectManager objectManager, Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(game, objectManager, startPosition, model, camera)
        {
            position = startPosition;
            hasBeenCollected = false;
            orientation = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            rotateKey(gameTime);

            base.Update(gameTime);
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
            base.Draw(gameTime);
        }
    }
}
