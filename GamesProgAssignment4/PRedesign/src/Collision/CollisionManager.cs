using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    static class CollisionManager
    {
        private static QuadTree quadTree;
        private static List<Collider> colliders = new List<Collider>();

        public static void addCollider(Collider collider)
        {
            if (quadTree != null)
            {
                //quadTree = new QuadTree(LevelManager.LevelEnclosure);
                quadTree.Add(collider);
            }
            colliders.Add(collider);
        }

        public static void constructQuadTree() {
            //quadTree = null;
            quadTree = new QuadTree(LevelManager.LevelEnclosure);
            foreach(Collider collider in colliders)
            {
                quadTree.Add(collider);
            }
        }

        /// <summary>
        /// Removes a game object from the master list
        /// </summary>
        /// <param name="gameObject"></param>
        public static void removeCollider(Collider collider)
        {
            if(quadTree != null)
                quadTree.Remove(collider);
            colliders.Remove(collider);
        }

        /// <summary>
        /// Removes all game objects from the master list, the current list will be cleared next update or draw
        /// </summary>
        public static void clearAll()
        {
            if (quadTree != null) {
                quadTree.UpdateTree();
                foreach (Collider collider in colliders)
                {
                    collider.QuadTreeNode = null;
                    quadTree.Remove(collider);
                }
                quadTree.TreeBuilt = false;
            }
            colliders.Clear();
        }

        public static void Render(Color colour, bool showRegions, bool showColliders) {
            if (quadTree != null)
                quadTree.RenderTree(colour, showRegions, showColliders);
        }

        public static void ForceTreeConstruction() {
            if (quadTree != null)
                quadTree.UpdateTree();
        }

        public static List<Collider> GetCollidingWith(Collider collider) {
            if (quadTree != null)
                return quadTree.collidingWith(collider);
            return new List<Collider>();
        }
    }
}
