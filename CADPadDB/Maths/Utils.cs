using System;

namespace CADPadDB.Maths
{
    public class Utils
    {
        public const double EPSILON = 1E-05;
        public const double PI = 3.14159265358979323846;

        public static double Clamp(double value, double minv, double maxv)
        {
            return System.Math.Max(System.Math.Min(value, maxv), minv);
        }

        public static double DegreeToRadian(double angle)
        {
            return ((angle * PI) / 180.0);
        }

        public static bool IsEqual(double x, double y, double epsilon = EPSILON)
        {
            return IsEqualZero(x - y, epsilon);
        }

        public static bool IsEqualZero(double x, double epsilon = EPSILON)
        {
            return (System.Math.Abs(x) < epsilon);
        }

        public static double RadianToDegree(double angle)
        {
            return ((angle * 180.0) / PI);
        }

        public static (
            CADPoint center,
            double startAngle,
            double endAngle,
            double radius
            ) BulgeToArc(CADPoint startPoint, CADPoint endPoint, double bulge)
        {
            var angle = 4 * Math.Atan(bulge);
            var pointDistance = Distance(startPoint, endPoint);
            var radius = (pointDistance / 2) * (bulge * bulge + 1) / (2 * bulge);

            //TODO: implement calculation via http://www.lee-mac.com/bulgeconversion.html#bulgearc
            //or here https://woutware.com/Forum/Topic/251/center-from-bulge-value?returnUrl=%2FForum%2FUserPosts%3FuserId%3D98360890
            /*
              Vector2D delta = nextVertex.Position - vertex.Position;
              double length = delta.GetLength();
              double alpha = 4 * System.Math.Atan(vertex.Bulge);
              double radius = length / (2d * System.Math.Abs(System.Math.Sin(alpha * 0.5d)));
              Vector2D lnormalized = delta;
              lnormalized.Normalize();
              double bulgeSign = System.Math.Sign(vertex.Bulge);
              Vector2D lnormal = new Vector2D(-lnormalized.Y, lnormalized.X) * bulgeSign;
              Point2D arcCenter = (Point2D)(((Vector2D)nextVertex.Position + 
                (Vector2D)vertex.Position) * 0.5d) +
                lnormal * System.Math.Cos(alpha * 0.5d) * radius;
             */
            throw new NotImplementedException();
        }

        public static double Distance(CADPoint startPoint, CADPoint endPoint)
        {
            return (new Line2(startPoint, endPoint)).length;
        }
    }
}
