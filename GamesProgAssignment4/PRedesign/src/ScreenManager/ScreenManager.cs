using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        InputState inputState = new InputState();

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;

        bool isInitialized;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the default spritebatch shared by all screens
        /// </summary>
        public SpriteBatch SpriteBatch {
            get { return spriteBatch; }
        }

        /// <summary>
        /// Returns the default sprite font shared by all screens
        /// </summary>
        public SpriteFont SpriteFont {
            get { return font; }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Creates a new Screen Manager
        /// </summary>
        /// <param name="game"></param>
        public ScreenManager(Game game) : base(game) {}

        /// <summary>
        /// Initializes the screen manager
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            isInitialized = true;
        }

        /// <summary>
        /// Load your graphics content
        /// </summary>
        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load <SpriteFont>(@"Fonts/MenuFont");
            blankTexture = content.Load<Texture2D>(@"Textures/blank");

            foreach (GameScreen screen in screens) {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Unload your graphics content
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (GameScreen screen in screens) {
                screen.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates each screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //Read the keyboard
            inputState.Update();

            //Make a copy of the master screen list to prevent confusion when adding and removing screens
            screensToUpdate.Clear();
            screensToUpdate.AddRange(screens);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(inputState);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new screen to the manager
        /// </summary>
        /// <param name="screen"></param>
        public void AddScreen(GameScreen screen) {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            if (isInitialized)
                screen.LoadContent();

            screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen) {
            if (isInitialized)
                screen.UnloadContent();

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        /// <summary>
        /// Expose and array holding all the screens.
        /// </summary>
        /// <returns></returns>
        public GameScreen[] GetScreens() {
            return screens.ToArray();
        }

        /// <summary>
        /// Draw a translucent black fullscreen sprite, used for transitioning and fading to black
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin();

            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * alpha);

            spriteBatch.End();
        }

        #endregion


    }
}
