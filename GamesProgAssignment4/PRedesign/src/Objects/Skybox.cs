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
        public Skybox(Vector3 startPosition, BasicCamera camera, Model model) : base(startPosition) {
            this.camera = camera;
            this.model = model;
            scale = Matrix.CreateScale(1000);
        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            //Update position according to player
            position = player.Position;
            position.Y = 0;

            //Updates world matrix
            translation = Matrix.CreateTranslation(position);

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
            return scale * rotation * translation;
        }
        #endregion
    }
}
