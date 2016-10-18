using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class MessageBoxScreen : GameScreen
    {
        #region Fields
        string message;
        Texture2D gradientTexture;
        #endregion

        #region Events
        public event EventHandler Accepted;
        public event EventHandler Cancelled;
        #endregion

        #region Initialization
        /// <summary>
        /// Create the generic message box screen
        /// </summary>
        /// <param name="message"></param>
        public MessageBoxScreen(string message) : this(message, true) { }

        /// <summary>
        /// Full constuctor for making the message box screen
        /// </summary>
        /// <param name="message"></param>
        /// <param name="includeUsageText"></param>
        public MessageBoxScreen(string message, bool includeUsageText) {
            const string usageText = "\nEnter = ok" + "\nEsc = cancel";
            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            ObjectManager.Game.IsMouseVisible = false;
        }

        /// <summary>
        /// Load the message box screen backgound texture
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            gradientTexture = content.Load<Texture2D>(@"Textures/gradient");
        }
        #endregion

        #region Handle Input
        /// <summary>
        /// Check if the keys have been pressed and then fire the event handlers
        /// </summary>
        public override void HandleInput(InputState inputState)
        {
            if (inputState.IsMenuSelect()){
                if (Accepted != null)
                    Accepted(this, new EventArgs());

                ExitScreen();
            } else if (inputState.IsMenuCancel()){
                if (Cancelled != null)
                    Cancelled(this, new EventArgs());

                ExitScreen();
            }
        }
        #endregion

        #region Draw
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.SpriteFont;

            //Fade any other screens to black a little
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            const int hPad = 32;
            const int vPad = 16;
            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad, (int)textPosition.Y - vPad, (int)textSize.X + hPad * 2, (int)textSize.Y + vPad * 2);

            Color colour = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            //Draw the background rectangle
            if(gradientTexture != null)
                spriteBatch.Draw(gradientTexture, backgroundRectangle, colour);

            //Draw the message box text
            spriteBatch.DrawString(font, message, textPosition, colour);

            spriteBatch.End();
        }
        #endregion


    }
}
