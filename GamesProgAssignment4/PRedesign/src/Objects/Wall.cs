using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class Wall :  CubePrimitive
    {
        public Wall(Vector3 startPosition, Texture2D texture, float size) : base(startPosition, texture, size) {
            NavigationMap.setSearchNodeObstructed(centeredPosition, true);
        }
    }
}
