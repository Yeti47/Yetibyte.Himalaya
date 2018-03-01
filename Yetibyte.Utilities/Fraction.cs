using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Utilities {

    public struct Fraction : IEquatable<Fraction>, IComparable<Fraction>, IComparable {

        #region Constants

        public const double DOUBLE_CONVERSION_PRECISION = 0.000001;

        #endregion

        #region Properties

        public int Numerator { get; }

        public int Denominator { get; }

        public double Value => Numerator / (double)Denominator;

        public Fraction Reduced {

            get {

                int gcd = MathUtil.Gcd(Numerator, Denominator);
                int gcdSign = Math.Sign(gcd);
                
                return new Fraction(Numerator * gcdSign / gcd, Denominator * gcdSign / gcd);

            }

        }

        public Fraction Inverse => new Fraction(Denominator, Numerator);

        public static Fraction Whole => new Fraction(1);

        public static Fraction Half => new Fraction(1, 2);

        public static Fraction Third => new Fraction(1, 3);

        public static Fraction Quarter => new Fraction(1, 4);

        public static Fraction Zero => new Fraction(0);

        #endregion

        #region Constructors

        public Fraction(int numerator, int denominator) {

            if (denominator == 0)
                throw new ArgumentException("The denominator must not be zero.", nameof(denominator));

            Numerator = numerator;
            Denominator = denominator;

        }

        public Fraction(int number) : this(number, 1) { }

        public Fraction(double d) => this = ConvertDouble(d);

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

        public static Fraction operator -(Fraction fractionA, Fraction fractionB) => fractionA + (-fractionB);

        public static Fraction operator +(Fraction fraction) => fraction;

        public static Fraction operator -(Fraction fraction) => new Fraction(-fraction.Numerator, fraction.Denominator);

        public static bool operator >(Fraction fractionA, Fraction fractionB) => fractionA.Value > fractionB.Value;

        public static bool operator <(Fraction fractionA, Fraction fractionB) => fractionA.Value < fractionB.Value;

        public static bool operator >(Fraction fraction, double d) => fraction.Value > d;

        public static bool operator <(Fraction fraction, double d) => fraction.Value < d;

        public static Fraction operator ~(Fraction fraction) => fraction.Inverse;

        public static explicit operator Fraction(double d) => ConvertDouble(d);

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

        public override string ToString() => $"{Numerator}/{Denominator}";

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
        /// <param name="d">The double value to convert.</param>
        /// <param name="repetendThreshold">The number of identical consecutive digits the double needs
        /// to be considered a repetend. Note that only repetends where all decimal places are identical can be detected.
        /// Pass 0 or a negative number if you do not wish to detect repetends but get a precise result instead.
        /// Example: Calling Fraction.ConvertDouble(0.333, 3) will return {1/3}, while Fraction.ConvertDouble(0.33, 3) 
        /// will return {33/100}.</param>
        /// <returns></returns>
        public static Fraction ConvertDouble(double d, int repetendThreshold = 0) {

            checked {

                try {

                    int preDecimal = (int)d;
                    double decimalPlaces = Math.Abs(d - preDecimal);

                    double remainingDecimals = decimalPlaces;
                    double shifted = decimalPlaces;

                    int factor = 1;

                    bool isRepetend = false;

                    int previousPredecimal = 0;

                    int numberDecimalPlaces = 0;

                    while (remainingDecimals > DOUBLE_CONVERSION_PRECISION) {

                        factor *= 10;
                        shifted = decimalPlaces * factor;

                        int currentPredecimal = (int)shifted;

                        isRepetend = currentPredecimal != previousPredecimal && previousPredecimal != 0;

                        remainingDecimals = (shifted - currentPredecimal) / factor;

                        previousPredecimal = currentPredecimal;

                        numberDecimalPlaces++;

                    }

                    isRepetend = isRepetend && repetendThreshold > 0 && numberDecimalPlaces >= repetendThreshold;

                    Fraction fraction = new Fraction((int)shifted * Math.Sign(d), factor - (isRepetend ? 1 : 0)).Reduced;

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
