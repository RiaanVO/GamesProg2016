using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class Camera : GameComponent
    {
        public Matrix viewMatrix;
        public Matrix projectionMatrix;

        public Vector3 camPos;
        public Vector3 headPos;
        public Vector3 camDirection;
        public Vector3 camUp;
        public Vector3 camSide;
        
        //Simple constructor
        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up) 
            : this(game, pos, target, up, MathHelper.PiOver2, 
                  (float)game.Window.ClientBounds.Width / game.Window.ClientBounds.Height, 1f, 500f)
        {
        }

        //Main constructor
        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up, float fov, float aspectRatio, float nearPlane, float farPlane) : base(game)
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);

            camPos = pos;
            camDirection = target - camPos;
            camDirection.Normalize();
            camUp = up;

            CreateLookAt();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void CreateLookAt()
        {
            headPos = camPos;
            //headPos.Y += playerHeight;
            //viewMatrix = Matrix.CreateLookAt(headPos, headPos + camDirection, Vector3.Up);
            viewMatrix = Matrix.CreateLookAt(camPos, camPos + camDirection, camUp);
        }
        /*
        private void CreateLookAt()
        {
            viewMatrix = Matrix.CreateLookAt(camPos, camPos + camDirection, camUp);
        }*/

        //Adapted from http://rbwhitaker.wikidot.com/picking
        public Ray calculateRay(Vector2 mouseLocation, Matrix view, Matrix projection, Matrix world, Viewport viewport)
        {
            //Uses the near plane of Z
            Vector3 nearPoint = viewport.Unproject(
                new Vector3(mouseLocation.X, mouseLocation.Y, 0.0f),
                projection, view, world);

            //Uses a further away point to calculate the direction of the ray
            Vector3 farPoint = viewport.Unproject(
                new Vector3(mouseLocation.X, mouseLocation.Y, 1f),
                projection, view, world);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            //Ray = near point, and then the normalised direction it goes in 
            return new Ray(nearPoint, direction);
        }

        //Adapted from http://gamedev.stackexchange.com/questions/23227/how-to-calculate-the-contact-point-between-ray-and-plane
        public Vector3? GetRayPlaneIntersectionPoint(Ray ray, Plane plane)
        {
            float? distance = ray.Intersects(plane);
            return distance.HasValue ?  ray.Position + ray.Direction * distance.Value : (Vector3?)null;
        }
        
        public override void Update(GameTime gameTime)
        {
            //updatePhysics(gameTime);

            CreateLookAt();

            base.Update(gameTime);
        }

        /*
        private void updatePhysics(GameTime gameTime)
        {
            camPos.Y += velocityY * gameTime.ElapsedGameTime.Milliseconds;
            if (camPos.Y <= 0)
            {
                jumping = false;
                velocityY = 0;
                camPos.Y = 0;
            }
            else
            {
                velocityY += gravityY;
            }
        }*/
    }
}
