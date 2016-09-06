using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A_MAZE_ING
{
    abstract class GameObject
    {
        protected Game game;
        protected ObjectManager objectManager;
        public Vector3 position { get; protected set; }
        
        /// <summary>
        /// For creating a new game object, must have a refference to the game and to the object manager
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="game"></param>
        public GameObject(Game game, ObjectManager objectManager, Vector3 startPosition)
        {
            this.game = game;
            this.objectManager = objectManager;
            position = startPosition;
        }

        /// <summary>
        /// For setting up the game object, Can be used to re initilise game object
        /// </summary>
        public virtual void Initialize(){}

        /// <summary>
        /// The update method of the game object, can be overwritten
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime){}

        /// <summary>
        /// The draw method of the game object, can be overwritten
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime){}
    }
}
