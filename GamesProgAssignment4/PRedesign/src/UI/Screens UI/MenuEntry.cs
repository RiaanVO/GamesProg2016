using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class MenuEntry
    {
        #region Fields
        string text;
        float selectionFade;
        Vector2 position;
        float fadeSpeedScale;
        float baseTextScale;
        #endregion

        #region Properties
        public string Text {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }

        public float BaseTextScale {
            get { return baseTextScale; }
            set { baseTextScale = value; }
        }
        #endregion

        #region Events
        public event EventHandler Selected;

        protected internal virtual void OnSelectEntry() {
            if (Selected != null)
                Selected(this, new EventArgs());
        }
        #endregion

        #region Initialization
        public MenuEntry(string text) {
            this.text = text;
            fadeSpeedScale = 4;
            baseTextScale = 1;
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// Updates the menu entry - need to work on if there is mouse input, should not show that it is selected.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="isSelected"></param>
        /// <param name="gameTime"></param>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime) {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * fadeSpeedScale;
            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }
        
        /// <summary>
        /// Draws the menu entry
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="isSelected"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime) {
            Color colour = isSelected ? Color.YellowGreen : Color.White;
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = baseTextScale + pulsate * 0.05f * selectionFade;

            colour *= screen.TransitionAlpha;

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.SpriteFont;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, colour, 0, origin, scale, SpriteEffects.None, 0);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Returns how much space in height the menu entry requires
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public virtual int GetHeight(MenuScreen screen)
        {
            return (int)(screen.ScreenManager.SpriteFont.LineSpacing * baseTextScale);
        }

        /// <summary>
        /// Returns how long the menu option is
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)(screen.ScreenManager.SpriteFont.MeasureString(text).X * baseTextScale);
        }
        #endregion
    }
}
