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

        public override void updateColliderPos(Vector3 newPosition)
        {
            position = newPosition + positionOffset;
            collider.Center = position;
        }

        #region debug drawing
        /// <summary>
        /// 
        /// </summary>
        public override void drawCollider()
        {
            //ObjectMetaDrawer.RenderBoundingSphere(collider, drawColour);
        }
        #endregion
    }
}
