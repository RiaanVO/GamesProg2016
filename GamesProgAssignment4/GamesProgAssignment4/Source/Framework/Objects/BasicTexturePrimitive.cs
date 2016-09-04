using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class BasicTexturePrimitive : GameObject
    {
        protected BasicCamera camera;
        protected BasicEffect basicEffect;
        protected GraphicsDevice graphicsDevice;
        protected Matrix world, scale, rotation, translation;

        protected VertexPositionNormalTexture[] vertexData;
        protected int[] indexData;
        protected Texture2D texture;

        //Override if no lighting is wanted
        protected bool hasLighting = true;

        /// <summary>
        /// Constructor for a white basic primitive
        /// </summary>
        /// <param name="game"></param>
        /// <param name="objectManager"></param>
        /// <param name="position"></param>
        /// <param name="camera"></param>
        /// <param name="basicEffect"></param>
        public BasicTexturePrimitive(Game game, ObjectManager objectManager, Vector3 position, BasicCamera camera, GraphicsDevice graphicsDevice, Texture2D texture) :
            base(game, objectManager, position)
        {
            this.camera = camera;
            this.graphicsDevice = graphicsDevice;
            basicEffect = new BasicEffect(graphicsDevice);
            this.texture = texture;
            scale = Matrix.CreateScale(1);
            world = rotation = Matrix.Identity;
            translation = Matrix.CreateTranslation(position);
        }


        /// <summary>
        /// Constructor for a basic colour primitive
        /// </summary>
        /// <param name="game"></param>
        /// <param name="objectManager"></param>
        /// <param name="position"></param>
        /// <param name="camera"></param>
        /// <param name="basicEffect"></param>
        /// <param name="colour"></param>
        public BasicTexturePrimitive(Game game, ObjectManager objectManager, Vector3 position, BasicCamera camera, GraphicsDevice graphicsDevice, BasicEffect basicEffect, Texture2D texture) : base(game, objectManager, position)
        {
            this.camera = camera;
            this.graphicsDevice = graphicsDevice;
            this.basicEffect = basicEffect;
            this.texture = texture;
            world = scale = rotation = Matrix.Identity;
            translation = Matrix.CreateTranslation(position);
        }

        /// <summary>
        /// Using the set up vertex and index data, it will draw the primitive
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (vertexData == null || indexData == null)
                return;

            basicEffect.World = getWorld();
            basicEffect.View = camera.viewMatrix;
            basicEffect.Projection = camera.projectionMatrix;

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
                graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertexData, 0, vertexData.Length, indexData, 0, indexData.Length / 3);
            }

            base.Draw(gameTime);
        }

        public virtual Matrix getWorld()
        {
            return world;
        }

        /// <summary>
        /// Sets the position and adjusts the tranlation of the object
        /// </summary>
        /// <param name="position"></param>
        public void setPosition(Vector3 position) {
            this.position = position;
            translation = Matrix.CreateTranslation(position);
        }
    }
}
