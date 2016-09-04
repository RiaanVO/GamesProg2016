using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class Player : GameObject
    {
        protected BasicCamera camera;

        Vector3 lookDirection;
        MouseState prevMouseState;
        //float yawRotationRate = ;
        float pitchRotationRate = MathHelper.PiOver4 / 150;
        float currentPitch;// = MathHelper.PiOver2;
        float maxPitch = MathHelper.PiOver2 * (19 / 20f);


        //Physics stuff
        Vector3 movementDirection;
        Vector3 velocity, acceleration;
        float maxVelocity = 20f; 
        //float gravity = 0.1f;
        float accerationRate = 2f;
        float drag = 0.1f;
        float slowestSpeed = 0.5f;
        bool jumped = false;

        //BoxCollider box;
        SphereCollider col;
        float colliderRadius = 5f;

        //Limited constructor
        public Player(Game game, Vector3 startPos) :
            this(game, startPos, new BasicCamera(game, startPos, Vector3.Forward, Vector3.Up))
        {

        }

        //Main Constructor
        public Player(Game game, Vector3 startPos, BasicCamera camera) : base(startPos, game)
        {
            this.camera = camera;
        }

        public override void Initialize()
        {
            //Set up cameras' values and create a look at
            lookDirection = Vector3.Forward;
            //Initilise the movement varibles
            velocity = Vector3.Zero;
            movementDirection = Vector3.Zero;
            acceleration = Vector3.Zero;


            //System.Console.WriteLine("Acc: " + acceleration);
            //System.Console.WriteLine("Vel: " + velocity);
            //System.Console.WriteLine("Pos: " + position);
            //System.Console.WriteLine("Loo: " + lookDirection);

            //Set up the collider
            //box = new BoxCollider();
            col = new SphereCollider(game, position, colliderRadius);

            base.Initialize();
        }

        /// <summary>
        /// Upadates the players input, looking and moving direction, collision, and camera
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //lookDirection = new Vector3(0, 0, -3);//position + Vector3.Forward;
            //lookDirection.Normalize();
            handleInput();
            handleMovement();

            //NOTE: NEEDS TESTING (and almost certainly debugging)
            //CollisionManager.checkCollision(box);
            //System.Console.WriteLine("Acc: " + acceleration);
            //System.Console.WriteLine("Vel: " + velocity);
            //System.Console.WriteLine("Pos: " + position);
            //System.Console.WriteLine("Loo: " + lookDirection);

            camera.setCameraPositionDirection(position, lookDirection);
            //camera.Update(gameTime);

            base.Update(gameTime);
        }

        //Without collision detection at the moment
        private void handleMovement() {
            //applying acceretion to the player
            acceleration = Vector3.Zero;
            acceleration = Vector3.Multiply(movementDirection, accerationRate);
            
            //Application of gravity - does not include collision with the floor yet
            /*
            acceleration.Y = -gravity;
            if (position.Y < 10) {
                acceleration.Y = 0;
                position.Y = 10;
            }

            if (jumped)
            {
                acceleration.Y = 10;
                jumped = false;
            }*/
            velocity -= Vector3.Multiply(velocity, drag);

            if ((velocity).Length() < slowestSpeed) {
                velocity = Vector3.Zero;
            }

            velocity += acceleration;

            if ((velocity).Length() > maxVelocity) {
                velocity.Normalize();
                velocity = Vector3.Multiply(velocity, maxVelocity);
            }

            //Collision code goes here to determine if the player should move.

            position += velocity;
        }

        private void handleInput() {
            //Get the states of the keyboard and mouse
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            //Resets the movement direction to zero, then checks which keys are pressed to create a new movement direction.
            if(lookDirection != Vector3.Zero)
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

            if (movementDirection != Vector3.Zero)
                movementDirection.Normalize();

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
    }
}
