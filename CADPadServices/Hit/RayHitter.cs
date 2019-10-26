using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.Hit
{
    internal class RayHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Ray ray = entity as Ray;
            if (ray == null)
                return false;

            Bounding bounding = pkbox.reservedBounding;
            return BoundingIntersectWithRay(bounding, ray);
        }

        internal static bool BoundingIntersectWithRay(Bounding bounding, Ray ray)
        {
            if (!ray.Bounding.IntersectWith(bounding))
            {
                return false;
            }

            var pkPnt1 = new CADPoint(bounding.left, bounding.bottom);
            var pkPnt2 = new CADPoint(bounding.left, bounding.top);
            var pkPnt3 = new CADPoint(bounding.right, bounding.top);
            var pkPnt4 = new CADPoint(bounding.right, bounding.bottom);

            double d1 = CADVector.Cross(pkPnt1 - ray.basePoint, ray.direction);
            double d2 = CADVector.Cross(pkPnt2 - ray.basePoint, ray.direction);
            double d3 = CADVector.Cross(pkPnt3 - ray.basePoint, ray.direction);
            double d4 = CADVector.Cross(pkPnt4 - ray.basePoint, ray.direction);

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
