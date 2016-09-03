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

        float radian = MathHelper.Pi / 180.0f;
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

            translate = Matrix.CreateTranslation(position);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //update position:
            //set movementDirection vector to the vector between the enemy and player.position
            //rotate model depending on which way it needs to face as per movementDirection vector
            //apply incremental movement speed etc.

            //going to use this for getting pursuit code:
            //http://xnafan.net/2012/12/pointing-and-moving-towards-a-target-in-xna-2d/
            //will work for this as the enemy cannot move in one plane anyway

            base.Update(gameTime);
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
