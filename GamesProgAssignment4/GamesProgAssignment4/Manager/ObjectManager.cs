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
    class ObjectManager : DrawableGameComponent
    {
        List<GameObject> objects = new List<GameObject>();
        //List<TexturedPrimitives> primitives = new List<TexturedPrimitives>();
        //Skybox skybox;

        Game game;
        BasicCamera camera;
        Player player;

        //Graphics device.sampler state (first element is to specify how the textures are wrapped (wrap or clamp))
        //Basic constructor
        public ObjectManager(Game game) : base(game)
        {
            this.game = game;
            //NOTE: Camera is set up by player, needs to be passed back
            //camera = cam;
            //Shouldn't have to call LoadContent??
            LoadContent();
        }

        protected override void LoadContent()
        {
            camera = new BasicCamera(game, Vector3.Zero, Vector3.Left, Vector3.Up, 
                MathHelper.PiOver2, (float)game.Window.ClientBounds.Width / game.Window.ClientBounds.Height, 1f, 500f);

            //Playaaa
            player = new Player(camera.camPos, game, camera);
            objects.Add(player);

            objects.Add(new Skybox(camera.camPos, game, Game.Content.Load<Model>(@"Models\Skybox Model\skybox"), camera, player));

            //primitives.Add(new TexturedPlane(Vector3.Zero, game, Game.Content.Load<Texture2D>(@"Ground Model\Ak4c"), 1000, new Vector3(100, 0, 100), new Vector3(-100, 0, -100), Vector3.Up));

            //models.Add(new Tank(Game.Content.Load<Model>(@"Tank\tank"),new Vector3(-10, 0, -10), camera));
            

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Add update here
            //skybox.Update(gameTime);
            
            foreach (GameObject obj in objects)
            {
                obj.Update(gameTime);
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
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (GameObject obj in objects)
            {
                obj.Draw(gameTime);
            }

            //Draw the list of models
            /*
            foreach (BasicModel model in models)
            {
                model.Draw(camera, true);
            }
            */

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
