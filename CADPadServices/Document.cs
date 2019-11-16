using CADPadDB;
using CADPadDB.Colors;
using CADPadDB.TableRecord;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class Document : IDocument
    {
        public Database Database { get; } = null;
        public string CurrentBlockName { get; } = "ModelSpace";
        public Selections Selections { get; } = null;

        private ObjectId _currLayerId = ObjectId.Null;
        public ObjectId currentLayerId
        {
            get { return _currLayerId; }
            set
            {
                if (_currLayerId != value)
                {
                    Layer layer = Database.GetObject(value) as Layer;
                    if (layer != null)
                    {
                        ObjectId last = _currLayerId;
                        _currLayerId = value;
                        if (currentLayerChanged != null)
                        {
                            currentLayerChanged.Invoke(last, _currLayerId);
                        }
                    }
                    else
                    {
                        throw new System.Exception("invalid Layer id");
                    }
                }
            }
        }

        public delegate void CurrentLayerChanged(ObjectId last, ObjectId current);
        public event CurrentLayerChanged currentLayerChanged;

         private CommonColors _commonColors = null;
        public CommonColors commonColors
        {
            get { return _commonColors; }
        }

        private CADColor _currColor = CADColor.ByLayer;
        public CADColor currentColor
        {
            get { return _currColor; }
            set
            {
                CADColor last = _currColor;
                _currColor = value;
                if (currentColorChanged != null)
                {
                    currentColorChanged.Invoke(last, _currColor);
                }
            }
        }

        public CADColor currentColorValue
        {
            get
            {
                switch (_currColor.colorMethod)
                {
                    case ColorMethod.ByLayer:
                        Layer layer = Database.GetObject(_currLayerId) as Layer;
                        if (layer != null)
                        {
                            return layer.colorValue;
                        }
                        else
                        {
                            return CADColor.FromArgb(
                                _currColor.r, _currColor.g, _currColor.b);
                        }

                    case ColorMethod.ByColor:
                    case ColorMethod.ByBlock:
                    case ColorMethod.None:
                    default:
                        return CADColor.FromArgb(
                            _currColor.r, _currColor.g, _currColor.b);
                }
            }
        }

        public delegate void CurrentColorChanged(CADColor last, CADColor current);
        public event CurrentColorChanged currentColorChanged;

        public Document()
        {
            Database = new Database();
            Selections = new Selections();
            _commonColors = new CommonColors();
            _currLayerId = Database.layerTable["0"].id;
            _currColor = CADColor.ByLayer;
        }
    }
}
