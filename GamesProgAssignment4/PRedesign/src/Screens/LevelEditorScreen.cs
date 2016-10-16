﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PRedesign {

    class LevelEditorScreen : GameScreen {

        #region Fields
        ContentManager content;
        Texture2D backgroundTexture, editorEnemyTexture, editorNodeTexture, editorSpikeTexture, editorKeyTexture, editorDoorTexture, editorPlayerTexture, editorGunTexture;
        SpriteFont editorFont;
        #endregion

        #region Initialisation
        public override void LoadContent() {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            

            backgroundTexture = content.Load<Texture2D>(@"Textures/MenuBackground");
            editorFont = content.Load<SpriteFont>(@"Fonts/GameFont");
            editorEnemyTexture = content.Load<Texture2D>(@"Textures/editor_enemy");
            editorNodeTexture = content.Load<Texture2D>(@"Textures/editor_node");
            editorSpikeTexture = content.Load<Texture2D>(@"Textures/editor_spike");
            editorKeyTexture = content.Load<Texture2D>(@"Textures/editor_key");
            editorDoorTexture = content.Load<Texture2D>(@"Textures/editor_door");
            editorPlayerTexture = content.Load<Texture2D>(@"Textures/editor_player");
            editorGunTexture = content.Load<Texture2D>(@"Textures/editor_gun");




            InitialiseEditor();
        }

        private void InitialiseEditor() {
            LevelEditor.GraphicsDevice = ScreenManager.GraphicsDevice;
            LevelEditor.EditorFont = editorFont;
            LevelEditor.EnemyTexture = editorEnemyTexture;
            LevelEditor.NodeTexture = editorNodeTexture;
            LevelEditor.SpikeTexture = editorSpikeTexture;
            LevelEditor.KeyTexture = editorKeyTexture;
            LevelEditor.DoorTexture = editorDoorTexture;
            LevelEditor.PlayerTexture = editorPlayerTexture;
            LevelEditor.GunTexture = editorGunTexture;
            LevelEditor.GameScreen = this;
            LevelEditor.LoadEditor();

        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);        

            LevelEditor.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullScreeen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            Color drawColour = new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, fullScreeen, drawColour);           

            LevelEditor.Draw(spriteBatch);

            spriteBatch.End();
        }
        #endregion
    }
}
