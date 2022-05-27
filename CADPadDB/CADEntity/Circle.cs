using System.Collections.Generic;
using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{

    public class Circle : CADEntity.Entity
    {

        public override string ClassName
        {
            get { return "Circle"; }
        }


        private CADPoint _center = new CADPoint(0, 0);
        public CADPoint center
        {
            get { return _center; }
            set { _center = value; }
        }


        private double _radius = 0.0;
        public double radius
        {
            get { return _radius; }
            set { _radius = value; }
        }


        public double diameter
        {
            get { return _radius * 2; }
        }

        public override Bounding Bounding
        {
            get
            {
                return new Bounding(_center, this.diameter, this.diameter);
            }
        }


        public Circle()
        {
        }

        public Circle(CADPoint center, double radius)
        {
            _center = center;
            _radius = radius;
        }


      
        public override void Draw(IDrawingVisual gd)
        {
            if (gd == null)
                return;

            gd.Open();
            gd.SetColor(Color);
            gd.DrawCircle(_center, _radius);
            gd.Close();
        }

     

        public override object Clone()
        {
            Circle circle = base.Clone() as Circle;
            circle._center = _center;
            circle._radius = _radius;
            return circle;
        }


        protected override DBObject CreateInstance()
        {
            return new Circle();
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
            CADPoint pnt = _center + new CADPoint(_radius, 0);

            _center = transform * _center;
            _radius = (transform * pnt - _center).Length;
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
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new CADPoint(_radius, 0)));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new CADPoint(0, _radius)));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new CADPoint(-_radius, 0)));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new CADPoint(0, -_radius)));
            
            return gripPnts;
        }


        public override void SetGripPointAt(int index, GripPoint gripPoint, CADPoint newPosition)
        {
            if (index == 0)
            {
                _center = newPosition;
            }
            else if (index >= 1 && index <= 4)
            {
                _radius = (newPosition - _center).Length;
            }
        }


        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("center", _center);
            filer.Write("radius", _radius);
        }


        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("center", out _center);
            filer.Read("radius", out _radius);
        }
    }
}
