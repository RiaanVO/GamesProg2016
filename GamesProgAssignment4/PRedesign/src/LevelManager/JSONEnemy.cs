using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign {


    /// <summary>
    /// Data model for the enemy object in preparation for JSON serialisation
    /// </summary>
    class JSONEnemy {

        // PatrolPoint class for storing the enemy node positions using raw array coordinates
        public class PatrolPoint {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int ID { get; set; }
        public string Behavior { get; set; }
        public List<PatrolPoint> PatrolPoints { get; set; }
    }
}
