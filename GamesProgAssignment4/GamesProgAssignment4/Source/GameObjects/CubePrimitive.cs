using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class CubePrimitive : BasicTexturePrimitive
    {
        float size;
        Vector3[] structure;
        Vector2[] textureCoordinates;
        Vector3[] faceNormals;

        BoxCollider col;

        public CubePrimitive(Game game, ObjectManager objectManager, Vector3 position, BasicCamera camera, GraphicsDevice graphicsDevice, Texture2D texture, float size) :
            base(game, objectManager, position, camera, graphicsDevice, texture)
        {
            this.size = size;
            col = new BoxCollider(game, this, true, position, new Vector3(position.X + size, position.Y + size, position.Z + size));
            constructCube();
        }

        private void constructCube()
        {
            structure = new Vector3[8] {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, size),
                new Vector3(size, 0, size),
                new Vector3(size, 0, 0),
                new Vector3(0, size, 0),
                new Vector3(0, size, size),
                new Vector3(size, size, size),
                new Vector3(size, size, 0)
            };

            textureCoordinates = new Vector2[] {
                new Vector2(0,0),
                new Vector2(0.5f,0),
                new Vector2(1,0),
                new Vector2(0,0.5f),
                new Vector2(0.5f,0.5f),
                new Vector2(1,0.5f),
                new Vector2(0,1),
                new Vector2(0.5f,1),
                new Vector2(1,1)
            };

            faceNormals = new Vector3[] {
                new Vector3(-1,0,0),
                new Vector3(0,0,1),
                new Vector3(1,0,0),
                new Vector3(0,0,-1),
                new Vector3(0,1,0),
                new Vector3(0,-1,0)
            };

            vertexData = new VertexPositionNormalTexture[24];
            //Left
            vertexData[0] = new VertexPositionNormalTexture(structure[0], faceNormals[0], textureCoordinates[6]);
            vertexData[1] = new VertexPositionNormalTexture(structure[1], faceNormals[0], textureCoordinates[7]);
            vertexData[2] = new VertexPositionNormalTexture(structure[4], faceNormals[0], textureCoordinates[3]);
            vertexData[3] = new VertexPositionNormalTexture(structure[5], faceNormals[0], textureCoordinates[4]);

            //Front
            vertexData[4] = new VertexPositionNormalTexture(structure[1], faceNormals[1], textureCoordinates[3]);
            vertexData[5] = new VertexPositionNormalTexture(structure[2], faceNormals[1], textureCoordinates[4]);
            vertexData[6] = new VertexPositionNormalTexture(structure[5], faceNormals[1], textureCoordinates[0]);
            vertexData[7] = new VertexPositionNormalTexture(structure[6], faceNormals[1], textureCoordinates[1]);

            //Right
            vertexData[8] = new VertexPositionNormalTexture(structure[2], faceNormals[2], textureCoordinates[6]);
            vertexData[9] = new VertexPositionNormalTexture(structure[3], faceNormals[2], textureCoordinates[7]);
            vertexData[10] = new VertexPositionNormalTexture(structure[6], faceNormals[2], textureCoordinates[3]);
            vertexData[11] = new VertexPositionNormalTexture(structure[7], faceNormals[2], textureCoordinates[4]);

            //Back
            vertexData[12] = new VertexPositionNormalTexture(structure[3], faceNormals[3], textureCoordinates[4]);
            vertexData[13] = new VertexPositionNormalTexture(structure[0], faceNormals[3], textureCoordinates[5]);
            vertexData[14] = new VertexPositionNormalTexture(structure[7], faceNormals[3], textureCoordinates[1]);
            vertexData[15] = new VertexPositionNormalTexture(structure[4], faceNormals[3], textureCoordinates[2]);

            //Top
            vertexData[16] = new VertexPositionNormalTexture(structure[4], faceNormals[4], textureCoordinates[6]);
            vertexData[17] = new VertexPositionNormalTexture(structure[5], faceNormals[4], textureCoordinates[7]);
            vertexData[18] = new VertexPositionNormalTexture(structure[6], faceNormals[4], textureCoordinates[4]);
            vertexData[19] = new VertexPositionNormalTexture(structure[7], faceNormals[4], textureCoordinates[3]);

            //Bottom
            vertexData[20] = new VertexPositionNormalTexture(structure[0], faceNormals[5], textureCoordinates[6]);
            vertexData[21] = new VertexPositionNormalTexture(structure[1], faceNormals[5], textureCoordinates[7]);
            vertexData[22] = new VertexPositionNormalTexture(structure[2], faceNormals[5], textureCoordinates[4]);
            vertexData[23] = new VertexPositionNormalTexture(structure[3], faceNormals[5], textureCoordinates[3]);


            indexData = new int[36] {
                0,2,1, 2,3,1,
                4,6,5, 6,7,5,
                8,10,9, 10,11,9,
                12,14,13, 14,15,13,
                16,19,17, 19,18,17,
                20,21,23, 21,22,23
            };
        }

        public void setPosition(Vector3 position)
        {
            this.position = position;
            translation = Matrix.CreateTranslation(position);
        }

        public override Matrix getWorld()
        {
            return translation;
        }
    }
}
