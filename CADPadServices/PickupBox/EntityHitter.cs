using CADPadDB.CADEntity;

namespace CADPadServices.PickupBox
{
    internal abstract class EntityHitter
    {
        internal abstract bool Hit(PickupBox pkbox, Entity entity);
    }
}
