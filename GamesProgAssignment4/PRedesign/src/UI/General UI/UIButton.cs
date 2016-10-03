using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class UIButton : UITextblock
    {
        #region Properties
        public Texture2D Texture { get; set; }
        public bool Disabled { get; set; }
        public bool Pressed { get; set; }
        public Rectangle Bounds { get; protected set; }
        public int Padding { get; set; }
        #endregion

        #region Event related Items
        public delegate void ClickHandler(Object sender, UIButtonArgs e);
        public event ClickHandler Clicked;
        #endregion

        #region Initialize
        public UIButton(string id, Vector2 position, Vector2 textOffset, SpriteFont font, string text, Color textTint, Texture2D buttonTexture) : base(id, position, textOffset, font, text, textTint)
        {
            Texture = buttonTexture;
            //For stacked button textures
            //Bounds = new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
            if(Bounds == Rectangle.Empty)
            Bounds = new Rectangle((int)position.X, (int)position.Y, (int)font.MeasureString(Text).X, (int)font.MeasureString(Text).Y);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Returns true if the button contains the location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool Contains(Point location) {
            return Visible && Bounds.Contains(location);
        }

        /// <summary>
        /// Returns true if the button contains the location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool Contains(Vector2 location) {
            return Contains(new Point((int)location.X, (int)location.Y));
        }

        /// <summary>
        /// Tests to see if the mouse position is on the button and fires an event if it is
        /// </summary>
        /// <param name="location"></param>
        public void HitTest(Point location) {
            if (Visible && !Disabled) {
                if (Contains(location)) {
                    Pressed = true;
                    Clicked(this, new UIButtonArgs(this.ID, new Vector2(location.X, location.Y)));
                }
                else
                {
                    Pressed = false;
                }
            }
        }

        #endregion

        #region Update and Draw 
        /// <summary>
        /// Draw the button depending on its current state
        /// This is if the same texture contains all the different states of the button
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
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
                    spriteBatch.Draw(Texture, new Rectangle((int)Position.X - Padding, (int)Position.Y - Padding, (int)Font.MeasureString(Text).X + Padding * 2, (int)Font.MeasureString(Text).Y + Padding * 2), Color.White);
            }

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
