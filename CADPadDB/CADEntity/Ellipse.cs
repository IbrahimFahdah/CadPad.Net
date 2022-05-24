using System.Collections.Generic;
using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{
    public class Ellipse : CADEntity.Entity
    {

        public override string ClassName
        {
            get { return "Ellipse"; }
        }


        private CADPoint _center = new CADPoint(0, 0);
        public CADPoint center
        {
            get { return _center; }
            set { _center = value; }
        }


        public double RadiusX { get; set; } = 0.0;
        public double RadiusY { get; set; } = 0.0;



        public override Bounding Bounding
        {
            get
            {
                return new Bounding(_center, 2*this.RadiusX, 2 * this.RadiusY);
            }
        }


        public Ellipse()
        {
        }

        public Ellipse(CADPoint center, double radiusX, double radiusY)
        {
            _center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
        }



        public override void Draw(IDrawingVisual gd)
        {
            if (gd == null)
                return;

            gd.Open(Color);
            gd.DrawEllipse(_center, RadiusX, RadiusY);
            gd.Close();
        }



        public override object Clone()
        {
            Ellipse ellipse = base.Clone() as Ellipse;
            ellipse._center = _center;
            ellipse.RadiusX = RadiusX;
            ellipse.RadiusY = RadiusY;
            return ellipse;
        }


        protected override DBObject CreateInstance()
        {
            return new Ellipse();
        }


        public override void Translate(CADVector translation)
        {
            _center += translation;
        }

        /// <summary>
        /// Transform
        /// </summary>
        public override void TransformBy(Matrix3 transform)
        {
            CADPoint pntX = _center + new CADPoint(RadiusX, 0);
            CADPoint pntY = _center + new CADPoint(0, RadiusY);

            _center = transform * _center;
            RadiusX = (transform * pntX - _center).Length;
            RadiusY = (transform * pntY - _center).Length;
        }


        public override List<ObjectSnapPoint> GetSnapPoints()
        {
            List<ObjectSnapPoint> snapPnts = new List<ObjectSnapPoint>();
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Center, _center));

            return snapPnts;
        }

        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            gripPnts.Add(new GripPoint(GripPointType.Center, _center));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new CADPoint(RadiusX, 0)));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new CADPoint(-RadiusX, 0)));

            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new CADPoint(0, RadiusY)));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new CADPoint(0, -RadiusY)));

            return gripPnts;
        }


        public override void SetGripPointAt(int index, GripPoint gripPoint, CADPoint newPosition)
        {
            if (index == 0)
            {
                _center = newPosition;
            }
            else if (index >= 1 && index <= 2)
            {
                RadiusX = (newPosition - _center).Length;
            }
            else if (index >= 3 && index <= 4)
            {
                RadiusY = (newPosition - _center).Length;
            }
        }


        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("center", _center);
            filer.Write("RadiusX", RadiusX);
            filer.Write("RadiusY", RadiusY);
        }


        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("center", out _center);
            filer.Read("RadiusX", out double radiusX);
            filer.Read("RadiusY", out double radiusY);
            RadiusX = radiusX;
            RadiusY = radiusY;
        }
    }
}
