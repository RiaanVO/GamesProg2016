using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GamesProgAssignment4
{
    class ModelManager : DrawableGameComponent
    {
        List<BasicModel> models = new List<BasicModel>();
        //List<TexturedPrimitives> primitives = new List<TexturedPrimitives>();
        //Skybox skybox;

        Game game;
        Camera camera;
        //Graphics device.sampler state (first element is to specify how the textures are wrapped (wrap or clamp))
        //Basic constructor
        public ModelManager(Camera cam, Game game) : base(game)
        {
            this.game = game;
            camera = cam;
            //Shouldn't have to call LoadContent??
            LoadContent();
        }

        protected override void LoadContent()
        {
            //models.Add(new BasicModel(Game.Content.Load<Model>(@"Ground Model\Ground"), Vector3.Zero));

            //skybox = new Skybox(Game.Content.Load<Model>(@"Skybox Model\skybox"), camera.camPos, camera, game);

            //primitives.Add(new TexturedPlane(Vector3.Zero, game, Game.Content.Load<Texture2D>(@"Ground Model\Ak4c"), 1000, new Vector3(100, 0, 100), new Vector3(-100, 0, -100), Vector3.Up));

            //models.Add(new Tank(Game.Content.Load<Model>(@"Tank\tank"),new Vector3(-10, 0, -10), camera));
            

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Add update here
            //skybox.Update(gameTime);

            foreach (BasicModel model in models)
            {
                model.Update(gameTime);
            }

            /*
            foreach (TexturedPrimitives primitive in primitives)
            {
                primitive.Update(gameTime);
            }
            */
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Add drawing here
            //Test to fix draw order problem
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //skybox.Draw(camera, false);

            //Draw the list of models
            foreach (BasicModel model in models)
            {
                model.Draw(camera, true);
            }

            //Draw the list of primitives
            /*
            foreach (TexturedPrimitives primitive in primitives)
            {
                primitive.Draw(camera);
            }
            */
            
            base.Draw(gameTime);
        }

    }
}
