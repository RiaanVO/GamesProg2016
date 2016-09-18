using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    /// <summary>
    /// Used for updating and drawing the objects
    /// </summary>
    static class ObjectManager
    {
        #region Fields
        //Current list will copy master before update to avoid adding and removing errors
        private static List<GameObject> objectsMaster = new List<GameObject>();
        private static List<GameObject> objectsCurrent = new List<GameObject>();

        //Need to find a better location for these varibles
        private static GraphicsDevice graphicsDevice;
        private static BasicCamera camera;
        private static Game game;
        #endregion

        #region Properties
        public static GraphicsDevice GraphicsDevice {
            get { return graphicsDevice; }
            set { graphicsDevice = value; }
        }

        public static BasicCamera Camera {
            get { return camera; }
            set { camera = value; }
        }

        public static Game Game {
            get { return game; }
            set { game = value; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Tell all game objects to update themselves
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime) {
            objectsCurrent.Clear();
            objectsCurrent.AddRange(objectsMaster);

            foreach (GameObject obj in objectsCurrent)
                obj.Update(gameTime);
        }

        /// <summary>
        /// Tell all game objects to draw themselves
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Draw(GameTime gameTime) {
            objectsCurrent.Clear();
            objectsCurrent.AddRange(objectsMaster);

            foreach (GameObject obj in objectsCurrent)
                obj.Draw(gameTime);
        }

        /// <summary>
        /// Adds a game object to the master list
        /// </summary>
        /// <param name="gameObject"></param>
        public static void addGameObject(GameObject gameObject) {
            objectsMaster.Add(gameObject);
        }

        /// <summary>
        /// Removes a game object from the master list
        /// </summary>
        /// <param name="gameObject"></param>
        public static void removeGameObject(GameObject gameObject) {
            objectsMaster.Remove(gameObject);
        }

        /// <summary>
        /// Removes all game objects from the master list, the current list will be cleared next update or draw
        /// </summary>
        public static void clearAll() {
            objectsMaster.Clear();
        }
        #endregion

    }
}
