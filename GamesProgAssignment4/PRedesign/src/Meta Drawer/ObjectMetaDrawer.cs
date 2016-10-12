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

        public static void RenderNavigationMap(Color color)
        {
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

            for (int i = 0; i < verts.Count(); i++)
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


        /*
        static VertexPositionColor[] cubeVerts = new VertexPositionColor[8];
        static short[] indices = new short[]
        {
        0, 1,
        1, 2,
        2, 3,
        3, 0,
        0, 4,
        1, 5,
        2, 6,
        3, 7,
        4, 5,
        5, 6,
        6, 7,
        7, 4,
        };

        public static void RenderBoundingBox(BoundingBox box, Color colour)
        {
            if (effect == null)
            {
                effect = new BasicEffect(ObjectManager.GraphicsDevice);
                if (effect == null)
                    return;
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
            }

            Vector3[] corners = box.GetCorners();
            for (int i = 0; i < 8; i++)
            {
                verts[i].Position = corners[i];
                verts[i].Color = colour;
            }

            effect.View = ObjectManager.Camera.View;
            effect.Projection = ObjectManager.Camera.Projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                ObjectManager.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, verts, 0, 8, indices, 0, indices.Length / 2);
            }
        }


        // This holds the vertices for our unit sphere that we will use when drawing bounding spheres
        private const int sphereResolution = 30;
        private const int sphereLineCount = (sphereResolution + 1) * 3;
        private static Vector3[] unitSphere;

        /// <summary>
        /// 
        /// https://github.com/thestonefox/XNA-Alien-Lander/blob/master/AlienGrab/AlienGrab/Helper/DebugShapeRenderer.cs
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="color"></param>
        public static void RenderBoundingSphere(BoundingSphere sphere, Color color)
        {
            if (effect == null)
            {
                effect = new BasicEffect(ObjectManager.GraphicsDevice);
                if (effect == null)
                    return;
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
            }

            if (unitSphere == null)
                InitializeSphere();

            VertexPositionColor[] vertices = new VertexPositionColor[sphereLineCount];
            for (int i = 0; i < unitSphere.Length; i++)
            {
                // Compute the vertex position by transforming the point by the radius and center of the sphere
                Vector3 vertPos = unitSphere[i] * sphere.Radius + sphere.Center;
                vertices[i] = new VertexPositionColor(vertPos, color);
            }

            effect.View = ObjectManager.Camera.View;
            effect.Projection = ObjectManager.Camera.Projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                ObjectManager.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Count(), indices, 0, indices.Count() / 2 - 1);
            }

        }

        private static void InitializeSphere()
        {
            // We need two vertices per line, so we can allocate our vertices
            unitSphere = new Vector3[sphereLineCount * 2];

            // Compute our step around each circle
            float step = MathHelper.TwoPi / sphereResolution;

            // Used to track the index into our vertex array
            int index = 0;

            // Create the loop on the XY plane first
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                unitSphere[index++] = new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f);
                unitSphere[index++] = new Vector3((float)Math.Cos(a + step), (float)Math.Sin(a + step), 0f);
            }

            // Next on the XZ plane
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                unitSphere[index++] = new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a));
                unitSphere[index++] = new Vector3((float)Math.Cos(a + step), 0f, (float)Math.Sin(a + step));
            }

            // Finally on the YZ plane
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                unitSphere[index++] = new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a));
                unitSphere[index++] = new Vector3(0f, (float)Math.Cos(a + step), (float)Math.Sin(a + step));
            }
        }
        */


    }
}
