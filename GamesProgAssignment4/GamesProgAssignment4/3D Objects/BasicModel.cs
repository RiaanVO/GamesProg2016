using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesProgAssignment4
{
    class BasicModel
    {
        public Model model { get; protected set; }
        protected Matrix world = Matrix.Identity;
        public Vector3 position;

        public BasicModel(Model m, Vector3 startPos)
        {
            model = m;
            position = startPos;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(Camera cam, bool basicLighting)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = cam.viewMatrix;
                    effect.Projection = cam.projectionMatrix;
                    if (basicLighting)
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
