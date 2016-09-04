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

        public BoxCollider(Game game, Vector3 position) : base(game, position)
        {
            halfSize = 0.5f;
            updateColliderPos();
        }

        /// <summary> </summary>
        /// <param name="game"></param>
        /// <param name="position">The centre of the box</param>
        /// <param name="size">Length of each side of the box</param>
        public BoxCollider(Game game, Vector3 position, float size) : base(game, position)
        {
            halfSize = size / 2;
            updateColliderPos();
        }

        /// <summary>
        /// Creates a new BoxCollider
        /// </summary>
        /// <param name="min">The minimum point the BoundingBox includes. One corner of the box (lower-left if looking along positive Z)</param>
        /// <param name="max">The maximum point the BoundingBox includes. Other corner of the box (upper-right if looking along positive Z)</param>
        public BoxCollider(Game game, Vector3 position, Vector3 min, Vector3 max) : base(game, position)
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
