using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PRedesign
{
    //Adapted from:
    //http://xnatd.blogspot.com.au/2011/12/pathfinding-tutorial-part-3.html
    class SearchNode {
        public Vector2 Position;
        public bool Obstructed;
        public SearchNode[] Neighbours;
        public SearchNode Parent;
        public bool InOpenList;
        public bool InClosedList;
        public float DistanceToGoal;
        public float DistanceTraveled;
        public BoundingBox nodeRegion;
    }

    static class NavigationMap
    {
        #region Fields
        private static SearchNode[,] searchNodes;
        private static int levelWidth;
        private static int levelHeight;
        private static int nodeWidth;
        private static float halfNodeWidth;
        private static int numNodesWidth;
        private static int numNodesHeight;
        private static List<SearchNode> openList = new List<SearchNode>();
        private static List<SearchNode> closedList = new List<SearchNode>();
        private static List<VertexPositionColor> verts = new List<VertexPositionColor>();
        private const float regionScale = 0.9f;
        #endregion

        #region Initialization
        public static void CreateNavigationMap(int newLevelWidth, int newLevelHeight, int newNodeWidth) {
            levelWidth = newLevelWidth;
            levelHeight = newLevelHeight;
            nodeWidth = newNodeWidth;
            halfNodeWidth = (float)nodeWidth / 2;
            numNodesWidth = levelWidth / nodeWidth + 1;
            numNodesHeight = levelHeight / nodeWidth + 1;
            InitializeSearchNodes();
            verts.Clear();
        }

        private static void InitializeSearchNodes() {
            if (levelWidth == 0 || levelHeight == 0 || nodeWidth == 0)
                return;
            //Create the search nodes
            searchNodes = new SearchNode[numNodesWidth, numNodesHeight];
            Vector3 nodeRegionOffset = new Vector3(nodeWidth * regionScale, nodeWidth * regionScale, nodeWidth * regionScale);
            for (int x = 0; x < numNodesWidth; x ++) {
                for (int y = 0; y < numNodesHeight; y ++) {
                    SearchNode node = new SearchNode();
                    node.Position = new Vector2(halfNodeWidth + x * nodeWidth, halfNodeWidth + y * nodeWidth);
                    node.Obstructed = false;
                    node.Neighbours = new SearchNode[4];
                    Vector3 nodeCenter = new Vector3(node.Position.X, 0, node.Position.Y);
                    node.nodeRegion = new BoundingBox(nodeCenter - nodeRegionOffset, nodeCenter + nodeRegionOffset);
                    searchNodes[x, y] = node;
                }
            }
            //connect up the search nodes
            for (int x = 0; x < numNodesWidth; x++) {
                for (int y = 0; y < numNodesHeight; y++) {
                    SearchNode node = searchNodes[x, y];
                    SearchNode[] neighbours = new SearchNode[4];
                    if (x - 1 >= 0)
                        neighbours[0] = searchNodes[x - 1, y];
                    if (y + 1 < numNodesHeight)
                        neighbours[1] = searchNodes[x, y + 1];
                    if (x + 1 < numNodesWidth)
                        neighbours[2] = searchNodes[x + 1, y];
                    if (y - 1 >= 0)
                        neighbours[3] = searchNodes[x, y - 1];
                    node.Neighbours = neighbours;

                }
            }

        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Finds the optimal path from one point to another
        /// </summary>
        /// <param name="startPointV3"></param>
        /// <param name="endPointV3"></param>
        /// <returns></returns>
        public static List<Vector3> FindPath(Vector3 startPointV3, Vector3 endPointV3)
        {
            //Only try if the points are different
            if (startPointV3 == null || endPointV3 == null)
                return new List<Vector3>();
            if (startPointV3 == endPointV3)
                return new List<Vector3>();

            ResetSearchNodes();
            Vector2 startPoint = new Vector2(startPointV3.X, startPointV3.Z);
            Vector2 endPoint = new Vector2(endPointV3.X, endPointV3.Z);

            SearchNode startNode = positionToSearchNode(startPointV3);
            SearchNode endNode = positionToSearchNode(endPointV3);
            if (startNode == null || endNode == null)
                return new List<Vector3>();

            startNode.InOpenList = true;
            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                SearchNode currentNode = FindBestNode();
                if (currentNode == null)
                    break;
                if (currentNode == endNode)
                    return FindFinalPath(startNode, endNode);

                for (int i = 0; i < currentNode.Neighbours.Length; i++)
                {
                    SearchNode neighbour = currentNode.Neighbours[i];
                    if (neighbour == null)
                        continue;
                    if (neighbour.Obstructed == true)
                        continue;
                    float distanceTraveled = currentNode.DistanceTraveled + 1;
                    float heuristic = Heuristic(neighbour.Position, endPoint);

                    if (neighbour.InOpenList == false && neighbour.InClosedList == false)
                    {
                        neighbour.DistanceTraveled = distanceTraveled;
                        neighbour.DistanceToGoal = distanceTraveled + heuristic;
                        neighbour.Parent = currentNode;
                        neighbour.InOpenList = true;
                        openList.Add(neighbour);
                    }
                    else if (neighbour.InOpenList || neighbour.InClosedList)
                    {
                        if (neighbour.DistanceTraveled > distanceTraveled)
                        {
                            neighbour.DistanceTraveled = distanceTraveled;
                            neighbour.DistanceToGoal = distanceTraveled + heuristic;
                            neighbour.Parent = currentNode;
                        }
                    }

                }

                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            return new List<Vector3>();
        }

        /// <summary>
        /// Sets the search node in that position to be obstructed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="isObstructed"></param>
        public static void setSearchNodeObstructed(Vector3 position, bool isObstructed)
        {
            SearchNode node = positionToSearchNode(position);
            if (node != null)
                node.Obstructed = isObstructed;
        }

        /// <summary>
        /// Do not use. Unless apsolutly nessasary. Very inefficant.
        /// </summary>
        /// <param name="obstructedRegion"></param>
        /// <param name="isObstructed"></param>
        //public static void setSearchNodeObstructed(BoundingBox obstructedRegion, bool isObstructed) { }

        public static bool isPositionObstructed(Vector3 position) {
            SearchNode node = positionToSearchNode(position);
            if (node == null)
                return true;
            return node.Obstructed;
        }

        public static List<VertexPositionColor> getNodesVerts() {
            if (verts.Count != 0)
                return verts;
            float halfNodeSmall = halfNodeWidth * 0.95f;
            foreach (SearchNode node in searchNodes) {
                Vector3 blCorner = new Vector3(node.Position.X - halfNodeSmall, 0.01f, node.Position.Y - halfNodeSmall);
                Vector3 brCorner = new Vector3(node.Position.X + halfNodeSmall, 0.01f, node.Position.Y - halfNodeSmall);
                Vector3 tlCorner = new Vector3(node.Position.X - halfNodeSmall, 0.01f, node.Position.Y + halfNodeSmall);
                Vector3 trCorner = new Vector3(node.Position.X + halfNodeSmall, 0.01f, node.Position.Y + halfNodeSmall);
                Color squareColor;
                if (node.Obstructed)
                    squareColor = Color.Red;
                else
                    squareColor = Color.Blue;

                //Bottom
                verts.Add(new VertexPositionColor(blCorner, squareColor));
                verts.Add(new VertexPositionColor(brCorner, squareColor));
                //Right
                verts.Add(new VertexPositionColor(brCorner, squareColor));
                verts.Add(new VertexPositionColor(trCorner, squareColor));
                //Top
                verts.Add(new VertexPositionColor(trCorner, squareColor));
                verts.Add(new VertexPositionColor(tlCorner, squareColor));
                //Left
                verts.Add(new VertexPositionColor(tlCorner, squareColor));
                verts.Add(new VertexPositionColor(blCorner, squareColor));
            }
            return verts;
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Calculates the heuristic of the node - might need to be changed to distance squared
        /// </summary>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <returns></returns>
        private static float Heuristic(Vector2 position1, Vector2 position2) {
            //return Vector2.Distance(position1, position2); //More computing intensive
            return Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);
        }
        
        //Resets the state of the search nodes
        private static void ResetSearchNodes() {
            openList.Clear();
            closedList.Clear();

            for(int x = 0; x < numNodesWidth; x++){
                for (int y = 0; y < numNodesHeight; y++) {
                    SearchNode node = searchNodes[x, y];
                    node.InClosedList = false;
                    node.InOpenList = false;
                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        /// <summary>
        /// Returns the node wiht the smallest distance to the goal and ensures it is not obstructed
        /// </summary>
        /// <returns></returns>
        private static SearchNode FindBestNode() {
            SearchNode currentTile = openList[0];
            float smallestDistanceToGoal = float.MaxValue;

            for(int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal && !openList[i].Obstructed) {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }

        private static SearchNode positionToSearchNode(Vector3 position)
        {
            if (searchNodes == null)
                return null;
            int x = (int)(position.X / nodeWidth);
            int y = (int)(position.Z / nodeWidth);
            if (x >= numNodesWidth || y >= numNodesHeight || x < 0 || y < 0)
                return null;
            return searchNodes[x, y];
        }

        /// <summary>
        /// Uses the parent field of the search nodes to trace a path from the end node to the starting node
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        private static List<Vector3> FindFinalPath(SearchNode startNode, SearchNode endNode) {
            closedList.Add(endNode);
            SearchNode parentTile = endNode.Parent;
            List<Vector3> finalPath = new List<Vector3>();

            if (startNode == endNode)
                return finalPath;

            while (parentTile != startNode) {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }


            for (int i = closedList.Count - 1; i >= 0; i--) {
                Vector3 position = new Vector3(closedList[i].Position.X, 0, closedList[i].Position.Y);
                finalPath.Add(position);
            }

            return finalPath;
        }
        #endregion
    }
}
