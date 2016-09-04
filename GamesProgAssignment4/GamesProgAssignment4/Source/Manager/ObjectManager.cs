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
        //Master list of all game objects
        List<GameObject> objectsMaster = new List<GameObject>();
        //Used for the current update and draw cycles - avoid deleting objects that need to be updated.
        List<GameObject> objectsCurrent = new List<GameObject>();

        //2D & UI drawing
        SpriteBatch spriteBatch;
        List<UIObject> ui = new List<UIObject>();

        //Camera that will be passed into all game objects 
        BasicCamera camera;
        Player player;

        //Basic constructor
        public ObjectManager(Game game) : base(game)
        {
            //Graphics device.sampler state (first element is to specify how the textures are wrapped (wrap or clamp))???
        }

        public override void Initialize()
        {
            foreach(GameObject obj in objectsMaster)
            {
                obj.Initialize();
            }

            foreach (UIObject obj in ui)
            {
                obj.Initialize();
            }

            base.Initialize();
        }

        /// <summary>
        /// Used to load in all the game objects and create the scene.
        /// </summary>
        protected override void LoadContent()
        {
            float aspectRatio = 16f / 9f;//Game.Window.ClientBounds.Width / Game.Window.ClientBounds.Height;
            Vector3 camPos = new Vector3(0, 0, 0);
            camera = new BasicCamera(Game, camPos, camPos + Vector3.Forward, Vector3.Up, MathHelper.PiOver4, aspectRatio, 1f, 3000F);

            //Create player and add to the object list
            player = new Player(Game, this, camera.camPos, camera);
            addGameObject(player);

            //Add objects for the player to interact with
            //objects.Add(new Skybox(Game, player.position, Game.Content.Load<Model>(@"Models\Skybox Model\skybox"), camera, player));
            addGameObject(new Skybox(Game, this, player.position, Game.Content.Load<Model>(@"Models\Skyboxes\envmap_miramar\envmap_miramar"), camera, player));

            //addGameObject(new GroundModel(Game, this, Vector3.Zero, Game.Content.Load<Model>(@"Models\Ground Model\Ground"), camera));
            addGameObject(new GroundPrimitive(Game, this, Vector3.Zero, camera, Game.GraphicsDevice, Game.Content.Load<Texture2D>(@"Models/Ground Model/sanddf"), 10, 100));

            addGameObject(new Enemy(Game, this, new Vector3(200f, 0f, 0f), Game.Content.Load<Model>(@"Models\Enemy Model\tank"), camera, player));
            //models.Add(new Tank(Game.Content.Load<Model>(@"Tank\tank"),new Vector3(-10, 0, -10), camera));

            //Add some Cubes
            int seperationDistance = 30;
            float crateSize = 5;
            addGameObject(new CubePrimitive(Game, this, new Vector3(0, 0, -seperationDistance), camera, GraphicsDevice, Game.Content.Load<Texture2D>(@"Textures/crate"), crateSize));
            addGameObject(new CubePrimitive(Game, this, new Vector3(seperationDistance, 0, 0), camera, GraphicsDevice, Game.Content.Load<Texture2D>(@"Textures/crate"), crateSize));
            addGameObject(new CubePrimitive(Game, this, new Vector3(0, 0, seperationDistance), camera, GraphicsDevice, Game.Content.Load<Texture2D>(@"Textures/crate"), crateSize));
            addGameObject(new CubePrimitive(Game, this, new Vector3(-seperationDistance, 0, 0), camera, GraphicsDevice, Game.Content.Load<Texture2D>(@"Textures/crate"), crateSize));

            //Test UI
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ui.Add(new UIString(Game, Vector2.Zero, Game.Content.Load<SpriteFont>(@"SpriteFonts\Arial"), "Test String", Color.White, 0.01f));
            //ui.Add(new UISprite(game, Vector2.Zero, game.Content.Load<Texture2D>(@"Textures\Crate"), Color.Red));

            base.LoadContent();
            Initialize();
        }

        /// <summary>
        /// For each game object in the list of objects, update them
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            objectsCurrent.Clear();
            objectsCurrent.AddRange(objectsMaster);

            foreach (GameObject obj in objectsCurrent)
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

            //Draw all 3D objects
            foreach (GameObject obj in objectsCurrent)
            {
                obj.Draw(gameTime);
            }

            //Draw all UI / 2D objects with same settings (same spritebatch)
            //System needs changing if you want different sprite batches with different settings
            spriteBatch.Begin();
            foreach (UIObject uiobj in ui)
            {
                uiobj.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void addGameObject(GameObject gameObject) {
            objectsMaster.Add(gameObject);
        }

    }
}
