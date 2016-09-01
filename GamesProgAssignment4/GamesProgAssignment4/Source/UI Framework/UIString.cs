using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class UIString : UIObject
    {
        SpriteFont font;
        string str;

        public UIString(Game game, Vector2 startPos, SpriteFont font, string str, Color color, float scale) : base(startPos, game, color)
        {
            this.font = font;
            this.str = str;
        }

        /// <summary>
        /// Draws the string
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, str, position, color);
            
            //Uncomment when fixed
            //spriteBatch.DrawString(font, str, position, color, rotation, origin, scale, effect, layerDepth);
        }

        public virtual void SetPosition(Vector2 position)
        {
            this.position = position;
        }
    }
}
