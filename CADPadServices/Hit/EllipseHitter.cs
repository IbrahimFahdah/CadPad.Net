using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.Hit
{
    internal class EllipseHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Ellipse ellipse = entity as Ellipse;
            if (ellipse == null)
                return false;

            var inter = IntersectionUtility.IntersectEllipseRectangle(ellipse.center, ellipse.RadiusX, ellipse.RadiusY,
                new CADPoint(pkbox.reservedBounding.left, pkbox.reservedBounding.bottom),
                new CADPoint(pkbox.reservedBounding.right, pkbox.reservedBounding.top));
            return inter.Status == IntersectionUtility.Intersection.IntStatus.Intersection;

        }
    }
}