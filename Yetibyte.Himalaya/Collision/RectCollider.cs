using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Utilities;

namespace Yetibyte.Himalaya.Collision {

    public sealed class RectCollider : Collider, IEdges {

        #region Properties

        public float Width { get; set; }
        public float Height { get; set; }

        public Vector2 Size {

            get => new Vector2(this.Width, this.Height);

            set {

                this.Width = value.X;
                this.Height = value.Y;

            }

        }

        public float ScaledWidth => this.Width * this.Scale.X;
        public float ScaledHeight => this.Height * this.Scale.Y;

        public Vector2 ScaledSize => new Vector2(this.ScaledWidth, this.ScaledHeight);

        // TODO: Should the scale be factored in or not?
        public override RectangleF Bounds => new RectangleF(Position.X - ScaledWidth / 2, Position.Y - ScaledHeight / 2, ScaledWidth, ScaledHeight);

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether this <see cref="RectCollider"/> and the given RectCollider overlap. For an intersection to be registered, both Colliders
        /// must be on the same layer(s).
        /// </summary>
        /// <param name="otherRectCollider">The other instance of <see cref="RectCollider"/></param>
        /// <returns>True if the bounding boxes of both RectColliders intersect, false otherwise. Always false if
        /// the two RectColliders are not on the same layer(s).</returns>
        public override bool Intersects(RectCollider otherRectCollider) {

            if (!SharesLayerWith(otherRectCollider))
                return false;

            return Bounds.Intersects(otherRectCollider.Bounds);

        }

        /// <summary>
        /// Checks whether this <see cref="Collider"/> will intersect with the given <see cref="RectCollider"/> if it moves
        /// with the given velocity. 
        /// </summary>
        /// <param name="velocity">The velocity of this Collider.</param>
        /// <param name="otherRectCollider">The other Collider to check intersection with.</param>
        /// <returns>An instance of <see cref="CollisionInfo"/> which contains information on whether or not
        /// an intersection occured and a penetration vector that can be used to push the colliders apart.</returns>
        public override CollisionInfo WillIntersect(Vector2 velocity, RectCollider otherRectCollider) {

            bool intersect = false;
            Collider otherCollider = null;
            Vector2 penetration = Vector2.Zero;

            RectangleF futureBounds = new RectangleF(Bounds.Location + velocity, Bounds.Size);

            if (SharesLayerWith(otherRectCollider) && futureBounds.Intersects(otherRectCollider.Bounds)) {

                intersect = true;
                otherCollider = otherRectCollider;

                RectangleF velocityBounds = RectangleF.Union(Bounds, futureBounds);
                RectangleF penetrationRect = RectangleF.Intersect(velocityBounds, otherRectCollider.Bounds);

                if (Math.Abs(velocity.X) > Math.Abs(velocity.Y)) {

                    RectangleF futureBoundsX = new RectangleF(Bounds.Location + new Vector2(velocity.X, 0), Bounds.Size);
                    RectangleF velocityBoundsX = RectangleF.Union(Bounds, futureBoundsX);
                    RectangleF penetrationRectX = RectangleF.Intersect(velocityBoundsX, otherRectCollider.Bounds);

                    if(!penetrationRectX.IsEmpty)
                        penetration = new Vector2((penetrationRectX.Width + MathUtil.EPSILON) * MathUtil.Sign1(velocity.X), 0);
                    else
                        penetration = new Vector2(0, (penetrationRect.Height + MathUtil.EPSILON) * MathUtil.Sign1(velocity.Y));

                }
                else {

                    RectangleF futureBoundsY = new RectangleF(Bounds.Location + new Vector2(0, velocity.Y), Bounds.Size);
                    RectangleF velocityBoundsY = RectangleF.Union(Bounds, futureBoundsY);
                    RectangleF penetrationRectY = RectangleF.Intersect(velocityBoundsY, otherRectCollider.Bounds);

                    if (!penetrationRectY.IsEmpty)
                        penetration = new Vector2(0, (penetrationRectY.Height + MathUtil.EPSILON) * MathUtil.Sign1(velocity.Y));
                    else
                        penetration = new Vector2((penetrationRect.Width + MathUtil.EPSILON) * MathUtil.Sign1(velocity.X), 0);

                }
                                
            }
            
            return new CollisionInfo(intersect, otherCollider, penetration);

        }

        public IEnumerable<LineSegment> GetEdges() => Bounds.GetEdges();

        #endregion

    }

}
