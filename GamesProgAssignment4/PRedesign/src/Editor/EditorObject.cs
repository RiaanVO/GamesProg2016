﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign {

    class EditorObject {

        #region Fields
        Vector2 position;
        int size;
        int xData, yData;
        Rectangle bounds;
        Texture2D texture;
        LevelEditor.PaintObject type;
        string id;
        #endregion

        #region Properties
        public Vector2 Position {
            get { return position; }
        }

        public int XData {
            get { return xData; }
        }

        public int YData {
            get { return yData; }
        }

        public LevelEditor.PaintObject Type {
            get { return type; }
            set { type = value; }
        }

        public Texture2D Texture {
            set { texture = value; }
        }

        public string ID {
            set { id = value; }
            get { return id; }
        }
        #endregion

        #region Initialisation
        public EditorObject(Vector2 editorPos, int size, int offset, LevelEditor.PaintObject type, int xData, int yData, string id) {
            this.position = new Vector2((editorPos.X + size * xData) + offset * xData, (editorPos.Y + size * yData) + offset * yData);
            this.type = type;
            this.size = size;
            this.xData = xData;
            this.yData = yData;
            this.id = id;
            bounds = new Rectangle((int)position.X, (int)position.Y, size, size);
            setTextureFromType();     
        }

        private void setTextureFromType() {
            switch (type) {
                case LevelEditor.PaintObject.DOOR:
                    texture = LevelEditor.DoorTexture;
                    break;
                case LevelEditor.PaintObject.KEY:
                    texture = LevelEditor.KeyTexture;
                    break;
                case LevelEditor.PaintObject.PLAYER:
                    texture = LevelEditor.PlayerTexture;
                    break;
                case LevelEditor.PaintObject.SPIKE:
                    texture = LevelEditor.SpikeTexture;
                    break;
                case LevelEditor.PaintObject.GUN:
                    texture = LevelEditor.GunTexture;
                    break;
            }
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch) {
            if (texture != null) {
                Color color = Color.White;
                switch (type) {
                    case LevelEditor.PaintObject.DOOR:
                        color = Color.Cyan;
                        break;
                    case LevelEditor.PaintObject.KEY:
                        color = Color.Lime;
                        break;
                    case LevelEditor.PaintObject.PLAYER:
                        color = Color.Magenta;
                        break;
                    case LevelEditor.PaintObject.SPIKE:
                        color = Color.Orange;
                        break;
                    case LevelEditor.PaintObject.GUN:
                        color = Color.MediumPurple;
                        break;
                }
                spriteBatch.Draw(texture, bounds, color);
            }
        }
        #endregion
    }
}
