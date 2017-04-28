using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Utilities;

namespace Yetibyte.Himalaya {

    public struct LineSegment : IEquatable<LineSegment>, IEdges, IBounds {

        #region Fields

        public Vector2 Start;
        public Vector2 End;

        #endregion

        #region Properties

        public float Length => Vector2.Distance(this.Start, this.End);

        public Vector2 Delta => this.End - this.Start;
        public Vector2 Direction => Vector2.Normalize(this.Delta);

        /// <summary>
        /// Gets the bounding box surrounding this <see cref="LineSegment"/>.
        /// </summary>
        public RectangleF Bounds {

            get {

                Vector2 position = Vector2.Min(this.Start, this.End);
                Vector2 size = Vector2.Max(this.Start, this.End);

                return new RectangleF(position, size);

            }

        }

        #endregion

        #region Constructors

        public LineSegment(Vector2 start, Vector2 end) {

            this.Start = start;
            this.End = end;

        }

        public LineSegment(Point start, Point end) 
            : this(new Vector2(start.X, start.Y), new Vector2(end.X, end.Y)) { }

        public LineSegment(float startX, float startY, float endX, float endY) 
            : this(new Vector2(startX, startY), new Vector2(endX, endY)) { }

        public LineSegment(RectangleF bounds) 
            : this(bounds.Location, bounds.Size) { }

        public LineSegment(Rectangle bounds)
            : this(bounds.Location, bounds.Size) { }

        #endregion

        #region Operators

        public static bool operator ==(LineSegment a, LineSegment b) {

            return MathUtil.RoughlyEquals(a.Start.X, b.Start.X) && MathUtil.RoughlyEquals(a.Start.Y, b.Start.Y)
                && MathUtil.RoughlyEquals(a.End.X, b.End.X) && MathUtil.RoughlyEquals(a.End.Y, b.End.Y);

        }

        public static bool operator !=(LineSegment a, LineSegment b) => !(a==b);

        #endregion

        #region Methods

        public override bool Equals(object obj) => (obj is LineSegment) && ((LineSegment)obj == this);

        public bool Equals(LineSegment other) => this == other;

        public override int GetHashCode() {

            unchecked {

                int hash = 17;
#pragma warning disable RECS0025 // Non-readonly field referenced in 'GetHashCode()'
                hash = hash * 23 + Start.GetHashCode();
                hash = hash * 23 + End.GetHashCode();
#pragma warning restore RECS0025 // Non-readonly field referenced in 'GetHashCode()'
                return hash;
                
            }

        }

        public static bool Intersect(Vector2 pStart, Vector2 pEnd, Vector2 qStart, Vector2 qEnd, out Vector2 intersectionPoint) {

            /* Reference:

            Ax + By = C

            A = y2 - y1
            B = x1 - x2
            C = A * x1 + B * y1

            */

            intersectionPoint = Vector2.Zero;

            float a1 = pEnd.Y - pStart.Y;
            float b1 = pStart.X - pEnd.X;
            float c1 = a1 * pStart.X + b1 * pStart.Y;

            float a2 = qEnd.Y - qStart.Y;
            float b2 = qStart.X - qEnd.X;
            float c2 = a2 * qStart.X + b2 * qStart.Y;

            float det = a1 * b2 - a2 * b1;

            // Check if lines are parallel
            if (Math.Abs(det) < MathUtil.EPSILON)
                return false;

            float intersectX = (b2 * c1 - b1 * c2) / det;
            float intersectY = (a1 * c2 - a2 * c1) / det;

            bool isIntersectXonLine1 = (Math.Min(pStart.X, pEnd.X) <= intersectX) && (intersectX <= Math.Max(pStart.X, pEnd.X));
            bool isIntersectYOnLine1 = (Math.Min(pStart.Y, pEnd.Y) <= intersectY) && (intersectY <= Math.Max(pStart.Y, pEnd.Y));
            bool isIntersectXOnLine2 = (Math.Min(qStart.X, qEnd.X) <= intersectX) && (intersectX <= Math.Max(qStart.X, qEnd.X));
            bool isIntersectYOnLine2 = (Math.Min(qStart.Y, qEnd.Y) <= intersectY) && (intersectY <= Math.Max(qStart.Y, qEnd.Y));

            bool intersects = isIntersectXonLine1 && isIntersectYOnLine1 && isIntersectXOnLine2 && isIntersectYOnLine2;

            if (intersects)
                intersectionPoint = new Vector2(intersectX, intersectY);

            return intersects;

        }

        public static bool Intersect(Vector2 pStart, Vector2 pEnd, Vector2 qStart, Vector2 qEnd) {

            return Intersect(pStart, pEnd, qStart, qEnd, out Vector2 temp);

        }

        public static bool Intersect(LineSegment lineA, LineSegment lineB, out Vector2 intersectionPoint) {

            return LineSegment.Intersect(lineA.Start, lineA.End, lineB.Start, lineB.End, out intersectionPoint);

        }

        public static bool Intersect(LineSegment lineA, LineSegment lineB) {

            return LineSegment.Intersect(lineA.Start, lineA.End, lineB.Start, lineB.End, out Vector2 temp);

        }

        public bool Intersects(LineSegment other) {

            return LineSegment.Intersect(this.Start, this.End, other.Start, other.End);

        }

        public bool Intersects(LineSegment other, out Vector2 intersectionPoint) {

            return LineSegment.Intersect(this, other, out intersectionPoint);

        }

        public IEnumerable<LineSegment> GetEdges() => new LineSegment[] { this };

        #endregion

    }

}
