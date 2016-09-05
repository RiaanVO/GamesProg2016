using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace A_MAZE_ING
{
    class PlayerMovementSphereCollider : SphereCollider
    {
        Player player;
        Vector3 direction;

        public PlayerMovementSphereCollider(Game game, GameObject obj, objectTag tag, bool checkCollision, bool isKinematic, Vector3 direction) : this(game, obj, tag, checkCollision, isKinematic, 1f, direction)
        { }

        public PlayerMovementSphereCollider(Game game, GameObject obj, objectTag tag, bool checkCollision, bool isKinematic, float radius, Vector3 direction) : base(game, obj, tag, checkCollision, isKinematic)
        {
            this.radius = radius;
            player = obj as Player;
            this.direction = direction;
            updateColliderPos();
        }

        
        /// <summary>
        /// Gets the players current position, then creates a future position from the players velocity
        /// </summary>
        public override void updatePos()
        {
            base.updatePos();
            if (player != null) {
                Vector3 velocity = player.getVelocity();
                if (direction == Vector3.Forward)
                    velocity.Z = 0;
                if (direction == Vector3.Right)
                    velocity.X = 0;
                position = position + velocity;
            }
        }
    }
}
