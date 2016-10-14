using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class BoxCollider : Collider
    {
        #region fields
        private BoundingBox collider;
        private bool isCentered = false; //If not it is assumend to be from bottom origion
        Vector3 boxSize;
        #endregion

        #region Properties
        public BoundingBox Collider {
            get { return collider; }
        }
        public bool IsCentered {
            set {
                isCentered = value;
                updateColliderPos(position);
            }
        }
        #endregion

        /// <summary>
        /// Creates a box collider, Box size is from the origion out to the firest point
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="tag"></param>
        /// <param name="boxSize"></param>
        public BoxCollider(GameObject gameObject, ObjectTag tag, Vector3 boxSize) : base(gameObject, tag)
        {
            this.boxSize = boxSize;
            updateColliderPos(gameObject.Position);
        }

        public override bool isColliding(Collider otherCollider)
        {
            SphereCollider sphere = otherCollider as SphereCollider;
            if (sphere != null)
            {
                return collider.Intersects(sphere.Collider);
            }

            BoxCollider box = otherCollider as BoxCollider;
            if (box != null)
            {
                return collider.Intersects(box.collider);
            }

            return false;
        }

        public override void updateColliderPos(Vector3 newPosition)
        {
            position = newPosition;
            Vector3 minPoint, maxPoint;
            if (isCentered) {
                minPoint = position - (boxSize / 2) + positionOffset;
                maxPoint = position + (boxSize / 2) + positionOffset;
            } else {
                minPoint = position + positionOffset;
                maxPoint = position + boxSize + positionOffset;
            }

            collider = new BoundingBox(minPoint, maxPoint);
        }

        

        

        public override void drawCollider()
        {
            WireShapeDrawer.AddBoundingBox(collider, drawColour);
        }
    }
}
