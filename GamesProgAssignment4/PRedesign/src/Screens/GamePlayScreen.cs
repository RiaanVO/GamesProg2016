using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PRedesign
{
    class GamePlayScreen : GameScreen
    {
        #region Fields
        ContentManager content;
        SpriteFont gameFont;

        float pauseAlpha;
        #endregion

        #region initialization
        public GamePlayScreen() {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //Load game play content here
            gameFont = content.Load<SpriteFont>(@"Fonts/GameFont");
        
            //Once load has been completed, tell the game to not try and catch up frames - mainly for long loads
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //Gradually fade in or out the screen depeding if it is covered by other stuff
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            //Place update logic here so that the game will only update when it is active
            if (IsActive) {
                //Stuff



                ////////////////////////////
            }
        }

        public override void HandleInput(InputState inputState)
        {
            if (inputState == null)
                throw new ArgumentNullException("Inputstate is null");

            if (inputState.IsPauseGame()) {
                ScreenManager.AddScreen(new PauseMenuScreen());
            } else {

                //Place input handling here



                ////////////////////////////////
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            
            
            //Place draw logic here for game play


            //////////////////////////////////////

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        #endregion
    }
}
