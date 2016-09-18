using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class BasicModel : GameObject
    {
        #region Fields
        protected BasicCamera camera;
        protected Model model;
        protected Matrix worldMatrix, scaleMatrix, rotationMatrix, translationMatrix;
        protected bool hasLighting = false;
        protected float modelBaseOrientation;
        protected float scale;
        #endregion

        #region Properties
        public BasicCamera Camera {
            get { return camera; }
            set { camera = value; }
        }

        public Model Model {
            get { return model; }
            set { model = value; }
        }

        public Matrix ScaleMatrix {
            get { return scaleMatrix; }
            set { scaleMatrix = value; }
        }

        public Matrix RotationMatrix
        {
            get { return rotationMatrix; }
            set { rotationMatrix = value; }
        }

        public Matrix TranslationMatrix
        {
            get { return translationMatrix; }
            set { translationMatrix = value; }
        }

        public float Scale {
            get { return scale; }
            set {
                scale = value;
                scaleMatrix = Matrix.CreateScale(scale);
            }
        }

        public new Vector3 Position
        {
            get { return position; }
            set {
                position = value;
                translationMatrix = Matrix.CreateTranslation(position);
            }
        }

        public float ModelBaseOrientation {
            get { return modelBaseOrientation; }
            set { modelBaseOrientation = value; }
        }

        public bool HasLighting {
            get { return hasLighting; }
            set { hasLighting = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor which sets the model and the camera, alterations must be set though properties
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="camera"></param>
        /// <param name="model"></param>
        public BasicModel(Vector3 startPosition, BasicCamera camera, Model model) : base(startPosition) {
            this.camera = camera;
            this.model = model;

            translationMatrix = Matrix.CreateTranslation(position);
            Scale = 1;
            modelBaseOrientation = 0;
            rotationMatrix = Matrix.Identity;
            worldMatrix = Matrix.Identity;
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// Draws all the meshes in the model
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (model == null || camera == null)
                return;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    if (hasLighting)
                        effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * GetWorld();
                }
                mesh.Draw();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the world matrix for drawing. Overload if need to modify world at draw time.
        /// </summary>
        /// <returns></returns>
        public virtual Matrix GetWorld()
        {
            return worldMatrix;
        }
        #endregion
    }
}
