using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PRedesign
{
    class LoadingScreen : GameScreen
    {
        #region Fields
        bool loadingIsSlow;
        bool otherScreensAreGone;

        GameScreen[] screensToLoad;

        float textChangeTime;
        float textChangeDelay;
        int currentTextIndex;
        static readonly string[] messages = {"Loading", "Loading.", "Loading..", "Loading..."};
        #endregion

        #region Initialization
        /// <summary>
        /// Loading screens should be activated from a static method
        /// </summary>
        /// <param name="screenManager"></param>
        /// <param name="loadingIsSlow"></param>
        /// <param name="screensToLoad"></param>
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, GameScreen[] screensToLoad) {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            textChangeTime = 0;
            textChangeDelay = 1;
            currentTextIndex = 0;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Activate the loading screen
        /// </summary>
        /// <param name="screenManager"></param>
        /// <param name="loadingIsSlow"></param>
        /// <param name="screensToLoad"></param>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, params GameScreen[] screensToLoad) {
            //Tell all screens to transition off
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            //Create the loading screen and activate it
            LoadingScreen loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, screensToLoad);
            screenManager.AddScreen(loadingScreen);
        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (otherScreensAreGone)
                {
                    ScreenManager.RemoveScreen(this);

                    foreach (GameScreen screen in screensToLoad)
                        if (screen != null)
                            ScreenManager.AddScreen(screen);

                    //After a long load tell the game not to try an catch up frames
                    ScreenManager.Game.ResetElapsedTime();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (ScreenState == ScreenState.Active && ScreenManager.GetScreens().Length == 1)
                otherScreensAreGone = true;

            if (loadingIsSlow) {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.SpriteFont;

                textChangeDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textChangeTime >= textChangeDelay) {
                    textChangeTime = 0;
                    currentTextIndex++;
                    if (currentTextIndex >= messages.Length)
                        currentTextIndex = 0;
                }

                string message = messages[currentTextIndex];

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(messages[1]);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                // Draw the text.
                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
                
        }
        #endregion
    }
}
