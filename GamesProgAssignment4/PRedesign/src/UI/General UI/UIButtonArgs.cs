using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class UIButtonArgs : System.EventArgs
    {
        #region Properties
        public Vector2 Location { get; private set; }
        public string ID { get; private set; }
        #endregion

        #region Initialization
        public UIButtonArgs(string id, Vector2 location) {
            ID = id;
            Location = location;
        }
        #endregion
    }
}
