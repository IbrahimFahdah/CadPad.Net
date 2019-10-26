using CADPadDB;
using CADPadDB.CADEntity;
using CADPadServices.Hit;

namespace CADPadServices.ESelection
{
    internal class XlineRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Xline xline = entity as Xline;
            if (xline == null)
            {
                return false;
            }

            return XlineHitter.BoundingIntersectWithXline(selectBound, xline);
        }
    }
}
