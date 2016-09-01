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
        Game game;
        //Master list of all game objects
        List<GameObject> objects = new List<GameObject>();

        //Camera that will be passed into all game objects 
        BasicCamera camera;
        Player player;

        //Basic constructor
        public ObjectManager(Game game) : base(game)
        {
            this.game = game;
            //Graphics device.sampler state (first element is to specify how the textures are wrapped (wrap or clamp))???
        }

        public override void Initialize()
        {
            foreach(GameObject obj in objects) {
                obj.Initialize();
            }
            base.Initialize();
        }

        /// <summary>
        /// Used to load in all the game objects and create the scene.
        /// </summary>
        protected override void LoadContent()
        {
            float aspectRatio = game.Window.ClientBounds.Width / game.Window.ClientBounds.Height;
            camera = new BasicCamera(game, new Vector3(0,10,0), Vector3.Left, Vector3.Up, MathHelper.PiOver2, aspectRatio, 1f, 2000F);

            //Create player and add to the object list
            player = new Player(game, camera.camPos, camera);
            objects.Add(player);
        
            //Add objects for the player to interact with
            objects.Add(new BasicModel(game, Vector3.Zero, game.Content.Load<Model>(@"Models\Ground Model\Ground"), camera));
            objects.Add(new Skybox(game, player.position, Game.Content.Load<Model>(@"Models\Skybox Model\skybox"), camera, player));
            //models.Add(new Tank(Game.Content.Load<Model>(@"Tank\tank"),new Vector3(-10, 0, -10), camera));
            
            base.LoadContent();
        }

        /// <summary>
        /// For each game object in the list of objects, update them
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            foreach (GameObject obj in objects)
            {
                obj.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// For each game object in the list of objects, draw them
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //Add drawing here
            //Test to fix draw order problem
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (GameObject obj in objects)
            {
                obj.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

    }
}
