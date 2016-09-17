using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    abstract class GameObject
    {
        #region Fields
        protected Vector3 position;
        #endregion

        #region Properties
        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }
        #endregion

        #region Initialization
        public GameObject(Vector3 startPosition) {
            position = startPosition;
            ObjectManager.addGameObject(this);
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// The Update method of the game object, can be overwritten
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// The draw method of the game object, can be overwritten
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime) { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Removes itself from the object manager, should be over ridden to self remove from other managers as well
        /// </summary>
        public virtual void removeGameObject() {
            ObjectManager.removeGameObject(this);
        }
        #endregion
    }
}
