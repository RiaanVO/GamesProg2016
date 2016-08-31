﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    //Simple camera = only for creating 'look ats'
    //WIP splitting old camera funcionality among multiple classes
    class BasicCamera
    {
        public Matrix viewMatrix;
        public Matrix projectionMatrix;

        public Vector3 camPos;
        public Vector3 camDirection;
        public Vector3 camUp;
        
        //Simple constructor
        public BasicCamera(Game game, Vector3 pos, Vector3 target, Vector3 up) 
            : this(game, pos, target, up, MathHelper.PiOver2, 
                  (float)game.Window.ClientBounds.Width / game.Window.ClientBounds.Height, 1f, 500f)
        {
        }

        //Main constructor
        public BasicCamera(Game game, Vector3 pos, Vector3 target, Vector3 up, float fov, float aspectRatio, float nearPlane, float farPlane)
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);

            camPos = pos;
            camDirection = target - camPos;
            camDirection.Normalize();
            camUp = up;

            CreateLookAt();
        }

        public void Initialize()
        {
        }

        private void CreateLookAt()
        {
            viewMatrix = Matrix.CreateLookAt(camPos, camPos + camDirection, camUp);
        }

        public void CreateLookAt(Vector3 position, Vector3 direction)
        {
            viewMatrix = Matrix.CreateLookAt(position, position + direction, camUp);
        }

        public void CreateLookAt(Vector3 position, Vector3 direction, Vector3 up)
        {
            viewMatrix = Matrix.CreateLookAt(position, position + direction, up);
        }

        /* Ray picking stuff

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
        */ 

        //Redundant, not called as it's not a game component lels
        public void Update(GameTime gameTime)
        {
            CreateLookAt();
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
