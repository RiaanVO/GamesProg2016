using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GamesProgAssignment4
{
    abstract class Collider
    {
        //Generic class, inherited by bounding boxes and spheres
        BoundingBox box;
        public Collider()
        {
            CollisionManager.addCollider(this);
        }

        public abstract bool isColliding(Collider otherCollider);
    }
}
