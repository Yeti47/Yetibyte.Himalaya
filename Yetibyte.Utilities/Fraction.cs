using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Utilities {

    public struct Fraction : IEquatable<Fraction>, IComparable<Fraction>, IComparable {

        #region Constants

        /// <summary>
        /// The precision limit for converting a double into a <see cref="Fraction"/>. Decimal places
        /// lower than this number will be lost.
        /// </summary>
        public const double DOUBLE_CONVERSION_PRECISION = 0.000001;

        #endregion

        #region Properties

        /// <summary>
        /// The numerator (top number) of this <see cref="Fraction"/>.
        /// </summary>
        public int Numerator { get; }

        /// <summary>
        /// The denominator (bottom number) of this <see cref="Fraction"/>.
        /// </summary>
        public int Denominator { get; }

        /// <summary>
        /// The actual numeric value (read-only).
        /// </summary>
        public double Value => Numerator / (double)Denominator;

        /// <summary>
        /// Returns a new <see cref="Fraction"/> that has the same value as this Fraction, but is reduced
        /// to the lowest terms.
        /// </summary>
        public Fraction Reduced {

            get {

                int gcd = MathUtil.Gcd(Numerator, Denominator);
                int gcdSign = Math.Sign(gcd);
                
                return new Fraction(Numerator * gcdSign / gcd, Denominator * gcdSign / gcd);

            }

        }
        
        /// <summary>
        /// The reciprocal of this <see cref="Fraction"/>.
        /// </summary>
        public Fraction Inverse => new Fraction(Denominator, Numerator);
        
        /// <summary>
        /// Returns the fraction 1/1.
        /// </summary>
        public static Fraction Whole => new Fraction(1);

        /// <summary>
        /// Returns the fraction 1/2.
        /// </summary>
        public static Fraction Half => new Fraction(1, 2);

        /// <summary>
        /// Returns the fraction 1/3.
        /// </summary>
        public static Fraction Third => new Fraction(1, 3);

        /// <summary>
        /// Returns the fraction 1/4.
        /// </summary>
        public static Fraction Quarter => new Fraction(1, 4);

        /// <summary>
        /// Returns the fraction 1/5.
        /// </summary>
        public static Fraction Fifth => new Fraction(1, 5);

        /// <summary>
        /// Returns the fraction 0/1.
        /// </summary>
        public static Fraction Zero => new Fraction(0);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="Fraction"/> with the numerator and the denominator set to the
        /// given values respectively.
        /// </summary>
        /// <param name="numerator">The value of the fraction's numerator.</param>
        /// <param name="denominator">The value of the fraction's denominator; must not be 0 (zero), otherwise
        /// an exception is thrown.</param>
        public Fraction(int numerator, int denominator) {

            if (denominator == 0)
                throw new ArgumentException("The denominator must not be zero.", nameof(denominator));

            Numerator = numerator;
            Denominator = denominator;

        }

        /// <summary>
        /// Constructs a new <see cref="Fraction"/> with the numerator set to the given number and a denominator of 1.
        /// </summary>
        /// <param name="number">The numerator of the fraction.</param>
        public Fraction(int number) : this(number, 1) { }

        /// <summary>
        /// Creates a new <see cref="Fraction"/> by converting the given double.
        /// </summary>
        /// <param name="d">The double precision value to convert into a Fraction.</param>
        public Fraction(double d) {

            this = ConvertDouble(d);

        }

        #endregion

        #region Operators

        public static bool operator ==(Fraction fractionA, Fraction fractionB) => fractionA.Equals(fractionB);

        public static bool operator !=(Fraction fractionA, Fraction fractionB) => !fractionA.Equals(fractionB);

        public static Fraction operator *(Fraction a, Fraction b) => new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

        public static Fraction operator *(Fraction fraction, int factor) => new Fraction(fraction.Numerator * factor, fraction.Denominator);

        public static Fraction operator /(Fraction a, Fraction b) => a * b.Inverse;

        public static Fraction operator /(Fraction fraction, int divisor) => new Fraction(fraction.Numerator, fraction.Denominator * divisor);

        public static Fraction operator +(Fraction fractionA, Fraction fractionB) {
            
            CommonDenominator(ref fractionA, ref fractionB);

            return new Fraction(fractionA.Numerator + fractionB.Numerator, fractionA.Denominator);

        }

        public static double operator +(Fraction left, double right) => left.Value + right;

        public static double operator +(double left, Fraction right) => left + right.Value;

        public static double operator -(Fraction left, double right) => left.Value - right;

        public static double operator -(double left, Fraction right) => left - right.Value;

        public static Fraction operator -(Fraction fractionA, Fraction fractionB) => fractionA + (-fractionB);

        public static Fraction operator +(Fraction fraction) => fraction;

        public static Fraction operator -(Fraction fraction) => new Fraction(-fraction.Numerator, fraction.Denominator);

        public static bool operator >(Fraction fractionA, Fraction fractionB) => fractionA.Value > fractionB.Value;

        public static bool operator <(Fraction fractionA, Fraction fractionB) => fractionA.Value < fractionB.Value;

        public static bool operator >(Fraction fraction, double d) => fraction.Value > d;

        public static bool operator <(Fraction fraction, double d) => fraction.Value < d;

        /// <summary>
        /// Returns the reciprocal of this <see cref="Fraction"/>.
        /// </summary>
        /// <param name="fraction">The fraction to invert.</param>
        /// <returns>The reciprocal (inverse value) of this Fraction.</returns>
        public static Fraction operator ~(Fraction fraction) => fraction.Inverse;

        public static explicit operator Fraction(double d) => ConvertDouble(d);

        public static explicit operator Fraction(decimal d) => ConvertDouble((double)d);

        public static implicit operator Fraction(int i) => new Fraction(i);

        #endregion

        #region Methods

        public override int GetHashCode() {
            
            unchecked {

                int hash = 17;

                hash = hash * 23 + Numerator.GetHashCode();
                hash = hash * 23 + Denominator.GetHashCode();

                return hash;

            }

        }

        public bool Equals(Fraction other) => Numerator == other.Numerator && Denominator == other.Denominator;

        public override bool Equals(object obj) => obj is Fraction && this.Equals((Fraction)obj);

        /// <summary>
        /// Returns a <see cref="string"/> representation of this Fraction in the format {Numerator}/{Denominator}.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Numerator}/{Denominator}";

        /// <summary>
        /// Finds the common denominator of the two given <see cref="Fraction"/>s and adjusts their numerator and
        /// denominator accordingly.
        /// </summary>
        /// <param name="left">The first Fraction.</param>
        /// <param name="right">The second Fraction.</param>
        public static void CommonDenominator(ref Fraction left, ref Fraction right) {

            if (left.Denominator == right.Denominator)
                return;

            Fraction minDenomFraction = left;
            Fraction maxDenomFraction = right;

            bool isLeftMax = left.Denominator > right.Denominator;

            if (isLeftMax) {

                minDenomFraction = right;
                maxDenomFraction = left;

            }

            if(maxDenomFraction.Denominator % minDenomFraction.Denominator == 0 && minDenomFraction.Denominator > 1) {

                if(maxDenomFraction.Numerator % minDenomFraction.Denominator == 0) {

                    maxDenomFraction = new Fraction(maxDenomFraction.Numerator / minDenomFraction.Denominator, maxDenomFraction.Denominator / minDenomFraction.Denominator);

                    if (isLeftMax)
                        left = maxDenomFraction;
                    else
                        right = maxDenomFraction;

                }
                else {

                    int factor = maxDenomFraction.Denominator / minDenomFraction.Denominator;

                    minDenomFraction = new Fraction(minDenomFraction.Numerator * factor, minDenomFraction.Denominator * factor);

                    if (isLeftMax)
                        right = minDenomFraction;
                    else
                        left = minDenomFraction;

                }

            }
            else {

                int gcd = MathUtil.Gcd(left.Denominator, right.Denominator);
                int rightFactor = left.Denominator / gcd;
                int leftFactor = right.Denominator / gcd;

                right = new Fraction(right.Numerator * rightFactor, right.Denominator * rightFactor);
                left = new Fraction(left.Numerator * leftFactor, left.Denominator * leftFactor);
                
            }

        }

        public int CompareTo(Fraction other) => Value.CompareTo(other.Value);

        public int CompareTo(object obj) {
            
            Fraction? frac = obj as Fraction?;

            return frac != null ? Value.CompareTo(frac.Value) : 1;

        }

        /// <summary>
        /// Converts the given double value to a <see cref="Fraction"/>. 
        /// </summary>
        /// <param name="value">The double value to convert.</param>
        /// <param name="repetendThreshold">The number of identical consecutive decimal digits the double needs
        /// to be considered a repetend. Note that only repetends where all decimal places are identical can be detected.
        /// Pass 0 or a negative number if you do not wish to detect repetends but get a precise result instead.
        /// Example: Calling Fraction.ConvertDouble(0.333, 3) will return {1/3}, while Fraction.ConvertDouble(0.33, 3) 
        /// will return {33/100}.</param>
        /// <returns>The given double converted to a <see cref="Fraction"/>.</returns>
        public static Fraction ConvertDouble(double value, int repetendThreshold = 0) {

            checked {

                try {

                    int preDecimal = (int)value;
                    double decimalPlaces = Math.Abs(value - preDecimal);

                    double remainingDecimals = decimalPlaces;
                    double shifted = decimalPlaces;

                    bool isRepetend = false;
                    
                    int factor = 1;
                    int previousPredecimal = 0;
                    int numberDecimalPlaces = 0;

                    while (remainingDecimals >= DOUBLE_CONVERSION_PRECISION) {

                        factor *= 10;
                        shifted = decimalPlaces * factor;

                        int currentPredecimal = (int)shifted;

                        isRepetend = currentPredecimal != previousPredecimal && previousPredecimal != 0;

                        remainingDecimals = (shifted - currentPredecimal) / factor;

                        previousPredecimal = currentPredecimal;

                        numberDecimalPlaces++;

                    }

                    isRepetend = isRepetend && repetendThreshold > 0 && numberDecimalPlaces >= repetendThreshold;

                    Fraction fraction = new Fraction((int)shifted * Math.Sign(value), factor - (isRepetend ? 1 : 0)).Reduced;

                    return fraction + new Fraction(preDecimal);

                }
                catch(OverflowException oe) { 

                    throw new OverflowException("An overflow occurred while converting the double value to a Fraction. The number may be too large.", oe);

                }

            }
            
        }

        #endregion

    }

}
