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
        
        public int Id { get; set; }
        public int[,] Data { get; set; }
        public List<JSONGameObject> Objects { get; set; }
        public List<JSONEnemy> Enemies { get; set; }
    }

}
