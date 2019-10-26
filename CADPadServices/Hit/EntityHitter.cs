using CADPadDB.CADEntity;

namespace CADPadServices.Hit
{
    internal abstract class EntityHitter
    {
        internal abstract bool Hit(PickupBox pkbox, Entity entity);
    }
}
