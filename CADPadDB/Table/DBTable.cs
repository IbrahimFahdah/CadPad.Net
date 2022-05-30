using System;
using System.Collections.Generic;
using CADPadDB.TableRecord;

namespace CADPadDB.Table
{
    public abstract class DBTable : DBObject, IEnumerable<DBTableRecord>
    {

        public override string ClassName
        {
            get { return "DBTable"; }
        }

        protected List<DBTableRecord> _items = new List<DBTableRecord>();
        protected Dictionary<string, DBTableRecord> _dictName2Item = new Dictionary<string, DBTableRecord>();

        public delegate void ItemAdded(DBTableRecord item);
        public event ItemAdded itemAdded;

        public delegate void ItemRemoved(DBTableRecord item);
        public event ItemRemoved itemRemoved;


        private Database _database = null;
        public override Database database
        {
            get
            {
                return _database;
            }
        }

        public override DBTable dbtable
        {
            get { return this; }
        }


        public int Count
        {
            get { return _items.Count; }
        }


        internal DBTable(Database db, ObjectId tableId)
        {
            _database = db;
            _id = tableId;
        }


        public override object Clone()
        {
            throw new System.Exception("DBTable can not be cloned.");
        }

        protected override DBObject CreateInstance()
        {
            throw new NotImplementedException();
        }


        protected override void _Erase()
        {
        }


        public virtual ObjectId Add(DBTableRecord tblRecord)
        {
            if (tblRecord.id != ObjectId.Null)
            {
                throw new System.Exception("TableRecord is not newly created");
            }

            return _Add(tblRecord);
        }

        protected ObjectId _Add(DBTableRecord tblRecord)
        {
            _items.Add(tblRecord);
            tblRecord._dbtable = this;
            _dictName2Item[tblRecord.name] = tblRecord;
            tblRecord.SetParent(this);
            _database.IdentifyObject(tblRecord);

            if (itemAdded != null)
            {
                itemAdded.Invoke(tblRecord);
            }

            return tblRecord.id;
        }


        public virtual void Remove(DBTableRecord tblRecord)
        {
            if (_items.Remove(tblRecord))
            {
                _dictName2Item.Remove(tblRecord.name);

                if (itemRemoved != null)
                {
                    itemRemoved.Invoke(tblRecord);
                }
            }
        }


        internal virtual void Clear()
        {
            _items.Clear();
            _dictName2Item.Clear();
        }


        public bool Has(string key)
        {
            return _dictName2Item.ContainsKey(key);
        }

        public bool Has(ObjectId id)
        {
            foreach (DBTableRecord item in _items)
            {
                if (item.id == id)
                {
                    return true;
                }
            }
            return false;
        }


        public DBTableRecord this[string key]
        {
            get
            {
                if (this.Has(key))
                {
                    return _dictName2Item[key];
                }
                else
                {
                    return null;
                }
            }
        }


        public override void XmlOut(Filer.XmlFiler filer)
        {
            Filer.XmlCADDatabase filerImpl = filer as Filer.XmlCADDatabase;

            base.XmlOut(filer);
            foreach (DBTableRecord item in _items)
            {
                filerImpl.NewSubNodeAndInsert(item.ClassName);
                item.XmlOut(filer);
                filerImpl.Pop();
            }
        }


        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);
        }

        #region IEnumerable<DBTableRecord>
        public IEnumerator<DBTableRecord> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
