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
        protected Vector3 position { get; set; }

        public Collider(Game game, Vector3 position)
        {
            collidingWith = new List<Collider>();
            this.position = position;
            game.Services.GetService<CollisionManager>().addCollider(this);

        }

        public abstract bool isColliding(Collider otherCollider);

        /// <summary>
        /// Recreates the collider using the position and other given variables.
        /// </summary>
        public abstract void updateColliderPos();
    }
}
