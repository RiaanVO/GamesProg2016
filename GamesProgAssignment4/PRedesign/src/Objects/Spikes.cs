using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class Spikes : BasicModel
    {
        BoxCollider collider;

        public Spikes(Vector3 startPosition, Model model) : base(startPosition, ObjectManager.Camera, model)
        {
            scale = 0.4f;
            scaleMatrix = Matrix.CreateScale(scale);
            collider = new BoxCollider(this, ObjectTag.hazard, new Vector3(15, 3, 15));
            collider.DrawColour = Color.Blue;
            CollisionManager.ForceTreeConstruction();
            hasLighting = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override Matrix GetWorld()
        {
            translationMatrix = Matrix.CreateTranslation(position);
            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
