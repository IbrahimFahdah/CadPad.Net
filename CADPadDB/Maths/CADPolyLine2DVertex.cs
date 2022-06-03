using System;
using System.Collections.Generic;
using System.Text;

namespace CADPadDB.Maths
{
    public class CADPolyLine2DVertex
    {
        public CADVector Position { get; set; }
        public double Bulge { get; set; }
        public double StartWidth { get; set; }
        public double EndWidth { get; set; }

        public CADPoint Point { get => ((CADPoint)Position); set => Position = (CADVector)value; }

        public CADPolyLine2DVertex()
        {
            Position = new CADVector();
            Bulge = 0;
            StartWidth = 0;
            EndWidth = 0;
        }

        public CADPolyLine2DVertex(CADPoint point) : this()
        {
            Position = new CADVector(point.X, point.Y);
        }

        public CADPolyLine2DVertex(double x, double y) : this()
        {
            Position = new CADVector(x, y);
        }

        public CADPolyLine2DVertex(double x, double y, double bulge): this()
        {
            Position = new CADVector(x, y);
            Bulge = bulge;
        }

        public CADPolyLine2DVertex(double x, double y, double bulge, double startWidth, double endWidth) : this()
        {
            Position = new CADVector(x, y);
            Bulge = bulge;
            StartWidth = startWidth;
            EndWidth = endWidth;
        }
    }
}
