using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class BasicCamera
    {
        #region Fields
        Matrix cachedViewMatrix;
        Vector3 position;
        Vector3 direction;
        Vector3 upVector;
        bool needViewResync;

        Matrix projectionMatrix;
        float fieldOfView;
        float aspectRatio;
        float nearClip;
        float farClip;
        #endregion

        #region Properties
        public Matrix Projection {
            get { return projectionMatrix; }
            set { projectionMatrix = value; }
        }

        public Matrix View {
            get {
                if (needViewResync){
                    cachedViewMatrix = Matrix.CreateLookAt(position, position + direction, upVector);
                    needViewResync = false;
                }
                return cachedViewMatrix;
            }
        }

        public Vector3 Position {
            get { return position; }
            set {
                position = value;
                needViewResync = true;
            }
        }

        public Vector3 Direction {
            get { return direction; }
            set {
                direction = value;
                if (direction == Vector3.Zero)
                    direction = Vector3.Forward;
                needViewResync = true;
            }
        }

        public Vector3 UpVector {
            get { return upVector; }
            set {
                upVector = value;
                if (upVector == Vector3.Zero)
                    upVector = Vector3.Up;
                needViewResync = true;
            }
        }
        
        public float FieldOfView {
            get { return fieldOfView; }
            set {
                fieldOfView = value;
                createPerpectiveProjection(fieldOfView, aspectRatio, nearClip, farClip);
            }
        }

        public float AspectRatio {
            get { return aspectRatio; }
            set {
                aspectRatio = value;
                createPerpectiveProjection(fieldOfView, aspectRatio, nearClip, farClip);
            }
        }

        public float NearClip {
            get { return nearClip; }
            set {
                nearClip = value;
                createPerpectiveProjection(fieldOfView, aspectRatio, nearClip, farClip);
            }
        }

        public float FarClip {
            get { return farClip; }
            set {
                farClip = value;
                createPerpectiveProjection(fieldOfView, aspectRatio, nearClip, farClip);
            }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Constructor with 45 degree field of view;
        /// </summary>
        /// <param name="position"></param>
        /// <param name="target"></param>
        /// <param name="upVector"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="nearClip"></param>
        /// <param name="farClip"></param>
        public BasicCamera(Vector3 position, Vector3 target, Vector3 upVector, float aspectRatio, float nearClip, float farClip) : this(position, target, upVector, MathHelper.PiOver4, aspectRatio, nearClip, farClip) { }

        /// <summary>
        /// Constructor with 45 degrees FOV and near clip 0.1f and far clip 1000f
        /// </summary>
        /// <param name="position"></param>
        /// <param name="target"></param>
        /// <param name="upVector"></param>
        /// <param name="aspectRatio"></param>
        public BasicCamera(Vector3 position, Vector3 target, Vector3 upVector, float aspectRatio) : this(position, target, upVector, MathHelper.PiOver4, aspectRatio, 0.1f, 1000f) { }

        /// <summary>
        /// Full constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="target"></param>
        /// <param name="upVector"></param>
        /// <param name="fieldOfView"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="nearClip"></param>
        /// <param name="farClip"></param>
        public BasicCamera(Vector3 position, Vector3 target, Vector3 upVector, float fieldOfView, float aspectRatio, float nearClip, float farClip) {
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.nearClip = nearClip;
            this.farClip = farClip;
            createPerpectiveProjection(fieldOfView, aspectRatio, nearClip, farClip);
            this.position = position;
            this.upVector = upVector;
            direction = target - position;
            //Prevent the camera from trying to look at itself
            if (direction == Vector3.Zero)
                direction = Vector3.Forward;
            needViewResync = true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the cameras look at direction based on the provided target
        /// </summary>
        /// <param name="target"></param>
        public void lookAt(Vector3 target) {
            Direction = target - position;
        }

        /// <summary>
        /// Sets the position and direction of the camera
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void setPositionAndDirection(Vector3 position, Vector3 direction) {
            Position = position;
            Direction = direction;
        }

        /// <summary>
        /// Creates a perspective projection matrix
        /// </summary>
        /// <param name="fieldOfView"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="nearClip"></param>
        /// <param name="farClip"></param>
        private void createPerpectiveProjection(float fieldOfView, float aspectRatio, float nearClip, float farClip) {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClip, farClip);
        }
        #endregion
    }
}
