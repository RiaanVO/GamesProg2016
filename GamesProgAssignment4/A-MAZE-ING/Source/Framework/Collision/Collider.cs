using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace A_MAZE_ING
{
    public enum objectTag
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
        //Generic class, inherited by bounding boxes and spheres
        //BoundingBox box;
        public List<Collider> collidingWith;
        public GameObject gameObject;
        public Vector3 position { get; protected set; }
        //Used to determine if position should be updated regularly or not
        public bool checkCollision;
        public bool isKinematic;
        public objectTag tag;

        public Collider(Game game, GameObject obj, objectTag tag, bool checkCollision, bool isKinematic)
        {
            collidingWith = new List<Collider>();
            this.gameObject = obj;
            position = obj.position;
            this.tag = tag;
            this.checkCollision = checkCollision;
            this.isKinematic = isKinematic;
            game.Services.GetService<CollisionManager>().addCollider(this);
        }

        public abstract bool isColliding(Collider otherCollider);

        public virtual void updatePos()
        {
            position = gameObject.position;
        }

        /// <summary>
        /// Recreates the collider using the position and other given variables.
        /// </summary>
        public abstract void updateColliderPos();
    }
}
