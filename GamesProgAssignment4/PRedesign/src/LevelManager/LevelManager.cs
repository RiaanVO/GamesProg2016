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

        private const int TILE_SIZE = 20;

        private static Texture2D groundTexture;
        private static Texture2D wallTexture;
        private static Texture2D ceilingTexture;

        // Master list of levels
        private static IList<Level> levels = new List<Level>();

        // Current level data
        private static Level currentLevel;
        private static bool isLevelLoaded = false;
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

        #endregion

        #region Public Methods
        public static void LoadLevelsFromFile() {

            // Loads all of the level data into a list, or a single level if only one object exists
            if (File.Exists(LEVEL_FILEPATH)) {
                if (File.ReadLines(LEVEL_FILEPATH).Count() > 1) {
                    levels = JsonConvert.DeserializeObject<List<Level>>(File.ReadAllText(LEVEL_FILEPATH));
                } else {
                    string test = File.ReadAllText(LEVEL_FILEPATH);
                    levels.Add(JsonConvert.DeserializeObject<Level>(File.ReadAllText(LEVEL_FILEPATH)));
                }
            }
        }

        public static void WriteLevelsToFile() {

            if (File.Exists(LEVEL_FILEPATH)) {
                File.WriteAllText(LEVEL_FILEPATH, string.Empty);
            }

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(LEVEL_FILEPATH)) {
                using (JsonTextWriter writer = new JsonTextWriter(sw)) {
                    writer.WriteStartArray();
                    if (levels.Count > 1) {
                        foreach (Level level in levels) {
                            if (levels.IndexOf(level) != 0) {
                                writer.WriteRaw("\n");
                            }
                            serializer.Serialize(writer, level);
                        }
                    } else {
                        serializer.Serialize(writer, levels[0]);
                    }
                    writer.WriteEndArray();
                }
            }
        }

        public static void LoadLevel(int id) {
            if (id <= levels.Count && id > 0) {
                currentLevel = levels[id - 1];
            }
            if (!isLevelLoaded) {
                LoadLevelData();
            }
        }

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
        private static void LoadLevelData() {
            for (int i = 0; i <= currentLevel.Data.GetUpperBound(0); i++) {
                for (int j = 0; j <= currentLevel.Data.GetUpperBound(1); j++) {
                    switch (currentLevel.Data[i, j]) {
                        case TILE_EMPTY:
                            break;
                        case TILE_WALL:
                            ObjectManager.addGameObject(new CubePrimitive(new Vector3((TILE_SIZE * j) - TILE_SIZE / 2, 0, (TILE_SIZE * i) - TILE_SIZE / 2), wallTexture, TILE_SIZE));
                            break;
                        case TILE_PATH:
                            ObjectManager.addGameObject(new GroundPrimitive(new Vector3((TILE_SIZE * j) / 2, 0, (TILE_SIZE * i) / 2), groundTexture, TILE_SIZE, 1));
                            ObjectManager.addGameObject(new CeilingPrimitive(new Vector3((TILE_SIZE * j) / 2, TILE_SIZE/2, (TILE_SIZE * i) / 2), ceilingTexture, TILE_SIZE, 1));
                            break;
                    }
                }
            }
            isLevelLoaded = true;
        }

        private static void UnloadLevel() {
            ObjectManager.clearAll();
            isLevelLoaded = false;
        }
        #endregion
    }
}
