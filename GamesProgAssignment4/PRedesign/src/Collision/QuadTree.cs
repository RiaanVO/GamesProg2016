using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    //http://www.gamedev.net/page/resources/_/technical/game-programming/introduction-to-octrees-r3529
    /// <summary>
    /// The Oct Tree is a 3 Dimensional version of the Quad Tree. It breaks up a 3D space into enclosing boxes.
    /// Partitioning logic: If a node contains two or more objects, partition the node by subdividing it into 8 quadrants. 
    /// Then, try to place all objects into the subdivided regions of space. If the objects cannot be fully contained, leave them in their current node.
    /// This is implemented recursively.
    /// </summary>
    class QuadTree
    {
        #region Fields
        BoundingBox m_region;

        List<Collider> m_objects;

        static Queue<Collider> m_pendingInsertion = new Queue<Collider>();

        QuadTree[] m_childNode = new QuadTree[4];

        const int MIN_SIZE = 10;
        const int MIN_COLLIDER_COUNT = 1;

        QuadTree _parent;

        static bool m_treeReady = false;       //the tree has a few objects which need to be inserted before it is complete
        static bool m_treeBuilt = false;       //there is no pre-existing tree yet.

        private bool isRendered = false;
        #endregion

        #region Properties
        private bool IsRoot
        {
            get { return _parent == null; }
        }

        public bool TreeBuilt
        {
            set
            {
                m_treeBuilt = m_treeReady = value;
            }
        }
        #endregion

        #region Initialization
        /*Note: we want to avoid allocating memory for as long as possible since there can be lots of nodes.*/
        /// <summary>
        /// Creates an oct tree which encloses the given region and contains the provided objects.
        /// </summary>
        /// <param name="region">The bounding region for the oct tree.</param>
        /// <param name="objList">The list of objects contained within the bounding region</param>
        private QuadTree(BoundingBox region, List<Collider> objList)
        {
            m_region = region;
            m_objects = objList;
        }

        public QuadTree()
        {
            m_objects = new List<Collider>();
            m_region = new BoundingBox(Vector3.Zero, Vector3.Zero);
        }

        /// <summary>
        /// Creates an octTree with a suggestion for the bounding region containing the items.
        /// </summary>
        /// <param name="region">The suggested dimensions for the bounding region. 
        /// Note: if items are outside this region, the region will be automatically resized.</param>
        public QuadTree(BoundingBox region)
        {
            m_region = region;
            m_objects = new List<Collider>();
        }

        #endregion

        public void moveCollider(Collider collider)
        {
            if (!m_treeBuilt)
                return;

            QuadTree current = this;
            BoxCollider boxCol = collider as BoxCollider;
            SphereCollider sphereCol = collider as SphereCollider;
            if (boxCol != null)
            {
                while (current.m_region.Contains(boxCol.Collider) != ContainmentType.Contains)
                    if (current._parent != null) current = current._parent;
                    else break;
            }
            else if (sphereCol != null)
            {
                while (current.m_region.Contains(sphereCol.Collider) != ContainmentType.Contains)//we must be using a bounding sphere, so check for its containment.
                    if (current._parent != null) current = current._parent;
                    else break;
            }

            //now, remove the object from the current node and insert it into the current containing node.
            m_objects.Remove(collider);
            current.Insert(collider);   //this will try to insert the object as deep into the tree as we can go.
        }

        #region Adding, Inserting and Removing colliders
        public void Add(List<Collider> colliders)
        {
            foreach (Collider collider in colliders)
            {
                m_pendingInsertion.Enqueue(collider);
                m_treeReady = false;
            }
        }

        public void Add(Collider collider)
        {
            m_pendingInsertion.Enqueue(collider);
            m_treeReady = false;
            collider.QuadTreeNode = this;
        }

        public void Remove(Collider collider)
        {
            if (m_objects.Contains(collider))
                m_objects.Remove(collider);
            else
            {
                if (m_childNode != null)
                    foreach (QuadTree childNode in m_childNode)
                        if (childNode != null)
                            childNode.Remove(collider);
            }
        }



        /// <summary>
        /// A tree has already been created, so we're going to try to insert an item into the tree without rebuilding the whole thing
        /// </summary>
        /// <typeparam name="T">A Collider object</typeparam>
        /// <param name="Item">The Collider object to insert into the tree</param>
        private void Insert(Collider collider)
        {
            /*make sure we're not inserting an object any deeper into the tree than we have to.
                -if the current node is an empty leaf node, just insert and leave it.*/
            if (m_objects.Count <= MIN_COLLIDER_COUNT)
            {
                m_objects.Add(collider);
                return;
            }

            Vector3 dimensions = m_region.Max - m_region.Min;
            //Check to see if the dimensions of the box are greater than the minimum dimensions
            if (dimensions.X <= MIN_SIZE && dimensions.Z <= MIN_SIZE)
            {
                m_objects.Add(collider);
                return;
            }
            Vector3 half = dimensions / 2.0f;
            Vector3 center = m_region.Min + half;

            //Find or create subdivided regions for each octant in the current region
            BoundingBox[] childQuadrant = new BoundingBox[4];
            childQuadrant[0] = (m_childNode[0] != null) ? m_childNode[0].m_region : new BoundingBox(m_region.Min, new Vector3(center.X, m_region.Max.Y, center.Z));
            childQuadrant[1] = (m_childNode[1] != null) ? m_childNode[1].m_region : new BoundingBox(new Vector3(center.X, m_region.Min.Y, m_region.Min.Z), new Vector3(m_region.Max.X, m_region.Max.Y, center.Z));
            childQuadrant[2] = (m_childNode[2] != null) ? m_childNode[2].m_region : new BoundingBox(new Vector3(m_region.Min.X, m_region.Min.Y, center.Z), new Vector3(center.X, m_region.Max.Y, m_region.Max.Z));
            childQuadrant[3] = (m_childNode[3] != null) ? m_childNode[3].m_region : new BoundingBox(new Vector3(center.X, m_region.Min.Y, center.Z), m_region.Max);

            //First, is the item completely contained within the root bounding box?
            //note2: I shouldn't actually have to compensate for this. If an object is out of our predefined bounds, then we have a problem/error.
            //          Wrong. Our initial bounding box for the terrain is constricting its height to the highest peak. Flying units will be above that.
            //             Fix: I resized the enclosing box to 256x256x256. This should be sufficient.
            BoxCollider boxCol = collider as BoxCollider;
            SphereCollider sphereCol = collider as SphereCollider;
            bool notContained = true;
            if (boxCol != null)
            {
                if (m_region.Contains(boxCol.Collider) == ContainmentType.Contains)
                {
                    notContained = false;
                    bool found = false;
                    for (int i = 0; i < 4; i++)
                    {
                        //is the object fully contained within a quadrant?
                        if (childQuadrant[i].Contains(boxCol.Collider) == ContainmentType.Contains)
                        {
                            if (m_childNode[i] != null)
                                m_childNode[i].Insert(collider);   //Add the item into that tree and let the child tree figure out what to do with it
                            else
                            {
                                m_childNode[i] = CreateNode(childQuadrant[i], collider);
                            }
                            found = true;
                        }
                    }
                    if (!found) m_objects.Add(collider);
                }
            }
            else if (sphereCol != null)
            {
                if (m_region.Contains(sphereCol.Collider) == ContainmentType.Contains)
                {
                    notContained = false;
                    bool found = false;
                    for (int i = 0; i < 4; i++)
                    {
                        //is the object fully contained within a quadrant?
                        if (childQuadrant[i].Contains(sphereCol.Collider) == ContainmentType.Contains)
                        {
                            if (m_childNode[i] != null)
                                m_childNode[i].Insert(collider);   //Add the item into that tree and let the child tree figure out what to do with it
                            else
                            {
                                m_childNode[i] = CreateNode(childQuadrant[i], collider);
                            }
                            found = true;
                        }
                    }
                    if (!found) m_objects.Add(collider);
                }
            }

            if (notContained)
                BuildTree();
        }
        #endregion


        #region Tree Constuction

        private void BuildTree()
        {
            if (m_objects.Count <= MIN_COLLIDER_COUNT)
                return;

            Vector3 dimensions = m_region.Max - m_region.Min;

            if (dimensions == Vector3.Zero)
            {
                //FindEnclosingCube();
                //dimensions = m_region.Max - m_region.Min;
                Console.WriteLine("Quadrent is a zero cube???");
                //return;
            }

            //Check to see if the dimensions of the box are greater than the minimum dimensions
            if (dimensions.X <= MIN_SIZE && dimensions.Z <= MIN_SIZE)
            {
                return;
            }

            Vector3 half = dimensions / 2.0f;
            Vector3 center = m_region.Min + half;

            //Create subdivided regions for each octant
            BoundingBox[] quadrant = new BoundingBox[4];
            quadrant[0] = new BoundingBox(m_region.Min, new Vector3(center.X, m_region.Max.Y, center.Z));
            quadrant[1] = new BoundingBox(new Vector3(center.X, m_region.Min.Y, m_region.Min.Z), new Vector3(m_region.Max.X, m_region.Max.Y, center.Z));
            quadrant[2] = new BoundingBox(new Vector3(m_region.Min.X, m_region.Min.Y, center.Z), new Vector3(center.X, m_region.Max.Y, m_region.Max.Z));
            quadrant[3] = new BoundingBox(new Vector3(center.X, m_region.Min.Y, center.Z), m_region.Max);

            //This will contain all of our objects which fit within each respective octant.
            List<Collider>[] quadList = new List<Collider>[4];
            for (int i = 0; i < 4; i++) quadList[i] = new List<Collider>();

            //this list contains all of the objects which got moved down the tree and can be delisted from this node.
            List<Collider> delist = new List<Collider>();

            foreach (Collider collider in m_objects)
            {
                BoxCollider boxCol = collider as BoxCollider;
                SphereCollider sphereCol = collider as SphereCollider;
                if (boxCol != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (quadrant[i].Contains(boxCol.Collider) == ContainmentType.Contains)
                        {
                            quadList[i].Add(collider);
                            delist.Add(collider);
                            break;
                        }
                    }
                }
                else if (sphereCol != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (quadrant[i].Contains(sphereCol.Collider) == ContainmentType.Contains)
                        {
                            quadList[i].Add(collider);
                            delist.Add(collider);
                            break;
                        }
                    }
                }
            }

            //delist every moved object from this node.
            foreach (Collider obj in delist)
                m_objects.Remove(obj);

            //Create child nodes where there are items contained in the bounding region
            for (int i = 0; i < 4; i++)
            {
                if (quadList[i].Count != 0)
                {
                    m_childNode[i] = CreateNode(quadrant[i], quadList[i]);
                    m_childNode[i].BuildTree();
                }
            }

            m_treeBuilt = true;
            m_treeReady = true;
        }

        private QuadTree CreateNode(BoundingBox region, List<Collider> objList)  //complete & tested
        {
            if (objList.Count == 0)
                return null;

            QuadTree tree = new QuadTree(region, objList);
            tree._parent = this;

            return tree;
        }

        private QuadTree CreateNode(BoundingBox region, Collider Item)
        {
            List<Collider> objList = new List<Collider>(1); //sacrifice potential CPU time for a smaller memory footprint
            objList.Add(Item);
            QuadTree tree = new QuadTree(region, objList);
            tree._parent = this;
            return tree;
        }

        #endregion

        /// <summary>
        /// Processes all pending insertions by inserting them into the tree.
        /// </summary>
        /// <remarks>Consider deprecating this?</remarks>
        public void UpdateTree()   //complete & tested
        {
            /*I think I can just directly insert items into the tree instead of using a queue.*/
            if (!m_treeBuilt)
            {
                while (m_pendingInsertion.Count != 0)
                    m_objects.Add(m_pendingInsertion.Dequeue());

                //trim out any objects which have the exact same bounding areas

                BuildTree();
            }
            else
            {
                while (m_pendingInsertion.Count != 0)
                    Insert(m_pendingInsertion.Dequeue());
            }

            m_treeReady = true;
        }

        public List<Collider> collidingWith(Collider collider)
        {
            if (!m_treeReady)
                UpdateTree();

            List<Collider> collidingWith = new List<Collider>();
            foreach (Collider otherCollider in m_objects)
            {
                if (collider != otherCollider)
                {
                    if (collider.isColliding(otherCollider))
                        collidingWith.Add(otherCollider);
                }
            }

            if (m_childNode != null)
                foreach (QuadTree childNode in m_childNode)
                    if (childNode != null)
                        if (childNode.colliderInQuadTree(collider))
                            collidingWith.AddRange(childNode.collidingWith(collider));


            return collidingWith;
        }

        private bool colliderInQuadTree(Collider collider)
        {
            bool colliderInQuadTree = false;
            BoxCollider boxCol = collider as BoxCollider;
            SphereCollider sphereCol = collider as SphereCollider;
            if (boxCol != null)
            {
                ContainmentType type = m_region.Contains(boxCol.Collider);
                if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                    colliderInQuadTree = true;
            }
            else if (sphereCol != null)
            {
                ContainmentType type = m_region.Contains(sphereCol.Collider);
                if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                    colliderInQuadTree = true;
            }
            return colliderInQuadTree;
        }

        public void RenderTree(Color colour, bool showRegions, bool showColliders)
        {
            if (showRegions && m_region.Min - m_region.Max != Vector3.Zero && !isRendered)
            {
                WireShapeDrawer.AddBoundingBox(m_region, colour, CollisionManager.renderTime);
                isRendered = true;
            }
            if (m_objects != null && showColliders)
                foreach (Collider collider in m_objects)
                    collider.drawCollider();
            if (m_childNode != null)
                foreach (QuadTree childNode in m_childNode)
                    if (childNode != null)
                        childNode.RenderTree(colour, showRegions, showColliders);
        }

        public bool pruneDeadBranches()
        {
            bool prune = true;

            for(int i = 0; i < m_childNode.Length; i ++)
            {
                QuadTree branch = m_childNode[i];
                if (branch != null)
                {
                    bool currentBranch = branch.pruneDeadBranches();

                    if (!currentBranch)
                        prune = false;
                    else
                        m_childNode[i] = null;
                }
            }
            if (prune)
                prune = shouldPrune();
            return prune;
        }

        private bool shouldPrune()
        {
            return m_objects.Count == 0;
        }

        public int countActiveBranches()
        {
            int childrenCount = 1; //First one is the branch itself 
            foreach (QuadTree branch in m_childNode)
            {
                if (branch != null)
                {
                    childrenCount += branch.countActiveBranches();
                }
            }
            return childrenCount;
        }

        public void resetRender() {
            isRendered = false;
            foreach (QuadTree childNode in m_childNode)
                if (childNode != null)
                    childNode.resetRender();
        }
    }
}