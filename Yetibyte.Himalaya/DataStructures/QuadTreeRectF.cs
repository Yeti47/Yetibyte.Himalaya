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
        public Vector2 Position { get; private set; }

        /// <summary>
        /// An enumeration of all nodes in this <see cref="QuadTreeRectF"/>.
        /// </summary>
        public IEnumerable<QuadTreeNodeRectF> Nodes => _rootNode.DeepSubnodes.Concat(new[] { _rootNode });

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new quad tree data structure where each node is represented by a <see cref="RectangleF"/> (square) and stores a list of <see cref="IBounds"/> objects.
        /// </summary>
        /// <param name="maxObjectsPerNode">The maximum number of objects a node can hold before it splits.</param>
        /// <param name="minNodeSize">The minimum size of a node (used for both width and height).</param>
        /// <param name="position">The location in worldspace of this quad tree.</param>
        /// <param name="rootNodeSize">The size of the initial node (used for both width and height). Will automatically be set to minNodeSize if a value
        /// smaller than minNodeSize is passed.</param>
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

        /// <summary>
        /// Inserts the provided <see cref="IBounds"/> into this <see cref="QuadTreeRectF"/>.
        /// </summary>
        /// <param name="boundingBoxObject">The object to insert.</param>
        public void Insert(IBounds boundingBoxObject) {

            _rootNode.Insert(boundingBoxObject);

        }

        /// <summary>
        /// Gets an enumeration of all objects located in the nodes that overlap the given <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="area">The rectangular area to retrieve objects from.</param>
        /// <returns>An enumeration of all objects located in the nodes that overlap the given area.</returns>
        public IEnumerable<IBounds> GetObjectsAt(RectangleF area) => _rootNode.GetObjectsAt(area);

        /// <summary>
        /// Removes all objects from the quad tree's nodes and the nodes themselves. Only an empty root node remains.
        /// </summary>
        public void Clear() => _rootNode.Clear();

        /// <summary>
        /// Recreates this <see cref="QuadTreeRectF"/> by clearing it and creating a new root node with the given position and size.
        /// </summary>
        /// <param name="position">The location in worldspace of this quad tree.</param>
        /// <param name="rootNodeSize">The size of the initial node (used for both width and height). Will automatically be set to <see cref="MinNodeSize"/> if a value
        /// smaller than <see cref="MinNodeSize"/> is passed.</param>
        public void Recreate(Vector2 position, float rootNodeSize) {

            Clear();

            if (rootNodeSize < MinNodeSize)
                rootNodeSize = MinNodeSize;

            _rootNode = new QuadTreeNodeRectF(this, new RectangleF(position, new Vector2(rootNodeSize, rootNodeSize)));

        }

        /// <summary>
        /// Adds the given <see cref="IBounds"/> to the dictionary that keeps track of which objects are included in this <see cref="QuadTreeRectF"/>
        /// and which node they're assigned to.
        /// </summary>
        /// <param name="boundingBoxObject"></param>
        internal void AddToObjectNodeMap(IBounds boundingBoxObject, QuadTreeNodeRectF node) {



        }

        #endregion



    }
}
