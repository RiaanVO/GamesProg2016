using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class Enemy : BasicModel
    {
        Player player;

        Vector3 movementDirection;
        Vector3 velocity, acceleration;
        float rotation;

        float maxVelocity = 5f; //temporary test value
        float accelerationRate = 1f;
        float drag = 0.1f;
        float slowestSpeed = 0.5f;

        //BoxCollider box;

        public Enemy(Game game, Vector3 startPos, Model model, BasicCamera camera, Player player) : base(game, startPos, model, camera)
        {
            this.player = player; //so it can get the player's position for pursuing
            position = startPos;
            hasLighting = true;
            scale = Matrix.CreateScale(15f); //vary depending on size of ghost model (haven't been able to get one to work yet :/)
        }

        public override void Initialize()
        {
            velocity = Vector3.Zero;
            movementDirection = Vector3.Zero;
            acceleration = Vector3.Zero;
            rotation = 0f;

            translate = Matrix.CreateTranslation(position);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (player.position != position)
            {
                Vector3 difference = player.position - position;
                difference.Normalize();
                handleMovement(difference);
            }

            base.Update(gameTime);
        }

        private void handleMovement(Vector3 difference)
        {
            //handles XYZ movement (translate)

            acceleration = Vector3.Zero;
            acceleration = Vector3.Multiply(difference, accelerationRate);

            velocity -= Vector3.Multiply(velocity, drag);

            if ((velocity).Length() < slowestSpeed)
            {
                velocity = Vector3.Zero;
            }

            velocity += acceleration;

            if ((velocity).Length() > maxVelocity)
            {
                velocity.Normalize();
                velocity = Vector3.Multiply(velocity, maxVelocity);
            }

            position += velocity;
            translate = Matrix.CreateTranslation(position);

            //handle rotation (on Y axis) (no other axes needed at this stage)

            rotation = Vector3ToRadian(difference);
            rotate = Matrix.CreateRotationY(rotation);
        }

        private float Vector3ToRadian (Vector3 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y);
        }

        public override Matrix GetWorld()
        {
            return scale * rotate * translate;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
