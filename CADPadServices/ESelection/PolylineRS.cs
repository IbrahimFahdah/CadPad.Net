using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.ESelection
{
    internal class PolylineRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Polyline polyline = entity as Polyline;
            if (polyline == null)
            {
                return false;
            }

            Bounding polylineBound = polyline.Bounding;
            if (selectBound.Contains(polylineBound))
            {
                return true;
            }

            Rectangle2 selRect = new Rectangle2(
                new CADPoint(selectBound.left, selectBound.bottom),
                new CADPoint(selectBound.right, selectBound.top));

            Line2 rectLine1 = new Line2(selRect.leftBottom, selRect.leftTop);
            Line2 rectLine2 = new Line2(selRect.leftTop, selRect.rightTop);
            Line2 rectLine3 = new Line2(selRect.rightTop, selRect.rightBottom);
            Line2 rectLine4 = new Line2(selRect.rightBottom, selRect.leftBottom);

            for (int i = 1; i < polyline.NumberOfVertices; ++i)
            {
                CADPoint spnt = polyline.GetPointAt(i - 1);
                CADPoint epnt = polyline.GetPointAt(i);
                Line2 line2 = new Line2(spnt, epnt);
                CADPoint intersection = new CADPoint();
                if (Line2.Intersect(rectLine1, line2, ref intersection)
                    || Line2.Intersect(rectLine2, line2, ref intersection)
                    || Line2.Intersect(rectLine3, line2, ref intersection)
                    || Line2.Intersect(rectLine4, line2, ref intersection))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
