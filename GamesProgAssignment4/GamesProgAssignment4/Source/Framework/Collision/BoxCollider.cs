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
        //Variable to hold half the length of each side
        float halfSize;

        public BoxCollider(Game game, GameObject obj) : base(game, obj)
        {
            halfSize = 0.5f;
            updateColliderPos();
        }

        /// <summary>Creates box collider as a Cube</summary>
        /// <param name="game"></param>
        /// <param name="position">The centre of the *cube*</param>
        /// <param name="size">Length of each side of the *cube*</param>
        public BoxCollider(Game game, GameObject obj, float size) : base(game, obj)
        {
            halfSize = size / 2;
            updateColliderPos();
        }

        /// <summary>
        /// Creates a new BoxCollider (any type)
        /// </summary>
        /// <param name="min">The minimum point the BoundingBox includes. One corner of the box (lower-left if looking along positive Z)</param>
        /// <param name="max">The maximum point the BoundingBox includes. Other corner of the box (upper-right if looking along positive Z)</param>
        public BoxCollider(Game game, GameObject obj, Vector3 min, Vector3 max) : base(game, obj)
        {
            //collider = new BoundingBox(min, max);
            halfSize = (max.X - min.X) / 2;
            updateColliderPos();
        }

        /// <summary>
        /// Updates the BoundingBox's position (NOT WORKING)
        /// </summary>
        public override void updateColliderPos()
        {
            collider = new BoundingBox(new Vector3(position.X - halfSize, position.Y - halfSize, position.Z - halfSize),
                new Vector3(position.X + halfSize, position.Y + halfSize, position.Z + halfSize));
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
