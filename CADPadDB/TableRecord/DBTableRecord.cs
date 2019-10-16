using CADPadDB.Table;

namespace CADPadDB.TableRecord
{

    public abstract class DBTableRecord : DBObject
    {

        public override string ClassName
        {
            get { return "DBTableRecord"; }
        }


        protected string _name = "";
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }


        internal DBTable _dbtable = null;
        public override DBTable dbtable
        {
            get { return _dbtable; }
        }


        public override object Clone()
        {
            DBTableRecord tblRec = base.Clone() as DBTableRecord;
            tblRec._name = _name;
            tblRec._dbtable = null;
            return tblRec;
        }


        protected override void _Erase()
        {
            if (_dbtable != null)
            {
                _dbtable.Remove(this);
            }
        }


        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("name", _name);
        }


        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("name", out _name);
        }
    }
}
