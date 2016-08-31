using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    abstract class GameObject
    {
        protected Vector3 position; //{ get; set; }
        protected ObjectManager manager;
        protected Game game;

        public GameObject(Vector3 startPos, Game game)
        {
            this.game = game;
            position = startPos;
        }

        public virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        //public virtual void Update(GameTime gameTime){ }
        
        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}
