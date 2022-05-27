using System.Collections.Generic;
using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{
    public class Ray : Entity
    {

        public override string ClassName
        {
            get { return "Ray"; }
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
                double x = 0;
                if (_direction.X == 0)
                {
                    x = _basePoint.X;
                }
                else
                {
                    if (_direction.X > 0)
                    {
                        x = double.MaxValue;
                    }
                    else
                    {
                        x = double.MinValue;
                    }
                }

                double y = 0;
                if (_direction.Y == 0)
                {
                    y = _basePoint.Y;
                }
                else
                {
                    if (_direction.Y > 0)
                    {
                        y = double.MaxValue;
                    }
                    else
                    {
                        y = double.MinValue;
                    }
                }

                return new Bounding(_basePoint, new CADPoint(x, y));
            }
        }


        public Ray()
        {
        }

        public Ray(CADPoint basePoint, CADVector direction)
        {
            _basePoint = basePoint;
            _direction = direction.normalized;
        }

        public override void Draw(IDrawingVisual gd)
        {
            if (gd == null)
                return;

            gd.Open();
            gd.SetColor(Color);
            gd.DrawRay(_basePoint, _direction);
            gd.Close();
        }


        public override object Clone()
        {
            Ray ray = base.Clone() as Ray;
            ray._basePoint = _basePoint;
            ray._direction = _direction;
            return ray;
        }

        protected override DBObject CreateInstance()
        {
            return new Ray();
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
            List<ObjectSnapPoint> snapPnts = new List<ObjectSnapPoint>();
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, _basePoint));

            return snapPnts;
        }

        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            gripPnts.Add(new GripPoint(GripPointType.End, _basePoint));
            gripPnts.Add(new GripPoint(GripPointType.End, _basePoint + 10 * _direction));

            return gripPnts;
        }

        public override void SetGripPointAt(int index, GripPoint gripPoint, CADPoint newPosition)
        {
            if (index == 0)
            {
                _basePoint = newPosition;
            }
            else if (index == 1)
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
