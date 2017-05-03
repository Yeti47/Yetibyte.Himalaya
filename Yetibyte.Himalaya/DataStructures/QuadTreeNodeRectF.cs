using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.GameElements;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.DataStructures {

    /// <summary>
    /// Describes a node in a <see cref="QuadTreeRectF"/> that is represented by a <see cref="RectangleF"/> and stores a list of <see cref="IBounds"/> objects.
    /// </summary>
    public class QuadTreeNodeRectF {

        #region Fields

        private QuadTreeRectF _parentTree;

        #endregion

        #region Properties

        public RectangleF NodeBounds { get; }
        public List<IBounds> BoundingBoxObjects { get; set; } = new List<IBounds>();
        public QuadTreeNodeRectF[] SubNodes { get; private set; } = new QuadTreeNodeRectF[4];

        /// <summary>
        /// Whether or not this node can be split without the resulting sub-nodes becoming too small.
        /// </summary>
        public bool CanSplit => (NodeBounds.Width / 2 >= _parentTree.MinNodeSize) && (NodeBounds.Height / 2 >= _parentTree.MinNodeSize);
        public bool HasSubnodes => SubNodes[0] != null;

        #endregion

        #region Constructors

        public QuadTreeNodeRectF(QuadTreeRectF parentTree, RectangleF bounds) {

            _parentTree = parentTree;
            this.NodeBounds = bounds;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts the provided <see cref="IBounds"/> into this node or the appropriate subnode.
        /// </summary>
        /// <param name="boundingBoxObject">The object to insert.</param>
        public void Insert(IBounds boundingBoxObject) {

            QuadTreeNodeRectF allocatedNode = GetAllocatedNode(boundingBoxObject.Bounds);

            if(allocatedNode != this) {

                allocatedNode.Insert(boundingBoxObject);
                return;

            }

            BoundingBoxObjects.Add(boundingBoxObject);

            if(BoundingBoxObjects.Count > _parentTree.MaxObjectsPerNode && CanSplit) {

                if (!HasSubnodes)
                    Split();

                // Redistribute objects into sub-nodes

                BoundingBoxObjects.RemoveAll(o => {
                    var subnode = GetAllocatedNode(o.Bounds);
                    bool nodeIsParent = subnode == this;
                    if (!nodeIsParent) {
                        subnode.Insert(o);
                    }
                    return !nodeIsParent;
                });

            }


        }

        /// <summary>
        /// Splits this <see cref="QuadTreeNodeRectF"/> into four equally sized sub-nodes and stores them in <see cref="SubNodes"/>.
        /// </summary>
        private void Split() {

            Vector2 subnodeSize = new Vector2(NodeBounds.Width / 2, NodeBounds.Height / 2);

            SubNodes[0] = new QuadTreeNodeRectF(_parentTree, new RectangleF(NodeBounds.Location, subnodeSize));
            SubNodes[1] = new QuadTreeNodeRectF(_parentTree, new RectangleF(NodeBounds.Location + new Vector2(subnodeSize.X, 0), subnodeSize));
            SubNodes[2] = new QuadTreeNodeRectF(_parentTree, new RectangleF(NodeBounds.Location + new Vector2(subnodeSize.X, subnodeSize.Y), subnodeSize));
            SubNodes[3] = new QuadTreeNodeRectF(_parentTree, new RectangleF(NodeBounds.Location + new Vector2(0, subnodeSize.Y), subnodeSize));

        }

        /// <summary>
        /// Determines which sub-node the given <see cref="RectangleF"/> fits in. If it does not completely fit inside any of this node's sub-nodes or if 
        /// there are no sub-nodes at all, the parent node itself (meaning this instance) is returned.
        /// </summary>
        /// <param name="boundingBox">The bounding rectangle to assign to a sub-node.</param>
        /// <returns>The sub-node of this <see cref="QuadTreeNodeRectF"/> that the given bounding box completely fits in, or this node itself in case the 
        /// provided rectangle lies between sub-nodes or there aren't any sub-nodes.</returns>
        private QuadTreeNodeRectF GetAllocatedNode(RectangleF boundingBox) {

            QuadTreeNodeRectF result = this;

            if (!HasSubnodes)
                return result;

            Vector2 center = NodeBounds.Center;

            bool fitsInTopHalf = boundingBox.Bottom <= center.Y;
            bool fitsInBottomHalf = boundingBox.Top > center.Y;
            bool fitsInLeftHalf = boundingBox.Right <= center.X;
            bool fitsInRightHalf = boundingBox.Left > center.X;

            if (fitsInTopHalf) {

                if (fitsInLeftHalf)
                    result = SubNodes[0];

                if (fitsInRightHalf)
                    result = SubNodes[1];

            }
            else if (fitsInBottomHalf) {

                if (fitsInLeftHalf)
                    result = SubNodes[3];

                if (fitsInRightHalf)
                    result = SubNodes[2];

            }

            return result;

        }

        /// <summary>
        /// Gets an enumeration of all objects located in the nodes that overlap the given <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="area">The rectangular area to retrieve objects from.</param>
        /// <returns>An enumeration of all objects located in the nodes that overlap the given area.</returns>
        public IEnumerable<IBounds> GetObjectsAt(RectangleF area) {

            List<IBounds> resultList = new List<IBounds>();

            if (HasSubnodes) {

                QuadTreeNodeRectF node = GetAllocatedNode(area);

                if(node != this)
                    resultList.AddRange(node.GetObjectsAt(area));

            }

            resultList.AddRange(BoundingBoxObjects);

            return resultList;

        }

        /// <summary>
        /// Recursively removes all objects from this node and all its sub-nodes. Also sets all sub-nodes to null.
        /// </summary>
        public void Clear() {

            BoundingBoxObjects.Clear();

            for (int i = 0; i < SubNodes.Length; i++) {

                if(SubNodes[i] != null) {

                    SubNodes[i].Clear();
                    SubNodes[i] = null;
                    
                }

            }

        }
                
        #endregion

    }

}
