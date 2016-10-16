using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    class SoundGun : BasicModel
    {
        public bool isVisible;

        public SoundGun(Vector3 startPosition, Model model) : base(startPosition, model)
        {
            scale = 0.08f;
            scaleMatrix = Matrix.CreateScale(scale);
            hasLighting = true;

            isVisible = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Enables the gun to be seen etc
        /// </summary>
        public void Enable()
        {
            isVisible = true;
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


        //Fields
        //
        /*
        private Vector3 offset = new Vector3(-2f,0f,-2f);
        private Matrix tiltMatrix = Matrix.CreateRotationY(0.2f) * Matrix.CreateRotationX(0.2f);

        public SoundGun (Vector3 startPosition, Model model) : base(startPosition, model)
        {
            //isVisible = false;
            //isVisible = true;

            scale = 0.08f;
            scaleMatrix = Matrix.CreateScale(scale);

            //hasLighting = true;
        }


        /// <summary>
        /// Updates the gun with the correct position and orientation so it can display correctly.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="orientation"></param>
        public void updateMatrices(Vector3 pos, float orientation)
        {
            //Position = pos + offset;
            Orientation = orientation;
        }


        
        public override Matrix GetWorld()
        {
            //Rotate around the Y axis with the player
            //Possibly also tilt up in a good direction sort of thing
            //Then translate out by offset
            //Then rotate around the player, should be same angle as previous rotation
            //return scaleMatrix * rotationMatrix * translationMatrix;
            //rotationMatrix = Matrix.CreateRotationY(orientation);
            //tiltMatrix = Matrix.CreateRotation(x,y,z);
            //translationMatrix = Matrix.CreateTranslation(position + offset);

            return scaleMatrix * rotationMatrix * tiltMatrix * translationMatrix * rotationMatrix;
        }
    

        public override void Draw(GameTime gameTime)
        {
            //if (isVisible)
                base.Draw(gameTime);
        }
        
        */
    }
}
