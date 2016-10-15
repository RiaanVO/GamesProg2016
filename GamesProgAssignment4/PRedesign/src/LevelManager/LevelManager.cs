using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        private static Texture2D groundTexture;
        private static Texture2D wallTexture;
        private static Texture2D ceilingTexture;

        private static Model enemyModel;
        private static Player player;

        // Master list of levels
        private static IList<Level> levels = new List<Level>();

        // Current level data
        private static Level currentLevel;
        private static bool isLevelLoaded = false;
        private static BoundingBox levelEnclosure;
        #endregion

        #region Properties
        public static Texture2D GroundTexture {
            get { return groundTexture; }
            set { groundTexture = value; }
        }

        public static Texture2D WallTexture {
            get { return wallTexture; }
            set { wallTexture = value; }
        }
        
        public static Texture2D CeilingTexture {
            get { return ceilingTexture; }
            set { ceilingTexture = value; }
        }

        public static IList<Level> Levels {
            get { return levels; }
        }
		  
        public static int TileSize {
            get { return TILE_SIZE; }
        }


        public static Model EnemyModel {
            set { enemyModel = value; }
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

                //Temporary - call GamePlayScreen to reload
                

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
                            ObjectManager.addGameObject(new Wall(new Vector3((TILE_SIZE * j), 0, (TILE_SIZE * i)), wallTexture, TILE_SIZE));
                            break;
                        case TILE_PATH:
                            ObjectManager.addGameObject(new GroundPrimitive(new Vector3((float)(TILE_SIZE * j) / 2f, 0, (float)(TILE_SIZE * i) / 2f), groundTexture, TILE_SIZE, 1));
                            ObjectManager.addGameObject(new CeilingPrimitive(new Vector3((float)(TILE_SIZE * j) / 2, TILE_SIZE / 2, (float)(TILE_SIZE * i) / 2), ceilingTexture, TILE_SIZE, 1));

                            break;
                    }
                }
            }

            foreach (Enemy enemy in currentLevel.Enemies) {
                NPCEnemy newEnemy = new NPCEnemy(new Vector3(enemy.X * TileSize + (TileSize / 2), 6, enemy.Y * TileSize + (TileSize / 2)), enemyModel, player);
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
