using System.Windows;
using CADPadDB.Maths;

namespace CADPadDrawing
{
    internal static class ExtensionMethods
    {
        public static CADPoint AsCAD(this Point p)
        {
            return new CADPoint(p.X, p.Y);
        }
        public static Point AsWPF(this CADPoint p)
        {
            return new Point(p.X, p.Y);
        }
    }
}