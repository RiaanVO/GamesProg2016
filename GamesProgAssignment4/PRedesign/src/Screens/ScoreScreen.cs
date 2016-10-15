using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class ScoreScreen : GameScreen
    {
        String scoreString;

        public ScoreScreen(double score)
        {
            scoreString = score.ToString("0.00");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont spriteFont = ScreenManager.SpriteFont;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = spriteFont.MeasureString(scoreString);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, scoreString, textPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
