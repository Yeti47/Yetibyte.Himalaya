using System;
using System.Runtime.Serialization;
using System.Diagnostics;
using Yetibyte.Utilities;
using Yetibyte.Utilities.Extensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Yetibyte.Himalaya {

    /// <summary>
    /// Describes a 2D-rectangle. Unlike XNA's built-in <see cref="Microsoft.Xna.Framework.Rectangle"/>, this representation uses float values instead of integers.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct RectangleF : IEquatable<RectangleF>, IEdges {

        #region Private Fields
        
        private static RectangleF emptyRectangle = new RectangleF();

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of the top-left corner of this <see cref="RectangleF"/>.
        /// </summary>
        [DataMember]
        public float X;

        /// <summary>
        /// The y coordinate of the top-left corner of this <see cref="RectangleF"/>.
        /// </summary>
        [DataMember]
        public float Y;

        /// <summary>
        /// The width of this <see cref="RectangleF"/>.
        /// </summary>
        [DataMember]
        public float Width;

        /// <summary>
        /// The height of this <see cref="RectangleF"/>.
        /// </summary>
        [DataMember]
        public float Height;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a <see cref="RectangleF"/> with X=0f, Y=0f, Width=0f, Height=0f.
        /// </summary>
        public static RectangleF Empty => emptyRectangle;

        /// <summary>
        /// Returns the x coordinate of the left edge of this <see cref="RectangleF"/>.
        /// </summary>
        public float Left => this.X;

        /// <summary>
        /// Returns the x coordinate of the right edge of this <see cref="RectangleF"/>.
        /// </summary>
        public float Right => (this.X + this.Width);

        /// <summary>
        /// Returns the y coordinate of the top edge of this <see cref="RectangleF"/>.
        /// </summary>
        public float Top => this.Y;

        /// <summary>
        /// Returns the y coordinate of the bottom edge of this <see cref="RectangleF"/>.
        /// </summary>
        public float Bottom => (this.Y + this.Height);

        /// <summary>
        /// Whether or not this <see cref="RectangleF"/> has a <see cref="Width"/> and
        /// <see cref="Height"/> of 0f, and a <see cref="Location"/> of (0f, 0f).
        /// </summary>
        public bool IsEmpty {

            get => (((MathUtil.IsCloseToZero(this.Width) && MathUtil.IsCloseToZero(this.Height)) && MathUtil.IsCloseToZero(this.X)) && MathUtil.IsCloseToZero(this.Y));
            
        }

        /// <summary>
        /// The top-left coordinates of this <see cref="RectangleF"/>.
        /// </summary>
        public Vector2 Location {

            get => new Vector2(this.X, this.Y);
            
            set {

                X = value.X;
                Y = value.Y;

            }

        }

        /// <summary>
        /// The width-height coordinates of this <see cref="RectangleF"/>.
        /// </summary>
        public Vector2 Size {

            get => new Vector2(this.Width, this.Height);
            
            set {

                Width = value.X;
                Height = value.Y;

            }

        }

        /// <summary>
        /// A <see cref="Vector2"/> located in the center of this <see cref="RectangleF"/>.
        /// </summary>
        public Vector2 Center => new Vector2(this.X + (this.Width / 2), this.Y + (this.Height / 2));

        /// <summary>
        /// The surface area of this <see cref="RectangleF"/> (Width * Height).
        /// </summary>
        public float SurfaceArea => Width * Height;

        /// <summary>
        /// The length of this <see cref="RectangleF"/>'s diagonal.
        /// </summary>
        public float Diagonal => (float)Math.Sqrt(Width * Width + Height * Height);

        #endregion

        #region Internal Properties

        internal string DebugDisplayString {

            get {

                return string.Concat(
                    this.X, "  ",
                    this.Y, "  ",
                    this.Width, "  ",
                    this.Height
                    );

            }

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="RectangleF"/> struct, with the specified
        /// position, width, and height.
        /// </summary>
        /// <param name="x">The x coordinate of the top-left corner of the created <see cref="RectangleF"/>.</param>
        /// <param name="y">The y coordinate of the top-left corner of the created <see cref="RectangleF"/>.</param>
        /// <param name="width">The width of the created <see cref="RectangleF"/>.</param>
        /// <param name="height">The height of the created <see cref="RectangleF"/>.</param>
        public RectangleF(float x, float y, float width, float height) {

            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleF"/> struct, with the specified
        /// location and size.
        /// </summary>
        /// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="RectangleF"/>.</param>
        /// <param name="size">The width and height of the created <see cref="RectangleF"/>.</param>
        public RectangleF(Vector2 location, Vector2 size) {

            this.X = location.X;
            this.Y = location.Y;
            this.Width = size.X;
            this.Height = size.Y;

        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleF"/> struct using the location and size of the given <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        public RectangleF(Rectangle rectangle) : this(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height) {}

        #endregion

        #region Operators

        /// <summary>
        /// Compares whether two <see cref="RectangleF"/> instances are equal. The instances are considered equal if both their locations and their sizes are similar.
        /// </summary>
        /// <param name="a"><see cref="RectangleF"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="RectangleF"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(RectangleF a, RectangleF b) {

            return (a.X.RoughlyEquals(b.X) && a.Y.RoughlyEquals(b.Y) && a.Width.RoughlyEquals(b.Width) && a.Height.RoughlyEquals(b.Height));

        }

        /// <summary>
        /// Compares whether two <see cref="RectangleF"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="RectangleF"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="RectangleF"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(RectangleF a, RectangleF b) => !(a==b);

        /// <summary>
        /// Converts the provided <see cref="Rectangle"/> to an instance of <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="rectangleInt"></param>
        public static explicit operator RectangleF(Rectangle rectangleInt) => new RectangleF(rectangleInt);

        /// <summary>
        /// Converts the provided <see cref="RectangleF"/> to an instance of <see cref="Rectangle"/>. Float values are rounded down.
        /// </summary>
        /// <param name="rectangleInt"></param>
        public static implicit operator Rectangle(RectangleF rectangleF) => new Rectangle((int)rectangleF.X, (int)rectangleF.Y, (int)rectangleF.Width, (int)rectangleF.Height);

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(int x, int y) {

            return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));

        }

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(float x, float y) {

            return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));

        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Point value) => this.Contains(value.X, value.Y);

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Vector2 value) => this.Contains(value.X, value.Y);

        /// <summary>
        /// Gets whether or not the provided <see cref="RectangleF"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The <see cref="RectangleF"/> to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="RectangleF"/>'s bounds lie entirely inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(RectangleF value) {
            return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Rectangle"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The <see cref="Rectangle"/> to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Rectangle"/>'s bounds lie entirely inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Rectangle value) {
            return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj) {

            return (obj is RectangleF) && this == ((RectangleF)obj);

        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="other">The <see cref="RectangleF"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(RectangleF other) => this == other;

        /// <summary>
        /// Gets the hash code of this <see cref="RectangleF"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="RectangleF"/>.</returns>
        public override int GetHashCode() {

            unchecked {
                var hash = 17;

#pragma warning disable RECS0025 // Non-readonly field referenced in 'GetHashCode()'
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
#pragma warning restore RECS0025 // Non-readonly field referenced in 'GetHashCode()'

                return hash;
            }

        }

        /// <summary>
        /// Adjusts the edges of this <see cref="RectangleF"/> by specified horizontal and vertical amounts. 
        /// </summary>
        /// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
        /// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
        public void Inflate(float horizontalAmount, float verticalAmount) {

            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2f;
            Height += verticalAmount * 2f;

        }

        /// <summary>
        /// Gets whether or not the other <see cref="RectangleF"/> intersects with this rectangle.
        /// </summary>
        /// <param name="value">The other rectangle for testing.</param>
        /// <returns><c>true</c> if other <see cref="RectangleF"/> intersects with this rectangle; <c>false</c> otherwise.</returns>
        public bool Intersects(RectangleF value) {

            return value.Left < Right &&
                   Left < value.Right &&
                   value.Top < Bottom &&
                   Top < value.Bottom;

        }


        /// <summary>
        /// Gets whether or not the other <see cref="RectangleF"/> intersects with this rectangle.
        /// </summary>
        /// <param name="value">The other rectangle for testing.</param>
        /// <param name="result"><c>true</c> if other <see cref="RectangleF"/> intersects with this rectangle; <c>false</c> otherwise. As an output parameter.</param>
        public void Intersects(ref RectangleF value, out bool result) {

            result = value.Left < Right &&
                     Left < value.Right &&
                     value.Top < Bottom &&
                     Top < value.Bottom;

        }

        /// <summary>
        /// Gets whether or not the given <see cref="Rectangle"/> intersects with this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The other rectangle for testing.</param>
        /// <returns><c>true</c> if other <see cref="Rectangle"/> intersects with this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Intersects(Rectangle value) {

            return value.Left < Right &&
                   Left < value.Right &&
                   value.Top < Bottom &&
                   Top < value.Bottom;

        }

        /// <summary>
        /// Creates a new <see cref="RectangleF"/> that contains overlapping region of two other rectangles.
        /// </summary>
        /// <param name="value1">The first <see cref="RectangleF"/>.</param>
        /// <param name="value2">The second <see cref="RectangleF"/>.</param>
        /// <returns>Overlapping region of the two rectangles.</returns>
        public static RectangleF Intersect(RectangleF value1, RectangleF value2) {

            RectangleF rectangle;
            Intersect(ref value1, ref value2, out rectangle);
            return rectangle;

        }

        /// <summary>
        /// Creates a new <see cref="RectangleF"/> that contains overlapping region of two other rectangles.
        /// </summary>
        /// <param name="value1">The first <see cref="RectangleF"/>.</param>
        /// <param name="value2">The second <see cref="RectangleF"/>.</param>
        /// <param name="result">Overlapping region of the two rectangles as an output parameter.</param>
        public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result) {

            if (value1.Intersects(value2)) {

                float right_side = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                float left_side = Math.Max(value1.X, value2.X);
                float top_side = Math.Max(value1.Y, value2.Y);
                float bottom_side = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                result = new RectangleF(left_side, top_side, right_side - left_side, bottom_side - top_side);

            }
            else { result = new RectangleF(0, 0, 0, 0); }

        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="RectangleF"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="RectangleF"/>.</param>
        public void Offset(float offsetX, float offsetY) {

            X += offsetX;
            Y += offsetY;

        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="RectangleF"/>.</param>
        public void Offset(Point amount) {

            Offset(amount.X, amount.Y);

        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="RectangleF"/>.</param>
        public void Offset(Vector2 amount) {

            Offset(amount.X, amount.Y);

        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="RectangleF"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Width:[<see cref="Width"/>] Height:[<see cref="Height"/>]}
        /// </summary>
        /// <returns><see cref="String"/> representation of this <see cref="RectangleF"/>.</returns>
        public override string ToString() => "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
        

        /// <summary>
        /// Creates a new <see cref="RectangleF"/> that completely contains two other rectangles.
        /// </summary>
        /// <param name="value1">The first <see cref="RectangleF"/>.</param>
        /// <param name="value2">The second <see cref="RectangleF"/>.</param>
        /// <returns>The union of the two rectangles.</returns>
        public static RectangleF Union(RectangleF value1, RectangleF value2) {

            float x = Math.Min(value1.X, value2.X);
            float y = Math.Min(value1.Y, value2.Y);
            return new RectangleF(x, y,
                                 Math.Max(value1.Right, value2.Right) - x,
                                     Math.Max(value1.Bottom, value2.Bottom) - y);

        }

        /// <summary>
        /// Creates a new <see cref="RectangleF"/> that completely contains two other rectangles.
        /// </summary>
        /// <param name="value1">The first <see cref="RectangleF"/>.</param>
        /// <param name="value2">The second <see cref="RectangleF"/>.</param>
        /// <param name="result">The union of the two rectangles as an output parameter.</param>
        public static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result) {

            result.X = Math.Min(value1.X, value2.X);
            result.Y = Math.Min(value1.Y, value2.Y);
            result.Width = Math.Max(value1.Right, value2.Right) - result.X;
            result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;

        }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/> using the x, y, width and height components of this <see cref="RectangleF"/> rounded to the nearest integers.
        /// </summary>
        /// <returns>A new <see cref="Rectangle"/> created from the original <see cref="RectangleF"/>'s location and size components rounded to the nearest integers.</returns>
        public Rectangle ToRectangleRounded() {

            return new Rectangle(MathUtil.RoundToInt(X), MathUtil.RoundToInt(Y), MathUtil.RoundToInt(Width), MathUtil.RoundToInt(Height));

        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleF"/> where the width and height are the same size.
        /// </summary>
        /// <param name="x">The x coordinate of the top-left corner of the square.</param>
        /// <param name="y">The y coordinate of the top-left corner of the square.</param>
        /// <param name="size">The value used for both the widht and the height of the RectangleF.</param>
        /// <returns>A new RectangleF with equally sized dimensions.</returns>
        public static RectangleF CreateSquare(float x, float y, float size) => new RectangleF(x, y, size, size);

        /// <summary>
        /// Creates a new instance of <see cref="RectangleF"/> where the width and height are the same size.
        /// </summary>
        /// <param name="location">The coordinates of the top-left corner of the square.</param>
        /// <param name="size">The value used for both the widht and the height of the RectangleF.</param>
        /// <returns>A new RectangleF with equally sized dimensions.</returns>
        public static RectangleF CreateSquare(Vector2 location, float size) => RectangleF.CreateSquare(location.X, location.Y, size);

        /// <summary>
        /// Enumerates all edges of this <see cref="RectangleF"/> as <see cref="LineSegment"/>s. 
        /// </summary>
        /// <returns>An enumeration of all edges of this <see cref="RectangleF"/> as <see cref="LineSegment"/>s.</returns>
        public IEnumerable<LineSegment> GetEdges() { 

            Vector2[] points = {

                this.Location,
                new Vector2(this.Right, this.Top),
                new Vector2(this.Right, this.Bottom),
                new Vector2(this.Left, this.Bottom)

                };

            for (int i = 0; i < points.Length; i++)
                yield return new LineSegment(points[i], i < points.Length - 1 ? points[i + 1] : points[0]);

        }

        #endregion
    }
}