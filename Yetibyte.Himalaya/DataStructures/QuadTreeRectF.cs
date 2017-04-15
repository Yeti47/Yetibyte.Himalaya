using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.DataStructures {

    /// <summary>
    /// Describes a quad tree data structure where each node is represented by a <see cref="RectangleF"/> and stores a list of <see cref="IBounds"/> objects.
    /// </summary>
    public class QuadTreeRectF {

        #region Constants

        private const float ABSOLUTE_MIN_NODE_SIZE = 1f;

        #endregion

        #region Fields 

        private QuadTreeNodeRectF _rootNode;

        #endregion 

        #region Properties

        public uint MaxObjectsPerNode { get; }
        public float MinNodeSize { get; }
        public QuadTreeNodeRectF RootNode => _rootNode;
        public Vector2 Position { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new quad tree data structure where each node is represented by a <see cref="RectangleF"/> (square) and stores a list of <see cref="IBounds"/> objects.
        /// </summary>
        /// <param name="maxObjectsPerNode">The maximum number of objects a node can hold before it splits.</param>
        /// <param name="minNodeSize">The minimum size of a node (used for both width and height).</param>
        /// <param name="position">The location in worldspace of this quad tree.</param>
        /// <param name="rootNodeSize">The size of the initial node (used for both width and height).</param>
        public QuadTreeRectF(uint maxObjectsPerNode, float minNodeSize, Vector2 position, float rootNodeSize) {

            if (minNodeSize < ABSOLUTE_MIN_NODE_SIZE)
                throw new Exception("The quad tree's nodes cannot be smaller than " + ABSOLUTE_MIN_NODE_SIZE + ". Please provide a greater value for 'minNodeSize'.");

            this.MaxObjectsPerNode = maxObjectsPerNode;
            this.MinNodeSize = minNodeSize;

            if (rootNodeSize < minNodeSize)
                rootNodeSize = minNodeSize;

            _rootNode = new QuadTreeNodeRectF(this, new RectangleF(position, new Vector2(rootNodeSize, rootNodeSize)));

        }

        #endregion

        #region Methods

        public void Insert(IBounds boundingBoxObject) {

            _rootNode.Insert(boundingBoxObject);

        }

        public IEnumerable<IBounds> GetObjectsAt(RectangleF area) => _rootNode.GetObjectsAt(area);

        #endregion



    }
}
