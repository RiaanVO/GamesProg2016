using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GamesProgAssignment4
{
    class SphereCollider : Collider
    {
        public BoundingSphere collider;

        public SphereCollider() : base()
        {
            
        }

        //Pass in collider to compare
        /// <summary>
        /// Checks if this collider is colliding with another collider.
        /// </summary>
        /// <param name="otherCollider">The collider to check this object against.</param>
        /// <returns></returns>
        public override bool isColliding(Collider otherCollider)
        {
            SphereCollider sphere = otherCollider as SphereCollider;
            if (sphere != null)
            {
                return collider.Intersects(sphere.collider);
            }

            BoxCollider box = otherCollider as BoxCollider;
            if (box != null)
            {
                return collider.Intersects(box.collider);
            }

            return false;
        }
    }
}
