using System;

namespace CADPadDB.Maths
{
    public partial struct CADVector
    {
       
     

        public CADVector normalized
        {
            get
            {
                double length = this.Length;
                if (length != 0.0)
                {
                    return new CADVector(this.X / length, this.Y / length);
                }
                return this;
            }
        }

        public static double Dot(CADVector a, CADVector b)
        {
            return a.X  * b.X  + a.Y  * b.Y;
        }

        public static double Cross(CADVector a, CADVector b)
        {
            return ((a.X  * b.Y ) - (a.Y  * b.X ));
        }


        /// <summary>
        /// Returns the unsigned angle in degrees between a and b.
        /// The smaller of the two possible angles between the two vectors is used.
        /// The result value range: [0, 180]
        /// </summary>
        public static double Angle(CADVector a, CADVector b)
        {
            return Utils.RadianToDegree(AngleInRadian(a, b));
        }

        /// <summary>
        /// Returns the unsigned angle in radians between a and b.
        /// The smaller of the two possible angles between the two vectors is used.
        /// The result value range: [0, PI]
        /// </summary>
        public static double AngleInRadian(CADVector a, CADVector b)
        {
            double num = a.Length * b.Length;
            if (num == 0.0)
            {
                return 0.0;
            }
            double num2 = Dot(a, b) / num;
            return System.Math.Acos(Utils.Clamp(num2, -1.0, 1.0));
        }

        /// <summary>
        /// Returns the signed acute clockwise angle in degrees between from and to.
        /// The result value range: [-180, 180]
        /// </summary>
        public static double SignedAngle(CADVector from, CADVector to)
        {
            return Utils.RadianToDegree(SignedAngleInRadian(from, to));
        }

        /// <summary>
        /// Returns the signed acute clockwise angle in radians between from and to.
        /// The result value range: [-PI, PI]
        /// </summary>
        public static double SignedAngleInRadian(CADVector from, CADVector to)
        {
            double rad = AngleInRadian(from, to);
            if (Cross(from, to) < 0)
            {
                rad = -rad;
            }
            return rad;
        }

        public static double Distance(CADVector a, CADVector b)
        {
            CADVector vector = b - a;
            return vector.Length;
        }

        public static CADVector Rotate(CADVector v, double angle)
        {
            return RotateInRadian(v, Utils.DegreeToRadian(angle));
        }

        public static CADVector Rotate(CADVector point, CADVector basePoint, double angle)
        {
            return RotateInRadian(point, basePoint, Utils.DegreeToRadian(angle));
        }

        public static CADVector RotateInRadian(CADVector v, double rad)
        {
            double x = v.X * System.Math.Cos(rad) - v.Y * System.Math.Sin(rad);
            double y = v.X * System.Math.Sin(rad) + v.Y * System.Math.Cos(rad);
            return new CADVector(x, y);
        }

        public static CADVector RotateInRadian(CADVector point, CADVector basePoint, double rad)
        {
            double cos = System.Math.Cos(rad);
            double sin = System.Math.Sin(rad);
            double x = point.X * cos - point.Y * sin + basePoint.X * (1 - cos) + basePoint.Y * sin;
            double y = point.X * sin + point.Y * cos + basePoint.Y * (1 - cos) + basePoint.X * sin;

            return new CADVector(x, y);
        }
    }
    public partial struct CADVector : IFormattable
    {
        internal double _x;
        internal double _y;

        /// <summary> Compares two vectors for equality.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>
        /// <see langword="true" /> if the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> components of <paramref name="vector1" /> and <paramref name="vector2" /> are equal; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(CADVector vector1, CADVector vector2)
        {
            if (vector1.X == vector2.X)
                return vector1.Y == vector2.Y;
            return false;
        }

        /// <summary>Compares two vectors for inequality.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>
        /// <see langword="true" /> if the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> components of <paramref name="vector1" /> and <paramref name="vector2" /> are different; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(CADVector vector1, CADVector vector2)
        {
            return !(vector1 == vector2);
        }

        /// <summary>Compares the two specified vectors for equality.</summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>
        /// <see langword="true" /> if t he <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> components of <paramref name="vector1" /> and <paramref name="vector2" /> are equal; otherwise, <see langword="false" />.</returns>
        public static bool Equals(CADVector vector1, CADVector vector2)
        {
            if (vector1.X.Equals(vector2.X))
                return vector1.Y.Equals(vector2.Y);
            return false;
        }

        /// <summary>Determines whether the specified <see cref="T:System.Object" /> is a <see cref="T:System.Windows.Vector" /> structure and, if it is, whether it has the same <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values as this vector.</summary>
        /// <param name="o">The vector to compare.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Windows.Vector" /> and has the same <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values as this vector; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object o)
        {
            if (o == null || !(o is CADVector))
                return false;
            return CADVector.Equals(this, (CADVector)o);
        }

        /// <summary> Compares two vectors for equality.</summary>
        /// <param name="value">The vector to compare with this vector.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="value" /> has the same <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values as this vector; otherwise, <see langword="false" />.</returns>
        public bool Equals(CADVector value)
        {
            return CADVector.Equals(this, value);
        }

        /// <summary>Returns the hash code for this vector. </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            double num = this.X;
            int hashCode1 = num.GetHashCode();
            num = this.Y;
            int hashCode2 = num.GetHashCode();
            return hashCode1 ^ hashCode2;
        }

        /// <summary>Gets or sets the <see cref="P:System.Windows.Vector.X" /> component of this vector. </summary>
        /// <returns>The <see cref="P:System.Windows.Vector.X" /> component of this vector. The default value is 0.</returns>
        public double X
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        /// <summary>Gets or sets the <see cref="P:System.Windows.Vector.Y" /> component of this vector. </summary>
        /// <returns>The <see cref="P:System.Windows.Vector.Y" /> component of this vector. The default value is 0.</returns>
        public double Y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }

        /// <summary>Returns the string representation of this <see cref="T:System.Windows.Vector" /> structure.</summary>
        /// <returns>A string that represents the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values of this <see cref="T:System.Windows.Vector" />.</returns>
        public override string ToString()
        {
            return this.ConvertToString((string)null, (IFormatProvider)null);
        }

        /// <summary>Returns the string representation of this <see cref="T:System.Windows.Vector" /> structure with the specified formatting information. </summary>
        /// <param name="provider">The culture-specific formatting information.</param>
        /// <returns>A string that represents the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values of this <see cref="T:System.Windows.Vector" />.</returns>
        public string ToString(IFormatProvider provider)
        {
            return this.ConvertToString((string)null, provider);
        }

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return this.ConvertToString(format, provider);
        }

        internal string ConvertToString(string format, IFormatProvider provider)
        {
            char numericListSeparator = ',';
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[3]
            {
        (object) numericListSeparator,
        (object) this._x,
        (object) this._y
            });
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Vector" /> structure. </summary>
        /// <param name="x">The <see cref="P:System.Windows.Vector.X" />-offset of the new <see cref="T:System.Windows.Vector" />.</param>
        /// <param name="y">The <see cref="P:System.Windows.Vector.Y" />-offset of the new <see cref="T:System.Windows.Vector" />.</param>
        public CADVector(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>Gets the length of this vector. </summary>
        /// <returns>The length of this vector. </returns>
        public double Length
        {
            get
            {
                return Math.Sqrt(this._x * this._x + this._y * this._y);
            }
        }

        /// <summary>Gets the square of the length of this vector. </summary>
        /// <returns>The square of the <see cref="P:System.Windows.Vector.Length" /> of this vector.</returns>
        public double LengthSquared
        {
            get
            {
                return this._x * this._x + this._y * this._y;
            }
        }

        /// <summary> Normalizes this vector. </summary>
        public void Normalize()
        {
            this = this / Math.Max(Math.Abs(this._x), Math.Abs(this._y));
            this = this / this.Length;
        }

        /// <summary>Calculates the cross product of two vectors. </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The cross product of <paramref name="vector1" /> and <paramref name="vector2" />. The following formula is used to calculate the cross product:
        ///   (Vector1.X * Vector2.Y) - (Vector1.Y * Vector2.X)
        /// </returns>
        public static double CrossProduct(CADVector vector1, CADVector vector2)
        {
            return vector1._x * vector2._y - vector1._y * vector2._x;
        }

        /// <summary>Retrieves the angle, expressed in degrees, between the two specified vectors. </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The angle, in degrees, between <paramref name="vector1" /> and <paramref name="vector2" />.</returns>
        public static double AngleBetween(CADVector vector1, CADVector vector2)
        {
            return Math.Atan2(vector1._x * vector2._y - vector2._x * vector1._y, vector1._x * vector2._x + vector1._y * vector2._y) * (180.0 / Math.PI);
        }

        /// <summary>Negates the specified vector. </summary>
        /// <param name="vector">The vector to negate.</param>
        /// <returns>A vector with <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values opposite of the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values of <paramref name="vector" />.</returns>
        public static CADVector operator -(CADVector vector)
        {
            return new CADVector(-vector._x, -vector._y);
        }

        /// <summary>Negates this vector. The vector has the same magnitude as before, but its direction is now opposite. </summary>
        public void Negate()
        {
            this._x = -this._x;
            this._y = -this._y;
        }

        /// <summary>Adds two vectors and returns the result as a vector. </summary>
        /// <param name="vector1">The first vector to add.</param>
        /// <param name="vector2">The second vector to add.</param>
        /// <returns>The sum of <paramref name="vector1" /> and <paramref name="vector2" />. </returns>
        public static CADVector operator +(CADVector vector1, CADVector vector2)
        {
            return new CADVector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        /// <summary>Adds two vectors and returns the result as a <see cref="T:System.Windows.Vector" /> structure. </summary>
        /// <param name="vector1">The first vector to add.</param>
        /// <param name="vector2">The second vector to add.</param>
        /// <returns>The sum of <paramref name="vector1" /> and <paramref name="vector2" />.</returns>
        public static CADVector Add(CADVector vector1, CADVector vector2)
        {
            return new CADVector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        /// <summary>Subtracts one specified vector from another. </summary>
        /// <param name="vector1">The vector from which <paramref name="vector2" /> is subtracted. </param>
        /// <param name="vector2">The vector to subtract from <paramref name="vector1" />.</param>
        /// <returns>The difference between <paramref name="vector1" /> and <paramref name="vector2" />. </returns>
        public static CADVector operator -(CADVector vector1, CADVector vector2)
        {
            return new CADVector(vector1._x - vector2._x, vector1._y - vector2._y);
        }

        /// <summary>Subtracts the specified vector from another specified vector. </summary>
        /// <param name="vector1">The vector from which <paramref name="vector2" /> is subtracted.</param>
        /// <param name="vector2">The vector to subtract from <paramref name="vector1" />.</param>
        /// <returns>The difference between <paramref name="vector1" /> and <paramref name="vector2" />. </returns>
        public static CADVector Subtract(CADVector vector1, CADVector vector2)
        {
            return new CADVector(vector1._x - vector2._x, vector1._y - vector2._y);
        }

        /// <summary> Translates a point by the specified vector and returns the resulting point. </summary>
        /// <param name="vector">The vector used to translate <paramref name="point" />.</param>
        /// <param name="point">The point to translate.</param>
        /// <returns>The result of translating <paramref name="point" /> by <paramref name="vector" />.</returns>
        public static CADPoint operator +(CADVector vector, CADPoint point)
        {
            return new CADPoint(point._x + vector._x, point._y + vector._y);
        }

        /// <summary>Translates the specified point by the specified vector and returns the resulting point.</summary>
        /// <param name="vector">The amount to translate the specified point.</param>
        /// <param name="point">The point to translate.</param>
        /// <returns>The result of translating <paramref name="point" /> by <paramref name="vector" />.</returns>
        public static CADPoint Add(CADVector vector, CADPoint point)
        {
            return new CADPoint(point._x + vector._x, point._y + vector._y);
        }

        /// <summary>Multiplies the specified vector by the specified scalar and returns the resulting vector. </summary>
        /// <param name="vector">The vector to multiply.</param>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <returns>The result of multiplying <paramref name="vector" /> and <paramref name="scalar" />.</returns>
        public static CADVector operator *(CADVector vector, double scalar)
        {
            return new CADVector(vector._x * scalar, vector._y * scalar);
        }

        /// <summary> Multiplies the specified vector by the specified scalar and returns the resulting <see cref="T:System.Windows.Vector" />. </summary>
        /// <param name="vector">The vector to multiply.</param>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <returns>The result of multiplying <paramref name="vector" /> and <paramref name="scalar" />.</returns>
        public static CADVector Multiply(CADVector vector, double scalar)
        {
            return new CADVector(vector._x * scalar, vector._y * scalar);
        }

        /// <summary> Multiplies the specified scalar by the specified vector and returns the resulting vector. </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>The result of multiplying <paramref name="scalar" /> and <paramref name="vector" />.</returns>
        public static CADVector operator *(double scalar, CADVector vector)
        {
            return new CADVector(vector._x * scalar, vector._y * scalar);
        }

        /// <summary> Multiplies the specified scalar by the specified vector and returns the resulting <see cref="T:System.Windows.Vector" />. </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>The result of multiplying <paramref name="scalar" /> and <paramref name="vector" />.</returns>
        public static CADVector Multiply(double scalar, CADVector vector)
        {
            return new CADVector(vector._x * scalar, vector._y * scalar);
        }

        /// <summary> Divides the specified vector by the specified scalar and returns the resulting vector.</summary>
        /// <param name="vector">The vector to divide.</param>
        /// <param name="scalar">The scalar by which <paramref name="vector" /> will be divided.</param>
        /// <returns>The result of dividing <paramref name="vector" /> by <paramref name="scalar" />.</returns>
        public static CADVector operator /(CADVector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        /// <summary>Divides the specified vector by the specified scalar and returns the result as a <see cref="T:System.Windows.Vector" />.</summary>
        /// <param name="vector">The vector structure to divide.</param>
        /// <param name="scalar">The amount by which <paramref name="vector" /> is divided.</param>
        /// <returns>The result of dividing <paramref name="vector" /> by <paramref name="scalar" />.</returns>
        public static CADVector Divide(CADVector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }


        /// <summary> Calculates the dot product of the two specified vector structures and returns the result as a <see cref="T:System.Double" />.</summary>
        /// <param name="vector1">The first vector to multiply.</param>
        /// <param name="vector2">The second vector to multiply.</param>
        /// <returns>Returns a <see cref="T:System.Double" /> containing the scalar dot product of <paramref name="vector1" /> and <paramref name="vector2" />, which is calculated using the following formula:
        ///   vector1.X * vector2.X + vector1.Y * vector2.Y
        /// </returns>
        public static double operator *(CADVector vector1, CADVector vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        /// <summary>Calculates the dot product of the two specified vectors and returns the result as a <see cref="T:System.Double" />.</summary>
        /// <param name="vector1">The first vector to multiply.</param>
        /// <param name="vector2">The second vector structure to multiply.</param>
        /// <returns>A <see cref="T:System.Double" /> containing the scalar dot product of <paramref name="vector1" /> and <paramref name="vector2" />, which is calculated using the following formula:
        ///    (vector1.X * vector2.X) + (vector1.Y * vector2.Y)
        /// </returns>
        public static double Multiply(CADVector vector1, CADVector vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        /// <summary>Calculates the determinant of two vectors.</summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The determinant of <paramref name="vector1" /> and <paramref name="vector2" />.</returns>
        public static double Determinant(CADVector vector1, CADVector vector2)
        {
            return vector1._x * vector2._y - vector1._y * vector2._x;
        }


        /// <summary>Creates a <see cref="T:System.Windows.Point" /> with the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values of this vector. </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>A point with <see cref="P:System.Windows.Point.X" />- and <see cref="P:System.Windows.Point.Y" />-coordinate values equal to the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> offset values of <paramref name="vector" />.</returns>
        public static explicit operator CADPoint(CADVector vector)
        {
            return new CADPoint(vector._x, vector._y);
        }
    }
}
