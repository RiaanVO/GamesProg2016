using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A_MAZE_ING
{
    class GroundModel : BasicModel
    {
        public GroundModel(Game game, ObjectManager objectManager,Vector3 startPos, Model m, BasicCamera camera) : base(game, objectManager, startPos, m, camera)
        {
            this.position = startPos;
            hasLighting = false;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
