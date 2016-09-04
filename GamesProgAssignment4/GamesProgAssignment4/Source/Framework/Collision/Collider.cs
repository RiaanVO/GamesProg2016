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
        //BoundingBox box;
        public List<Collider> collidingWith;
        protected GameObject obj;
        protected Vector3 position { get; set; }
        //Used to determine if position should be updated regularly or not
        public bool isKinematic;

        public Collider(Game game, GameObject obj, bool isKinematic)
        {
            collidingWith = new List<Collider>();
            this.obj = obj;
            position = obj.position;
            game.Services.GetService<CollisionManager>().addCollider(this);
        }

        public abstract bool isColliding(Collider otherCollider);

        public virtual void updatePos()
        {
            if (!isKinematic)
            {
                position = obj.position;
            }
        }

        /// <summary>
        /// Recreates the collider using the position and other given variables.
        /// </summary>
        public abstract void updateColliderPos();
    }
}
