using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.Hit
{
    internal class XlineHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Xline xline = entity as Xline;
            if (xline == null)
                return false;

            Bounding bounding = pkbox.reservedBounding;
            return BoundingIntersectWithXline(bounding, xline);
        }

        internal static bool BoundingIntersectWithXline(Bounding bounding, Xline xline)
        {
            CADPoint pkPnt1 = new CADPoint(bounding.left, bounding.bottom);
            CADPoint pkPnt2 = new CADPoint(bounding.left, bounding.top);
            CADPoint pkPnt3 = new CADPoint(bounding.right, bounding.top);
            CADPoint pkPnt4 = new CADPoint(bounding.right, bounding.bottom);

            double d1 = CADVector.Cross(pkPnt1 - xline.basePoint, xline.direction);
            double d2 = CADVector.Cross(pkPnt2 - xline.basePoint, xline.direction);
            double d3 = CADVector.Cross(pkPnt3 - xline.basePoint, xline.direction);
            double d4 = CADVector.Cross(pkPnt4 - xline.basePoint, xline.direction);

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
