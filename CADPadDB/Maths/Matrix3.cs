namespace CADPadDB.Maths
{
    public struct Matrix3
    {
        public double m11;
        public double m12;
        public double m13;
        public double m21;
        public double m22;
        public double m23;
        public double m31;
        public double m32;
        public double m33;

        public Matrix3(
            double m11 = 0.0, double m12 = 0.0, double m13 = 0.0,
            double m21 = 0.0, double m22 = 0.0, double m23 = 0.0,
            double m31 = 0.0, double m32 = 0.0, double m33 = 0.0)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public void Set(
            double m11 = 0.0, double m12 = 0.0, double m13 = 0.0,
            double m21 = 0.0, double m22 = 0.0, double m23 = 0.0,
            double m31 = 0.0, double m32 = 0.0, double m33 = 0.0)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public override string ToString()
        {
            string pattern = @"Matrix3(
{0,10:0.00}, {1,10:0.00}, {2,10:0.00},
{3,10:0.00}, {4,10:0.00}, {5,10:0.00},
{6,10:0.00}, {7,10:0.00}, {8,10:0.00}  )";
            return string.Format(pattern,
                m11, m12, m13,
                m21, m22, m23,
                m31, m32, m33);
        }

        public double determinant
        {
            get
            {
                return m11 * m22 * m33 +
                       m12 * m23 * m31 +
                       m13 * m21 * m32 -
                       m11 * m23 * m32 -
                       m12 * m21 * m33 -
                       m13 * m22 * m31;
            }
        }

        public Matrix3 inverse
        {
            get
            {
                double d = this.determinant;
                if (d == 0.0)
                    return Matrix3.identity;
        
                Matrix3 tmp = new Matrix3();
                tmp.m11 = (m22 * m33 - m23 * m32) / d;
                tmp.m12 = (m13 * m32 - m12 * m33) / d;
                tmp.m13 = (m12 * m23 - m13 * m22) / d;
                tmp.m21 = (m23 * m31 - m21 * m33) / d;
                tmp.m22 = (m11 * m33 - m13 * m31) / d;
                tmp.m23 = (m13 * m21 - m11 * m23) / d;
                tmp.m31 = (m21 * m32 - m22 * m31) / d;
                tmp.m32 = (m12 * m31 - m11 * m32) / d;
                tmp.m33 = (m11 * m22 - m12 * m21) / d;
                return tmp;
            }
        }

        public Matrix3 transpose
        {
            get
            {
                return new Matrix3(
                    m11, m21, m31,
                    m12, m22, m32,
                    m13, m23, m33);
            }
        }

        public CADVector MultiplyPoint(CADVector v)
        {
            return new CADVector(
                m11 * v.X + m12 * v.Y + m13,
                m21 * v.X + m22 * v.Y + m23);
        }

        public CADVector MultiplyVector(CADVector v)
        {
            return new CADVector(
                m11 * v.X + m12 * v.Y,
                m21 * v.X + m22 * v.Y);
        }

        public static Matrix3 identity
        {
            get
            {
                return new Matrix3(
                    1.0, 0.0, 0.0,
                    0.0, 1.0, 0.0,
                    0.0, 0.0, 1.0);
            }
        }

        public static Matrix3 zero
        {
            get
            {
                return new Matrix3(
                    0.0, 0.0, 0.0,
                    0.0, 0.0, 0.0,
                    0.0, 0.0, 0.0);
            }
        }

        public static Matrix3 Translate(CADVector v)
        {
            return new Matrix3(
                1.0, 0.0, v.X,
                0.0, 1.0, v.Y,
                0.0, 0.0, 1.0);
        }

        public static Matrix3 Translate(CADPoint  v)
        {
            return new Matrix3(
                1.0, 0.0, v.X,
                0.0, 1.0, v.Y,
                0.0, 0.0, 1.0);
        }

        public static Matrix3 Rotate(double angle)
        {
            return RotateInRadian(Utils.DegreeToRadian(angle));
        }

        public static Matrix3 RotateInRadian(double angle)
        {
            double cos = System.Math.Cos(angle);
            double sin = System.Math.Sin(angle);
            return new Matrix3(
                cos, -sin, 0.0,
                sin,  cos, 0.0,
                0.0,  0.0, 1.0);
        }

        public static Matrix3 Scale(CADVector v)
        {
            return new Matrix3(
                v.X, 0.0, 0.0,
                0.0, v.Y, 0.0,
                0.0, 0.0, 1.0);
        }

        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            Matrix3 M = new Matrix3();
            M.m11 = a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31;
            M.m12 = a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32;
            M.m13 = a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33;
            M.m21 = a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31;
            M.m22 = a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32;
            M.m23 = a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33;
            M.m31 = a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31;
            M.m32 = a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32;
            M.m33 = a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33;
            return M;
        }

        public static CADVector operator *(Matrix3 m, CADVector v)
        {
            return new CADVector(
                m.m11 * v.X + m.m12 * v.Y + m.m13,
                m.m21 * v.X + m.m22 * v.Y + m.m23);
        }

        public static CADPoint operator *(Matrix3 m, CADPoint v)
        {
            return new CADPoint(
                m.m11 * v.X + m.m12 * v.Y + m.m13,
                m.m21 * v.X + m.m22 * v.Y + m.m23);
        }
    }
}
