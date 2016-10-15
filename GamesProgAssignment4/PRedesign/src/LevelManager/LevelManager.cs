using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace PRedesign {

    static class LevelManager {

        #region Fields
        private const string LEVEL_FILEPATH = "Content/levels.json";

        private const int TILE_EMPTY = -1;
        private const int TILE_WALL = 0;
        private const int TILE_PATH = 1;

        private const int TILE_SIZE = 15;
        
        //Game init variables
        private static ScreenManager screenManager;
        private static Player player;

        // Master list of levels
        private static IList<Level> levels = new List<Level>();

        // Current level data
        private static Level currentLevel;
        private static bool isLevelLoaded = false;
        private static BoundingBox levelEnclosure;
        #endregion

        #region Properties

        public static IList<Level> Levels {
            get { return levels; }
        }
		  
        public static int TileSize {
            get { return TILE_SIZE; }
        }

        public static Player Player {
            set { player = value; }
        }

        public static BoundingBox LevelEnclosure {
            get {
                if (levelEnclosure == null)
                    levelEnclosure = new BoundingBox(Vector3.Zero, Vector3.Zero);
                return levelEnclosure;
            }

        }

        /// <summary>
        /// ScreenManager class for use when loading level & objects
        /// </summary>
        public static ScreenManager ScreenManager {
            get { return screenManager; }
            set { screenManager = value; }
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// Attempts to load all the levels from the external level file
        /// </summary>
        public static void LoadLevelsFromFile() {

            // Loads all of the level data into a list, or a single level if only one object exists
            if (File.Exists(LEVEL_FILEPATH)) {
                if (File.ReadLines(LEVEL_FILEPATH).Count() > 1) {
                    levels = JsonConvert.DeserializeObject<List<Level>>(File.ReadAllText(LEVEL_FILEPATH));
                } else if (File.ReadLines(LEVEL_FILEPATH).Count() == 0) {
                    return;
                } else { 
                    string test = File.ReadAllText(LEVEL_FILEPATH);
                    levels.Add(JsonConvert.DeserializeObject<Level>(File.ReadAllText(LEVEL_FILEPATH)));
                }
            }
        }

        /// <summary>
        /// Attempts to write all the data models for levels into the level external level file
        /// </summary>
        public static void WriteLevelsToFile() {

            if (File.Exists(LEVEL_FILEPATH)) {
                File.WriteAllText(LEVEL_FILEPATH, string.Empty);
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(LEVEL_FILEPATH)) {
                using (JsonTextWriter writer = new JsonTextWriter(sw)) {
                    if (levels.Count > 1) {
                        writer.WriteStartArray();
                        foreach (Level level in levels) {
                            if (levels.IndexOf(level) != 0) {
                                writer.WriteRaw("\n");
                            }
                            serializer.Serialize(writer, level);
                        }
                        writer.WriteEndArray();
                    } else {
                        serializer.Serialize(writer, levels[0]);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the main level data and creatses the navigation map using the data
        /// </summary>
        /// <param name="id"></param>
        public static void LoadLevel(int id) {
            if (id <= levels.Count && id > 0) {
                currentLevel = levels[id - 1];
            }
            if (!isLevelLoaded) {
                //Clear the managers
                //ObjectManager.clearAll();
                //AudioManager.clearAll();
                NavigationMap.CreateNavigationMap(currentLevel.Data.GetLength(1) * TILE_SIZE, currentLevel.Data.GetLength(0) * TILE_SIZE, TILE_SIZE);
                
                //This is where all the 'x = new x' stuff has been moved from the GameplayScreen
                //in order to get level reloading working correctly.
                LoadGameObjects();

                //Construct the objects for the level
                LoadLevelData();
            }
        }


        public static void ReloadLevel()
        {
            UnloadLevel();
            LoadLevel(currentLevel.Id);
        }

        /// <summary>
        /// Unloads the current level and attempts to load the next level
        /// </summary>
        public static void NextLevel() {
            if(currentLevel.Id + 1 <= levels.Count) {
                currentLevel = levels[levels.IndexOf(currentLevel) + 1];
                UnloadLevel();
                LoadLevel(currentLevel.Id);
            } else {
                // Completed Game screen?
            }
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Method to replace 'GamePlayScreen' initialisation of objects
        /// Object loading can be moved out of here into LoadLevelData() as objects are added to levels.json.
        /// Player must be loaded first though
        /// </summary>
        private static void LoadGameObjects()
        {
            //Create camera and set up object manager
            BasicCamera camera = new BasicCamera(new Vector3(0, 10, 0), new Vector3(-1, 10, 0), Vector3.Up, ScreenManager.GraphicsDevice.Viewport.AspectRatio);
            camera.FarClip = 3000;
            ObjectManager.Camera = camera;
            ObjectManager.GraphicsDevice = ScreenManager.GraphicsDevice;
            ObjectManager.Game = ScreenManager.Game;

            Player player = new Player(new Vector3(20, 3.5f, 20));
            Player = player;

            Skybox skybox = new Skybox(Vector3.Zero, ContentStore.loadedModels["skybox"]);
            skybox.Player = player;

            //This should be replaced with actual loading later
            ObjectManager.addGameObject(new TetraKey(new Vector3(-5f, 5f, -5f), ContentStore.loadedModels["tetraKey"], camera, player));
            ObjectManager.addGameObject(new Spikes(new Vector3(0f, 0f, 15f), ContentStore.loadedModels["spikes"]));
        }

        /// <summary>
        /// Parses the data from the selected level and populates the level with objects
        /// </summary>
        private static void LoadLevelData() {
            int currentLevelWidth = currentLevel.Data.GetUpperBound(0) * TILE_SIZE;
            int currentLevelHeight = currentLevel.Data.GetUpperBound(0) * TILE_SIZE;
            int largestDimension = (currentLevelWidth > currentLevelHeight) ? currentLevelWidth : currentLevelHeight;
            int heightScale = 3;
            levelEnclosure = new BoundingBox(new Vector3(-TILE_SIZE, -TILE_SIZE * heightScale, -TILE_SIZE), new Vector3(largestDimension + TILE_SIZE, TILE_SIZE * heightScale, largestDimension + TILE_SIZE));

            for (int i = 0; i <= currentLevel.Data.GetUpperBound(0); i++) {
                for (int j = 0; j <= currentLevel.Data.GetUpperBound(1); j++) {
                    switch (currentLevel.Data[j, i]) {
                        case TILE_EMPTY:
                            break;
                        case TILE_WALL:
                            //Adam - can replace the "wall" string here with a variable to allow for more detailed configuration. 
                            ObjectManager.addGameObject(new Wall(new Vector3((TILE_SIZE * j), 0, (TILE_SIZE * i)), ContentStore.loadedTextures["wall"], TILE_SIZE));
                            break;
                        case TILE_PATH:
                            ObjectManager.addGameObject(new GroundPrimitive(new Vector3((float)(TILE_SIZE * j) / 2f, 0, (float)(TILE_SIZE * i) / 2f), ContentStore.loadedTextures["ground"], TILE_SIZE, 1));
                            ObjectManager.addGameObject(new CeilingPrimitive(new Vector3((float)(TILE_SIZE * j) / 2, TILE_SIZE / 2, (float)(TILE_SIZE * i) / 2), ContentStore.loadedTextures["ceiling"], TILE_SIZE, 1));

                            break;
                    }
                }
            }

            foreach (Enemy enemy in currentLevel.Enemies) {
                NPCEnemy newEnemy = new NPCEnemy(new Vector3(enemy.X * TileSize + (TileSize / 2), 6, enemy.Y * TileSize + (TileSize / 2)), ContentStore.loadedModels["tetraEnemy"], player);
                newEnemy.Scale = 0.08f;
                newEnemy.HasLighting = true;
                Vector3[] patrolPoints = new Vector3[enemy.PatrolPoints.Count];
                for (int i = 0; i <= enemy.PatrolPoints.Count - 1; i++) {
                    patrolPoints[i] = new Vector3(enemy.PatrolPoints[i].X * TileSize, 5, enemy.PatrolPoints[i].Y * TileSize);
                }
                newEnemy.PatrolPoints = patrolPoints;
                ObjectManager.addGameObject(newEnemy);                
            }

            CollisionManager.ForceTreeConstruction();

            isLevelLoaded = true;
        }

        /// <summary>
        /// Unloads the current level - cleaing all lists holding objects/references
        /// </summary>
        private static void UnloadLevel() {
            ObjectManager.clearAll();
            AudioManager.clearAll();
            CollisionManager.clearAll();
            isLevelLoaded = false;
        }
        #endregion
    }
}
