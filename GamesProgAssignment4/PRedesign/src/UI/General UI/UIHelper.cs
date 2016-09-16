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
    static class UIHelper
    {
        #region Fields
        public static Texture2D ButtonTexture;
        public static SpriteFont ButtonFont;
        #endregion

        #region Helper Methods
        /// <summary>
        /// creates a default button
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static UIButton CreateButton(string id, string text, int x, int y) {
            UIButton b = new UIButton(id, new Vector2(x, y), new Vector2(25 - ButtonFont.MeasureString(text).X / 2, 10), ButtonFont, text, Color.White, ButtonTexture);
            b.Disabled = false;
            return b;
        }

        /// <summary>
        /// Creates a default text block
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static UITextblock CreateTextBlock(string id, string text, int x, int y) {
            return new UITextblock(id, new Vector2(x, y), Vector2.Zero, ButtonFont, text, Color.White);
        }

        /// <summary>
        /// Sets the disabled state of the ui button
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="disabled"></param>
        /// <param name="uiElements"></param>
        public static void SetButtonState(string prefix, bool disabled, Dictionary<string, UIWidget> uiElements) {
            foreach (string widget in uiElements.Keys) {
                if (uiElements[widget].ID.StartsWith(prefix)) 
                    if (uiElements[widget] is UIButton) 
                        ((UIButton)uiElements[widget]).Disabled = disabled;
            }
        }

        /// <summary>
        /// Sets the visiability of the ui element
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="visible"></param>
        /// <param name="uiElements"></param>
        public static void SetElementVisibility(string prefix, bool visible, Dictionary<string, UIWidget> uiElements)
        {
            foreach (string widget in uiElements.Keys)
            {
                if (uiElements[widget].ID.StartsWith(prefix))
                    uiElements[widget].Visible = visible;
            }
        }

        /// <summary>
        /// Sets the text of a ui text block
        /// </summary>
        /// <param name="uiElement"></param>
        /// <param name="text"></param>
        public static void SetElementText(UIWidget uiElement, string text) {
            if (uiElement is UITextblock)
                ((UITextblock)uiElement).Text = text;
        }
        #endregion
    }
}
