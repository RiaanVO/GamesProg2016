using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PRedesign
{
    class Player : GameObject
    {
        #region Fields
        BasicCamera camera;

        Vector3 lookDirection;
        Vector3 headHeightOffset = new Vector3(0, 5, 0);
        MouseState prevMouseState;
        float pitchRotationRate = MathHelper.PiOver4 / 150;
        float currentPitch = 0;// = MathHelper.PiOver2;
        float maxPitch = MathHelper.PiOver2 * (19 / 20f);

        Game game;
        #endregion

        #region Properties
        public BasicCamera Camera {
            get { return camera; }
            set { camera = value; }
        }

        public Vector3 HeadHeightOffset {
            get { return headHeightOffset; }
            set { headHeightOffset = value; }
        }

        public Game Game {
            get { return game; }
            set { game = value; }
        }
        #endregion

        #region Initialization
        public Player(Vector3 startPosition) : base(startPosition)
        {
            lookDirection = Vector3.Forward;
        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            handleInput();

            camera.setPositionAndDirection(position + headHeightOffset, lookDirection);

            base.Update(gameTime);
        }

        private void handleInput() {
            //Get the states of the keyboard and mouse
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            //Resets the movement direction to zero, then checks which keys are pressed to create a new movement direction.
            if (lookDirection != Vector3.Zero)
                lookDirection.Normalize();
            
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
            if (game.Window != null)
                Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
        }
        #endregion

    }
}
