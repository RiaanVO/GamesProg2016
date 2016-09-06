using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;

namespace A_MAZE_ING
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum GameState { START, PLAY, LEVEL_CHANGE, END }
        GameState currentGameState = GameState.PLAY; //change to START for proper menu functionality - set up later :)

        //MenuScreen menuScreen;
        int score = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ObjectManager objectManager;
        CollisionManager collisionManager;
        AudioManager audioManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.ToggleFullScreen();
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        public void ChangeGameState(GameState state, int level)
        {
            currentGameState = state;
            //add levels for Gold Master build
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic 
            //menuScreen = new MenuScreen(this);
            //Components.Add(menuScreen);
            //menuScreen.SetData(score, currentGameState);

            objectManager = new ObjectManager(this);
            collisionManager = new CollisionManager(this);
            Services.AddService(collisionManager);

            audioManager = new AudioManager(this);
            Services.AddService(audioManager);

            Components.Add(objectManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //SoundEffect footsteps = Content.Load<SoundEffect>(@"Sounds/footsteps");
            //footsteps.Play();

            // TODO: use this.Content to load your game content here
        }

        public void gameEnd()
        {
            //Ends the game
            currentGameState = GameState.END;
            //Does whatever it needs to in order to wrap up the game
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (currentGameState == GameState.PLAY)
            {
                if (objectManager.returnCurrentGameState() != GameState.PLAY)
                {
                    objectManager.setCurrentGameState(GameState.PLAY);
                }
                collisionManager.Update(gameTime);
                audioManager.Update(gameTime);
            }
            else
            {
                objectManager.setCurrentGameState(currentGameState);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
