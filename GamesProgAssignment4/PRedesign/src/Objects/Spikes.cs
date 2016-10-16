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

        public Spikes(Vector3 startPosition, Model model) : base(startPosition, model)
        {
            scale = 0.4f;
            scaleMatrix = Matrix.CreateScale(scale, 0.3f, scale);
            collider = new BoxCollider(this, ObjectTag.hazard, new Vector3(12, 7f, 12));
            collider.PositionOffset = new Vector3(1.5f, 0f, 1.5f);
            collider.DrawColour = Color.Blue;
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
