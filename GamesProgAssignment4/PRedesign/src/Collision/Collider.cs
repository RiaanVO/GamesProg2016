using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    public enum ObjectTag
    {
        player,
        wall,
        door,
        obstacle,
        enemy,
        hazard,
        pickup
    }

    abstract class Collider
    {
        #region fields
        protected List<Collider> collidingWidth = new List<Collider>();
        protected GameObject gameObject;
        protected Vector3 position;
        protected Color drawColour = Color.Black;
        protected ObjectTag tag;
        protected Vector3 positionOffset = Vector3.Zero;
        #endregion

        #region properties
        public GameObject GameObject {
            get { return gameObject; }
        }

        public ObjectTag Tag {
            get { return tag; }
            set { tag = value; }
        }

        public Color DrawColour
        {
            set { drawColour = value; }
        }
        public Vector3 PositionOffset
        {
            get { return positionOffset; }
            set { positionOffset = value; }
        }
        #endregion

        public Collider(GameObject gameObject, ObjectTag tag) {
            this.gameObject = gameObject;
            position = gameObject.Position;
            this.tag = tag;
            CollisionManager.addCollider(this);
        }

        public abstract List<Collider> getCollisions();

        public abstract bool isColliding(Collider otherCollider);
        public abstract void updateColliderPos(Vector3 newPosition);
        public void Remove() {
            CollisionManager.removeCollider(this);
        }
        public abstract void drawCollider();
    }
}
