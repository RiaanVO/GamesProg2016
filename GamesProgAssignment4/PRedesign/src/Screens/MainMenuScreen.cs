using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class MainMenuScreen : MenuScreen
    {

        #region initialization

        public MainMenuScreen() : base("Main Menu")
        {
            //Create the entries
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry quitMenuEntry = new MenuEntry("Quit");

            //Attach the event handlers
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            quitMenuEntry.Selected += OnCancel;

            //Resize the text
            TitleScale = 5f;
            playGameMenuEntry.BaseTextScale = 2;
            quitMenuEntry.BaseTextScale = 2;

            //Add them to the list of entries
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(quitMenuEntry);
        }

        #endregion

        #region Handle Input
        /// <summary>
        /// Handler for when the player selects play game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlayGameMenuEntrySelected(object sender, EventArgs e) {
            LoadingScreen.Load(ScreenManager, true, new GamePlayScreen());
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
        #endregion

    }
}
