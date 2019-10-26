using System;
using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadDB.TableRecord;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;
using CADPadServices.Hit;
namespace CADPadServices
{

    public class PickupBox
    {
        private IDrawing _presenter = null;
        internal IDrawing presenter
        {
            get { return _presenter; }
        }
        private Dictionary<Type, EntityHitter> _entType2EntHitter = new Dictionary<Type, EntityHitter>();

        /// <summary>
        ///  中心位置
        /// </summary>
        private CADPoint _center = new CADPoint(0, 0);
        public CADPoint center
        {
            get { return _center; }
            set { _center = value; }
        }

        /// <summary>
        /// 边长
        /// </summary>
        private int _side = 20;
        public int side
        {
            get { return _side; }
            set
            {
                if (_side != value)
                {
                    _side = value;
                    //this.RenewBitmap();
                }
            }
        }

      

        /// <summary>
        /// 预留外包围盒
        /// </summary>
        private Bounding _reservedBounding = new Bounding();
        internal Bounding reservedBounding
        {
            get { return _reservedBounding; }
        }

        private void UpdateReservedBounding()
        {
            CADPoint centerInModel = _presenter.CanvasToModel(_center);
            double sideInModel = _presenter.CanvasToModel(_side);
            _reservedBounding = new Bounding(centerInModel, sideInModel, sideInModel);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PickupBox(IDrawing presenter)
        {
            _presenter = presenter;
            //RenewBitmap();

            //
            RegisterEntityHitter(typeof(Line), new LineHitter());
            RegisterEntityHitter(typeof(Xline), new XlineHitter());
            RegisterEntityHitter(typeof(Ray), new RayHitter());
            RegisterEntityHitter(typeof(Polyline), new PolylineHitter());
            RegisterEntityHitter(typeof(Circle), new CircleHitter());
            RegisterEntityHitter(typeof(Ellipse), new EllipseHitter());
            RegisterEntityHitter(typeof(Arc), new ArcHitter());
            //RegisterEntityHitter(typeof(Text), new TextHitter());
        }

        private void RegisterEntityHitter(Type entType, EntityHitter entHitter)
        {
            _entType2EntHitter[entType] = entHitter;
        }


        public List<Selection> Select(Block block)
        {
            UpdateReservedBounding();

            List<Selection> sels = new List<Selection>();
            foreach (Entity entity in block)
            {
                if (HitEntity(entity))
                {
                    Selection selection = new Selection();
                    selection.objectId = entity.id;
                    selection.position = _presenter.CanvasToModel(this.center);
                    sels.Add(selection);
                }
            }

            return sels;
        }

        private bool HitEntity(Entity entity)
        {
            if (_entType2EntHitter.ContainsKey(entity.GetType()))
            {
                return _entType2EntHitter[entity.GetType()].Hit(this, entity);
            }
            else
            {
                return false;
            }
        }
    }
}
