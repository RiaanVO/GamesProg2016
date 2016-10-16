using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRedesign {

  /// <summary>
  /// Data model for level JSON serialisation
  /// </summary>
    class JSONLevel {
        
        // Level data
        public int Id { get; set; }
        public int[,] Data { get; set; }

        // Unique identifiers
        public JSONGameObject Player { get; set; }
        public JSONGameObject Door { get; set; }
        public JSONGameObject Key { get; set; }
        public JSONGameObject SoundGun { get; set; }

        // All other objects
        public List<JSONGameObject> Objects { get; set; }

        // List of enemies
        public List<JSONEnemy> Enemies { get; set; }
    }

}
