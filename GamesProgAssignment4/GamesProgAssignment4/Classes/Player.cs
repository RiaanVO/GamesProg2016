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
        MouseState prevMouseState;
        protected BasicCamera camera;
        Vector3 lookDirection;

        //Physics stuff
        Vector3 movementDirection;

        BoxCollider box;


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

            //Quick initialize to look forward
            this.camera = camera;
            lookDirection = startPos + Vector3.Forward;

            box = new BoxCollider();
        }


        public override void Update(GameTime gameTime)
        {
            //Update player position / look direction according to mouse input

            //NOTE: NEEDS TESTING (and almost certainly debugging)
            CollisionManager.checkCollision(box);

            camera.CreateLookAt(position, lookDirection);

            base.Update(gameTime);
        }

        

    }
}
