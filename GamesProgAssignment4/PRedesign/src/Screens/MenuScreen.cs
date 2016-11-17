using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace PRedesign
{
    class MenuScreen : GameScreen
    {
        #region Fields
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;
        string menuTitle;
        string menuSubtitle;
        float titleScale;

        float titleYPosition = 80f;
        float menuStartingYPosition = 175f;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the list of menu entries, so that derived classes can edit the list
        /// </summary>
        protected IList<MenuEntry> MenuEntries {
            get { return menuEntries; }
        }

        protected float TitleScale {
            get { return titleScale; }
            set { titleScale = value; }
        }

        protected string SubTitle
        {
            get { return menuSubtitle; }
            set { menuSubtitle = value; }
        }

        protected float TitleYPosition {
            set { titleYPosition = value; }
        }
        protected float MenuStartingYPosition
        {
            set { menuStartingYPosition = value; }
        }
        #endregion

        #region Initialization
        public MenuScreen(string menuTitle) {
            this.menuTitle = menuTitle;
            menuSubtitle = "";

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            titleScale = 3f;

            //The menus are placed on top of other screens
            IsPopup = true;

            ObjectManager.Game.IsMouseVisible = false;
        }
        #endregion

        #region Handle Input
        /// <summary>
        /// handles the different input cases when dealing with a menu
        /// </summary>
        public override void HandleInput(InputState inputState)
        {
            if (inputState.IsMenuUp()) {
                selectedEntry--;
                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            if (inputState.IsMenuDown()){
                selectedEntry++;
                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            if (inputState.IsMenuSelect())
                OnSelectEntry(selectedEntry);

            if (inputState.IsMenuCancel())
                OnCancel();
        }

        /// <summary>
        /// Handles the event of a player selecting an entry
        /// </summary>
        /// <param name="entryIndex"></param>
        protected virtual void OnSelectEntry(int entryIndex) {
            menuEntries[entryIndex].OnSelectEntry();
        }
        
        /// <summary>
        /// Handles the event of a player quiting the menu
        /// </summary>
        protected virtual void OnCancel() {
            ExitScreen();
        }

        /// <summary>
        /// Helper Overload for the handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCancel(object sender, EventArgs e) {
            OnCancel();
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// Allows the screen to position the menu entries
        /// </summary>
        protected virtual void UpdateMenuEntryLocations() {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            Vector2 position = new Vector2(0f, menuStartingYPosition);

            for (int i = 0; i < menuEntries.Count; i++) {
                MenuEntry menuEntry = menuEntries[i];
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                menuEntry.Position = position;

                position.Y += menuEntry.GetHeight(this);
            }
        }

        /// <summary>
        /// Update all of the menu entries
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < menuEntries.Count; i++) {
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draw the title and the menu entries
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.SpriteFont;

            spriteBatch.Begin();

            for (int i = 0; i < menuEntries.Count; i++) {
                MenuEntry menuEntry = menuEntries[i];
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntry.Draw(this, isSelected, gameTime);
            }

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, titleYPosition);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColour = new Color(192, 192, 192) * TransitionAlpha;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColour, 0, titleOrigin, titleScale, SpriteEffects.None, 0);

            if (SubTitle != "")
            {
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 textSize = font.MeasureString(SubTitle);
                Vector2 textPosition = new Vector2((viewport.Width - textSize.X) / 2, (viewport.Height - textSize.Y) / 3);

                spriteBatch.DrawString(font, menuSubtitle, textPosition, Color.White);
            }

            spriteBatch.End();
        }
        #endregion
    }
}
