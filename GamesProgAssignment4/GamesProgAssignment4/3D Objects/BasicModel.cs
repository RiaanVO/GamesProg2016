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
        Model model { get; set; }
        BasicCamera camera;
        //Override if not needed
        bool hasLighting = true;
        protected Matrix world = Matrix.Identity;

        public BasicModel(Vector3 startPos, Game game, Model m, BasicCamera camera) : base(startPos, game)
        {
            model = m;
            this.camera = camera;
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
