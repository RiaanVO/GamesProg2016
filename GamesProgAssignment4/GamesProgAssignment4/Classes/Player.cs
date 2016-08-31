using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4.Classes
{
    class Player : GameObject
    {
        MouseState prevMouseState;
        BasicCamera camera;

        Vector3 lookDirection;
        BasicMovement movement;

        //Limited constructor
        public Player(Vector3 startPos, Game game) :
            this(startPos, game, new BasicCamera(game, startPos, Vector3.Forward, Vector3.Up))
        {

        }

        //Main Constructor
        public Player(Vector3 startPos, Game game, BasicCamera camera) : base(startPos, game)
        {
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();

            //Quick initialize on forward
            lookDirection = startPos + Vector3.Forward;
            movement = new BasicMovement(ref position);
        }


        public override void Update(GameTime gameTime)
        {
            //Update player position / look direction according to mouse input
            


            camera.CreateLookAt(position, lookDirection);

            base.Update(gameTime);
        }

        

    }
}
