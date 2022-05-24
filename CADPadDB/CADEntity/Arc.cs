using System;
using System.Collections.Generic;
using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{
    public class Arc : Entity
    {
        public override string ClassName => "Arc";


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

        /// <summary>
        /// Start angle in radian
        /// </summary>
        private double _startAngle = 0.0;
        public double startAngle
        {
            get { return _startAngle; }
            set
            {
                _startAngle = MathUtils.NormalizeRadianAngle(value);
            }
        }

        /// <summary>
        /// End angle in radian
        /// </summary>
        private double _endAngle = 0.0;
        public double endAngle
        {
            get { return _endAngle; }
            set
            {
                _endAngle = MathUtils.NormalizeRadianAngle(value);
            }
        }

        /// <summary>
        /// Start point
        /// </summary>
        public CADPoint startPoint
        {
            get
            {
                return _center + CADVector.RotateInRadian(
                     new CADVector(_radius, 0), _startAngle);
            }
        }

        /// <summary>
        /// End point
        /// </summary>
        public CADPoint endPoint
        {
            get
            {
                return _center + CADVector.RotateInRadian(
                    new CADVector(_radius, 0), _endAngle);
            }
        }


        private CADPoint middlePoint
        {
            get
            {
                double angle = 0;
                if (this.endAngle >= this.startAngle)
                {
                    angle = (this.startAngle + this.endAngle) / 2;
                }
                else
                {
                    angle = (this.startAngle + this.endAngle + Utils.PI * 2) / 2;
                }

                return _center + CADVector.RotateInRadian(
                     new CADVector(_radius, 0), angle);
            }
        }

        public override Bounding Bounding
        {
            get
            {
                List<CADPoint> pnts = new List<CADPoint>();
                pnts.Add(this.startPoint);
                pnts.Add(this.endPoint);

                for (double ang = 0; ang < System.Math.PI * 2; ang += System.Math.PI / 2)
                {
                    if (AngleInRange(ang, _startAngle, _endAngle))
                    {
                        pnts.Add(_center + CADVector.RotateInRadian(
                             new CADVector(_radius, 0), ang));
                    }
                }

                double minX = double.MaxValue;
                double minY = double.MaxValue;
                double maxX = double.MinValue;
                double maxY = double.MinValue;
                foreach (CADPoint pnt in pnts)
                {
                    minX = pnt.X < minX ? pnt.X : minX;
                    minY = pnt.Y < minY ? pnt.Y : minY;

                    maxX = pnt.X > maxX ? pnt.X : maxX;
                    maxY = pnt.Y > maxY ? pnt.Y : maxY;
                }

                return new Bounding(new CADPoint(minX, minY),
                    new CADPoint(maxX, maxY));
            }
        }

        private bool AngleInRange(double angle, double startAngle, double endAngle)
        {
            if (endAngle >= startAngle)
            {
                return angle >= startAngle
                    && angle <= endAngle;
            }
            else
            {
                return angle >= startAngle
                    || angle <= endAngle;
            }
        }

        public Arc()
        {
        }

        public Arc(CADPoint center, double radius, double startAngle, double endAngle)
        {
            _center = center;
            _radius = radius;
            this.startAngle = startAngle;
            this.endAngle = endAngle;
        }


      
        public override void Draw(IDrawingVisual gd)
        {
            if (gd == null)
                return;

            gd.Open(Color);            
            gd.DrawArc(_center, _radius, _startAngle, _endAngle);
            gd.Close();
        }


        public override object Clone()
        {
            Arc arc = base.Clone() as Arc;
            arc._center = _center;
            arc._radius = _radius;
            arc._startAngle = _startAngle;
            arc._endAngle = _endAngle;

            return arc;
        }

        protected override DBObject CreateInstance()
        {
            return new Arc();
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
            CADPoint spnt = transform * this.startPoint;
            CADPoint epnt = transform * this.endPoint;
            CADPoint mpnt = transform * this.middlePoint;

            _center = transform * _center;
            _radius = (spnt - _center).Length;

            double sAngle = CADVector.SignedAngleInRadian(
                new CADVector(1, 0),
                spnt - _center);
            double eAngle = CADVector.SignedAngleInRadian(
                new CADVector(1, 0),
                epnt - _center);

            if (CADVector.SignedAngle(spnt - _center, mpnt - _center) >= 0)
            {
                this.startAngle = sAngle;
                this.endAngle = eAngle;
            }
            else
            {
                this.startAngle = eAngle;
                this.endAngle = sAngle;
            }
        }


        public override List<ObjectSnapPoint> GetSnapPoints()
        {
            List<ObjectSnapPoint> snapPnts = new List<ObjectSnapPoint>();
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Center, _center));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, startPoint));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, endPoint));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Mid, middlePoint));

            return snapPnts;
        }


        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            //
            gripPnts.Add(new GripPoint(GripPointType.Center, _center));
            //
            GripPoint startGripPnt = new GripPoint(GripPointType.End, startPoint);
            startGripPnt.xData1 = middlePoint;
            gripPnts.Add(startGripPnt);
            //
            GripPoint endGripPnt = new GripPoint(GripPointType.End, endPoint);
            endGripPnt.xData1 = middlePoint;
            gripPnts.Add(endGripPnt);
            //
            gripPnts.Add(new GripPoint(GripPointType.Mid, middlePoint));

            return gripPnts;
        }


        public override void SetGripPointAt(int index, GripPoint gripPoint, CADPoint newPosition)
        {
            if (index == 0)
            {
                _center = newPosition;
            }
            else if (index >= 1 && index <= 3)
            {
                CADPoint startPoint = this.startPoint;
                CADPoint endPoint = this.endPoint;
                CADPoint middlePoint = this.middlePoint;

                if (index == 1)
                {
                    startPoint = newPosition;
                    middlePoint = (CADPoint)gripPoint.xData1;
                }
                else if (index == 2)
                {
                    endPoint = newPosition;
                    middlePoint = (CADPoint)gripPoint.xData1;
                }
                else if (index == 3)
                {
                    middlePoint = newPosition;
                }

                Circle2 newCircle = Circle2.From3Points(
                    startPoint, middlePoint, endPoint);
                if (newCircle.radius > 0)
                {
                    CADVector xPositive = new CADVector(1, 0);
                    double startAngle = CADVector.SignedAngleInRadian(xPositive,
                        startPoint - newCircle.center);
                    double endAngle = CADVector.SignedAngleInRadian(xPositive,
                        endPoint - newCircle.center);
                    double middleAngle = CADVector.SignedAngleInRadian(xPositive,
                        middlePoint - newCircle.center);
                    startAngle = MathUtils.NormalizeRadianAngle(startAngle);
                    endAngle = MathUtils.NormalizeRadianAngle(endAngle);
                    middleAngle = MathUtils.NormalizeRadianAngle(middleAngle);

                    _center = newCircle.center;
                    _radius = newCircle.radius;
                    if (AngleInArcRange(middleAngle, startAngle, endAngle))
                    {
                        this.startAngle = startAngle;
                        this.endAngle = endAngle;
                    }
                    else
                    {
                        this.startAngle = endAngle;
                        this.endAngle = startAngle;
                    }
                }
            }
        }

        private bool AngleInArcRange(double angle, double startAngle, double endAngle)
        {
            if (endAngle >= startAngle)
            {
                return angle >= startAngle
                    && angle <= endAngle;
            }
            else
            {
                return angle >= startAngle
                    || angle <= endAngle;
            }
        }


        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("center", _center);
            filer.Write("radius", _radius);
            filer.Write("startAngle", _startAngle);
            filer.Write("endAngle", _endAngle);
        }

        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("center", out _center);
            filer.Read("radius", out _radius);
            filer.Read("startAngle", out _startAngle);
            filer.Read("endAngle", out _endAngle);
        }
    }
}
