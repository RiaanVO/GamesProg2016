using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class GroundPrimitive : BasicTexturePrimitive
    {
        #region Fields
        int tileSize;
        int numTilesAcross;
        bool centerGridPlane = false;
        bool gridPlaneConstructed = false;
        BoxCollider collider;
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
        public GroundPrimitive(Vector3 startPosition, Texture2D texture, int tileSize, int numTilesAcross) : base(startPosition, texture) {
            this.tileSize = tileSize;
            this.numTilesAcross = numTilesAcross;
            
            //setupCollider();
        }

        private void setupCollider() {
            collider = new BoxCollider(this, ObjectTag.floor, new Vector3(tileSize, 1, tileSize));
            collider.DrawColour = Color.LightCyan;
            collider.PositionOffset = new Vector3(0, -1, 0);
            collider.updateColliderPos(position * 2);
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
                    vertexData[currVertexAdd] = new VertexPositionNormalTexture(topLeft, Vector3.Up, new Vector2(0, 0));
                    vertexData[currVertexAdd + 1] = new VertexPositionNormalTexture(topRight, Vector3.Up, new Vector2(1, 0));
                    vertexData[currVertexAdd + 2] = new VertexPositionNormalTexture(bottomLeft, Vector3.Up, new Vector2(0, 1));
                    vertexData[currVertexAdd + 3] = new VertexPositionNormalTexture(bottomRight, Vector3.Up, new Vector2(1, 1));

                    int currIndexAdd = x * 6 + z * numTilesAcross * 6;
                    indexData[currIndexAdd] = currVertexAdd;
                    indexData[currIndexAdd + 1] = currVertexAdd + 1;
                    indexData[currIndexAdd + 2] = currVertexAdd + 2;
                    indexData[currIndexAdd + 3] = currVertexAdd + 2;
                    indexData[currIndexAdd + 4] = currVertexAdd + 1;
                    indexData[currIndexAdd + 5] = currVertexAdd + 3;
                }
            }

            if (centerGridPlane)
                TranslationMatrix = Matrix.CreateTranslation(position - new Vector3(TotalSize / 2f, 0, TotalSize / 2f));
            else
                TranslationMatrix = Matrix.CreateTranslation(position);

            gridPlaneConstructed = true;
        }
        #endregion
    }
}
