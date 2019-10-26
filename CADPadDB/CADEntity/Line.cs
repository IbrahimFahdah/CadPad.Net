using System.Collections.Generic;
using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{

    public class Line : Entity
    {

        public override string ClassName
        {
            get { return "Line"; }
        }


        private CADPoint _startPoint = new CADPoint();
        public CADPoint startPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }


        private CADPoint _endPoint = new CADPoint();
        public CADPoint endPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }


        public override Bounding Bounding
        {
            get
            {
                return new Bounding(_startPoint, _endPoint);
            }
        }

     
        public Line()
        {
        }

        public Line(CADPoint startPnt, CADPoint endPnt)
        {
            _startPoint = startPnt;
            _endPoint = endPnt;
        }

   
        public override void Draw(IDrawingVisual gd)
        {
            if (gd == null)
                return;

            gd.Open();
            gd.DrawLine(_startPoint, _endPoint);
            gd.Close();
        }


        public override object Clone()
        {
            Line line = base.Clone() as Line;
            line._startPoint = _startPoint;
            line._endPoint = _endPoint;
            return line;
        }

        protected override DBObject CreateInstance()
        {
            return new Line();
        }


        public override void Translate(CADVector translation)
        {
            _startPoint += translation;
            _endPoint += translation;
        }


        public override void TransformBy(Matrix3 transform)
        {
            _startPoint = transform * _startPoint;
            _endPoint = transform * _endPoint;
        }


        public override List<ObjectSnapPoint> GetSnapPoints()
        {
            List<ObjectSnapPoint> snapPnts = new List<ObjectSnapPoint>();
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, _startPoint));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, _endPoint));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Mid, (_startPoint + _endPoint) / 2));

            return snapPnts;
        }


        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            gripPnts.Add(new GripPoint(GripPointType.End, _startPoint));
            gripPnts.Add(new GripPoint(GripPointType.End, _endPoint));

            return gripPnts;
        }


        public override void SetGripPointAt(int index, GripPoint gripPoint, CADPoint newPosition)
        {
            if (index == 0)
            {
                _startPoint = newPosition;
            }
            else if (index == 1)
            {
                _endPoint = newPosition;
            }
        }


        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("startPoint", _startPoint);
            filer.Write("endPoint", _endPoint);
        }


        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("startPoint", out _startPoint);
            filer.Read("endPoint", out _endPoint);
        }
    }
}
