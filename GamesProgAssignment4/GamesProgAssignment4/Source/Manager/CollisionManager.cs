using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GamesProgAssignment4
{
    class CollisionManager : GameComponent
    {
        List<Collider> colliders;
        float elapsedTime;
        //Milliseconds between each collision check
        float tickRate = 100;

        public CollisionManager(Game game) : base(game)
        {
            colliders = new List<Collider>();
        }

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
            //NEEDS TO EXCLUDE 'collider' ITSELF FROM CHECKS!
            foreach (Collider c1 in colliders)
            {
                foreach (Collider c2 in colliders)
                {
                    if (c1.isColliding(c2))
                    {
                        
                    }
                }
            }
            //return false;
        }

        public void updateColliderPositions()
        {
            foreach (Collider c in colliders)
            {
                c.updateColliderPos();
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
                //update collider positions
                updateColliderPositions();
                //check all collisions
                checkAllCollisions();
            }
            else
            {
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            }
        }
    }
}
