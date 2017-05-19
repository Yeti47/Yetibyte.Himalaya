using System;
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
        private IEnumerable<Collider> myActiveNonTriggerColliders;

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

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        public float VelocityX {

            get => Velocity.X;
            set => Velocity = new Vector2(value, Velocity.Y);

        }

        public float VelocityY {

            get => Velocity.Y;
            set => Velocity = new Vector2(Velocity.X, value);

        }

        /// <summary>
        /// The collision detection method to use.
        /// </summary>
        public CollisionDetectionMethods CollisionDetectionMethod { get; set; } = CollisionDetectionMethods.Lazy;

        /// <summary>
        /// Determines which <see cref="GameEntity"/> should be notified when a collision event occurs. It can be either
        /// the entity this Controller is attached to, the respective entity of the collider that caused the event, or both.
        /// </summary>
        public CollisionListeners CollisionListener { get; set; } = CollisionListeners.Both;

        /// <summary>
        /// If set to 'true', the <see cref="GameEntity"/> attached to this CollisionController will never be moved, regardless
        /// of the <see cref="Velocity"/>.
        /// </summary>
        public bool IsStatic { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Enumerates all <see cref="Collider"/>s this CollisionController has control of.
        /// </summary>
        /// <returns>An enumeration of all Colliders this CollisionController has control of.</returns>
        public IEnumerable<Collider> GetAttachedColliders() {

            return IsAttached ? GameEntity.GetComponentsInChildren<Collider>(true, true) : new Collider[0];

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
            myActiveNonTriggerColliders = GetAttachedColliders().Where(c => c.IsActive && !c.IsTrigger);

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
            foreach (Collider myCollider in myActiveNonTriggerColliders) {

                RectangleF futureBounds = new RectangleF(myCollider.Bounds.Location + offset, myCollider.Bounds.Size);
                RectangleF velocityBounds = RectangleF.Union(myCollider.Bounds, futureBounds);

                // perform collision detection between the current collider and all colliders in the scene it could potentially collide with
                foreach (IBounds potentialTarget in _physics.CollisionTree.GetObjectsAt(velocityBounds)) {

                    Collider potentialTargetCollider = potentialTarget as Collider;

                    // skip detection for triggers and colliders that belong to this controller
                    if (potentialTargetCollider == null || potentialTargetCollider.IsTrigger || myActiveNonTriggerColliders.Contains(potentialTargetCollider)) 
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
                        
                        // raise OnCollision Event???

                    }
                        
                }

            }

            return hasCollided;

        }

        /// <summary>
        /// Gets the <see cref="Physics"/> instance of the <see cref="Scene"/> this Collision Controller lives in.
        /// </summary>
        /// <returns>The <see cref="Physics"/> instance of the <see cref="Scene"/> this Collision Controller lives in.</returns>
        public Physics GetPhysics() => GameEntity?.Scene?.Physics;

        /// <summary>
        /// Updates this <see cref="CollisionController"/>. It will be moved by its <see cref="Velocity"/> and check for collisions.
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="globalTimeScale"></param>
        public void Update(GameTime gameTime, float globalTimeScale) {

            if (!IsAttached)
                return;

            if(!IsStatic && !(MathUtil.IsCloseToZero(Velocity.X) && MathUtil.IsCloseToZero(Velocity.Y))) {

                Move(Velocity);

            }

            CheckTriggerIntersections();

        }

        /// <summary>
        /// Checks whether the triggers this <see cref="CollisionController"/> is in control of intersect with any colliders in the <see cref="Scene"/>.
        /// Also fires the appropriate trigger events.
        /// </summary>
        private void CheckTriggerIntersections() {

            _physics = GetPhysics();
            IEnumerable<Collider> myActiveColliders = GetAttachedColliders().Where(c => c.IsActive);
            IEnumerable<Collider> myActiveTriggers = myActiveColliders.Where(c => c.IsTrigger);

            foreach (Collider trigger in myActiveTriggers) {

                trigger.IntersectingColliders.Clear();

                foreach (IBounds potentialTarget in _physics.CollisionTree.GetObjectsAt(trigger.Bounds)) {

                    Collider potentialTargetCollider = potentialTarget as Collider;

                    // skip detection for colliders that belong to this controller
                    if (potentialTargetCollider == null || myActiveColliders.Contains(potentialTargetCollider))
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

            if ((CollisionListener & CollisionListeners.ControllerEntity) != 0) {

                GameEntity?.OnTriggerEnter(ownCollider, otherCollider);

                // If the collider is attached to the GameEntity with the CollisionController directly, we are done now 
                // even if TriggerReceiver is set to 'both'
                if (ownCollider.GameEntity == GameEntity)
                    return;

            }

            if((CollisionListener & CollisionListeners.AttachedEntity) != 0) {

                ownCollider.GameEntity?.OnTriggerEnter(ownCollider, otherCollider);

            }

        }

        protected virtual void OnTrigger(Collider ownCollider, Collider otherCollider) {

            RaiseTrigger?.Invoke(ownCollider, otherCollider);

            if ((CollisionListener & CollisionListeners.ControllerEntity) != 0) {

                GameEntity?.OnTrigger(ownCollider, otherCollider);

                // If the collider is attached to the GameEntity with the CollisionController directly, we are done now 
                // even if TriggerReceiver is set to 'both'
                if (ownCollider.GameEntity == GameEntity)
                    return;

            }

            if ((CollisionListener & CollisionListeners.AttachedEntity) != 0) {

                ownCollider.GameEntity?.OnTrigger(ownCollider, otherCollider);

            }

        }

        protected virtual void OnTriggerLeave(Collider ownCollider, Collider otherCollider) {

            RaiseTriggerLeave?.Invoke(ownCollider, otherCollider);

            if ((CollisionListener & CollisionListeners.ControllerEntity) != 0) {

                GameEntity?.OnTriggerLeave(ownCollider, otherCollider);

                // If the collider is attached to the GameEntity with the CollisionController directly, we are done now 
                // even if TriggerReceiver is set to 'both'
                if (ownCollider.GameEntity == GameEntity)
                    return;

            }

            if ((CollisionListener & CollisionListeners.AttachedEntity) != 0) {

                ownCollider.GameEntity?.OnTriggerLeave(ownCollider, otherCollider);

            }

        }

        #endregion



    }

}
