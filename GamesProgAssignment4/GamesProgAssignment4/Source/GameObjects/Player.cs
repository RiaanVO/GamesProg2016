using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace GamesProgAssignment4
{
    class Player : GameObject
    {
        BasicCamera camera;

        //Looking varibles
        Vector3 lookDirection;
        Vector3 headHeightOffset = new Vector3(0, 5, 0);
        MouseState prevMouseState;
        float pitchRotationRate = MathHelper.PiOver4 / 150;
        float currentPitch = 0;// = MathHelper.PiOver2;
        float maxPitch = MathHelper.PiOver2 * (19 / 20f);

        //Movement varibles
        Vector3 movementDirection;
        float accelerationRate = 200f;
        float decelerationRate = 300f;
        Vector3 acceleration;
        float maxVelocity = 50f;
        Vector3 velocity;
        float minVelocity = 5f;


        bool jumped = false;
        bool grounded = true;
        float jumpVelocity = 50f;
        float fallRate = 200f; // Is gravity

        bool hasKey = false;

        SphereCollider collider;
        float colliderRadius = 1f;

        AudioListenerComponet audioListenerComponent;
        AudioEmitterComponent audioEmitterComponent;

        public Player(Game game, ObjectManager objectManager, Vector3 position, BasicCamera camera) : base(game, objectManager, position)
        {
            this.camera = camera;
            audioListenerComponent = new AudioListenerComponet(game, this);
            audioEmitterComponent = new AudioEmitterComponent(game, this);
            audioEmitterComponent.createSoundEffectInstance("footsteps", game.Content.Load<SoundEffect>(@"Sounds/footsteps"), false, true, false);
        }

        public override void Initialize()
        {
            collider = new SphereCollider(game, this, objectTag.player, true, true, colliderRadius);
            lookDirection = Vector3.Forward;
            velocity = Vector3.Zero;
            acceleration = Vector3.Zero;
            lookDirection = Vector3.Forward;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            handleInput();
            handleMovement(gameTime);
            camera.setCameraPositionDirection(position + headHeightOffset, lookDirection);

            if (velocity.Length() > 0)
            {
                audioEmitterComponent.setInstancePlayback("footsteps", true);
            }
            else
            {
                audioEmitterComponent.setInstancePlayback("footsteps", false);
            }

            base.Update(gameTime);
        }

        //Without collision detection at the moment
        private void handleMovement(GameTime gameTime)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            if (movementDirection != Vector3.Zero)
                movementDirection.Normalize();

            //Acceleration and deceleration if the player is moving or not
            acceleration = movementDirection * accelerationRate;
            Vector2 horizontalVelocity = new Vector2(velocity.X, velocity.Z);

            if (acceleration == Vector3.Zero && horizontalVelocity != Vector2.Zero)
            {
                Vector2 horizontalDeceleration = Vector2.Normalize(horizontalVelocity) * -decelerationRate;
                acceleration = new Vector3(horizontalDeceleration.X, acceleration.Y, horizontalDeceleration.Y);
                if (horizontalVelocity.Length() < minVelocity)
                {
                    velocity = Vector3.Zero;
                }
            }
            //Remove y component to be calculated seperatly;
            acceleration.Y = 0;
            velocity += acceleration * deltaTime;

            if (horizontalVelocity.Length() > maxVelocity)
            {
                horizontalVelocity = Vector2.Normalize(horizontalVelocity) * (maxVelocity - 1);
                velocity = new Vector3(horizontalVelocity.X, velocity.Y, horizontalVelocity.Y);
            }

            acceleration = Vector3.Zero;
            if (!grounded)
                acceleration = Vector3.Down * fallRate;

            if (grounded && jumped)
            {
                velocity += Vector3.Up * jumpVelocity;
                grounded = jumped = false;
            }

            velocity += acceleration * deltaTime;

            if (position.Y < 0)
            {
                velocity.Y = 0;
                acceleration.Y = 0;
                grounded = true;
                position = new Vector3(position.X, 0, position.Z);
            }

            //Collision code goes here to determine if the player should move.
            if (collider.collidingWith.Count != 0)
            {
                //do something, handle collision with object
                //Random bouncing off a cube lmao
                
            }

            position += velocity * deltaTime;

        }

        private void handleInput()
        {
            //Get the states of the keyboard and mouse
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            //Resets the movement direction to zero, then checks which keys are pressed to create a new movement direction.
            if (lookDirection != Vector3.Zero)
                lookDirection.Normalize();

            movementDirection = Vector3.Zero;

            if (keyboardState.IsKeyDown(Keys.W))
                movementDirection += lookDirection;
            if (keyboardState.IsKeyDown(Keys.S))
                movementDirection -= lookDirection;
            if (keyboardState.IsKeyDown(Keys.A))
                movementDirection += Vector3.Cross(Vector3.Up, lookDirection);
            if (keyboardState.IsKeyDown(Keys.D))
                movementDirection -= Vector3.Cross(Vector3.Up, lookDirection);
            //Set jumped to true if the condition is correct
            jumped = (keyboardState.IsKeyDown(Keys.Space) && !jumped);

            //Sets the yaw rotation of the cameras' look direction
            lookDirection = Vector3.Transform(lookDirection, Matrix.CreateFromAxisAngle(Vector3.Up, (-MathHelper.PiOver4 / 150) * (mouseState.X - prevMouseState.X)));

            //Sets the pitch rotation of the cameras look direction, maxes out so that the player cant look directly up or down
            float pitchAngle = (pitchRotationRate) * (mouseState.Y - prevMouseState.Y);
            if (Math.Abs(currentPitch + pitchAngle) < maxPitch)
            {
                lookDirection = Vector3.Transform(lookDirection, Matrix.CreateFromAxisAngle(Vector3.Cross(Vector3.Up, lookDirection), pitchAngle));
                currentPitch += pitchAngle;
            }

            //Resets the mouses position to the center of the screen, also resets the prevouse mouse state so the camera wont jump around
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
        }

        public void setHasKey(bool hasKeyStatus)
        {
            hasKey = hasKeyStatus;
        }
    }
}
