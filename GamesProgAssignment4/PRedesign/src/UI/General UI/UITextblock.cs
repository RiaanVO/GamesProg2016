using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class UITextblock : UIWidget
    {
        #region Properties
        public Vector2 TextOffset { get; set; }
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public Color TextTint { get; set; }
        #endregion

        #region Initialization
        public UITextblock(string id, Vector2 position, Vector2 textOffset, SpriteFont font, string text, Color textTint) : base(id, position)
        {
            TextOffset = TextOffset;
            Font = font;
            Text = text;
            TextTint = textTint;
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// Draw the text block
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.DrawString(Font, Text, Position + TextOffset, TextTint);
            base.Draw(spriteBatch);
        }
        #endregion
    }
}
