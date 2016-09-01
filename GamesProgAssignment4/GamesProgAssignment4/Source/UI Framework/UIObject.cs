using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    abstract class UIObject
    {
        protected Game game;
        protected ObjectManager manager;
        public Vector2 position { get; protected set; }

        //Additional parameters
        protected Color color;
        protected Vector2? scale;
        protected float rotation;
        protected Vector2? origin;
        protected SpriteEffects effect;
        protected float layerDepth;

        
        /// <summary>
        /// Basic-ass UIObject constructor
        /// </summary>
        /// <param name="startPos">Position starting from top left of object.</param>
        /// <param name="game"></param>
        /// <param name="scale"></param>
        public UIObject(Vector2 startPos, Game game) : this (startPos, game, Color.White, null, 0f, null, SpriteEffects.None, 0)
        {
        }

        /// <summary>
        /// Semi-full UIObject constructor
        /// </summary>
        /// <param name="startPos">Position starting from top left of object.</param>
        /// <param name="game"></param>
        /// <param name="scale"></param>
        public UIObject(Vector2 startPos, Game game, Color color) : this(startPos, game, color, null, 0f, null, SpriteEffects.None, 0)
        {
            //NOTE: NOT IMPLEMENTED YET
        }

        /// <summary>
        /// Fine-ass UIObject constructor
        /// </summary>
        /// <param name="startPos">Position starting from top left of object.</param>
        /// <param name="game"></param>
        /// <param name="scale"></param>
        public UIObject(Vector2 startPos, Game game, Color color, Vector2? scale, float rotation, Vector2? origin, SpriteEffects effect, float layerDepth)
        {
            this.game = game;
            position = startPos;
            this.color = color;
            this.scale = scale;
            this.rotation = rotation;
            this.origin = origin;
            this.effect = effect;
            this.layerDepth = layerDepth;

            //NOTE: NOT IMPLEMENTED YET
        }

        /// <summary>
        /// For setting up the game object, Can be used to re initilise game object
        /// </summary>
        public virtual void Initialize(){}

        /// <summary>
        /// The update method of the game object, can be overwritten
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime){}

        /// <summary>
        /// The draw method of the game object, can be overwritten
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch){}
    }
}
