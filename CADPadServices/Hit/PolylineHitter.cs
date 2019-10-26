using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.Hit
{
    internal class PolylineHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Polyline polyline = entity as Polyline;
            if (polyline == null)
                return false;

            Bounding pkBounding = pkbox.reservedBounding;
            for (int i = 1; i < polyline.NumberOfVertices; ++i)
            {
                Line2 line = new Line2(
                    polyline.GetPointAt(i - 1),
                    polyline.GetPointAt(i));

                if (LineHitter.BoundingIntersectWithLine(pkBounding, line))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
