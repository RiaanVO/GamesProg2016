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
        float maxVelocity = 100f; 
        //float gravity = 0.1f;
        float accerationRate = 10;
        float drag = 0.01f;
        float slowestSpeed = 0.5f;
        bool jumped = false;

        BoxCollider box;


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
            velocity = acceleration = Vector3.Zero;
            
            //Set up the collider
            //box = new BoxCollider();

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
            //handleMovement();

            //NOTE: NEEDS TESTING (and almost certainly debugging)
            //CollisionManager.checkCollision(box);

            camera.setCameraPositionDirection(position, lookDirection);
            camera.Update(gameTime);

            base.Update(gameTime);
        }

        //Without collision detection at the moment
        private void handleMovement() {
            //applying acceretion to the player
            movementDirection.Normalize();
            acceleration = movementDirection * accerationRate;
            //Application of gravity - does not include collision with the floor yet
            /*
            acceleration.Y = -gravity;
            if (position.Y < 10) {
                acceleration.Y = 0;
            }

            if (jumped)
            {
                acceleration.Y = 10;
                jumped = false;
            }*/

            if ((velocity -= velocity * drag).Length() < slowestSpeed) {
                velocity *= 0;
            }
            if ((velocity += acceleration).Length() > maxVelocity) {
                velocity.Normalize();
                velocity *= maxVelocity;
            }

            //Collision code goes here to determine if the player should move.

            position += velocity;

            //Reset the accerelation to zero
            acceleration *= 0;
        }

        private void handleInput() {
            //Get the states of the keyboard and mouse
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();


            //Resets the movement direction to zero, then checks which keys are pressed to create a new movement direction.
            lookDirection.Normalize();
            movementDirection *= 0;
            if (keyboardState.IsKeyDown(Keys.W))
                movementDirection += lookDirection;
            if (keyboardState.IsKeyDown(Keys.S))
                movementDirection -= lookDirection;
            if (keyboardState.IsKeyDown(Keys.A))
                movementDirection -= Vector3.Cross(Vector3.Up, lookDirection);
            if (keyboardState.IsKeyDown(Keys.D))
                movementDirection += Vector3.Cross(Vector3.Up, lookDirection);
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
            
            camera.setCameraDirection(lookDirection);

            //Resets the mouses position to the center of the screen, also resets the prevouse mouse state so the camera wont jump around
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
        }



    }
}
