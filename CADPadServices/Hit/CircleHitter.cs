using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.Hit
{
    internal class CircleHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Circle circle = entity as Circle;
            if (circle == null)
                return false;

            return MathUtils.BoundingCross(pkbox.reservedBounding, circle);
        }
    }
}
