using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Utilities;

namespace Yetibyte.Himalaya.Collision {

    public class CollisionController : EntityComponent {

        #region Constants

        private const float CONTINUOUS_DETECTION_MAX_STEP_SIZE = 2f;

        #endregion

        #region Properties

        public float GravityScale { get; set; }
        public bool IgnoreGravity { get; set; }
        public CollisionDetectionMethods CollisionDetectionMethod { get; set; } = CollisionDetectionMethods.Lazy;

        #endregion

        #region Methods

        public IEnumerable<Collider> GetAttachedColliders() {

            return IsAttached ? GameEntity.GetComponentsInChildren<Collider>(true, true) : new Collider[0];

        }

        public void ApplyGravity() {



        }

        public void Move(Vector2 offset) {

            if (!IsAttached)
                return;

            Physics physics = GetPhysics();
            IEnumerable<Collider> myActiveColliders = GetAttachedColliders().Where(c => c.IsActive && !c.IsTrigger);

            if (CollisionDetectionMethod == CollisionDetectionMethods.Lazy) {

                DetectCollisions(physics, myActiveColliders, ref offset);
                GameEntity.Transform.Position += offset;
            }
            else {

                Vector2 moveDirection = Vector2.Normalize(offset);

                float remainingDistance = offset.Length();
                bool hasCollided = false;

                while (remainingDistance > 0 && !hasCollided) {

                    Vector2 step = moveDirection * Math.Min(CONTINUOUS_DETECTION_MAX_STEP_SIZE, remainingDistance);
                    hasCollided = DetectCollisions(physics, myActiveColliders, ref step);
                    GameEntity.Transform.Position += step;
                    remainingDistance -= step.Length();

                }

            }

        }

        private bool DetectCollisions(Physics physics, IEnumerable<Collider> myActiveColliders, ref Vector2 offset) {

            bool hasCollided = false;

            // Check collision for all colliders attached to this Collision Controller
            foreach (Collider myCollider in myActiveColliders) {

                RectangleF futureBounds = new RectangleF(myCollider.Bounds.Location + offset, myCollider.Bounds.Size);
                RectangleF velocityBounds = RectangleF.Union(myCollider.Bounds, futureBounds);

                // perform collision detection between the current collider and all colliders in the scene it could potentially collide with
                foreach (IBounds potentialTarget in physics.CollisionTree.GetObjectsAt(velocityBounds)) {

                    Collider potentialTargetCollider = potentialTarget as Collider;

                    // skip detection for triggers and colliders that belong to this controller
                    if (potentialTargetCollider == null || potentialTargetCollider.IsTrigger || myActiveColliders.Contains(potentialTargetCollider)) 
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

                    }
                        
                }

            }

            return hasCollided;

        }

        /// <summary>
        /// Gets the <see cref="Physics"/> instance of the <see cref="Scene"/> this Collision Controller lives in.
        /// </summary>
        /// <returns>Returns the <see cref="Physics"/> instance of the <see cref="Scene"/> this Collision Controller lives in.</returns>
        private Physics GetPhysics() => GameEntity?.Scene?.Physics;

        #endregion

    }

}
