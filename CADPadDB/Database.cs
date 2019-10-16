using System.Collections.Generic;
using System.Xml;
using CADPadDB.CADEntity;
using CADPadDB.Table;
using CADPadDB.TableRecord;

namespace CADPadDB
{
    public class Database
    {

        private BlockTable _blockTable = null;
        public BlockTable blockTable
        {
            get { return _blockTable; }
        }
        public static ObjectId BlockTableId
        {
            get { return new ObjectId(TableIds.BlockTableId); }
        }

     
        private LayerTable _layerTable = null;
        public static ObjectId LayerTableId
        {
            get { return new ObjectId(TableIds.LayerTableId); }
        }
        public LayerTable layerTable
        {
            get { return _layerTable; }
        }

        /// <summary>
        /// ID
        /// </summary>
        private Dictionary<ObjectId, DBObject> _dictId2Object = null;
        internal ObjectId currentMaxId
        {
            get
            {
                if (_dictId2Object == null || _dictId2Object.Count == 0)
                {
                    return ObjectId.Null;
                }
                else
                {
                    ObjectId id = ObjectId.Null;
                    foreach (KeyValuePair<ObjectId, DBObject> kvp in _dictId2Object)
                    {
                        if (kvp.Key.CompareTo(id) > 0)
                        {
                            id = kvp.Key;
                        }
                    }
                    return id;
                }
            }
        }

        private ObjectIdMgr _idMgr = null;

      
        private string _fileName = null;
        public string fileName
        {
            get { return _fileName; }
        }

      
        public Database()
        {
            _dictId2Object = new Dictionary<ObjectId, DBObject>();
            _idMgr = new ObjectIdMgr(this);

            _blockTable = new BlockTable(this);
            Block modelSpace = new Block();
            modelSpace.name = "ModelSpace";
            _blockTable.Add(modelSpace);
            IdentifyDBTable(_blockTable);

            _layerTable = new LayerTable(this);
            IdentifyDBTable(_layerTable);
        }

      
        public DBObject GetObject(ObjectId oid)
        {
            if (_dictId2Object.ContainsKey(oid))
            {
                return _dictId2Object[oid];
            }
            else
            {
                return null;
            }
        }

     
        public void Open(string fileFullPath)
        {
            if (_fileName == null
                || _fileName == "")
            {
                XmlIn(fileFullPath);
            }
        }

     
        public void Save()
        {
            if (_fileName != null
                && System.IO.File.Exists(_fileName))
            {
                XmlOut(_fileName);
            }
        }

    
        public void SaveAs(string fileFullPath, bool rename = false)
        {
            XmlOut(fileFullPath);
            if (rename)
            {
                _fileName = fileFullPath;
            }
        }

    
        internal void XmlOut(string xmlFileFullPath)
        {
            Filer.XmlFilerImpl xmlFilerImpl = new Filer.XmlFilerImpl();

            //
            xmlFilerImpl.NewSubNodeAndInsert("Database");
            {
                // block table
                xmlFilerImpl.NewSubNodeAndInsert(_blockTable.ClassName);
                _blockTable.XmlOut(xmlFilerImpl);
                xmlFilerImpl.Pop();

                // Layer table
                xmlFilerImpl.NewSubNodeAndInsert(_layerTable.ClassName);
                _layerTable.XmlOut(xmlFilerImpl);
                xmlFilerImpl.Pop();
            }
            xmlFilerImpl.Pop();

            //
            xmlFilerImpl.Save(xmlFileFullPath);
        }


        internal bool XmlIn(string xmlFileFullPath)
        {
            Filer.XmlFilerImpl xmlFilerImpl = new Filer.XmlFilerImpl();
            xmlFilerImpl.Load(xmlFileFullPath);

            //
            XmlDocument xmldoc = xmlFilerImpl.xmldoc;
            XmlNode dbNode = xmldoc.SelectSingleNode("Database");
            if (dbNode == null)
            {
                return false;
            }
            xmlFilerImpl.curXmlNode = dbNode;

            //
            ClearLayerTable();
            ClearBlockTable();

            // Layer table
            XmlNode layerTblNode = dbNode.SelectSingleNode(_layerTable.ClassName);
            if (layerTblNode == null)
            {
                return false;
            }
            xmlFilerImpl.curXmlNode = layerTblNode;
            _layerTable.XmlIn(xmlFilerImpl);

            // block table
            XmlNode blockTblNode = dbNode.SelectSingleNode(_blockTable.ClassName);
            if (blockTblNode == null)
            {
                return false;
            }
            xmlFilerImpl.curXmlNode = blockTblNode;
            _blockTable.XmlIn(xmlFilerImpl);

            //
            _fileName = xmlFileFullPath;
            _idMgr.reset();
            return true;
        }


        private void ClearLayerTable()
        {
            List<Layer> allLayers = new List<Layer>();
            foreach (Layer layer in _layerTable)
            {
                allLayers.Add(layer);
            }
            _layerTable.Clear();

            foreach (Layer layer in allLayers)
            {
                layer.Erase();
            }
        }

      
        private void ClearBlockTable()
        {
            Dictionary<Entity, Entity> allEnts = new Dictionary<Entity,Entity>();
            List<Block> allBlocks = new List<Block>();

            foreach (Block block in _blockTable)
            {
                foreach (Entity entity in block)
                {
                    allEnts[entity] = entity;
                }
                block.Clear();
                allBlocks.Add(block);
            }
            _blockTable.Clear();

            foreach (KeyValuePair<Entity, Entity> kvp in allEnts)
            {
                kvp.Key.Erase();
            }

            foreach (Block block in allBlocks)
            {
                block.Erase();
            }
        }

        #region IdentityObject
        private void IdentifyDBTable(DBTable table)
        {
            MapSingleObject(table);
        }

        internal void IdentifyObject(DBObject obj)
        {
            IdentifyObjectSingle(obj);
            if (obj is Block)
            {
                Block block = obj as Block;
                foreach (Entity entity in block)
                {
                    IdentifyObjectSingle(entity);
                }
            }
        }

        private void IdentifyObjectSingle(DBObject obj)
        {
            if (obj.id.isNull)
            {
                obj.SetId(_idMgr.NextId);
            }
            MapSingleObject(obj);
        }
        #endregion

        #region MapObject
        private void MapObject(DBObject obj)
        {
            MapSingleObject(obj);
            if (obj is Block)
            {
                Block block = obj as Block;
                foreach (Entity entity in block)
                {
                    MapSingleObject(entity);
                }
            }
        }

        internal void UnmapObject(DBObject obj)
        {
            UnmapSingleObject(obj);
            if (obj is Block)
            {
                Block block = obj as Block;
                foreach (Entity entity in block)
                {
                    UnmapSingleObject(entity);
                }
            }
        }

        private void MapSingleObject(DBObject obj)
        {
            _dictId2Object[obj.id] = obj;
        }

        private void UnmapSingleObject(DBObject obj)
        {
            _dictId2Object.Remove(obj.id);
        }
        #endregion
    }
}
