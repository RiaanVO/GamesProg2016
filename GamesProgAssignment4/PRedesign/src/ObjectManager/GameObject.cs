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
        protected float orientation;
        protected Vector3 velocity;
        protected float angular;
        #endregion

        #region Properties
        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }

        public float Orientation {
            get { return orientation; }
            set { orientation = value; }
        }

        public Vector3 Velocity{
            get { return velocity; }
            set { velocity = value; }
        }

        public float Angular
        {
            get { return angular; }
            set { angular = value; }
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GameObject() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
