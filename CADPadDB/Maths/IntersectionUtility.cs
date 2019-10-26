using System;
using System.Collections.Generic;
using System.Text;

namespace CADPadDB.Maths
{
    /// <summary>
    /// http://www.kevlindev.com/gui/math/intersection/Intersection.js
    /// </summary>
    public class IntersectionUtility
    {

        public struct Intersection
        {
            public enum IntStatus
            {
                No_Intersection,
                Intersection,
                Outside,
                Inside
            }
            public List<CADPoint> Points;

            public IntStatus Status { get; set; }

            public Intersection(IntStatus v)
            {
                Points = new List<CADPoint>();
                Status = v;
            }

            public void appendPoints(List<CADPoint> points)
            {
                Points.AddRange(points);
            }
            public void appendPoint(CADPoint point)
            {
                Points.Add(point);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">Ellipse centre</param>
        /// <param name="rx">Ellipse X radius</param>
        /// <param name="ry">Ellipse Y radius</param>
        /// <param name="r1">first corner of  the rectangle</param>
        /// <param name="r2">opposite corner of  the rectangle</param>
        /// <returns></returns>
        public static Intersection IntersectEllipseRectangle(CADPoint c, double rx, double ry, CADPoint r1, CADPoint r2)
        {
            var min = r1.Min(r2);
            var max = r1.Max(r2);
            var topRight = new CADPoint(max.X, min.Y);
            var bottomLeft = new CADPoint(min.X, max.Y);

            var inter1 = IntersectEllipseLine(c, rx, ry, min, topRight);
            var inter2 = IntersectEllipseLine(c, rx, ry, topRight, max);
            var inter3 = IntersectEllipseLine(c, rx, ry, max, bottomLeft);
            var inter4 = IntersectEllipseLine(c, rx, ry, bottomLeft, min);

            var result = new Intersection(Intersection.IntStatus.No_Intersection);

            result.appendPoints(inter1.Points);
            result.appendPoints(inter2.Points);
            result.appendPoints(inter3.Points);
            result.appendPoints(inter4.Points);

            if (result.Points.Count > 0)
                result.Status = Intersection.IntStatus.Intersection;

            return result;
        }


        /// <summary>
        ///  NOTE: Rotation will need to be added to this function
        /// </summary>
        /// <param name="c">Ellipse centre</param>
        /// <param name="rx">Ellipse X radius</param>
        /// <param name="ry">Ellipse Y radius</param>
        /// <param name="a1">Line P1</param>
        /// <param name="a2">Line P2</param>
        /// <returns></returns>
        public static Intersection IntersectEllipseLine(CADPoint c, double rx, double ry, CADPoint a1, CADPoint a2)
        {
            Intersection result;
            var origin = new CADVector(a1.X, a1.Y);
            var dir = a2 - a1;
            var center = new CADVector(c.X, c.Y);
            var diff = origin - center;
            var mDir = new CADVector(dir.X / (rx * rx), dir.Y / (ry * ry));
            var mDiff = new CADVector(diff.X / (rx * rx), diff.Y / (ry * ry));

            var a = CADVector.Dot(dir, mDir);
            var b = CADVector.Dot(dir, mDiff);
            var cc = CADVector.Dot(diff, mDiff) - 1.0;
            var d = b * b - a * cc;

            if (d < 0)
            {
                result = new Intersection(Intersection.IntStatus.Outside);
            }
            else if (d > 0)
            {
                var root = Math.Sqrt(d);
                var t_a = (-b - root) / a;
                var t_b = (-b + root) / a;

                if ((t_a < 0 || 1 < t_a) && (t_b < 0 || 1 < t_b))
                {
                    if ((t_a < 0 && t_b < 0) || (t_a > 1 && t_b > 1))
                        result = new Intersection(Intersection.IntStatus.Outside);
                    else
                        result = new Intersection(Intersection.IntStatus.Inside);
                }
                else
                {
                    result = new Intersection(Intersection.IntStatus.Intersection);
                    if (0 <= t_a && t_a <= 1)
                        result.appendPoint(a1.lerp(a2, t_a));
                    if (0 <= t_b && t_b <= 1)
                        result.appendPoint(a1.lerp(a2, t_b));
                }
            }
            else
            {
                var t = -b / a;
                if (0 <= t && t <= 1)
                {
                    result = new Intersection(Intersection.IntStatus.Intersection);
                    result.appendPoint(a1.lerp(a2, t));
                }
                else
                {
                    result = new Intersection(Intersection.IntStatus.Outside);
                }
            }

            return result;
        }
    }
}
