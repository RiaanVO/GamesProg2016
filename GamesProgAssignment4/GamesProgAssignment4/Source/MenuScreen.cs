using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamesProgAssignment4
{
    class MenuScreen : DrawableGameComponent
    {
        //like, halfway through this tutorial: http://proquestcombo.safaribooksonline.com.ezproxy.lib.uts.edu.au/book/programming/game-programming/9781449397210/15dot-wrapping-up-your-3d-game/adding_a_splash_screen_game_component
        //above is Learning XNA 4.0 textbook, chapter 15, Adding a Splash Screen (roughly 1/2 done)

        SpriteBatch spriteBatch;
        List<UIObject> ui = new List<UIObject>();

        string header;
        string primaryText;
        string secondaryText;

        Game1.GameState currentGameState;

        public MenuScreen (Game game) : base(game)
        {
            //this.game = game; needed?
        }

        public override void Initialize()
        {
            foreach (UIObject obj in ui) obj.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            List<UIString> uiStrings = new List<UIString>();
            uiStrings.Add(new UIString(Game, Vector2.Zero, Game.Content.Load<SpriteFont>(@"SpriteFonts\Arial"), header, Color.White, 0.01f));
            uiStrings.Add(new UIString(Game, Vector2.Zero, Game.Content.Load<SpriteFont>(@"SpriteFonts\Arial"), primaryText, Color.White, 0.01f));
            uiStrings.Add(new UIString(Game, Vector2.Zero, Game.Content.Load<SpriteFont>(@"SpriteFonts\Arial"), secondaryText, Color.White, 0.01f));

            int count = 0;
            Vector2 titleSize = uiStrings[0].StringSize();
            foreach (UIString uiStr in uiStrings)
            {
                count++;
                Vector2 stringSize = uiStr.StringSize();
                uiStr.ChangePosition(new Vector2(Game.Window.ClientBounds.Width / 2 - stringSize.X / 2, Game.Window.ClientBounds.Height / 2 + titleSize.Y + (10 * count)));
            }

            foreach (UIString uiStr in uiStrings)
            {
                ui.Add(uiStr);
            }

            //ui.Add(new UIString(Game, Vector2.Zero, Game.Content.Load<SpriteFont>(@"SpriteFonts\Arial"), newGame, Color.White, 0.01f));
            //add new game and quit strings here too

            base.LoadContent();
            Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                //((Game1)Game).ChangeGameState(Game1.GameState.END, 0);
                Game.Exit();
            }
            else if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (currentGameState == Game1.GameState.LEVEL_CHANGE || currentGameState == Game1.GameState.START)
                {
                    ((Game1)Game).ChangeGameState(Game1.GameState.PLAY, 0);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (UIObject uiobj in ui) uiobj.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetData(int score, Game1.GameState currGameState)
        {
            this.currentGameState = currGameState;

            switch (currentGameState)
            {
                case Game1.GameState.START:
                    header = "a-MAZE-ing game!";
                    primaryText = "Press ENTER to start a new game";
                    secondaryText = "Press ESCAPE to quit";
                    break;
                case Game1.GameState.LEVEL_CHANGE:
                    header = "";
                    primaryText = "Press ENTER to start the next maze";
                    secondaryText = "";
                    break;
                case Game1.GameState.END:
                    header = "You died!";
                    primaryText = "Score: " + score;
                    secondaryText = "Press ENTER to return to main menu";
                    break;
            }
        }

    }
}
