using System.Xml;
using CADPadDB.TableRecord;

namespace CADPadDB.Table
{
    public class BlockTable : DBTable
    {

        public override string ClassName
        {
            get { return "BlockTable"; }
        }


        internal BlockTable(Database db)
            : base(db, Database.BlockTableId)
        {
        }


        public override void XmlIn(Filer.XmlFiler filer)
        {
            Filer.XmlCADDatabase filerImpl = filer as Filer.XmlCADDatabase;

            base.XmlIn(filer);

            XmlNode curXmlNode = filerImpl.curXmlNode;
            XmlNodeList blocks = curXmlNode.SelectNodes("Block");
            foreach (XmlNode blockNode in blocks)
            {
                Block block = new Block();
                block._dbtable = this;
                filerImpl.curXmlNode = blockNode;
                block.XmlIn(filerImpl);
                this._Add(block);
            }
            filerImpl.curXmlNode = curXmlNode;
        }
    }
}
