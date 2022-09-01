using System.Collections.Generic;
using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{
    
    public class Xline : Entity
    {

        public override string ClassName
        {
            get { return "Xline"; }
        }


        private CADPoint _basePoint = new CADPoint(0, 0);
        public CADPoint basePoint
        {
            get { return _basePoint; }
            set { _basePoint = value; }
        }


        private CADVector _direction = new CADVector(1, 0);
        public CADVector direction
        {
            get { return _direction; }
            set { _direction = value.normalized; }
        }


        public override Bounding Bounding
        {
            get
            {
                return new Bounding(
                    new CADPoint(double.MinValue, double.MinValue),
                    new CADPoint(double.MaxValue, double.MaxValue));
            }
        }


        public Xline()
        {
        }

        public Xline(CADPoint basePoint, CADVector direction)
        {
            _basePoint = basePoint;
            _direction = direction;
        }


        public override void Draw(IDrawingVisual gd)
        {
            if (gd == null)
                return;

            gd.Open();
            gd.SetColor(ColorValue);
            gd.DrawXLine(_basePoint, _direction);
            gd.Close();
        }


        public override object Clone()
        {
            Xline xline = base.Clone() as Xline;
            xline._basePoint = _basePoint;
            xline._direction = _direction;
            return xline;
        }

        protected override DBObject CreateInstance()
        {
            return new Xline();
        }


        public override void Translate(CADVector translation)
        {
            _basePoint += translation;
        }

        public override void TransformBy(Matrix3 transform)
        {
            CADPoint refPnt = _basePoint + _direction;
            _basePoint = transform * _basePoint;
            refPnt = transform * refPnt;
            _direction = (refPnt - _basePoint).normalized;
        }


        public override List<ObjectSnapPoint> GetSnapPoints()
        {
            return null;
        }


        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            gripPnts.Add(new GripPoint(GripPointType.End, _basePoint));
            gripPnts.Add(new GripPoint(GripPointType.End, _basePoint + 10 * _direction));
            gripPnts.Add(new GripPoint(GripPointType.End, _basePoint - 10 * _direction));

            return gripPnts;
        }


        public override void SetGripPointAt(int index, GripPoint gripPoint, CADPoint newPosition)
        {
            if (index == 0)
            {
                _basePoint = newPosition;
            }
            else if (index == 1 || index == 2)
            {
                CADVector dir = (newPosition - _basePoint).normalized;
                if (!dir.Equals(new CADVector(0, 0)))
                {
                    _direction = dir;
                }
            }
        }


        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("basePoint", _basePoint);
            filer.Write("direction", _direction);
        }

        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("basePoint", out _basePoint);
            filer.Read("direction", out _direction);
        }
    }
}
