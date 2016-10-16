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

        //Debugging
        public const float renderTime = 360;

        public static void addCollider(Collider collider)
        {
            if (quadTree != null)
                quadTree.Add(collider);
            colliders.Add(collider);

        }

        public static void constructQuadTree()
        {
            Console.WriteLine("Quadtree size" + LevelManager.LevelEnclosure);
            quadTree = new QuadTree(LevelManager.LevelEnclosure);
            foreach (Collider collider in colliders)
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
            if (quadTree != null)
                quadTree.Remove(collider);
            colliders.Remove(collider);
        }

        /// <summary>
        /// Removes all game objects from the master list, the current list will be cleared next update or draw
        /// </summary>
        public static void clearAll()
        {
            if (quadTree != null)
            {
                quadTree.UpdateTree();
                foreach (Collider collider in colliders)
                {
                    collider.QuadTreeNode = null;
                    quadTree.Remove(collider);
                }
                quadTree.TreeBuilt = false;
                PruneQuadTree();

            }
            colliders.Clear();
            
        }

        public static void Render(Color colour, bool showRegions, bool showColliders)
        {
            if (quadTree != null)
                quadTree.RenderTree(colour, showRegions, showColliders);
        }

        public static void ForceTreeConstruction()
        {
            Console.WriteLine("QuadTree status: " + (quadTree != null));
            if (quadTree != null)
                quadTree.UpdateTree();
        }

        public static List<Collider> GetCollidingWith(Collider collider)
        {
            if (quadTree != null)
                return quadTree.collidingWith(collider);
            return new List<Collider>();
        }

        public static string Stats()
        {
            return "Number of colliders: " + colliders.Count + "\nCurrent active branches: " + quadTree.countActiveBranches();
        }

        public static void PruneQuadTree() {
            if (quadTree != null)
            {
                quadTree.UpdateTree();
                quadTree.pruneDeadBranches();
            }
            Console.WriteLine("\n" + Stats());
        }

        public static void resetRender() {
            foreach (Collider collider in colliders)
                collider.IsRendered = false;
            if (quadTree != null)
                quadTree.resetRender();
        }
    }
}
