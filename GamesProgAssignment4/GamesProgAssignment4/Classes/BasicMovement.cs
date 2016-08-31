using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GamesProgAssignment4.Classes
{
    class BasicMovement
    {
        //
        Vector3 position;

        public BasicMovement(ref Vector3 pos)
        {
            position = pos;

            //Needs testing whether this will have access or not
            position = new Vector3(5,0,5);
        }
    }
}
