using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.Extensions;
using Yetibyte.Utilities;

namespace Yetibyte.Himalaya {

    /// <summary>
    /// Describes a simple two-dimensional polygon. Complex polygons are not (yet) supported. Note: This struct is immutable.
    /// </summary>
    public struct Polygon : IEquatable<Polygon>, IEdges, IBounds {

        #region Constants

        /// <summary>
        /// The minimum amount of points a <see cref="Polygon"/> must have.
        /// </summary>
        public const int MIN_NUMBER_POINTS = 3;

        #endregion

        #region Fields

        private readonly List<Vector2> _points;

        #endregion

        #region Properties

        /// <summary>
        /// An array of all the polygon's points (vertices).
        /// </summary>
        public Vector2[] Points => _points.ToArray();

        /// <summary>
        /// The number of points (vertices) in this <see cref="Polygon"/>.
        /// </summary>
        public int NumberPoints => _points.Count;

        /// <summary>
        /// The perimeter of the polygon, which is the sum of the lengths of all edges.
        /// </summary>
        public float Perimeter => GetEdges().Sum(e => e.Length);

        /// <summary>
        /// Whether or not this <see cref="Polygon"/> is convex. If this returns false, the polygon is concave.
        /// </summary>
        public bool IsConvex {

            get {

                LineSegment[] edges = GetEdges().ToArray();

                for (int i = 0; i < edges.Length; i++) {

                    Vector2 edgeA = edges[i].Delta;
                    Vector2 edgeB = i < edges.Length - 1 ? edges[i + 1].Delta : edges[0].Delta;

                    int direction = IsClockwise ? 1 : -1;

                    float innerAngle = MathHelper.WrapAngle(Vector2Helper.AngleBetween(edgeA, edgeB) * direction);

                    // if the inner angle between two edges is greater than 180 degrees, the polygon is not convex
                    if (innerAngle < 0)
                        return false;

                }

                return true;
                
            }

        }

        /// <summary>
        /// Whether or not the points of this <see cref="Polygon"/> are in clockwise order.
        /// </summary>
        public bool IsClockwise => SignedArea < 0;

        /// <summary>
        /// The surface area of this <see cref="Polygon"/> that is positive for polygons with points in counter-clockwise order
        /// and negative for polygons with points in clockwise order.
        /// </summary>
        public float SignedArea {

            get {

                float sum = 0;

                foreach (LineSegment edge in GetEdges())
                    sum += (edge.End.X - edge.Start.X) * (edge.End.Y + edge.Start.Y);

                return sum;

            }

        }

        /// <summary>
        /// The surface area of this <see cref="Polygon"/>. This always returns a positive value, regardless of whether the points are 
        /// ordered clockwise or counter-clockwise.
        /// </summary>
        public float Area => Math.Abs(SignedArea);

        /// <summary>
        /// Whether or not this <see cref="Polygon"/> is of rectangular shape. Note: In this case, it is assumed that a
        /// rectangle consists of exactly 4 points. Also, this returns true for both axis-aligned and non-axis-aligned rectangles.
        /// </summary>
        public bool IsRectangle {

            get {

                if (_points.Count != 4)
                    return false;

                LineSegment[] edges = GetEdges().ToArray();

                // We don't need to check the last angle. If 3 out of 4 angles are 90 degrees, the last one is guaranteed to be 90 degrees as well.
                for (int i = 0; i < (edges.Length - 1); i++) {

                    Vector2 edgeA = edges[i].Delta;
                    Vector2 edgeB = i < edges.Length - 1 ? edges[i + 1].Delta : edges[0].Delta;

                    int direction = IsClockwise ? 1 : -1;

                    float innerAngle = MathHelper.WrapAngle(Vector2Helper.AngleBetween(edgeA, edgeB) * direction);

                    // 90 degrees = 1.5708 radians = pi/2
                    if (!MathUtil.RoughlyEquals(innerAngle, MathHelper.PiOver2))
                        return false;

                }

                return true;
                
            }

        }

        /// <summary>
        /// Whether or not this <see cref="Polygon"/> is of triangular shape.>
        /// </summary>
        public bool IsTriangle => _points.Count == 3;

        /// <summary>
        /// The axis-aligned bounding box surrounding this <see cref="Polygon"/>.
        /// </summary>
        public RectangleF Bounds {

            get {

                float topLeftX = _points.Min(p => p.X);
                float topLeftY = _points.Min(p => p.Y);
                float bottomRightX = _points.Max(p => p.X);
                float bottomRightY = _points.Max(p => p.Y);

                float width = bottomRightX - topLeftX;
                float height = bottomRightY - topLeftY;

                return new RectangleF(topLeftX, topLeftY, width, height);

            }

        }

        #endregion

        #region Constructors

        public Polygon(IEnumerable<Vector2> points) {

            if (points.Count() < MIN_NUMBER_POINTS)
                throw new Exception("A polygon needs to have at least " + MIN_NUMBER_POINTS + " points.");

            _points = new List<Vector2>();

            foreach (Vector2 point in points) 
                _points.Add(point);
            
        }

        public Polygon(IEnumerable<Point> points) {

            if (points.Count() < MIN_NUMBER_POINTS)
                throw new Exception("A polygon needs to have at least " + MIN_NUMBER_POINTS + " points.");

            _points = new List<Vector2>();

            foreach (Point point in points)
                _points.Add(new Vector2(point.X, point.Y));

        }

        /// <summary>
        /// Creates a new <see cref="Polygon"/> from an existing <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="rectangleF">The rectangle to create the polygon from.</param>
        /// <param name="clockwise">Whether or not the points should be in clockwise order.</param>
        public Polygon(RectangleF rectangleF, bool clockwise = false) {

            _points = new List<Vector2> {

                rectangleF.Location,
                new Vector2(rectangleF.Left, rectangleF.Bottom),
                new Vector2(rectangleF.Right, rectangleF.Bottom),
                new Vector2(rectangleF.Right, rectangleF.Top)

            };

            if (clockwise)
                _points.Reverse();

        }

        public Polygon(Rectangle rectangle, bool clockwise = false) : this((RectangleF)rectangle, clockwise) { }

        #endregion

        #region Operators

        /// <summary>
        /// Checks if the two <see cref="Polygon"/>s are equal.
        /// </summary>
        /// <param name="polygonA">The first polygon.</param>
        /// <param name="polygonB">The second polygon.</param>
        /// <returns>True if the two polygons are equal, false otherwise.</returns>
        public static bool operator == (Polygon polygonA, Polygon polygonB) {

            if (polygonA.Points.Length != polygonB.Points.Length)
                return false;

            IEnumerable<Vector2> equalPoints = polygonA.Points.Union(polygonB.Points);

            if (equalPoints.Count() != polygonA.Points.Length)
                return false;

            return true;
                        
        }

        /// <summary>
        /// Checks if the two <see cref="Polygon"/>s are not equal.
        /// </summary>
        /// <param name="polygonA">The first polygon.</param>
        /// <param name="polygonB">The second polygon.</param>
        /// <returns>True if the two polygons are not equal, false if they are equal.</returns>
        public static bool operator != (Polygon polygonA, Polygon polygonB) => !(polygonA == polygonB);

        /// <summary>
        /// Converts the given<see cref="RectangleF"/> to a counter-clockwise <see cref="Polygon"/>.
        /// </summary>
        /// <param name="rectangleF"></param>
        public static explicit operator Polygon(RectangleF rectangleF) => new Polygon(rectangleF);

        /// <summary>
        /// Converts the given <see cref="Rectangle"/> to a counter-clockwise <see cref="Polygon"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        public static explicit operator Polygon(Rectangle rectangle) => new Polygon(rectangle);


        #endregion

        #region Methods

        /// <summary>
        /// Enumerates all of the <see cref="Polygon"/>'s edges.
        /// </summary>
        /// <returns>An enumeration of all of the polygon's edges.</returns>
        public IEnumerable<LineSegment> GetEdges() {

            for (int i = 0; i < _points.Count; i++)
                yield return new LineSegment(_points[i], i < _points.Count - 1 ? _points[i + 1] : _points[0]);

        }

        /// <summary>
        /// Translates the given <see cref="Polygon"/> by the given offset.
        /// </summary>
        /// <param name="polygon">The polygon to translate.</param>
        /// <param name="offset">The offset to translate the polygon by.</param>
        /// <returns>A new <see cref="Polygon"/> that equals the original polygon with the given translation applied to it.</returns>
        public static Polygon Translate(Polygon polygon, Vector2 offset) {

            Vector2[] points = new Vector2[polygon.Points.Length];
            int i = 0;

            foreach (Vector2 point in polygon.Points) {

                points[i] = point + offset;
                i++;

            }

            return new Polygon(points);

        }

        /// <summary>
        /// Translates this <see cref="Polygon"/> by the given offset.
        /// </summary>
        /// <param name="offset">The offset to translate the polygon by.</param>
        /// <returns>A new <see cref="Polygon"/> that equals this polygon with the given translation applied to it.</returns>
        public Polygon Translate(Vector2 offset) => Translate(this, offset);

        /// <summary>
        /// Constructs a new <see cref="Polygon"/> with the points of the given polygon in reverse order.
        /// </summary>
        /// <param name="polygon">The polygon to reverse.</param>
        /// <returns>A new <see cref="Polygon"/> that is equal to the given Polygon but with its points in reverse order.</returns>
        public static Polygon Reverse(Polygon polygon) => new Polygon(polygon.Points.Reverse());

        /// <summary>
        /// Creates a new <see cref="Polygon"/> with the points of this polygon in reverse order.
        /// </summary>
        /// <returns>A new polygon with the points of the original polygon in reverse order.</returns>
        public Polygon Reverse() => Reverse(this);

        /// <summary>
        /// Calculates and returns the hashcode of this <see cref="Polygon"/>.
        /// </summary>
        /// <returns>The hashcode of this polygon.</returns>
        public override int GetHashCode() {

            unchecked {

                int hash = (int)QuickPrimes.P17;
                int prime = (int)QuickPrimes.P23;

                foreach (Vector2 point in _points) {

                    hash = hash * prime + point.GetHashCode();

                }

                return hash;
            }

        }

        public override bool Equals(object obj) {

            return (obj is Polygon) && (Polygon)obj == this;

        }

        public bool Equals(Polygon other) => other == this;

        /// <summary>
        /// Creates a string that represents this <see cref="Polygon"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {

            string result = "(";

            Vector2[] points = Points;

            for (int i = 0; i < points.Length; i++) {

                result += string.Format("Point[{0}]: {1}{2}", i, points[i], (i < points.Length - 1 ? " | " : string.Empty));

            }

            return result + ")";
            
        }

        #endregion

    }

}
