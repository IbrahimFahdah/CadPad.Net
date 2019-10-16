using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.PickupBox
{
    internal class LineHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Line line = entity as Line;
            if (line == null)
                return false;

            Bounding pkBounding = pkbox.reservedBounding;
            return LineHitter.BoundingIntersectWithLine(
                pkBounding,
                new Line2(line.startPoint, line.endPoint));
        }

        internal static bool BoundingIntersectWithLine(Bounding bounding, Line2 line)
        {
            Bounding lineBound = new Bounding(line.startPoint, line.endPoint);
            if (!bounding.IntersectWith(lineBound))
            {
                return false;
            }

            if (bounding.Contains(line.startPoint)
                || bounding.Contains(line.endPoint))
            {
                return true;
            }

            CADPoint pkPnt1 = new CADPoint(bounding.left, bounding.bottom);
            CADPoint pkPnt2 = new CADPoint(bounding.left, bounding.top);
            CADPoint pkPnt3 = new CADPoint(bounding.right, bounding.top);
            CADPoint pkPnt4 = new CADPoint(bounding.right, bounding.bottom);

            double d1 = CADVector.Cross(line.startPoint - pkPnt1, line.endPoint - pkPnt1);
            double d2 = CADVector.Cross(line.startPoint - pkPnt2, line.endPoint - pkPnt2);
            double d3 = CADVector.Cross(line.startPoint - pkPnt3, line.endPoint - pkPnt3);
            double d4 = CADVector.Cross(line.startPoint - pkPnt4, line.endPoint - pkPnt4);

            if (d1 * d2 <= 0 || d1 * d3 <= 0 || d1 * d4 <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
