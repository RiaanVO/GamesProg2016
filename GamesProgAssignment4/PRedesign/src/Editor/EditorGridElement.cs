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
        #endregion

        #region Fields
        Vector2 position;
        int size;
        int xData, yData;
        Rectangle ElementBounds;
        Color DefaultColor = Color.White;
        Color CurrentColor;
        LevelEditor.PaintSelection type = LevelEditor.PaintSelection.EMPTY;


        Texture2D Texture;
        #endregion

        #region Properties
        public LevelEditor.PaintSelection Type {
            get { return type; }
            set { type = value; }
        }

        public Vector2 Position {
            get { return position; }
        }

        public int Size {
            get { return size; }
        }

        public int XData {
            get { return xData; }
        }

        public int YData {
            get { return yData; }
        }
        #endregion

        #region Initialisation

        /// <summary>
        /// Constructor for the grid object in the level editor
        /// </summary>
        /// <param name="texture"> Grid texture</param>
        /// <param name="x"> Editor x coordinate</param>
        /// <param name="y"> Editor y coordinate</param>
        /// <param name="size"> Grid element size</param>
        /// <param name="xData"> Raw array position X</param>
        /// <param name="yData">Raw array position Y</param>
        public EditorGridElement(Texture2D texture, float x, float y, int size, int xData, int yData) {
            position = new Vector2(x, y);
            this.xData = xData;
            this.yData = yData;
            this.size = size;
            Texture = texture;
            CurrentColor = DefaultColor;

            ElementBounds = new Rectangle((int)Position.X, (int)Position.Y, size, size);
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Checks if the mouse position is within the bounds of the element
        /// </summary>
        /// <param name="location">Mouse position</param>
        /// <returns></returns>
        public bool Contains(Point location) {
            return ElementBounds.Contains(location);
        }

        /// <summary>
        /// Checks to see if the mouse position is in the bounds of the element
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool HitTest(Point location) {
            if (Contains(location)) {
                // Prevent grid painting during the enemy creation process
                if (!LevelEditor.PlacingEnemy && !LevelEditor.PlacingNode && !LevelEditor.PaintingObject) {
                    ChangeElement();
                }
                return true;
            }
            return false;     
        }

        /// <summary>
        /// Changes the clicked element to the type of the current selected paint in the editor
        /// </summary>
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
