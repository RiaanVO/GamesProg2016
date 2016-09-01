using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GamesProgAssignment4
{
    class BoxCollider : Collider
    {
        public BoundingBox collider;

        public BoxCollider() : base()
        {
            
        }

        //Pass in collider to compare
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
