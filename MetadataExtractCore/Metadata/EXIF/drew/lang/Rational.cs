using System;

namespace com.drew.lang
{
    [Serializable]
    public class Rational
    {
        /// <summary>
        /// Holds the numerator.
        /// </summary>
        private readonly int numerator;

        /// <summary>
        /// Holds the denominator.
        /// </summary>
        private readonly int denominator;

        private int maxSimplificationCalculations = 1000;

        /// <summary>
        /// Creates a new instance of Rational. 
        /// Rational objects are immutable, so once you've set your numerator and 
        /// denominator values here, you're stuck with them! 
        /// </summary>
        /// <param name="aNumerator">a numerator</param>
        /// <param name="aDenominator"> a denominator</param>
        public Rational(int aNumerator, int aDenominator)
            : base()
        {
            this.numerator = aNumerator;
            this.denominator = aDenominator;
        }

        /// <summary>
        /// Returns the value of the specified number as a double. This may involve rounding. 
        /// </summary>
        /// <returns>the numeric value represented by this object after conversion to type double.</returns>
        public double DoubleValue()
        {
            return (double)this.numerator / (double)this.denominator;
        }

        /// <summary>
        /// Returns the value of the specified number as a float. This may involve rounding.
        /// </summary>
        /// <returns>the numeric value represented by this object after conversion to type float.</returns>
        public float FloatValue()
        {
            return (float)this.numerator / (float)this.denominator;
        }

        /// <summary>
        /// Returns the value of the specified number as a byte. 
        /// This may involve rounding or truncation.  
        /// This implementation simply casts the result of doubleValue() to byte. 
        /// </summary>
        /// <returns>the numeric value represented by this object after conversion to type byte.</returns>
        public byte ByteValue()
        {
            return (byte)this.DoubleValue();
        }

        /// <summary>
        /// Returns the value of the specified number as an int. 
        /// This may involve rounding or truncation.
        /// This implementation simply casts the result of doubleValue() to int. 
        /// </summary>
        /// <returns>the numeric value represented by this object after conversion to type int.</returns>
        public int IntValue()
        {
            return (int)this.DoubleValue();
        }

        /// <summary>
        /// Returns the value of the specified number as a long.
        /// This may involve rounding or truncation.
        /// This implementation simply casts the result of doubleValue() to long.
        /// </summary>
        /// <returns>the numeric value represented by this object after conversion to type long.</returns>
        public long LongValue()
        {
            return (long)this.DoubleValue();
        }

        /// <summary>
        /// Returns the value of the specified number as a short. 
        /// This may involve rounding or truncation.
        /// This implementation simply casts the result of doubleValue() to short. 
        /// </summary>
        /// <returns>the numeric value represented by this object after conversion to type short.</returns>
        public short ShortValue()
        {
            return (short)this.DoubleValue();
        }

        /// <summary>
        /// Returns the denominator. 
        /// </summary>
        /// <returns>the denominator.</returns>
        public int GetDenominator()
        {
            return this.denominator;
        }

        /// <summary>
        /// Returns the numerator. 
        /// </summary>
        /// <returns>the numerator.</returns>
        public int GetNumerator()
        {
            return this.numerator;
        }

        /// <summary>
        /// Returns the reciprocal value of this obejct as a new Rational. 
        /// </summary>
        /// <returns>the reciprocal in a new object</returns>
        public Rational GetReciprocal()
        {
            return new Rational(this.denominator, this.numerator);
        }

        /// <summary>
        /// Checks if this rational number is an Integer, either positive or negative. 
        /// </summary>
        /// <returns>true is Rational is an integer, false otherwize</returns>
        public bool IsInteger()
        {
            return (this.denominator == 1
                || (this.denominator != 0 && (this.numerator % this.denominator == 0))
                || (this.denominator == 0 && this.numerator == 0));
        }

        /// <summary>
        /// Returns a string representation of the object of form numerator/denominator. 
        /// </summary>
        /// <returns>a string representation of the object.</returns>
        public override String ToString()
        {
            return this.numerator + "/" + this.denominator;
        }

        /// <summary>
        /// Returns the simplest represenation of this Rational'str value possible. 
        /// </summary>
        /// <param name="isAllowDecimal">if true then decimal will be showned</param>
        /// <returns>the simplest represenation of this Rational'str value possible.</returns>
        public String ToSimpleString(bool isAllowDecimal)
        {
            if (this.denominator == 0 && this.numerator != 0)
            {
                return this.ToString();
            }
            else if (this.IsInteger())
            {
                return this.IntValue().ToString();
            }
            else if (this.numerator != 1 && this.denominator % this.numerator == 0)
            {
                // common lcFactor between denominator and numerator
                int lcNewDenominator = this.denominator / this.numerator;
                return new Rational(1, lcNewDenominator).ToSimpleString(isAllowDecimal);
            }
            else
            {
                Rational lcSimplifiedInstance = this.GetSimplifiedInstance();
                if (isAllowDecimal)
                {
                    String lcDoubleString =
                        lcSimplifiedInstance.DoubleValue().ToString();
                    if (lcDoubleString.Length < 5)
                    {
                        return lcDoubleString;
                    }
                }
                return lcSimplifiedInstance.ToString();
            }
        }


        /// <summary>
        /// Decides whether a brute-force simplification calculation should be avoided by comparing the 
        /// maximum number of possible calculations with some threshold. 
        /// </summary>
        /// <returns>true if the simplification should be performed, otherwise false</returns>
        private bool TooComplexForSimplification()
        {
            double lcMaxPossibleCalculations =
                (((double)(Math.Min(this.denominator, this.numerator) - 1) / 5d) + 2);
            return lcMaxPossibleCalculations > this.maxSimplificationCalculations;
        }

        /// <summary>
        /// Compares two Rational instances, returning true if they are mathematically equivalent. 
        /// </summary>
        /// <param name="anObject">the Rational to compare this instance to.</param>
        /// <returns>true if instances are mathematically equivalent, otherwise false. Will also return false if anObject is not an instance of Rational.</returns>
        public override bool Equals(object anObject)
        {
            if (anObject == null) return false;
            if (anObject == this) return true;
            if (anObject is Rational)
            {
                Rational that = (Rational)anObject;
                return this.DoubleValue() == that.DoubleValue();
            }
            return false;
        }

        /// <summary>
        /// Simplifies the Rational number.
        /// 
        /// Prime number series: 1, 2, 3, 5, 7, 9, 11, 13, 17
        /// 
        /// To reduce a rational, need to see if both numerator and denominator are divisible
        /// by a common lcFactor.  Using the prime number series in ascending order guarantees
        /// the minimun number of checks required.
        /// 
        /// However, generating the prime number series seems to be a hefty task.  Perhaps
        /// it'str simpler to check if both d & n are divisible by all numbers from 2 ->
        /// (Math.min(denominator, numerator) / 2).  In doing this, one can check for 2
        /// and 5 once, then ignore all even numbers, and all numbers ending in 0 or 5.
        /// This leaves four numbers from every ten to check.
        /// 
        /// Therefore, the max number of pairs of modulus divisions required will be:
        ///
        ///    4   Math.min(denominator, numerator) - 1
        ///   -- * ------------------------------------ + 2
        ///   10                    2
        ///
        ///   Math.min(denominator, numerator) - 1
        /// = ------------------------------------ + 2
        ///                  5
        /// </summary>
        /// <returns>a simplified instance, or if the Rational could not be simpliffied, returns itself (unchanged)</returns>
        public Rational GetSimplifiedInstance()
        {
            if (this.TooComplexForSimplification())
            {
                return this;
            }
            for (int lcFactor = 2;
                lcFactor <= Math.Min(this.denominator, this.numerator);
                lcFactor++)
            {
                if ((lcFactor % 2 == 0 && lcFactor > 2)
                    || (lcFactor % 5 == 0 && lcFactor > 5))
                {
                    continue;
                }
                if (this.denominator % lcFactor == 0 && this.numerator % lcFactor == 0)
                {
                    // found a common lcFactor
                    return new Rational(this.numerator / lcFactor, this.denominator / lcFactor);
                }
            }
            return this;
        }

        /// <summary>
        /// Returns the hash code of the object
        /// </summary>
        /// <returns>the hash code of the object</returns>
        public override int GetHashCode()
        {
            return this.denominator.GetHashCode() >> this.numerator.GetHashCode() * this.DoubleValue().GetHashCode();
        }
    }
}