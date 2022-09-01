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

        /// <summary>
        /// Bulge to Arc conversion 
        /// </summary>
        /// <remarks>
        /// inspired by<br/>
        /// <see href="http://www.lee-mac.com/bulgeconversion.html#bulgearc"/><br/>
        /// <see href="https://woutware.com/Forum/Topic/251/center-from-bulge-value?returnUrl=%2FForum%2FUserPosts%3FuserId%3D98360890"/>
        /// </remarks>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="bulge"></param>
        /// <returns></returns>
        public static (
            CADPoint center,
            double startAngle,
            double endAngle,
            double radius
            ) BulgeToArc(CADPoint startPoint, CADPoint endPoint, double bulge)
        {
            var angle = 4 * Math.Atan(bulge);
            var distance = Distance(startPoint, endPoint);
            var radius = distance / (2 * Math.Sin(angle / 2));            
            var epsilon = Math.Asin((endPoint.Y - startPoint.Y)/distance);
            var epsilonComma = Math.PI / 2 - epsilon;
            var alpha = epsilonComma - angle / 2;
            var beta = epsilonComma + angle / 2;
            var deltaY = radius * Math.Sin(alpha);
            var deltaX = radius * Math.Cos(alpha);

            CADPoint center = new CADPoint(startPoint.X - deltaX, startPoint.Y - deltaY);

            return (center, alpha, beta, radius);
        }

        public static double Distance(CADPoint startPoint, CADPoint endPoint)
        {
            return (new Line2(startPoint, endPoint)).length;
        }
    }
}
