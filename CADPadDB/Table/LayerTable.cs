using System.Collections.Generic;
using System.Linq;
using System.Xml;
using CADPadDB.TableRecord;

namespace CADPadDB.Table
{
    public class LayerTable : DBTable
    {

        public override string ClassName
        {
            get { return "LayerTable"; }
        }

        /// <summary>
        /// 
        /// </summary>
        private ObjectId _layerZeroId = ObjectId.Null;
        public ObjectId layerZeroId
        {
            get { return _layerZeroId; }
        }


        internal LayerTable(Database db)
            : base(db, Database.LayerTableId)
        {
            Layer layerZero = new Layer("0");
            this.Add(layerZero);

            _layerZeroId = layerZero.id;
        }

        public List<Layer> Layers
        {
            get
            {
                return _items.Select(a => a as Layer).ToList();
            }
        }

        public override void XmlIn(Filer.XmlFiler filer)
        {
            Filer.XmlCADDatabase filerImpl = filer as Filer.XmlCADDatabase;

            base.XmlIn(filer);

            XmlNode curXmlNode = filerImpl.curXmlNode;
            XmlNodeList layers = curXmlNode.SelectNodes("Layer");
            foreach (XmlNode layerNode in layers)
            {
                Layer layer = new Layer();
                filerImpl.curXmlNode = layerNode;
                layer.XmlIn(filerImpl);
                this._Add(layer);
            }
            filerImpl.curXmlNode = curXmlNode;
        }
    }
}
