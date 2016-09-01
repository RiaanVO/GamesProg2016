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
        protected Model model { get; set; }
        protected BasicCamera camera;
        protected Matrix world;
        protected Matrix scale, rotate, translate;
        //Override if not needed
        protected bool hasLighting = true;

        public BasicModel(Game game, Vector3 startPos, Model m, BasicCamera camera) : base(startPos, game)
        {
            model = m;
            this.camera = camera;

            //Sets up world matrices
            scale = rotate = translate = Matrix.Identity;

            //Set up any matrices that need to be set up here
            translate = Matrix.CreateTranslation(startPos);

            world = scale * rotate * translate;
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

        public virtual Matrix GetWorld()
        {
            return world;
        }

        public virtual void SetPosition(Vector3 position)
        {
            this.position = position;
        }
    }
}
