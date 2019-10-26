using CADPadDB;
using CADPadDB.CADEntity;
using CADPadServices.Hit;


namespace CADPadServices.ESelection
{
    internal class RayRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Ray ray = entity as Ray;
            if (ray == null)
            {
                return false;
            }

            return RayHitter.BoundingIntersectWithRay(selectBound, ray);
        }
    }
}
