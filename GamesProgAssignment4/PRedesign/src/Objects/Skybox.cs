using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class Skybox : BasicModel
    {
        #region Fields
        Player player;
        GraphicsDevice graphicsDevice;
        #endregion

        #region Properties
        public Player Player {
            get { return player; }
            set { player = value; }
        }
        
        public GraphicsDevice GraphicsDevice {
            get { return graphicsDevice; }
            set { graphicsDevice = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor which camera will be gotten from the object manager
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="model"></param>
        public Skybox(Vector3 startPosition, Model model) : base(startPosition, ObjectManager.Camera, model) {
            scaleMatrix = Matrix.CreateScale(1000);
            graphicsDevice = ObjectManager.GraphicsDevice;
        }

        /// <summary>
        /// Constructor where you must provide all values
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="camera"></param>
        /// <param name="model"></param>
        public Skybox(Vector3 startPosition, BasicCamera camera, GraphicsDevice graphicsDevice, Model model) : base(startPosition, camera, model) {
            this.graphicsDevice = graphicsDevice;
            scaleMatrix = Matrix.CreateScale(1000);
        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            //Update position according to player
            if(player != null)
                position = player.Position;
            position.Y = 0;

            //Updates world matrix
            translationMatrix = Matrix.CreateTranslation(position);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (graphicsDevice == null)
                return;

            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            graphicsDevice.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            graphicsDevice.DepthStencilState = dss;

            base.Draw(gameTime);

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            graphicsDevice.DepthStencilState = dss;

            ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Wrap;
            ss.AddressV = TextureAddressMode.Wrap;
            graphicsDevice.SamplerStates[0] = ss;
        }
        #endregion

        #region Public Methods

        public override Matrix GetWorld()
        {
            return scaleMatrix * translationMatrix;
        }
        #endregion
    }
}
