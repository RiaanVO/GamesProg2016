using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign {
    class EditorEnemy {

        public class Node {

            #region Fields
            Vector2 nodePosition;
            Rectangle bounds;
            Texture2D nodeTexture;
            int xData, yData;
            #endregion

            #region Properties
            public Vector2 Position {
                get { return nodePosition; }
            }

            public int XData {
                get { return xData; }
            }

            public int YData {
                get { return yData; }
            }
            #endregion

            #region Initialisation
            public Node(Vector2 position, int size, Texture2D texture, int xData, int yData) {
                nodePosition = position;
                bounds = new Rectangle((int)nodePosition.X, (int)nodePosition.Y, size, size);
                nodeTexture = texture;
                this.xData = xData;
                this.yData = yData;
            }
            #endregion

            #region Draw Method
            public void Draw(SpriteBatch spriteBatch) {
                spriteBatch.Draw(nodeTexture, bounds, Color.Yellow);
            }
            #endregion

        }

        #region Fields
        Vector2 enemyPosition;
        int size;
        Texture2D enemyTexture, nodeTexture;
        Rectangle bounds;
        IList<Node> nodes = new List<Node>();
        int xData, yData;
        #endregion

        #region Properties
        public Vector2 Position {
            get { return enemyPosition; }
        }

        public IList<Node> Nodes {
            get { return nodes; }
        }

        public int XData {
            get { return xData; }
        }

        public int YData {
            get { return yData; }
        }
        #endregion

        #region Initialisation

        /// <summary>
        /// Constructor for the EditorEnemy element used in the editor grid
        /// </summary>
        /// <param name="editorPos"> Position of the grid</param>
        /// <param name="size"> Grid element size</param>
        /// <param name="offset"> Grid element offset (grid spacing)</param>
        /// <param name="enemyTexture"> Texture for the enemy object</param>
        /// <param name="nodeTexture"> Texture for the node object</param>
        /// <param name="xData"> raw array data for x coordinate</param>
        /// <param name="yData">raw array data for y coordinate</param>
        public EditorEnemy(Vector2 editorPos, int size, int offset, Texture2D enemyTexture, Texture2D nodeTexture, int xData, int yData) {
            enemyPosition = new Vector2((editorPos.X + size * xData) + offset * xData, (editorPos.Y + size * yData) + offset * yData);
            this.enemyTexture = enemyTexture;
            this.nodeTexture = nodeTexture;
            this.size = size;
            this.xData = xData;
            this.yData = yData;
            bounds = new Rectangle((int)enemyPosition.X, (int)enemyPosition.Y, size, size);
        }

        /// <summary>
        /// Constructor for EditorEnemy when loading from a JSON deserialised file
        /// </summary>
        /// <param name="editorPos"> Position of the grid</param>
        /// <param name="size"> Grid element size</param>
        /// <param name="offset"> Grid element offset (grid spacing)</param>
        /// <param name="enemyTexture"> Texture for the enemy object</param>
        /// <param name="nodeTexture"> Texture for the node object</param>
        /// <param name="xData"> raw array data for x coordinate</param>
        /// <param name="yData">raw array data for y coordinate</param>
        /// <param name="nodes"> List of nodes beloning to the enemy object</param>
        public EditorEnemy(Vector2 editorPos, int size, int offset, Texture2D enemyTexture, Texture2D nodeTexture, int xData, int yData, IList<Enemy.PatrolPoint> nodes) {
            enemyPosition = new Vector2((editorPos.X + size * xData) + offset * xData, (editorPos.Y + size * yData) + offset * yData);
            this.enemyTexture = enemyTexture;
            this.nodeTexture = nodeTexture;
            this.size = size;
            this.xData = xData;
            this.yData = yData;
            bounds = new Rectangle((int)enemyPosition.X, (int)enemyPosition.Y, size, size);

            // Load a new node for each patrol point found in the JSON file
            foreach (Enemy.PatrolPoint node in nodes) {
                addNode(new Vector2((editorPos.X + size * node.X) + offset * node.X, (editorPos.Y + size * node.Y) + offset * node.Y), node.X, node.Y);
            }
        }
        #endregion

        #region Helper Methods
        public void addNode(Vector2 position, int xData, int yData) {
            nodes.Add(new Node(position, size, nodeTexture, xData, yData));
        }
        #endregion

        #region Draw Method
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(enemyTexture, bounds, Color.Red);

            if (nodes.Count > 0) {
                foreach (Node node in nodes) {
                    node.Draw(spriteBatch);
                }
            }
        }
        #endregion

    }
}
