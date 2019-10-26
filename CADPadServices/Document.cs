using CADPadDB;
using CADPadDB.Colors;
using CADPadDB.TableRecord;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class Document : IDocument
    {
        /// <summary>
        /// 数据库
        /// </summary>
        private Database _database = null;
        public Database database
        {
            get { return _database; }
        }

        /// <summary>
        /// 当前块名称
        /// </summary>
        private string _currentBlockName = "ModelSpace";
        public string currentBlockName
        {
            get { return _currentBlockName; }
        }

        /// <summary>
        /// 选择集
        /// </summary>
        private Selections _selections = null;
        public Selections selections
        {
            get { return _selections; }
        }

        /// <summary>
        /// 当前图层
        /// </summary>
        private ObjectId _currLayerId = ObjectId.Null;
        public ObjectId currentLayerId
        {
            get { return _currLayerId; }
            set
            {
                if (_currLayerId != value)
                {
                    Layer layer = _database.GetObject(value) as Layer;
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

        /// <summary>
        /// 常用颜色集
        /// </summary>
        private CommonColors _commonColors = null;
        public CommonColors commonColors
        {
            get { return _commonColors; }
        }

        /// <summary>
        /// 当前图元颜色
        /// </summary>
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
                        Layer layer = _database.GetObject(_currLayerId) as Layer;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        public Document()
        {
            _database = new Database();
            _selections = new Selections();
            _commonColors = new CommonColors();
            _currLayerId = _database.layerTable["0"].id;
            _currColor = CADColor.ByLayer;
        }
    }
}
