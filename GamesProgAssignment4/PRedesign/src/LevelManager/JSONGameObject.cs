using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRedesign {

    class JSONGameObject {

        public int X { get; set; }
        public int Y { get; set; }
        public string ID { get; set; }
        public LevelEditor.PaintObject Type { get; set; }
    }
}
