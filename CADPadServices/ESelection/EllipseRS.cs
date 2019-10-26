using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.ESelection
{
    internal class EllipseRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Ellipse ellipse = entity as Ellipse;
            if (ellipse == null)
            {
                return false;
            }
            var inter = IntersectionUtility.IntersectEllipseRectangle(ellipse.center, ellipse.RadiusX, ellipse.RadiusY,
                new CADPoint(selectBound.left, selectBound.bottom), 
                new CADPoint(selectBound.right, selectBound.top));
            return inter.Status == IntersectionUtility.Intersection.IntStatus.Intersection;
        }
    }
}
