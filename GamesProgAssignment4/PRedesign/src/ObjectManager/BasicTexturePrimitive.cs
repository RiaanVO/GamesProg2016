using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class BasicTexturePrimitive : GameObject
    {
        #region Fields
        protected Matrix worldMatrix, scaleMatrix, rotationMatrix, translationMatrix;

        protected VertexPositionNormalTexture[] vertexData;
        protected int[] indexData;
        protected Texture2D texture;

        protected bool hasLighting = false;
        #endregion

        #region Properties
        public VertexPositionNormalTexture[] VertexData
        {
            get { return vertexData; }
            set { vertexData = value; }
        }

        public int[] IndexData {
            get { return indexData; }
            set { indexData = value; }
        }

        public Texture2D Texture {
            get { return texture; }
            set { texture = value; }
        }

        public Matrix ScaleMatrix
        {
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

        public new Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                translationMatrix = Matrix.CreateTranslation(position);
            }
        }

        public bool HasLighting
        {
            get { return hasLighting; }
            set { hasLighting = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor for the basic texture primitive
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="camera"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="texture"></param>
        public BasicTexturePrimitive(Vector3 startPosition, Texture2D texture) : base(startPosition)
        {
            translationMatrix = Matrix.CreateTranslation(startPosition);
            scaleMatrix = Matrix.CreateScale(1);
            rotationMatrix = Matrix.Identity;
            worldMatrix = scaleMatrix * rotationMatrix * translationMatrix;
            this.texture = texture;
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// Using the set up vertex and index data, it will draw the primitive
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            BasicCamera camera = ObjectManager.Camera;
            BasicEffect basicEffect = ObjectManager.BasicEffect; //Will be null if there is no graphics device
            if (vertexData == null || indexData == null || basicEffect == null || camera == null)
                return;

            basicEffect.World = getWorld();
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;

            if (hasLighting)
                basicEffect.EnableDefaultLighting();
            if (texture != null)
            {
                basicEffect.Texture = texture;
                basicEffect.TextureEnabled = true;
            }

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                ObjectManager.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertexData, 0, vertexData.Length, indexData, 0, indexData.Length / 3);
            }
        }

        #endregion

        #region Public Methods
        public virtual Matrix getWorld()
        {
            return worldMatrix;
        }
        #endregion
    }
}
