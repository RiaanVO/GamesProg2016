using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
namespace PRedesign
{
    class MainMenuScreen : MenuScreen
    {

        #region initialization

        public MainMenuScreen() : base("nodeHack()")
        {
            MenuStartingYPosition = 300f;
            TitleYPosition = 220f;

            //Create the entries
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry openLevelEditorEntry = new MenuEntry("Level Editor");
            MenuEntry quitMenuEntry = new MenuEntry("Quit");

            //Attach the event handlers
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            openLevelEditorEntry.Selected += OpenLevelEditorEntrySelected;
            quitMenuEntry.Selected += OnCancel;

            //Resize the text
            TitleScale = 3f;
            playGameMenuEntry.BaseTextScale = 1;
            openLevelEditorEntry.BaseTextScale = 1;
            quitMenuEntry.BaseTextScale = 1;

            //Level select - experimental
            MenuEntry openLevelSelectEntry = new MenuEntry("Level Select");
            openLevelSelectEntry.Selected += LevelSelectEntrySelected;
            openLevelSelectEntry.BaseTextScale = 1;

            //Add them to the list of entries
            MenuEntries.Add(playGameMenuEntry);
            //=======================
            MenuEntries.Add(openLevelSelectEntry);
            //========================
            MenuEntries.Add(openLevelEditorEntry);
            MenuEntries.Add(quitMenuEntry);

            ObjectManager.Game.IsMouseVisible = false;
        }

        #endregion

        #region Handle Input
        /// <summary>
        /// Handler for when the player selects play game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlayGameMenuEntrySelected(object sender, EventArgs e) {
            LoadingScreen.Load(ScreenManager, true, new GamePlayScreen(1));
        }

        void OpenLevelEditorEntrySelected(object sender, EventArgs e) {
            LoadingScreen.Load(ScreenManager, true, new LevelEditorScreen());
        }

        protected override void OnCancel()
        {
            const string message = "Are you sure you want to quit?";
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmQuitMessageBox);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e) {
            ScreenManager.Game.Exit();
        }

        void LevelSelectEntrySelected(object sender, EventArgs e)
        {
            Console.WriteLine("Level Select selected");
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen("Textures/MainMenuBG"), new LevelSelectScreen());
        }
        #endregion

    }
}
