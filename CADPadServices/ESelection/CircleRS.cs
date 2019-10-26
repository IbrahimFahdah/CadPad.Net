using CADPadDB;
using CADPadDB.CADEntity;

namespace CADPadServices.ESelection
{
    internal class CircleRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Circle circle = entity as Circle;
            if (circle == null)
            {
                return false;
            }

            return CADPadDB.Maths.MathUtils.BoundingCross(selectBound, circle);
        }
    }
}
