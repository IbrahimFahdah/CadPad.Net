using CADPadDB.Maths;

namespace CADPadDB.CADEntity
{

    public class BlockReference : Entity
    {

        public override string ClassName
        {
            get { return "BlockReference"; }
        }


        public override Bounding Bounding
        {
            get
            {
                return new Bounding();
            }
        }


        public override object Clone()
        {
            BlockReference blkRef = base.Clone() as BlockReference;
            return blkRef;
        }

        protected override DBObject CreateInstance()
        {
            return new BlockReference();
        }


        public override void Translate(CADVector translation)
        {
        }


        public override void TransformBy(Matrix3 transform)
        {
        }
    }
}
