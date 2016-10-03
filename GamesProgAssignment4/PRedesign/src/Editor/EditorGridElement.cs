using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign {

   class EditorGridElement {

        #region Enums
        public enum ElementType {
            EMPTY,
            WALL,
            PATH
        }

        public enum ElementObstacle {
            NONE,
            SPIKE
        }
        #endregion

        #region Fields
        Vector2 Position;
        int Size;
        Rectangle ElementBounds;
        Color DefaultColor = Color.White;
        Color CurrentColor;
        LevelEditor.PaintSelection type = LevelEditor.PaintSelection.EMPTY;
        ElementObstacle obstacle = ElementObstacle.NONE;

        Texture2D Texture;
        #endregion

        #region Properties
        public LevelEditor.PaintSelection Type {
            get { return type; }
            set { type = value; }
        }

        public ElementObstacle Obstacle {
            get { return obstacle; }
            set { obstacle = value; }
        }
        #endregion

        #region Initialisation
        public EditorGridElement(Texture2D texture, float x, float y, int size) {
            Position = new Vector2(x, y);
            Size = size;
            Texture = texture;
            CurrentColor = DefaultColor;

            ElementBounds = new Rectangle((int)Position.X, (int)Position.Y, size, size);
        }
        #endregion

        #region Helper Methods
        public bool Contains(Point location) {
            return ElementBounds.Contains(location);
        }

        public bool HitTest(Point location) {
            if (Contains(location)) {
                Vector2 test = Position;
                ChangeElement();
                return true;
            }
            return false;     
        }

        public void ChangeElement() {
            if (!LevelEditor.SelectedPaint.Equals(type)) {
                switch (LevelEditor.SelectedPaint) {
                    case LevelEditor.PaintSelection.EMPTY:
                        Texture = LevelEditor.DefaultTexture;
                        break;
                    case LevelEditor.PaintSelection.WALL:
                        Texture = LevelEditor.WallTexture;
                        break;
                    case LevelEditor.PaintSelection.PATH:
                        Texture = LevelEditor.PathTexture;
                        break;
                }
                type = LevelEditor.SelectedPaint;
            }
        }
        #endregion

        #region Update and Draw
        public void Update(GameTime gametime) {

        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, ElementBounds, CurrentColor);
        }
        #endregion

    }
}
