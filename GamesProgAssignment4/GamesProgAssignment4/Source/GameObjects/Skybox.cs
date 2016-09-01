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
        Vector3 skyboxAnchor;

        public Skybox(Game game, Vector3 startPos, Model model, BasicCamera camera, Player player) : base(game, startPos, model, camera)
        {
            this.player = player;
            hasLighting = false;
            scale = Matrix.CreateScale(10000f);
        }

        public override void Initialize()
        {
            position = player.position;
            skyboxAnchor = position;
            translate = Matrix.CreateTranslation(position);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //Update position according to player
            position = player.position;
            skyboxAnchor = position;
            skyboxAnchor.Y = 0;
            
            //Updates world matrix
            translate = Matrix.CreateTranslation(skyboxAnchor);

            base.Update(gameTime);
        }

        public override Matrix GetWorld()
        {
            return scale * rotate * translate;
        }

        public override void Draw(GameTime gameTime)
        {
            //world = cam.viewMatrix;
            /*
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            game.GraphicsDevice.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            game.GraphicsDevice.DepthStencilState = dss;
            */
            System.Console.WriteLine("Drawing");
            base.Draw(gameTime);
            /*
            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            game.GraphicsDevice.DepthStencilState = dss;

            ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Wrap;
            ss.AddressV = TextureAddressMode.Wrap;
            game.GraphicsDevice.SamplerStates[0] = ss;
            */
        }
    }
}
