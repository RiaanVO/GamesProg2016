using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class SphereCollider : Collider
    {
        #region fields
        private BoundingSphere collider;
        private float radius;
        //Used for centering/ offseting the position of the collider from the game object
        #endregion

        #region properties
        public BoundingSphere Collider {
            get { return collider; }
        }
        public float Radius {
            get { return radius; }
            set { radius = value; }
        }
        #endregion

        public SphereCollider(GameObject gameObject, ObjectTag tag, float radius) : base(gameObject, tag)
        {
            this.radius = radius;
            collider = new BoundingSphere(position, radius);
        }

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
                return collider.Intersects(box.Collider);
            }

            return false;
        }

        public override float? intersectsRay(Ray ray)
        {
            return collider.Intersects(ray);
        }

        public override void updateColliderPos(Vector3 newPosition)
        {
            if (newPosition + positionOffset == position)
                return;

            position = newPosition + positionOffset;
            collider.Center = position;

            if (quadTreeNode != null)
                quadTreeNode.moveCollider(this);
        }

        #region debug drawing
        /// <summary>
        /// 
        /// </summary>
        public override void drawCollider()
        {
            if(!isRendered && collider != null)
            {
                WireShapeDrawer.AddBoundingSphere(collider, drawColour, CollisionManager.renderTime);
                isRendered = true;
            }
        }

        public override List<Collider> getCollisions()
        {
            return CollisionManager.GetCollidingWith(this);
        }
        #endregion
    }
}
