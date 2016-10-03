using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign {

    class EditorOptionButton : UIButton {

        #region Fields
        Texture2D BaseTexture;
        Texture2D SelectedTexture;
        bool isSelected = false;
        int Size;
        #endregion

        #region Properties
        public bool Selected {
            get { return isSelected; }
            set { isSelected = value; }
        }
        #endregion

        #region Initialisation
        public EditorOptionButton(string id, Vector2 position, Vector2 textOffset, SpriteFont font, string text, Color textTint, Texture2D buttonTexture, Texture2D selectedTexture, int size) : base(id, position, textOffset, font, text, textTint, buttonTexture) {
            BaseTexture = buttonTexture;
            SelectedTexture = selectedTexture;
            Size = size;

            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Size, Size);
        }
        #endregion

        #region Update and Draw
        public override void Draw(SpriteBatch spriteBatch) {
            if (isSelected) {
                Texture = SelectedTexture;
            } else {
                Texture = BaseTexture;
            }

            if (Visible) {
                Point drawBase = Point.Zero;
                if (Disabled)
                    // drawBase = new Point(0, Bounds.Height);
                    drawBase = new Point(0, (int)Font.MeasureString(Text).Y);
                if (Pressed)
                    // drawBase = new Point(0, Bound.Height* 2);
                    drawBase = new Point(0, ((int)Font.MeasureString(Text).Y) * 2);
                if (Texture != null)
                    // spriteBatch.Draw(Texture, Position, new Rectangle(drawBase.X, drawBase.Y, Bounds.Width, Bounds.Height), Color.White);
                    spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Size, Size / 2), Color.White);
                spriteBatch.DrawString(Font, Text, new Vector2(Position.X + (Size - (int)Font.MeasureString(Text).X) / 2, Position.Y + (Size / 2 - (int)Font.MeasureString(Text).Y) / 2), TextTint);
            }

        }
        #endregion

    }
}
