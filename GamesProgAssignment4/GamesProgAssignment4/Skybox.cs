using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class Skybox : BasicModel
    {
        Player player;

        public Skybox(Vector3 startPos, Game game, Model model, BasicCamera camera, Player player) : base(startPos, game, model, camera)
        {
            this.player = player;
            hasLighting = false;
            scale = Matrix.CreateScale(100f);
        }

        public override void Update(GameTime gameTime)
        {
            //Update position according to player
            position = player.position;

            //Updates world matrix
            translate = Matrix.CreateTranslation(position);

            base.Update(gameTime);
        }

        public override Matrix GetWorld()
        {
            return scale * rotate * translate;
        }

        public override void Draw(GameTime gameTime)
        {
            //world = cam.viewMatrix;
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            game.GraphicsDevice.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            game.GraphicsDevice.DepthStencilState = dss;
            
            base.Draw(gameTime);

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            game.GraphicsDevice.DepthStencilState = dss;

            ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Wrap;
            ss.AddressV = TextureAddressMode.Wrap;
            game.GraphicsDevice.SamplerStates[0] = ss;
        }
    }
}
