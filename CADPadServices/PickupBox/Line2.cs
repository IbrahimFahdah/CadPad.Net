using CADPadDB.Maths;

namespace CADPadServices.PickupBox
{
    public struct Line2
    {
        public CADPoint startPoint;
        public CADPoint endPoint;

        public Line2(CADPoint startPnt, CADPoint endPnt)
        {
            this.startPoint = startPnt;
            this.endPoint = endPnt;
        }

        public CADPoint centerPoint
        {
            get
            {
                return new CADPoint(
                    (this.startPoint.X + this.endPoint.X) / 2.0,
                    (this.startPoint.Y + this.endPoint.Y) / 2.0);
            }
        }

        public CADVector  direction
        {
            get
            {
                CADVector vector = this.endPoint - this.startPoint;
                return vector.normalized;
            }
        }

        public double length
        {
            get
            {
                CADVector vector = this.endPoint - this.startPoint;
                return vector.Length;
            }
        }

        public override string ToString()
        {
            return string.Format(
                "Line2(({0}, {1}), ({2}, {3})",
                this.startPoint.X,
                this.startPoint.Y,
                this.endPoint.X,
                this.endPoint.Y);
        }

        /// <summary>
        /// 求线段的交点
        /// </summary>
        /// <param name="line1st">线段1</param>
        /// <param name="line2nd">线段2</param>
        /// <param name="intersection">交点</param>
        /// <returns>
        ///     true  --- 相交
        ///     false --- 不相交
        /// </returns>
        public static bool Intersect(Line2 line1st, Line2 line2nd, ref CADPoint intersection,
            double tolerance = 1e-10)
        {
            CADPoint p = line1st.startPoint;
            CADVector r = line1st.endPoint - line1st.startPoint;
            CADPoint q = line2nd.startPoint;
            CADVector s = line2nd.endPoint - line2nd.startPoint;
            double rxs = CADVector.Cross(r, s);
            if (!Utils.IsEqualZero(rxs, tolerance))
            {
                double t = CADVector.Cross(q - p, s) / rxs;
                double u = CADVector.Cross(q - p, r) / rxs;
                if (t >= (0.0 - tolerance) && t <= (1.0 + tolerance)
                                           && u >= (0.0 - tolerance) && u <= (1.0 + tolerance))
                {
                    intersection = p + t * r;
                    return true;
                }
            }
            return false;
        }
    }
}