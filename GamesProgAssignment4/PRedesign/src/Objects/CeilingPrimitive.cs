using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class CeilingPrimitive : BasicTexturePrimitive
    {
        #region Fields
        int tileSize;
        int numTilesAcross;
        bool centerGridPlane = false;
        bool gridPlaneConstructed = false;
        #endregion

        #region Properties
        public int TileSize {
            get { return tileSize; }
            set {
                tileSize = value;
                gridPlaneConstructed = false;
            }
        }

        public int NumTilesAcross
        {
            get { return numTilesAcross; }
            set
            {
                numTilesAcross = value;
                gridPlaneConstructed = false;
            }
        }

        public int TotalSize {
            get { return tileSize * numTilesAcross; }
        }

        public bool CenterGridPlane {
            get { return centerGridPlane; }
            set {
                centerGridPlane = value;
                if (centerGridPlane)
                    TranslationMatrix = Matrix.CreateTranslation(position - new Vector3(TotalSize / 2, 0, TotalSize / 2));
                else
                    TranslationMatrix = Matrix.CreateTranslation(position);
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor for ground primitive which will get required values from object manager
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="texture"></param>
        /// <param name="tileSize"></param>
        /// <param name="numTilesAcross"></param>
        public CeilingPrimitive(Vector3 startPosition, Texture2D texture, int tileSize, int numTilesAcross) : base(startPosition, ObjectManager.Camera, ObjectManager.GraphicsDevice, texture) {
            this.tileSize = tileSize;
            this.numTilesAcross = numTilesAcross;
        }

        /// <summary>
        /// Constructor where you must define the values
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="camera"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="texture"></param>
        /// <param name="tileSize"></param>
        /// <param name="numTilesAcross"></param>
        public CeilingPrimitive(Vector3 startPosition, BasicCamera camera, GraphicsDevice graphicsDevice, Texture2D texture, int tileSize, int numTilesAcross) 
            : base(startPosition, camera, graphicsDevice, texture)
        {
            this.tileSize = tileSize;
            this.numTilesAcross = numTilesAcross;
        }
        #endregion

        #region Update and Draw
        public override void Draw(GameTime gameTime)
        {
            //Build the grid plane before drawing
            if (!gridPlaneConstructed)
                constructGridPlane();

            base.Draw(gameTime);
        }
        #endregion

        #region Public Methods
        public override Matrix getWorld()
        {
            return translationMatrix;
        }
        #endregion

        #region Helper Methods
        private void constructGridPlane()
        {
            vertexData = new VertexPositionNormalTexture[numTilesAcross * numTilesAcross * 4];
            indexData = new int[numTilesAcross * numTilesAcross * 6];

            for (int z = 0; z < numTilesAcross; z++)
            {
                for (int x = 0; x < numTilesAcross; x++)
                {
                    int currVertexAdd = x * 4 + z * numTilesAcross * 4;
                    Vector3 topLeft = new Vector3(position.X + tileSize * x, position.Y, position.Z + tileSize * z);
                    Vector3 topRight = new Vector3(position.X + tileSize * (x + 1), position.Y, position.Z + tileSize * z);
                    Vector3 bottomLeft = new Vector3(position.X + tileSize * x, position.Y, position.Z + tileSize * (z + 1));
                    Vector3 bottomRight = new Vector3(position.X + tileSize * (x + 1), position.Y, position.Z + tileSize * (z + 1));
                    vertexData[currVertexAdd] = new VertexPositionNormalTexture(topLeft, Vector3.Down, new Vector2(0, 0));
                    vertexData[currVertexAdd + 1] = new VertexPositionNormalTexture(topRight, Vector3.Down, new Vector2(1, 0));
                    vertexData[currVertexAdd + 2] = new VertexPositionNormalTexture(bottomLeft, Vector3.Down, new Vector2(0, 1));
                    vertexData[currVertexAdd + 3] = new VertexPositionNormalTexture(bottomRight, Vector3.Down, new Vector2(1, 1));
                    
                    int currIndexAdd = x * 6 + z * numTilesAcross * 6;
                    indexData[currIndexAdd] = currVertexAdd + 0;
                    indexData[currIndexAdd + 1] = currVertexAdd + 2;
                    indexData[currIndexAdd + 2] = currVertexAdd + 1;
                    indexData[currIndexAdd + 3] = currVertexAdd + 1;
                    indexData[currIndexAdd + 4] = currVertexAdd + 2;
                    indexData[currIndexAdd + 5] = currVertexAdd + 3;
                }
            }

            if (centerGridPlane)
                TranslationMatrix = Matrix.CreateTranslation(position - new Vector3(TotalSize / 2, 0, TotalSize / 2));
            else
                TranslationMatrix = Matrix.CreateTranslation(position);

            gridPlaneConstructed = true;
        }
        #endregion
    }
}
