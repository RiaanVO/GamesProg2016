using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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
        float maxVelocity = 10f;
        float acceleration = 0.5f;

        float stoppingDistFromTarget = 1;

        SphereCollider collider;
        AudioEmitterComponent audioEmitter;

        bool playerCaught = false;

        public Enemy(Game game, ObjectManager objectManager, Vector3 startPosition, Model model, BasicCamera camera, Player player) : base(game, objectManager, startPosition, model, camera)
        {
            this.player = player;

            position = startPosition;
            lookDirection = Vector3.Right;
            orientation = (float)Math.Atan2(lookDirection.X, lookDirection.Z);
            targetPosition = player.position;
            velocity = Vector3.Zero;
            hasLighting = false;

            collider = new SphereCollider(game, this, objectTag.enemy, true, true, 2);
            audioEmitter = new AudioEmitterComponent(game, this);
            audioEmitter.createSoundEffectInstance("heartbeat", game.Content.Load<SoundEffect>(@"Sounds/heartbeat"), true, true, true);
            audioEmitter.addSoundEffect("Scream", game.Content.Load<SoundEffect>(@"Sounds/scaryscream"));

        }

        public override void Update(GameTime gameTime)
        {
            handleMovement(gameTime);

            if (collider.collidingWith.Count != 0) {
                foreach(Collider col in collider.collidingWith)
                {
                    if(col.tag == objectTag.player && !playerCaught)
                    {
                        audioEmitter.playSoundEffect("Scream", 0.1f);
                        playerCaught = true;
                        objectManager.resetGame = true;
                    }
                }
            }

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
                if (targetDirection != Vector3.Zero) targetDirection.Normalize();

                float newOrientation = (float)Math.Atan2(targetDirection.X, targetDirection.Z);

                if (lookDirection != targetDirection)
                {
                    //Also multiply by turning acceleration?
                    lookDirection += targetDirection * 0.1f;
                    if (lookDirection != Vector3.Zero)
                        lookDirection.Normalize();

                    //Stops the enemy trying to be a spooky flying enemy
                    lookDirection.Y = 0;
                }

                //float rotationDirection = newOrientation > orientation ? +1 : -1;
                //float rotationalVelocity = rotationalSpeed * rotationDirection;

                //if (Math.Abs(newOrientation - orientation) > orientationDifference) orientation += rotationalVelocity * deltaTime;

                //Vector3 addVector = Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(orientation));
                //lookDirection += addVector;

                if ((velocity += lookDirection * acceleration).Length() > maxVelocity) velocity = Vector3.Normalize(velocity) * (maxVelocity-1);
            }
            else
            {
                velocity = Vector3.Zero;
            }

            position += velocity * deltaTime;

            translation = Matrix.CreateTranslation(position);
            rotation = Matrix.CreateRotationY((float)Math.Atan2(lookDirection.X, lookDirection.Z));
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
