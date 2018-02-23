using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Utilities {

    public struct Fraction : IEquatable<Fraction> {

        #region Properties

        public int Numerator { get; }

        public int Denominator { get; }

        public double Value => Numerator / (double)Denominator;

        #endregion

        #region Constructors

        public Fraction(int numerator, int denominator) {

            if (denominator == 0)
                throw new ArgumentException("The denominator must not be zero.", nameof(denominator));

            Numerator = numerator;
            Denominator = denominator;

        }

        public Fraction(int number) : this(number, 1) { }

        #endregion

        #region Operators

        public static bool operator ==(Fraction fractionA, Fraction fractionB) => fractionA.Equals(fractionB);

        public static bool operator !=(Fraction fractionA, Fraction fractionB) => !fractionA.Equals(fractionB);

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

        #endregion




    }

}
