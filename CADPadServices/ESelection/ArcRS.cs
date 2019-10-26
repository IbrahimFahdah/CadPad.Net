using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.ESelection
{
    internal class ArcRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Arc arc = entity as Arc;
            if (arc == null)
            {
                return false;
            }

            Bounding arcBounding = arc.Bounding;
            if (selectBound.Contains(arcBounding))
            {
                return true;
            }

            if (!selectBound.IntersectWith(arcBounding))
                return false;

            Circle circle = new Circle(arc.center, arc.radius);
            CADPoint nearestPntOnBound = new CADPoint(
                System.Math.Max(selectBound.left, System.Math.Min(circle.center.X, selectBound.right)),
                System.Math.Max(selectBound.bottom, System.Math.Min(circle.center.Y, selectBound.top)));

            if (CADPoint.Distance(nearestPntOnBound, circle.center) <= circle.radius)
            {
                double bdLeft = selectBound.left;
                double bdRight = selectBound.right;
                double bdTop = selectBound.top;
                double bdBottom = selectBound.bottom;

                List<CADPoint> pnts = new List<CADPoint>();
                pnts.Add(new CADPoint(bdLeft, bdTop));
                pnts.Add(new CADPoint(bdLeft, bdBottom));
                pnts.Add(new CADPoint(bdRight, bdTop));
                pnts.Add(new CADPoint(bdRight, bdBottom));
                CADVector xp = new CADVector(1, 0);
                foreach (CADPoint pnt in pnts)
                {
                    if (CADPoint.Distance(pnt, circle.center) >= circle.radius)
                    {
                        CADVector v = pnt - circle.center;
                        double rad = CADVector.AngleInRadian(xp, v);
                        if (CADVector.Cross(xp, v) < 0)
                            rad = System.Math.PI * 2 - rad;

                        if (AngleInRange(rad, arc.startAngle, arc.endAngle))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            else
            {
                return false;
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
    }
}
