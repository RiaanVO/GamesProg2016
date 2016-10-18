using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PRedesign
{
    class GamePlayScreen : GameScreen, IDisposable
    {
        #region Fields
        ContentManager content;
        SpriteFont gameFont;
        Texture2D crosshair;
        Rectangle crosshairBounds;

        float pauseAlpha;
        #endregion

        #region initialization
        public GamePlayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //Load game play content here
            gameFont = content.Load<SpriteFont>(@"Fonts/GameFont");

            //Load textures
            ContentStore.Add("ground", content.Load<Texture2D>(@"Textures/white floor"));
            ContentStore.Add("ceiling", content.Load<Texture2D>(@"Textures/ceiling v3"));
            ContentStore.Add("wall", content.Load<Texture2D>(@"Textures/blue stripe wall"));
            ContentStore.Add("crosshair", content.Load<Texture2D>(@"Textures/crosshair_large"));
            crosshair = ContentStore.loadedTextures["crosshair"];
            crosshairBounds = new Rectangle(ObjectManager.Game.Window.ClientBounds.Width / 2 - 15, ObjectManager.Game.Window.ClientBounds.Height / 2 - 15, 30, 30);

            //Load models
            ContentStore.Add("skybox", content.Load<Model>(@"Models/TechnoSkybox Model/technobox"));
            ContentStore.Add("tetraKey", content.Load<Model>(@"Models/TetraKey Model/SplitDiamond"));
            ContentStore.Add("tetraEnemy", content.Load<Model>(@"Models/Enemy Model/TetraEnemyRed"));
            ContentStore.Add("spikes", content.Load<Model>(@"Models/Spikes Model/red_spikes_v15_shorter"));
            ContentStore.Add("tetraDoor", content.Load<Model>(@"Models/TetraDoor Model/TetraDoorRigged"));
            ContentStore.Add("soundGun", content.Load<Model>(@"Models/Soundgun Model/SoundGunV2"));
            ContentStore.Add("sphere", content.Load<Model>(@"Models/Sphere model/hp_sphere"));

            //Load sounds
            ContentStore.Add("footsteps", content.Load<SoundEffect>(@"Sounds/Effects/footsteps"));
            ContentStore.Add("key", content.Load<SoundEffect>(@"Sounds/Effects/key"));
            ContentStore.Add("choir", content.Load<SoundEffect>(@"Sounds/Effects/choir"));
            ContentStore.Add("hover", content.Load<SoundEffect>(@"Sounds/hover"));
            ContentStore.Add("laser", content.Load<SoundEffect>(@"Sounds/Effects/laser"));
            Song bgMusic = content.Load<Song>(@"Sounds/Music/The Lift");
            MediaPlayer.Play(bgMusic);
            MediaPlayer.Volume = 0.05f;

            BasicEffect basicEffect = new BasicEffect(ScreenManager.GraphicsDevice); //Not needed?

            /* //Moved to LevelManager.LoadGameObjects()
            BasicCamera camera = new BasicCamera(new Vector3(0, 10, 0), new Vector3(-1, 10, 0), Vector3.Up, ScreenManager.GraphicsDevice.Viewport.AspectRatio);
            camera.FarClip = 3000;
            ObjectManager.Camera = camera;
            ObjectManager.GraphicsDevice = ScreenManager.GraphicsDevice;
            ObjectManager.Game = ScreenManager.Game;

            Player player = new Player(new Vector3(20, 3.5f, 20));
            LevelManager.Player = player;

            Skybox skybox = new Skybox(Vector3.Zero, ContentStore.loadedModels["skybox"]);
            skybox.Player = player;
            */
            LevelManager.ScreenManager = ScreenManager;
            LevelManager.LoadLevel(1);

            //ObjectManager.addGameObject(new TetraKey(new Vector3(7f,5f, 20f), ContentStore.loadedModels["tetraKey"], camera, player));

            /* //Test code no longer needed due to level load
            NPCEnemy Enemy = new NPCEnemy(new Vector3(20, 6, 20), tetraEnemyModel, player);
            Enemy.Scale = 0.08f;
            Enemy.HasLighting = true;
            Enemy.PatrolPoints = new Vector3[] {
                new Vector3(8, 5, 13),
                new Vector3(8, 5, 48),
                new Vector3(33, 5, 48),
            };
            */

            //Once load has been completed, tell the game to not try and catch up frames - mainly for long loads
            ScreenManager.Game.ResetElapsedTime();
            ObjectManager.Game.IsMouseVisible = false;
        }

        public override void UnloadContent()
        {
            content.Unload();
        }
        #endregion

        #region Update and Draw
        public override void HandleInput(InputState inputState)
        {
            if (inputState == null)
                throw new ArgumentNullException("Inputstate is null");

            if (inputState.IsPauseGame())
            {
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
            else
            {

                //Place input handling here



                ////////////////////////////////
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //Gradually fade in or out the screen depeding if it is covered by other stuff
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            //Place update logic here so that the game will only update when it is active
            if (IsActive)
            {
                //Stuff

                ObjectManager.Update(gameTime);
                AudioManager.Update(gameTime);
                ////////////////////////////
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Place draw logic here for game play
            ObjectManager.Draw(gameTime);

            //Debugging
            //ObjectMetaDrawer.RenderNavigationMap(Color.Violet);
            //CollisionManager.Render(Color.Violet, true, true);
            //WireShapeDrawer.Draw(gameTime, ObjectManager.Camera.View, ObjectManager.Camera.Projection);
            //===============

            if (IsActive)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                spriteBatch.Begin();
                spriteBatch.DrawString(gameFont, "Time Taken: " + gameTime.TotalGameTime.TotalSeconds.ToString("0.00"), Vector2.Zero, Color.Black);
                spriteBatch.DrawString(gameFont, "HP: " + LevelManager.PlayerHealth + " / 10", new Vector2(0f, 20f), Color.Red);
                spriteBatch.Draw(crosshair, crosshairBounds, Color.White);
                spriteBatch.End();
            }

            

            //////////////////////////////////////

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    content.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GamePlayScreen() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
