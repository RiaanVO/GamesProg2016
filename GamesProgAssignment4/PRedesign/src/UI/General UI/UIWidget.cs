using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class UIWidget
    {
        #region Properties

        public string ID { get; private set; }
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }

        #endregion

        #region Initialization
        public UIWidget(string id, Vector2 position) {
            ID = id;
            Position = position;
            Visible = false;
        }
        #endregion

        #region Update and Draw
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        #endregion

    }
}
