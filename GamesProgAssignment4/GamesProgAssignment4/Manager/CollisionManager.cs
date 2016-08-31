using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GamesProgAssignment4
{
    static class CollisionManager
    {
        static List<Collider> colliders;

        static CollisionManager()
        {

        }

        /// <summary>
        /// Gets all collider objects that the given collider is intersecting with
        /// </summary>
        /// <param name="collider">The collider to check against all other colliders</param>
        /// <returns></returns>
        public static List<Collider> collidingWith(Collider collider)
        {
            List<Collider> collidingWith = new List<Collider>();

            foreach (Collider c in colliders)
            {
                if (c.isColliding(collider))
                {
                    collidingWith.Add(c);
                }
            }
            return collidingWith;
        }

        /// <summary>
        /// Checks the given collider against all other colliders
        /// </summary>
        /// <param name="collider">The collider to check against all other colliders</param>
        /// <returns></returns>
        public static bool checkCollision(Collider collider)
        {
            //NEEDS TO EXCLUDE ITSELF FROM CHECKS!

            foreach (Collider c in colliders)
            {
                if ( c.isColliding(collider))
                {
                    return true;
                }
            }
            return false;
        }

        public static void addCollider(Collider collider)
        {
            colliders.Add(collider);
        }
        
        public static void removeCollider(Collider collider)
        {
            colliders.Remove(collider);
        }
    }
}
