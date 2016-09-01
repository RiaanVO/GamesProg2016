using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class UISprite : UIObject
    {
        Texture2D sprite;

        public UISprite(Game game, Vector2 startPos, Texture2D sprite, Color color) : base(startPos, game, color)
        {
            this.sprite = sprite;
        }

        /// <summary>
        /// Draws the string
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, null, origin, rotation, scale, color, effect, layerDepth);
        }

        public virtual void SetPosition(Vector2 position)
        {
            this.position = position;
        }
    }
}
