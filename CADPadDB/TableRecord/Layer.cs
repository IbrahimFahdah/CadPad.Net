using CADPadDB.Colors;

namespace CADPadDB.TableRecord
{

    public class Layer : DBTableRecord
    {
        public bool Visible { get; set; } = true;

        public override string ClassName
        {
            get { return "Layer"; }
        }


        private CADColor _color = CADColor.FromArgb(255, 255, 255);
        public CADColor color
        {
            get { return _color; }
            set
            {
                if (value.colorMethod == ColorMethod.ByColor)
                {
                    _color = value;
                }
                else
                {
                    throw new System.Exception("Layer set Color exception.");
                }
            }
        }

        public CADColor colorValue
        {
            get
            {
                return CADColor.FromArgb(_color.r, _color.g, _color.b);
            }
        }

        private LineWeight _lineWeight = LineWeight.ByLineWeightDefault;
        public LineWeight lineWeight
        {
            get { return _lineWeight; }
            set { _lineWeight = value; }
        }

        public Layer(string name = "")
        {
            _name = name;
        }


        public override object Clone()
        {
            Layer layer = base.Clone() as Layer;
            layer._color = _color;
            layer._lineWeight = _lineWeight;
            
            return layer;
        }

        protected override DBObject CreateInstance()
        {
            return new Layer();
        }

        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("Color", _color);
            filer.Write("LineWeight", _lineWeight);
        }

  
        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("Color", out _color);
            filer.Read("LineWeight", out _lineWeight);
        }
    }
}
