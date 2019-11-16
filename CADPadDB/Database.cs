using System.Collections.Generic;
using System.IO;
using System.Xml;
using CADPadDB.CADEntity;
using CADPadDB.Filer;
using CADPadDB.Table;
using CADPadDB.TableRecord;

namespace CADPadDB
{
    public class Database
    {
     
        public static ObjectId BlockTableId
        {
            get { return new ObjectId(TableIds.BlockTableId); }
        }

        public static ObjectId LayerTableId
        {
            get { return new ObjectId(TableIds.LayerTableId); }
        }

        public BlockTable blockTable { get; } = null;
        public LayerTable layerTable { get; } = null;

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

        public Database()
        {
            _dictId2Object = new Dictionary<ObjectId, DBObject>();
            _idMgr = new ObjectIdMgr(this);

            blockTable = new BlockTable(this);
            Block modelSpace = new Block();
            modelSpace.name = "ModelSpace";
            blockTable.Add(modelSpace);
            IdentifyDBTable(blockTable);

            layerTable = new LayerTable(this);
            IdentifyDBTable(layerTable);
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

        public void Write(Stream stream)
        {
            XmlCADDatabase xmlFilerImpl = new XmlCADDatabase();

            //
            xmlFilerImpl.NewSubNodeAndInsert("Database");
            {
                // block table
                xmlFilerImpl.NewSubNodeAndInsert(blockTable.ClassName);
                blockTable.XmlOut(xmlFilerImpl);
                xmlFilerImpl.Pop();

                // Layer table
                xmlFilerImpl.NewSubNodeAndInsert(layerTable.ClassName);
                layerTable.XmlOut(xmlFilerImpl);
                xmlFilerImpl.Pop();
            }
            xmlFilerImpl.Pop();

            xmlFilerImpl.Save(stream);
        }

        public void Read(Stream stream)
        {
            XmlIn(stream);
        }

        internal bool XmlIn(Stream stream)
        {
            XmlCADDatabase xmlFilerImpl = new XmlCADDatabase();
            xmlFilerImpl.Load(stream);

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
            XmlNode layerTblNode = dbNode.SelectSingleNode(layerTable.ClassName);
            if (layerTblNode == null)
            {
                return false;
            }
            xmlFilerImpl.curXmlNode = layerTblNode;
            layerTable.XmlIn(xmlFilerImpl);

            // block table
            XmlNode blockTblNode = dbNode.SelectSingleNode(blockTable.ClassName);
            if (blockTblNode == null)
            {
                return false;
            }
            xmlFilerImpl.curXmlNode = blockTblNode;
            blockTable.XmlIn(xmlFilerImpl);

            _idMgr.reset();
            return true;
        }


        private void ClearLayerTable()
        {
            List<Layer> allLayers = new List<Layer>();
            foreach (Layer layer in layerTable)
            {
                allLayers.Add(layer);
            }
            layerTable.Clear();

            foreach (Layer layer in allLayers)
            {
                layer.Erase();
            }
        }

      
        private void ClearBlockTable()
        {
            Dictionary<Entity, Entity> allEnts = new Dictionary<Entity,Entity>();
            List<Block> allBlocks = new List<Block>();

            foreach (Block block in blockTable)
            {
                foreach (Entity entity in block)
                {
                    allEnts[entity] = entity;
                }
                block.Clear();
                allBlocks.Add(block);
            }
            blockTable.Clear();

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
