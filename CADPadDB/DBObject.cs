using System;
using CADPadDB.Table;
using CADPadDB.TableRecord;

namespace CADPadDB
{
    public abstract class DBObject : ICloneable
    {

        public virtual string ClassName
        {
            get { return "DBObject"; }
        }


        protected ObjectId _id = ObjectId.Null;
        public ObjectId id
        {
            get { return _id; }
        }

        public DBObjectState State { get; set; } = DBObjectState.Default;

        internal void SetId(ObjectId newId)
        {
            _id = newId;
        }

        public virtual Database database
        {
            get
            {
                if (dbtable != null)
                {
                    return dbtable.database;
                }

                return null;
            }
        }


        public virtual DBTable dbtable
        {
            get
            {
                if (_parent == null)
                    return null;

                DBObject parentObj = _parent;
                while (parentObj != null)
                {
                    if (parentObj is DBTableRecord)
                    {
                        DBTableRecord tblRec = parentObj as DBTableRecord;
                        return tblRec.dbtable;
                    }

                    parentObj = parentObj.parent;
                }

                return null;
            }
        }


        protected DBObject _parent = null;
        public DBObject parent
        {
            get { return _parent; }
        }

        public ObjectId parentId
        {
            get
            {
                if (_parent == null)
                    return ObjectId.Null;
                return _parent.id;
            }
        }

        internal void SetParent(DBObject parent)
        {
            _parent = parent;
        }


        public DBObject()
        {
        }

    
        public virtual object Clone()
        {
            DBObject dbobj = this.CreateInstance();
            dbobj._id = ObjectId.Null;
            dbobj._parent = null;
            return dbobj;
        }


        protected abstract DBObject CreateInstance();

     
        public void Erase()
        {
            Database db = this.database;
            if (db != null)
            {
                db.UnmapObject(this);
                _Erase();
                this._id = ObjectId.Null;
            }
        }

        protected virtual void _Erase()
        {
        }


        public virtual void XmlOut(Filer.XmlFiler filer)
        {
            filer.Write("id", _id);
        }


        public virtual void XmlIn(Filer.XmlFiler filer)
        {
            filer.Read("id", out _id);
        }
    }
}
