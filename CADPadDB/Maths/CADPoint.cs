using System;

namespace CADPadDB.Maths
{
    public partial struct CADPoint
    {
        public static CADPoint operator + (CADPoint point, CADPoint vector)
        {
            return new CADPoint(point._x + vector._x, point._y + vector._y);
        }
        public static CADPoint operator /(CADPoint a, double d)
        {
            return new CADPoint(a.X / d, a.Y / d);
        }

        public static CADPoint operator -(CADPoint a)
        {
            return new CADPoint(-a.X, -a.Y);
        }

        public static double Distance(CADPoint a, CADPoint b)
        {
            CADVector vector = b - a;
            return vector.Length;
        }

        public CADPoint lerp(CADPoint a2, double amt)
        {
            var x = (1 - amt) * X + amt * a2.X;
            var y = (1 - amt) * Y + amt * a2.Y;
            return new CADPoint(x, y);
        }

        public CADPoint Min(CADPoint point2)
        {
            return new CADPoint(Math.Min(X, point2.X), Math.Min(Y, point2.Y));
        }

        public CADPoint Max(CADPoint point2)
        {
            return new CADPoint(Math.Max(X, point2.X), Math.Max(Y, point2.Y));
        }
    }

    public partial struct CADPoint : IFormattable
    {
        internal double _x;
        internal double _y;

        /// <summary>Compares two <see cref="T:System.Windows.Point" /> structures for equality. </summary>
        /// <param name="point1">The first <see cref="T:System.Windows.Point" /> structure to compare.</param>
        /// <param name="point2">The second <see cref="T:System.Windows.Point" /> structure to compare.</param>
        /// <returns>
        /// <see langword="true" /> if both the <see cref="P:System.Windows.Point.X" /> and <see cref="P:System.Windows.Point.Y" /> coordinates of <paramref name="point1" /> and <paramref name="point2" /> are equal; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(CADPoint point1, CADPoint point2)
        {
            if (point1.X == point2.X)
                return point1.Y == point2.Y;
            return false;
        }

        /// <summary>Compares two <see cref="T:System.Windows.Point" /> structures for inequality. </summary>
        /// <param name="point1">The first point to compare.</param>
        /// <param name="point2">The second point to compare.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="point1" /> and <paramref name="point2" /> have different <see cref="P:System.Windows.Point.X" /> or <see cref="P:System.Windows.Point.Y" /> coordinates; <see langword="false" /> if <paramref name="point1" /> and <paramref name="point2" /> have the same <see cref="P:System.Windows.Point.X" /> and <see cref="P:System.Windows.Point.Y" /> coordinates.</returns>
        public static bool operator !=(CADPoint point1, CADPoint point2)
        {
            return !(point1 == point2);
        }

        /// <summary>Compares two <see cref="T:System.Windows.Point" /> structures for equality. </summary>
        /// <param name="point1">The first point to compare.</param>
        /// <param name="point2">The second point to compare.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="point1" /> and <paramref name="point2" /> contain the same <see cref="P:System.Windows.Point.X" /> and <see cref="P:System.Windows.Point.Y" /> values; otherwise, <see langword="false" />.</returns>
        public static bool Equals(CADPoint point1, CADPoint point2)
        {
            if (point1.X.Equals(point2.X))
                return point1.Y.Equals(point2.Y);
            return false;
        }

        /// <summary>Determines whether the specified <see cref="T:System.Object" /> is a <see cref="T:System.Windows.Point" /> and whether it contains the same coordinates as this <see cref="T:System.Windows.Point" />. </summary>
        /// <param name="o">The <see cref="T:System.Object" /> to compare.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Windows.Point" /> and contains the same <see cref="P:System.Windows.Point.X" /> and <see cref="P:System.Windows.Point.Y" /> values as this <see cref="T:System.Windows.Point" />; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object o)
        {
            if (o == null || !(o is CADPoint))
                return false;
            return CADPoint.Equals(this, (CADPoint)o);
        }

        /// <summary>Compares two <see cref="T:System.Windows.Point" /> structures for equality.</summary>
        /// <param name="value">The point to compare to this instance.</param>
        /// <returns>
        /// <see langword="true" /> if both <see cref="T:System.Windows.Point" /> structures contain the same <see cref="P:System.Windows.Point.X" /> and <see cref="P:System.Windows.Point.Y" /> values; otherwise, <see langword="false" />.</returns>
        public bool Equals(CADPoint value)
        {
            return CADPoint.Equals(this, value);
        }

        /// <summary>Returns the hash code for this <see cref="T:System.Windows.Point" />.</summary>
        /// <returns>The hash code for this <see cref="T:System.Windows.Point" /> structure.</returns>
        public override int GetHashCode()
        {
            double num = this.X;
            int hashCode1 = num.GetHashCode();
            num = this.Y;
            int hashCode2 = num.GetHashCode();
            return hashCode1 ^ hashCode2;
        }


        /// <summary>Gets or sets the <see cref="P:System.Windows.Point.X" />-coordinate value of this <see cref="T:System.Windows.Point" /> structure. </summary>
        /// <returns>The <see cref="P:System.Windows.Point.X" />-coordinate value of this <see cref="T:System.Windows.Point" /> structure.  The default value is 0.</returns>
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

        /// <summary>Gets or sets the <see cref="P:System.Windows.Point.Y" />-coordinate value of this <see cref="T:System.Windows.Point" />. </summary>
        /// <returns>The <see cref="P:System.Windows.Point.Y" />-coordinate value of this <see cref="T:System.Windows.Point" /> structure.  The default value is 0.</returns>
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

        /// <summary>Creates a <see cref="T:System.String" /> representation of this <see cref="T:System.Windows.Point" />. </summary>
        /// <returns>A <see cref="T:System.String" /> containing the <see cref="P:System.Windows.Point.X" /> and <see cref="P:System.Windows.Point.Y" /> values of this <see cref="T:System.Windows.Point" /> structure.</returns>
        public override string ToString()
        {
            return this.ConvertToString((string)null, (IFormatProvider)null);
        }

        /// <summary>Creates a <see cref="T:System.String" /> representation of this <see cref="T:System.Windows.Point" />. </summary>
        /// <param name="provider">Culture-specific formatting information.</param>
        /// <returns>A <see cref="T:System.String" /> containing the <see cref="P:System.Windows.Point.X" /> and <see cref="P:System.Windows.Point.Y" /> values of this <see cref="T:System.Windows.Point" /> structure.</returns>
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

        /// <summary>Creates a new <see cref="T:System.Windows.Point" /> structure that contains the specified coordinates. </summary>
        /// <param name="x">The x-coordinate of the new <see cref="T:System.Windows.Point" /> structure. </param>
        /// <param name="y">The y-coordinate of the new <see cref="T:System.Windows.Point" /> structure. </param>
        public CADPoint(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>Offsets a point's <see cref="P:System.Windows.Point.X" /> and <see cref="P:System.Windows.Point.Y" /> coordinates by the specified amounts.</summary>
        /// <param name="offsetX">The amount to offset the point's
        /// <see cref="P:System.Windows.Point.X" /> coordinate. </param>
        /// <param name="offsetY">The amount to offset thepoint's <see cref="P:System.Windows.Point.Y" /> coordinate.</param>
        public void Offset(double offsetX, double offsetY)
        {
            this._x += offsetX;
            this._y += offsetY;
        }

        /// <summary>Translates the specified <see cref="T:System.Windows.Point" /> by the specified <see cref="T:System.Windows.Vector" /> and returns the result.</summary>
        /// <param name="point">The point to translate.</param>
        /// <param name="vector">The amount by which to translate <paramref name="point" />.</param>
        /// <returns>The result of translating the specified point by the specified vector.</returns>
        public static CADPoint operator +(CADPoint point, CADVector vector)
        {
            return new CADPoint(point._x + vector._x, point._y + vector._y);
        }

        /// <summary>Adds a <see cref="T:System.Windows.Vector" /> to a <see cref="T:System.Windows.Point" /> and returns the result as a <see cref="T:System.Windows.Point" /> structure.</summary>
        /// <param name="point">The <see cref="T:System.Windows.Point" /> structure to add.</param>
        /// <param name="vector">The <see cref="T:System.Windows.Vector" /> structure to add. </param>
        /// <returns>Returns the sum of <paramref name="point" /> and <paramref name="vector" />.</returns>
        public static CADPoint Add(CADPoint point, CADVector vector)
        {
            return new CADPoint(point._x + vector._x, point._y + vector._y);
        }

        /// <summary>Subtracts the specified <see cref="T:System.Windows.Vector" /> from the specified <see cref="T:System.Windows.Point" /> and returns the resulting <see cref="T:System.Windows.Point" />.</summary>
        /// <param name="point">The point from which <paramref name="vector" /> is subtracted. </param>
        /// <param name="vector">The vector to subtract from <paramref name="point1" /></param>
        /// <returns>The difference between <paramref name="point" /> and <paramref name="vector" />.</returns>
        public static CADPoint operator -(CADPoint point, CADVector vector)
        {
            return new CADPoint(point._x - vector._x, point._y - vector._y);
        }

        /// <summary>Subtracts the specified <see cref="T:System.Windows.Vector" /> from the specified <see cref="T:System.Windows.Point" /> and returns the resulting <see cref="T:System.Windows.Point" />. </summary>
        /// <param name="point">The point from which <paramref name="vector" /> is subtracted.</param>
        /// <param name="vector">The <paramref name="vector" /> to subtract from <paramref name="point" />.</param>
        /// <returns>The difference between <paramref name="point" /> and <paramref name="vector" />.</returns>
        public static CADPoint Subtract(CADPoint point, CADVector vector)
        {
            return new CADPoint(point._x - vector._x, point._y - vector._y);
        }

        /// <summary>Subtracts the specified <see cref="T:System.Windows.Point" /> from another specified <see cref="T:System.Windows.Point" /> and returns the difference as a <see cref="T:System.Windows.Vector" />.</summary>
        /// <param name="point1">The point from which <paramref name="point2" /> is subtracted.</param>
        /// <param name="point2">The point to subtract from <paramref name="point1" />.</param>
        /// <returns>The difference between <paramref name="point1" /> and <paramref name="point2" />.</returns>
        public static CADVector operator -(CADPoint point1, CADPoint point2)
        {
            return new CADVector(point1._x - point2._x, point1._y - point2._y);
        }

        /// <summary>Subtracts the specified <see cref="T:System.Windows.Point" /> from another specified <see cref="T:System.Windows.Point" /> and returns the difference as a <see cref="T:System.Windows.Vector" />.</summary>
        /// <param name="point1">The point from which <paramref name="point2" /> is subtracted. </param>
        /// <param name="point2">The point to subtract from <paramref name="point1" />.</param>
        /// <returns>The difference between <paramref name="point1" /> and <paramref name="point2" />.</returns>
        public static CADVector Subtract(CADPoint point1, CADPoint point2)
        {
            return new CADVector(point1._x - point2._x, point1._y - point2._y);
        }


        /// <summary>Creates a <see cref="T:System.Windows.Vector" /> structure with an <see cref="P:System.Windows.Vector.X" /> value equal to the point's <see cref="P:System.Windows.Point.X" /> value and a <see cref="P:System.Windows.Vector.Y" /> value equal to the point's <see cref="P:System.Windows.Point.Y" /> value.</summary>
        /// <param name="point">The point to convert.</param>
        /// <returns>A vector with an <see cref="P:System.Windows.Vector.X" /> value equal to the point's <see cref="P:System.Windows.Point.X" /> value and a <see cref="P:System.Windows.Vector.Y" /> value equal to the point's <see cref="P:System.Windows.Point.Y" /> value.</returns>
        public static explicit operator CADVector(CADPoint point)
        {
            return new CADVector(point._x, point._y);
        }
    }
}