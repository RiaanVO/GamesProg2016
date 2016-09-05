using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GamesProgAssignment4
{

    /// <summary>
    /// Octtree tutorial: http://www.gamedev.net/page/resources/_/technical/game-programming/introduction-to-octrees-r3529
    /// </summary>
    class CollisionManager : GameComponent
    {
        List<Collider> colliders;
        float elapsedTime;
        //Milliseconds between each collision check (currently 20/second)
        //float tickRate = 50;
        //Test for 40 ticks/second
        float tickRate = 25;

        public CollisionManager(Game game) : base(game)
        {
            colliders = new List<Collider>();
        }

        /*
        /// <summary>
        /// Checks the given collider against all other colliders
        /// </summary>
        /// <param name="collider">The collider to check against all other colliders</param>
        /// <returns></returns>
        public bool checkCollision(Collider collider)
        {
            //NEEDS TO EXCLUDE 'collider' ITSELF FROM CHECKS!

            foreach (Collider c in colliders)
            {
                if (c.isColliding(collider))
                {
                    return true;
                }
            }
            return false;
        }
        */

        /// <summary>
        /// Gets all collider objects that the given collider is intersecting with
        /// </summary>
        /// <param name="collider">The collider to check against all other colliders</param>
        /// <returns></returns>
        public List<Collider> collidingWith(Collider collider)
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
        /// Updates each collider's 'collidingWith' list with everything that it is colliding with.
        /// </summary>
        public void checkAllCollisions()
        {
            //System.Console.WriteLine(colliders.Count);
            foreach (Collider c1 in colliders)
            {
                if (c1.checkCollision)
                {
                    //if (c1.gameObject as Player != null)
                        //System.Console.WriteLine("Is player");

                    foreach (Collider c2 in colliders)
                    {
                        if (!ReferenceEquals(c1, c2) && !c1.collidingWith.Contains(c2) && c1.isColliding(c2))
                        {
                            c1.collidingWith.Add(c2);
                            c2.collidingWith.Add(c1);
                        }
                    }
                }
            }
        }

        public void updateColliders()
        {
            foreach (Collider c in colliders)
            {
                if (c.isKinematic)
                {
                    c.updatePos();
                    c.updateColliderPos();
                }
                c.collidingWith.Clear();
            }
        }

        public void addCollider(Collider collider)
        {
            colliders.Add(collider);
        }

        public void removeCollider(Collider collider)
        {
            colliders.Remove(collider);
        }

        public override void Update(GameTime gameTime)
        {
            if (elapsedTime >= tickRate)
            {
                elapsedTime = 0;
                //update collider positions & clear 'collidingWith' lists
                updateColliders();
                checkAllCollisions();
            }
            else
            {
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            }
        }
    }
}
