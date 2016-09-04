using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GamesProgAssignment4
{
    class GroundPrimitive : BasicTexturePrimitive
    {
        int tileSize;
        int numTilesAcross;
        int totalSize;
        Vector3 centredPosition;

        public GroundPrimitive(Game game, ObjectManager objectManager, Vector3 position, BasicCamera camera, GraphicsDevice graphicsDevice, Texture2D texture, int tileSize, int numTilesAcross) :
            base(game, objectManager, position, camera, graphicsDevice, texture)
        {
            this.tileSize = tileSize;
            this.numTilesAcross = numTilesAcross;
            totalSize = tileSize * numTilesAcross;
            hasLighting = false;
            centredPosition = new Vector3(-totalSize / 2, 0, -totalSize / 2);

            translation = Matrix.CreateTranslation(centredPosition);

            constructPlane();
        }

        public override Matrix getWorld()
        {
            return translation;
        }

        /// <summary>
        /// Constructs a textured plane based on the initial dimentions
        /// </summary>
        private void constructPlane()
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
        }
    }
}
