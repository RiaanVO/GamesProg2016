﻿using System;
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
        //Min point is essentially = to position.
        //TODO: Can probably remove minPoint altogether and replace with position
        Vector3 minPoint;
        //Essentially holds the size of each box side, to recalc the maxPoint each time
        Vector3 relativeMaxPt;
        Vector3 maxPoint;


        /// <summary>
        /// Creates new cube with size 1 starting with position as min corner
        /// </summary>
        /// <param name="game"></param>
        /// <param name="obj"></param>
        /// <param name="isKinematic"></param>
        public BoxCollider(Game game, GameObject obj, objectTag tag, bool checkCollision, bool isKinematic) : base(game, obj, tag, checkCollision, isKinematic)
        {
            minPoint = position;
            maxPoint = position + new Vector3(1, 1, 1);
            relativeMaxPt = maxPoint - minPoint;
            updateColliderPos();
        }

        /*NOTE: Currently commented out because it does not take into account the 'position' variable in the base class
        /// <summary>
        /// Creates a new *cube* collider using centre and size of sides
        /// </summary>
        /// <param name="game"></param>
        /// <param name="obj"></param>
        /// <param name="centre"></param>
        /// <param name="sideLength"></param>
        public BoxCollider(Game game, GameObject obj, bool isKinematic, Vector3 centre, float sideLength) : base(game, obj, isKinematic)
        {
            //collider = new BoundingBox(min, max);
            float halfSize = sideLength / 2;
            minPoint = new Vector3(centre.X - halfSize,centre.Y - halfSize, centre.Z - halfSize);
            maxPoint = new Vector3(centre.X + halfSize, centre.Y + halfSize, centre.Z + halfSize);
            updateColliderPos();
        }*/

        /// <summary>
        /// Creates a new BoxCollider (any type/size)
        /// </summary>
        /// <param name="min">The minimum point the BoundingBox includes. One corner of the box (lower-left if looking along positive Z)</param>
        /// <param name="max">The maximum point the BoundingBox includes. Other corner of the box (upper-right if looking along positive Z)</param>
        /// <param name="checkCollision">Whether or not to actually check collision for itself</param>
        /// <param name="isKinematic">Whether or not the object/collider is able to move around</param>
        /// <param name="tag">The object tag for this collider</param>
        public BoxCollider(Game game, GameObject obj, objectTag tag, bool checkCollision, bool isKinematic, Vector3 min, Vector3 max) : base(game, obj, tag, checkCollision, isKinematic)
        {
            //collider = new BoundingBox(min, max);
            //halfSize = (max.X - min.X) / 2;s
            minPoint = position = min;
            maxPoint = max;
            relativeMaxPt = maxPoint - minPoint;
            updateColliderPos();
        }

        /// <summary>
        /// Updates the BoundingBox's position
        /// </summary>
        public override void updateColliderPos()
        {
            //Update the min and max points with the position
            minPoint = position;
            maxPoint = minPoint + relativeMaxPt;
            collider = new BoundingBox(minPoint, maxPoint);
        }

        /// <summary>
        /// Checks collisions against another collider
        /// </summary>
        /// <param name="otherCollider"></param>
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
