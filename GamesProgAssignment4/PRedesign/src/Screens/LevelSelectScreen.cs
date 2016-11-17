using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PRedesign
{
    class LevelSelectScreen : MenuScreen
    {
        int currentLevelToStart = 1;
        string levelSelectDisplayText;
        MenuEntry playLevelEntry;

        public LevelSelectScreen() : base("Levels:")
        {
            MenuStartingYPosition = 300f;
            TitleYPosition = 220f;

            levelSelectDisplayText = "<--| Play Level: " + currentLevelToStart + " out of " + LevelManager.NumberOfLevels + " |-->";

            //Create the entries
            playLevelEntry = new MenuEntry(levelSelectDisplayText);
            MenuEntry returnToMainMenuEntry = new MenuEntry("Main Menu");

            //Attach the event handlers
            playLevelEntry.Selected += PlayLevelSelected;
            returnToMainMenuEntry.Selected += ReturnToMainMenuSelected;

            //Resize the text
            TitleScale = 3f;
            playLevelEntry.BaseTextScale = 1;
            returnToMainMenuEntry.BaseTextScale = 1;

            //Add them to the list of entries
            MenuEntries.Add(playLevelEntry);
            MenuEntries.Add(returnToMainMenuEntry);

            ObjectManager.Game.IsMouseVisible = false;
        }

        #region Update and draw
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen("Textures/MainMenuBG"), new MainMenuScreen());
            }
        }

        #endregion

        #region Handle Input

        public override void HandleInput(InputState inputState)
        {
            //Handle the base input
            base.HandleInput(inputState);

            if (inputState.IsNewKeyPressed(Keys.Left))
            {
                currentLevelToStart--;
                if (currentLevelToStart <= 0)
                    currentLevelToStart = LevelManager.NumberOfLevels;
                updateMenuText();
            }

            if (inputState.IsNewKeyPressed(Keys.Right))
            {
                currentLevelToStart++;
                if (currentLevelToStart > LevelManager.NumberOfLevels)
                    currentLevelToStart = 1;
                updateMenuText();
            }
        }

        void updateMenuText() {
            if (playLevelEntry == null) return;
            levelSelectDisplayText = "<--| Play Level: " + currentLevelToStart + " out of " + LevelManager.NumberOfLevels + " |-->";
            playLevelEntry.Text = levelSelectDisplayText;
        }

        /// <summary>
        /// Handler for when the player selects play game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlayLevelSelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GamePlayScreen(currentLevelToStart));
        }
        

        void ReturnToMainMenuSelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen("Textures/MainMenuBG"), new MainMenuScreen());
        }


        #endregion
    }
}
