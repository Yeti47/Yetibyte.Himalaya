﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Utilities;

namespace Yetibyte.Himalaya.Collision {

    public class CollisionController : EntityComponent, IUpdate {

        #region Constants

        /// <summary>
        /// The maximum length of each step when continuous collision detection is used.
        /// </summary>
        private const float CONTINUOUS_DETECTION_MAX_STEP_SIZE = 2f;

        #endregion

        #region Fields

        private Physics _physics;
        private IEnumerable<Collider> _myActiveColliders;

        #endregion

        #region Events

        public delegate void TriggerEnterHandler(Collider ownCollider, Collider otherCollider);
        public delegate void TriggerHandler(Collider ownCollider, Collider otherCollider);
        public delegate void TriggerLeaveHandler(Collider ownCollider, Collider otherCollider);

        public event TriggerEnterHandler RaiseTriggerEnter;
        public event TriggerHandler RaiseTrigger;
        public event TriggerLeaveHandler RaiseTriggerLeave;

        #endregion

        #region Properties

        public float GravityScale { get; set; }
        public bool IgnoreGravity { get; set; }

        /// <summary>
        /// The collision detection method to use.
        /// </summary>
        public CollisionDetectionMethods CollisionDetectionMethod { get; set; } = CollisionDetectionMethods.Lazy;

        #endregion

        #region Methods

        /// <summary>
        /// Enumerates all <see cref="Collider"/>s this CollisionController has control of.
        /// </summary>
        /// <returns>An enumeration of all Colliders this CollisionController has control of.</returns>
        public IEnumerable<Collider> GetAttachedColliders() {

            return IsAttached ? GameEntity.GetComponentsInChildren<Collider>(true, true) : new Collider[0];

        }

        public void ApplyGravity() {

            throw new NotImplementedException("Gravity has not been fully implemented yet.");

        }

        /// <summary>
        /// Moves the <see cref="GameEntity"/> this CollisionController is attached to by the given offset while responding
        /// to potential collisions.
        /// </summary>
        /// <param name="offset">The offset to move by.</param>
        public void Move(Vector2 offset) {

            if (!IsAttached)
                return;

            _physics = GetPhysics();
            _myActiveColliders = GetAttachedColliders().Where(c => c.IsActive && !c.IsTrigger);

            if (CollisionDetectionMethod == CollisionDetectionMethods.Lazy) {

                DetectCollisions(ref offset);
                GameEntity.Transform.Position += offset;
            }
            else {

                Vector2 moveDirection = Vector2.Normalize(offset);

                float remainingDistance = offset.Length();
                bool hasCollided = false;

                while (remainingDistance > 0 && !hasCollided) {

                    Vector2 step = moveDirection * Math.Min(CONTINUOUS_DETECTION_MAX_STEP_SIZE, remainingDistance);
                    hasCollided = DetectCollisions(ref step);
                    GameEntity.Transform.Position += step;
                    remainingDistance -= step.Length();

                }

            }

        }

        private bool DetectCollisions(ref Vector2 offset) {

            bool hasCollided = false;

            // Check collision for all colliders attached to this Collision Controller
            foreach (Collider myCollider in _myActiveColliders) {

                RectangleF futureBounds = new RectangleF(myCollider.Bounds.Location + offset, myCollider.Bounds.Size);
                RectangleF velocityBounds = RectangleF.Union(myCollider.Bounds, futureBounds);

                // perform collision detection between the current collider and all colliders in the scene it could potentially collide with
                foreach (IBounds potentialTarget in _physics.CollisionTree.GetObjectsAt(velocityBounds)) {

                    Collider potentialTargetCollider = potentialTarget as Collider;

                    // skip detection for triggers and colliders that belong to this controller
                    if (potentialTargetCollider == null || potentialTargetCollider.IsTrigger || _myActiveColliders.Contains(potentialTargetCollider)) 
                        continue;

                    CollisionInfo collisionInfo = CollisionInfo.Default;

                    switch (potentialTargetCollider) {

                        case RectCollider r:

                            collisionInfo = myCollider.WillIntersect(offset, r);
                            break;

                        default:
                            break;
                    }

                    if(collisionInfo.Intersects) {

                        hasCollided = true;
                        offset -= collisionInfo.Penetration;
                        
                        // raise OnCollision Event

                    }
                        
                }

            }

            return hasCollided;

        }

        /// <summary>
        /// Gets the <see cref="Physics"/> instance of the <see cref="Scene"/> this Collision Controller lives in.
        /// </summary>
        /// <returns>The <see cref="Physics"/> instance of the <see cref="Scene"/> this Collision Controller lives in.</returns>
        private Physics GetPhysics() => GameEntity?.Scene?.Physics;

        public void Update(GameTime gameTime, float globalTimeScale) {
            


        }

        public void UpdateTriggers() {

            IEnumerable<Collider> triggers = GetAttachedColliders().Where(c => c.IsTrigger && c.IsActive);

            foreach(Collider trigger in triggers) {

                trigger.IntersectingColliders.Clear();

                foreach (IBounds potentialTarget in _physics.CollisionTree.GetObjectsAt(trigger.Bounds)) {

                    Collider potentialTargetCollider = potentialTarget as Collider;

                    // skip detection for colliders that belong to this controller
                    if (potentialTargetCollider == null || triggers.Contains(potentialTargetCollider))
                        continue;

                    bool intersects = false;

                    switch (potentialTargetCollider) {

                        case RectCollider r:
                            intersects = trigger.Intersects(r);
                            break;
                        default:
                            break;
                    }

                    if(intersects) {

                        trigger.IntersectingColliders.Add(potentialTargetCollider);

                        // Were the two colliders intersecting in the previous frame as well?
                        if(trigger.PreviouslyIntersectingColliders.Contains(potentialTargetCollider)) {

                            // raise OnTrigger Event
                            OnTrigger(trigger, potentialTargetCollider);

                        }
                        else {

                            // raise OnTriggerEnter Event
                            OnTriggerEnter(trigger, potentialTargetCollider);

                        }

                    }

                }

                foreach (Collider collider in trigger.PreviouslyIntersectingColliders.Except(trigger.IntersectingColliders)) {

                    // raise OnTriggerLeave event
                    OnTriggerLeave(trigger, collider);

                }

                trigger.PreviouslyIntersectingColliders.Clear();
                trigger.PreviouslyIntersectingColliders = new List<Collider>(trigger.IntersectingColliders);

            }

        }

        protected virtual void OnTriggerEnter(Collider ownCollider, Collider otherCollider) {

            RaiseTriggerEnter?.Invoke(ownCollider, otherCollider);

        }

        protected virtual void OnTrigger(Collider ownCollider, Collider otherCollider) {

            RaiseTrigger?.Invoke(ownCollider, otherCollider);

        }

        protected virtual void OnTriggerLeave(Collider ownCollider, Collider otherCollider) {

            RaiseTriggerLeave?.Invoke(ownCollider, otherCollider);

        }

        #endregion



    }

}
