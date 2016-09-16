using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PRedesign
{
    public class InputState
    {
        #region Fields
        public KeyboardState CurrentKeyboardState { get; protected set; }
        public KeyboardState PreviousKeyboardState { get; protected set; }
        #endregion

        #region Initialization
        public InputState() {
            CurrentKeyboardState = new KeyboardState();
            PreviousKeyboardState = new KeyboardState();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Updates the keyboard state
        /// </summary>
        public void Update() {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Checks if a new key has been pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsNewKeyPressed(Keys key) {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks to see if the enter key has been pressed for menu control
        /// </summary>
        /// <returns></returns>
        public bool IsMenuSelect() {
            return IsNewKeyPressed(Keys.Enter);
        }
    
        /// <summary>
        /// Checks to see if the escape key has been pressed for menu control
        /// </summary>
        /// <returns></returns>
        public bool IsMenuCancel() {
            return IsNewKeyPressed(Keys.Escape);
        }

        /// <summary>
        /// Checks to see if the up arrow is pressed for menu control
        /// </summary>
        /// <returns></returns>
        public bool IsMenuUp() {
            return IsNewKeyPressed(Keys.Up);
        }

        /// <summary>
        /// Checks to see if the down arrow is pressed for menu control
        /// </summary>
        /// <returns></returns>
        public bool IsMenuDown()
        {
            return IsNewKeyPressed(Keys.Down);
        }

        /// <summary>
        /// Checks to see if the game should be paused
        /// </summary>
        /// <returns></returns>
        public bool IsPauseGame() {
            return IsNewKeyPressed(Keys.Escape);
        }
        #endregion
    }
}
