using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class PauseMenuScreen : MenuScreen
    {

        #region Initialization
        public PauseMenuScreen() : base("Paused")
        {
            //Create the menu entries
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            //Set up their handlers
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            //Add them to the list of options
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }
        #endregion

        #region Handle Input
        /// <summary>
        /// Event handler for the quite game menu option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void QuitGameMenuEntrySelected(object sender, EventArgs e) {
            const string message = "Are you sure you want to quite the game?";
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmQuitMessageBox);
        }

        //Event handler for the message box to quite the game and return to the main menu
        void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e) {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }
        #endregion
    }
}
