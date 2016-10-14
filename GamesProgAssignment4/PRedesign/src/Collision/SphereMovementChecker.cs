using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class SphereMovementChecker
    {
        private SphereCollider testCollider;
        private List<ObjectTag> tagsToCheck;

        public SphereMovementChecker(SphereCollider mainCollider, List<ObjectTag> tagsToCheck) {
            this.tagsToCheck = tagsToCheck;

            testCollider = new SphereCollider(mainCollider.GameObject, ObjectTag.movementChecker, mainCollider.Radius);
            testCollider.PositionOffset = mainCollider.PositionOffset;
            testCollider.DrawColour = Color.DarkGreen;
        }

        public bool canMoveX(Vector3 currentPosition, Vector3 velocity) {
            return testPosition(currentPosition + new Vector3(velocity.X, 0, 0));

        }

        public bool canMoveY(Vector3 currentPosition, Vector3 velocity)
        {
            Vector3 newPosition = currentPosition + new Vector3(0, velocity.Y, 0);
            testCollider.updateColliderPos(newPosition);
            List<Collider> collisions = testCollider.getCollisions();
            foreach (Collider otherCollider in collisions)
            {
                if (otherCollider.Tag == ObjectTag.floor)
                    return false;
            }
            return true;
        }

        public bool canMoveZ(Vector3 currentPosition, Vector3 velocity)
        {
            return testPosition(currentPosition + new Vector3(0, 0, velocity.Z));
        }

        private bool testPosition(Vector3 newPosition) {
            testCollider.updateColliderPos(newPosition);
            List<Collider> collisions = testCollider.getCollisions();
            foreach (Collider otherCollider in collisions)
            {
                if (tagsToCheck.Contains(otherCollider.Tag))
                    return false;
            }
            return true;
        }
    }
}
