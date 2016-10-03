using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class Tank : BasicModel
    {
        #region Fields
        Vector3 target;
        Vector3[] targets;
        //Position
        //Orientation
        float rotation;
        float maxSpeed;
        float dragPercentage;

        FollowPath steetingBehavior;
        #endregion

        #region Properties
        public Vector3[] Targets {
            get { return targets; }
            set {
                targets = value;
                steetingBehavior.Targets = targets;
            }
        }

        public Vector3 Target
        {
            get { return target; }
            set
            {
                target = value;
                Targets = NavigationMap.FindPath(position, target).ToArray();
            }
        }
        #endregion

        #region Initialize
        public Tank(Vector3 startPosition, Model model) : base(startPosition, ObjectManager.Camera, model)
        {
            velocity = Vector3.Zero;
            target = position;
            rotation = 0;
            maxSpeed = 5;
            modelBaseOrientation = 0;
            dragPercentage = 0.2f;
            
            steetingBehavior = new FollowPath(this, targets, 1, false, 1);
            steetingBehavior.TargetRadius = 0.5f;
            steetingBehavior.SlowRadius = 5;
            steetingBehavior.MaxSpeed = maxSpeed;
            steetingBehavior.MaxAcceleration = maxSpeed * 2;
            //steetingBehavior = new Arrive(this, position, 20, maxSpeed, 1, 10, 0.25f);
        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(steetingBehavior != null){
                update(steetingBehavior.getSteering(), deltaTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (targets != null)
                if (targets.Count() > 1)
                    //PathRenderer.RenderPath(targets, Color.White, 0);
            base.Draw(gameTime);
        }

        public override Matrix GetWorld()
        {
            return scaleMatrix * rotationMatrix * translationMatrix;
        }
        #endregion

        #region Helper Methods
        public void update(SteeringOutput steering, float deltaTime) {
            if (steering == null) {
                velocity = Vector3.Zero;
            } else {
                velocity *= 1 - dragPercentage * dragPercentage;
                //Update position and rotation
                position += velocity * deltaTime;
                orientation += rotation * deltaTime;

                //Update the velocity and rotation
                velocity += steering.Velocity * deltaTime;
                rotation += steering.Rotation * deltaTime;

                if (velocity.Length() > maxSpeed)
                    velocity = Vector3.Normalize(velocity) * (maxSpeed);
            }

            rotationMatrix = Matrix.CreateRotationY(orientation + modelBaseOrientation);
            translationMatrix = Matrix.CreateTranslation(position);
        }


        #endregion
    }
}
