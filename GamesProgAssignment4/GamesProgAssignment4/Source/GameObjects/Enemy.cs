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

        Vector3 targetPosition;

        Vector3 lookDirection;
        float rotationalSpeed = 2f;
        float orientation;
        float orientationDifference = 0.001f;

        Vector3 velocity;
        float maxVelocity = 20f;
        float acceleration = 0.5f;

        float stoppingDistFromTarget = 1;

        //old fields:
        /*Player player;

        Vector3 movementDirection;
        Vector3 velocity, acceleration;
        float localRotation;

        float maxVelocity = 5f; //temporary test value
        float accelerationRate = 1f;
        float drag = 0.1f;
        float slowestSpeed = 0.5f;
        */

        //BoxCollider box;

        public Enemy(Game game, ObjectManager objectManager, Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(game, objectManager, startPosition, model, camera)
        {
            this.player = player;

            position = startPosition;
            lookDirection = Vector3.Right;
            orientation = (float)Math.Atan2(lookDirection.X, lookDirection.Z);
            targetPosition = player.position;
            velocity = Vector3.Zero;

            /*this.player = player; //so it can get the player's position for pursuing
            position = startPosition;
            hasLighting = true;
            scale = Matrix.CreateScale(5f);
            *///vary depending on size of ghost model (haven't been able to get one to work yet :/)
        }

        public override void Initialize()
        {
            /*velocity = Vector3.Zero;
            movementDirection = Vector3.Zero;
            acceleration = Vector3.Zero;
            localRotation = 0f;

            translation = Matrix.CreateTranslation(position);
            */
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //if (player.position != position) handleMovement(gameTime);
            handleMovement(gameTime);

            base.Update(gameTime);
        }

        private void handleMovement(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            targetPosition = player.position;

            if (lookDirection != Vector3.Zero) lookDirection.Normalize();

            float distanceToTarget = Vector3.Distance(position, targetPosition);
            if (distanceToTarget > stoppingDistFromTarget)
            {
                Vector3 targetDirection = Vector3.Subtract(targetPosition, position);
                float newOrientation = (float)Math.Atan2(targetDirection.X, targetDirection.Z);
                float rotationDirection = newOrientation > orientation ? +1 : -1;
                float rotationalVelocity = rotationalSpeed * rotationDirection;

                if (Math.Abs(newOrientation - orientation) > orientationDifference) orientation += rotationalVelocity * deltaTime;

                Vector3 addVector = Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(orientation));
                lookDirection += addVector;

                if ((velocity += lookDirection * acceleration).Length() > maxVelocity) velocity = Vector3.Normalize(velocity) * maxVelocity;
            }
            else
            {
                velocity = Vector3.Zero;
            }

            position += velocity * deltaTime;

            translation = Matrix.CreateTranslation(position);
            rotation = Matrix.CreateRotationY(orientation);

            /*Vector3 difference = player.position - position;
            difference.Normalize();

            //handles XYZ movement (translate)

            //acceleration = Vector3.Zero;
            acceleration = Vector3.Multiply(difference, accelerationRate);
            acceleration.Y = 0;

            velocity -= Vector3.Multiply(velocity, drag);

            if ((velocity).Length() < slowestSpeed)
            {
                velocity = Vector3.Zero;
            }

            velocity += acceleration * deltaTime;

            if ((velocity).Length() > maxVelocity)
            {
                velocity.Normalize();
                velocity = Vector3.Multiply(velocity, maxVelocity);
            }

            position += velocity * deltaTime;
            translation = Matrix.CreateTranslation(position);

            //handle rotation (on Y axis) (no other axes needed at this stage)

            localRotation = Vector3ToRadian(difference);
            rotation = Matrix.CreateRotationY(localRotation); */
        }

        private float Vector3ToRadian (Vector3 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y);
        }

        public override Matrix GetWorld()
        {
            return scale * base.rotation * translation;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
