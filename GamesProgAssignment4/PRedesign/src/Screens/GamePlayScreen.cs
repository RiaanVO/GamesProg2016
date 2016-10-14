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
    class GamePlayScreen : GameScreen
    {
        #region Fields
        ContentManager content;
        SpriteFont gameFont;

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

            //Load content
            Texture2D groundTexture = content.Load<Texture2D>(@"Textures/white floor");
            Texture2D ceilingTexture = content.Load<Texture2D>(@"Textures/ceiling v3");
            Model skyModel = content.Load<Model>(@"Models/TechnoSkybox Model/technobox");
            BasicEffect basicEffect = new BasicEffect(ScreenManager.GraphicsDevice);
            Model tankModel = content.Load<Model>(@"Models/Enemy Model/tank");
            Texture2D crateTexture = content.Load<Texture2D>(@"Textures/blue stripe wall");
            //New models
            Model tetraKeyModel = content.Load<Model>(@"Models/TetraKey Model/SplitDiamond");
            Model tetraEnemyModel = content.Load<Model>(@"Models/Enemy Model/TetraEnemyRed");
            Model spikesModel = content.Load<Model>(@"Models/Spikes Model/red_spikes_v15_shorter");
				//Music
            Song bgMusic = content.Load<Song>(@"Sounds/Music/The Lift");
            MediaPlayer.Play(bgMusic);
            //Create camera and set up object manager
            BasicCamera camera = new BasicCamera(new Vector3(0, 10, 0), new Vector3(-1, 10, 0), Vector3.Up, ScreenManager.GraphicsDevice.Viewport.AspectRatio);
            camera.FarClip = 3000;
            ObjectManager.Camera = camera;
            ObjectManager.GraphicsDevice = ScreenManager.GraphicsDevice;
            ObjectManager.Game = ScreenManager.Game;

            /*
            int levelWidth = 100;
            int tileSize = 3;
            NavigationMap.CreateNavigationMap(levelWidth, levelWidth, tileSize);

            Player player = new Player(Vector3.Zero);

            Skybox skybox = new Skybox(Vector3.Zero, skyModel);
            skybox.Player = player;

            GroundPrimitive ground = new GroundPrimitive(Vector3.Zero, groundTexture, tileSize, levelWidth);
            ground.CenterGridPlane = false;

            Tank tank = new Tank(new Vector3(4,0,4), tankModel);
            tank.Scale = 0.25f;
            player.Tank = tank;

            int numCrates = 10;
            List<CubePrimitive> crates = new List<CubePrimitive>();
            for(int x = 0; x < numCrates; x+=4){
                for (int z = 0; z < numCrates; z+=4) {
                    crates.Add(new CubePrimitive(new Vector3(x *tileSize, 0, z * tileSize), crateTexture, tileSize));
                    crates.Add(new CubePrimitive(new Vector3((x + 1) * tileSize, 0, z * tileSize), crateTexture, tileSize));

                }
            }
            foreach (CubePrimitive crate in crates) {
                NavigationMap.setSearchNodeObstructed(crate.CenteredPosition, true);
            }
            */
            Player player = new Player(new Vector3(20, 3.5f, 20));

            Skybox skybox = new Skybox(Vector3.Zero, skyModel);
            skybox.Player = player;

            LevelManager.GroundTexture = groundTexture;
            LevelManager.WallTexture = crateTexture;
            LevelManager.CeilingTexture = ceilingTexture;
            LevelManager.LoadLevel(1);

            ObjectManager.addGameObject(new TetraKey(new Vector3(7f,5f, 20f), tetraKeyModel, camera, player));

            ObjectManager.addGameObject(new Spikes(new Vector3(0f, 0f, 15f), spikesModel));

            /*Tank tank = new Tank(new Vector3(3, 0, 8), tankModel);
            tank.Scale = 0.2f;
            player.Tank = tank;*/

            NPCEnemy Enemy = new NPCEnemy(new Vector3(20, 6, 20), tetraEnemyModel, player);
            Enemy.Scale = 0.08f;
            Enemy.HasLighting = true;
            Enemy.PatrolPoints = new Vector3[] {
                new Vector3(8, 5, 13),
                new Vector3(8, 5, 48),
                new Vector3(33, 5, 48),
            };

            //Once load has been completed, tell the game to not try and catch up frames - mainly for long loads
            ScreenManager.Game.ResetElapsedTime();
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

                ////////////////////////////
            }
        }

        private BoundingSphere testSphere = new BoundingSphere(Vector3.Zero, 5);

        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Place draw logic here for game play

            ObjectManager.Draw(gameTime);


            //ObjectMetaDrawer.RenderNavigationMap(Color.Violet);
            CollisionManager.Render(Color.Violet, false, true);
            WireShapeDrawer.Draw(gameTime, ObjectManager.Camera.View, ObjectManager.Camera.Projection);

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
