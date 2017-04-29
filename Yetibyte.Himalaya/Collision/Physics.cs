﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.DataStructures;

namespace Yetibyte.Himalaya.Collision {

    public class Physics {

        #region Constants

        private const float QUAD_TREE_SIZE_TOLERANCE = 10f;
        private const uint QUAD_TREE_MAX_OBJECTS_PER_NODE = 15;
        private const float QUAD_TREE_MIN_NODE_SIZE = 50f;

        #endregion

        #region Fields

        private Scene _scene;

        #endregion

        #region Properties

        public bool IgnoreGravity { get; set; }
        public float Gravity { get; set; } = 9f;
        public QuadTreeRectF CollisionTree { get; private set; }

        #endregion

        #region Constructors

        public Physics(Scene scene) {

            this._scene = scene;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Enumerates all the <see cref="Collider"/>s attached to GameEntities in this <see cref="Scene"/> that are active and not destroyed.
        /// </summary>
        /// <returns>A collection of all Colliders attached to GameEntities that live in the current Scene.</returns>
        public IEnumerable<Collider> GetCollidersInActiveEntities() => _scene.LiveGameEntities.SelectMany(e => e.GetComponents<Collider>());

        /// <summary>
        /// Enumerates all the <see cref="Collider"/>s that match the given base Type and are
        /// attached to active GameEntities in this <see cref="Scene"/>.
        /// </summary>
        /// <typeparam name="T">The type to match the colliders with.</typeparam>
        /// <returns>A collection of all Colliders that match the given Type and are attached to GameEntities that live in the current Scene.</returns>
        public IEnumerable<T> GetCollidersInActiveEntities<T>() where T : Collider => _scene.LiveGameEntities.SelectMany(e => e.GetComponents<T>());

        /// <summary>
        /// Enumerates all active <see cref="Collider"/>s that are currently in this <see cref="Scene"/>.
        /// </summary>
        /// <returns>A collection of all active Colliders of the given Type that live in the current Scene.</returns>
        public IEnumerable<Collider> GetActiveColliders() => _scene.LiveGameEntities.SelectMany(e => e.GetActiveComponents<Collider>());

        /// <summary>
        /// Enumerates all active <see cref="Collider"/>s of the given type that are currently in this <see cref="Scene"/>.
        /// </summary>
        /// <typeparam name="T">The type to match the colliders with.</typeparam>
        /// <returns>A collection of all active Colliders of the given Type that live in the current Scene.</returns>
        public IEnumerable<Collider> GetActiveColliders<T>() where T : Collider => _scene.LiveGameEntities.SelectMany(e => e.GetActiveComponents<T>());

        /// <summary>
        /// (Re)creates the <see cref="QuadTreeRectF"/> used in order to reduce the number of pairings that need to be checked for collision detection.
        /// </summary>
        public void BuildCollisionTree() {

            IEnumerable<Collider> activeColliders = GetActiveColliders();

            if (activeColliders == null || activeColliders.Count() <= 0)
                return;

            float minX = activeColliders.Min(c => c.Bounds.X);
            float minY = activeColliders.Min(c => c.Bounds.Y);
            float maxX = activeColliders.Max(c => c.Bounds.Right);
            float maxY = activeColliders.Min(c => c.Bounds.Bottom);

            float quadTreeSize = Math.Max(maxX - minX, maxY - minY) + QUAD_TREE_SIZE_TOLERANCE;
            Vector2 quadTreePosition = new Vector2(minX - QUAD_TREE_SIZE_TOLERANCE / 2, minY - QUAD_TREE_SIZE_TOLERANCE / 2);

            if (CollisionTree == null)
                CollisionTree = new QuadTreeRectF(QUAD_TREE_MAX_OBJECTS_PER_NODE, QUAD_TREE_MIN_NODE_SIZE, quadTreePosition, quadTreeSize);
            else
                CollisionTree.Recreate(quadTreePosition, quadTreeSize);

            foreach (Collider collider in activeColliders)
                CollisionTree.Insert(collider);

        }

        public RaycastInfo Raycast(Vector2 origin, Vector2 direction, float length, CollisionLayers collisionLayers = CollisionLayers.All) {

            RaycastInfo result = RaycastInfo.Default;

            direction.Normalize();
            LineSegment ray = new LineSegment(origin, direction * length);

            foreach (IEdges colliderWithEdges in CollisionTree.GetObjectsAt(ray.Bounds).OfType<IEdges>()) {

                Collider collider = colliderWithEdges as Collider;

                if (collider == null || !collider.IsOnLayer(collisionLayers))
                    continue;

                foreach (LineSegment edge in colliderWithEdges.GetEdges()) {

                    bool intersects = LineSegment.Intersect(ray, edge, out Vector2 intersectionPoint);

                    if(intersects) {

                        result = new RaycastInfo(true, intersectionPoint, collider, origin);
                        ray = result.Ray;

                    }
                    
                }

            }

            return result;

        }

        #endregion

    }

}
