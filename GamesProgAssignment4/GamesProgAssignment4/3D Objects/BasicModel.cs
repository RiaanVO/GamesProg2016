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
        //Override if not needed
        protected bool hasLighting = true;
        protected Matrix world;
        protected Matrix scale, rotate, translate;

        public BasicModel(Vector3 startPos, Game game, Model m, BasicCamera camera) : base(startPos, game)
        {
            model = m;
            this.camera = camera;

            //Sets up world matrices
            scale = rotate = translate = Matrix.Identity;

            //Set up any matrices that need to be set up here
            translate = Matrix.CreateTranslation(startPos);

            world = scale * rotate * translate;
        }

        public override void Update(GameTime gameTime)
        {

        }

        //BasicCamera cam, bool hasLighting

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

                    effect.World = transforms[mesh.ParentBone.Index] * GetWorld(); //* mesh.ParentBone.Transform;
                    

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
