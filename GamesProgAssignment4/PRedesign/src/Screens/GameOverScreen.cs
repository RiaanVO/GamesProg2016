using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class GameOverScreen : MenuScreen
    {
        string timeScore;

        public GameOverScreen(double time) : base("Game Over")
        {
            //Show player score... how?

            timeScore = time.ToString("0.00");
            SubTitle = "You survived the maze for " + timeScore + " seconds.";

            MenuEntry restartGameEntry = new MenuEntry("Restart Game");
            MenuEntry returnToMainMenuEntry = new MenuEntry("Main Menu");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            restartGameEntry.Selected += restartGameEntrySelected;
            returnToMainMenuEntry.Selected += OnMainMenuSelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            //insert handlers here

            MenuEntries.Add(restartGameEntry);
            MenuEntries.Add(returnToMainMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
            ObjectManager.Game.IsMouseVisible = false;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.SpriteFont;

            Color color = Color.White * TransitionAlpha;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }

        void restartGameEntrySelected(Object sender, EventArgs e)
        {
            //go back to first level
            //OR
            //go back to the start of THIS level (?)
        }

        /// <summary>
        /// Event handler for the quite game menu option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void QuitGameMenuEntrySelected(object sender, EventArgs e)
        {
            const string message = "Are you sure you want to quite the game?";
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmQuitMessageBox);
        }

        //Event handler for the message box to quite the game and return to the main menu
        void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e)
        {
            ObjectManager.clearAll();
            ScreenManager.Game.Exit();
        }

        /// <summary>
        /// Event handler for the return to main menu option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMainMenuSelected(object sender, EventArgs e)
        {
            const string message = "Are you sure you want to return to the main menu?";
            MessageBoxScreen confirmMainMenuMessageBox = new MessageBoxScreen(message);
            confirmMainMenuMessageBox.Accepted += ConfirmMainMenuMessageBoxAccepted;
            ScreenManager.AddScreen(confirmMainMenuMessageBox);
        }

        //Event handler for the message box to return to the main menu
        void ConfirmMainMenuMessageBoxAccepted(object sender, EventArgs e)
        {
            ObjectManager.clearAll();
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }
    }
}
