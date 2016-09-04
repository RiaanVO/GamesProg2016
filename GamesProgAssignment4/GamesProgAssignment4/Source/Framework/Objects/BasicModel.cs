using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class BasicModel : GameObject
    {
        protected BasicCamera camera;
        public Model model { get; protected set; }
        protected Matrix world, scale, rotation, translation;
        //Override if not needed
        protected bool hasLighting = true;

        public BasicModel(Game game, ObjectManager objectManager, Vector3 startPosition, Model model, BasicCamera camera) : base(game, objectManager, startPosition)
        {
            this.camera = camera;
            this.model = model;
            scale = Matrix.CreateScale(1);
            rotation = Matrix.Identity;
            translation = Matrix.CreateTranslation(position);
            world = scale * rotation * translation; ;
        }

        /// <summary>
        /// Draws all the meshes in the model
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                    if (hasLighting)
                        effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * GetWorld();
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// Returns the world matrix for drawing. Overload if need to modify world at draw time.
        /// </summary>
        /// <returns></returns>
        public virtual Matrix GetWorld()
        {
            return world;
        }

        /// <summary>
        /// Sets the position of the object and adjusts the translation to the new position
        /// </summary>
        /// <param name="position"></param>
        public virtual void SetPosition(Vector3 position)
        {
            this.position = position;
            translation = Matrix.CreateTranslation(position);
        }
    }
}
