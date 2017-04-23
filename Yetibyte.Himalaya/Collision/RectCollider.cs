using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Utilities;

namespace Yetibyte.Himalaya.Collision {

    public sealed class RectCollider : Collider {

        //TODO: Consider including Transform.Scale into calculation

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

        public override RectangleF Bounds => new RectangleF(Position.X - Width / 2, Position.Y - Height / 2, Width, Height);


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

                if (penetrationRect.Width < penetrationRect.Height) {

                    penetration = new Vector2(penetrationRect.Width * MathUtil.Sign1(otherRectCollider.Bounds.Center.X - penetrationRect.Center.X), 0);

                }
                else {

                    penetration = new Vector2(0, penetrationRect.Height * MathUtil.Sign1(otherRectCollider.Bounds.Center.Y - penetrationRect.Center.Y));

                }
                
            }
            
            return new CollisionInfo(intersect, otherCollider, penetration);

        }

        #endregion

    }

}
