using System;
using System.Xml;
using CADPadDB.Colors;
using CADPadDB.Maths;

namespace CADPadDB.Filer
{

    internal class XmlFilerImpl : XmlFiler
    {

        protected XmlDocument _xmldoc = null;
        internal XmlDocument xmldoc
        {
            get { return _xmldoc; }
        }


        protected XmlNode _curXmlNode = null;
        internal XmlNode curXmlNode
        {
            get { return _curXmlNode; }
            set { _curXmlNode = value; }
        }


        public XmlFilerImpl()
        {
            _xmldoc = new XmlDocument();
        }

        public XmlFilerImpl(XmlNode parentNode)
        {
            _curXmlNode = parentNode.OwnerDocument;
            _curXmlNode = parentNode;
        }

        public void Load(string xmlFileFullPath)
        {
            _xmldoc.Load(xmlFileFullPath);
        }

        public void Save(string fileFullPath)
        {
            if (_xmldoc != null)
            {
                _xmldoc.Save(fileFullPath);
            }
        }

        internal void NewSubNodeAndInsert(string name)
        {
            XmlElement elem = _xmldoc.CreateElement(name);
            if (_curXmlNode != null)
            {
                _curXmlNode.AppendChild(elem);
            }
            else
            {
                _xmldoc.AppendChild(elem);
            }
            _curXmlNode = elem;
        }

        internal void Pop()
        {
            _curXmlNode = _curXmlNode.ParentNode;
        }

        internal bool _Write(string name, object value)
        {
            XmlElement elem = _xmldoc.CreateElement(name);
            if (elem == null)
            {
                return false;
            }
            elem.InnerText = value.ToString();
            _curXmlNode.AppendChild(elem);

            return true;
        }

        public override bool Write(string name, string value)
        {
            XmlElement elem = _xmldoc.CreateElement(name);
            if (elem == null)
            {
                return false;
            }
            elem.InnerText = value;
            _curXmlNode.AppendChild(elem);

            return true;
        }

        public override bool Write(string name, bool value)
        {
            return _Write(name, value);
        }

        public override bool Write(string name, byte value)
        {
            return _Write(name, value);
        }

        public override bool Write(string name, uint value)
        {
            return _Write(name, value);
        }

        public override bool Write(string name, int value)
        {
            return _Write(name, value);
        }

        public override bool Write(string name, double value)
        {
            return _Write(name, value);
        }

        public override bool Write(string name, CADPoint value)
        {
            XmlElement elem = _xmldoc.CreateElement(name);
            if (elem == null)
            {
                return false;
            }
            elem.InnerText = value.X.ToString() + ";" + value.Y.ToString();
            _curXmlNode.AppendChild(elem);

            return true;
        }

        public override bool Write(string name, CADVector value)
        {
            XmlElement elem = _xmldoc.CreateElement(name);
            if (elem == null)
            {
                return false;
            }
            elem.InnerText = value.X.ToString() + ";" + value.Y.ToString();
            _curXmlNode.AppendChild(elem);

            return true;
        }

        public override bool Write(string name, CADColor value)
        {
            return _Write(name, value);
        }

        public override bool Write(string name, ObjectId value)
        {
            return _Write(name, value);
        }

        public override bool Write(string name, LineWeight value)
        {
            return _Write(name, value);
        }

        public override bool Read(string name, out string value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = "";
                return false;
            }
            value = node.InnerText;

            return true;
        }

        public override bool Read(string name, out bool value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = true;
                return false;
            }

            return bool.TryParse(node.InnerText, out value);
        }

        public override bool Read(string name, out byte value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = 0;
                return false;
            }

            return byte.TryParse(node.InnerText, out value);
        }

        public override bool Read(string name, out uint value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = 0;
                return false;
            }

            return uint.TryParse(node.InnerText, out value);
        }

        public override bool Read(string name, out int value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = 0;
                return false;
            }

            return int.TryParse(node.InnerText, out value);
        }

        public override bool Read(string name, out double value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = 0.0;
                return false;
            }

            return double.TryParse(node.InnerText, out value);
        }

        public override bool Read(string name, out CADPoint value)
        {
            value = new CADPoint(0, 0);
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                return false;
            }

            string strValue = node.InnerText;
            string[] xy = strValue.Split(';');
            if (xy.Length != 2)
            {
                return false;
            }

            double x = 0;
            if (!double.TryParse(xy[0], out x))
            {
                return false;
            }
            double y = 0;
            if (!double.TryParse(xy[1], out y))
            {
                return false;
            }
            value.X = x;
            value.Y = y;


            return true;
        }

        public override bool Read(string name, out CADVector value)
        {
            CADPoint tmp;
            var ret= Read(name, out tmp);
            value = (CADVector)tmp;
            return ret;
        }

        public override bool Read(string name, out CADColor value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = CADColor.ByLayer;
                return false;
            }

            return CADColor.TryParse(node.InnerText, out value);
        }

        public override bool Read(string name, out ObjectId value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = ObjectId.Null;
                return false;
            }

            return ObjectId.TryParse(node.InnerText, out value);
        }

        public override bool Read(string name, out LineWeight value)
        {
            XmlNode node = _curXmlNode.SelectSingleNode(name);
            if (node == null)
            {
                value = LineWeight.ByLineWeightDefault;
                return false;
            }

            // 
            value = (LineWeight)Enum.Parse(
                typeof(LineWeight), node.InnerText, true);
            return true;
        }
    }
}
