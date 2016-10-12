using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    static class ObjectMetaDrawer
    {
        static VertexPositionColor[] verts;
        static BasicEffect effect;

        /// <summary>
        /// Renders the line for debugging purposes.
        /// </summary>
        public static void RenderPath(Vector3[] points, Color color)
        {
            if (effect == null)
            {
                effect = new BasicEffect(ObjectManager.GraphicsDevice);
                if (effect == null)
                    return;
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
            }
            verts = new VertexPositionColor[points.Count()];
            for (int i = 0; i < verts.Count(); i++)
            {
                verts[i].Position = points[i];
                verts[i].Color = color;
            }
            int[] indices = new int[verts.Count() * 2];
            for (int i = 0; i < verts.Count(); i++)
            {
                indices[i * 2] = i;
                indices[i * 2 + 1] = i + 1;
            }

            effect.View = ObjectManager.Camera.View;
            effect.Projection = ObjectManager.Camera.Projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                ObjectManager.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, verts, 0, verts.Count(), indices, 0, indices.Count() / 2 - 1);
            }
        }

        public static void RenderNavigationMap(Color color) {
             if (effect == null)
            {
                effect = new BasicEffect(ObjectManager.GraphicsDevice);
                if (effect == null)
                    return;
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
            }
            verts = NavigationMap.getNodesVerts().ToArray();

            int[] indices = new int[verts.Count() * 2];

            for (int i = 0; i < verts.Count(); i ++)
            {
                indices[i] = i;
            }

            effect.View = ObjectManager.Camera.View;
            effect.Projection = ObjectManager.Camera.Projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                ObjectManager.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, verts, 0, verts.Count(), indices, 0, indices.Count() / 2 - 1);
            }
        }
    }
}
