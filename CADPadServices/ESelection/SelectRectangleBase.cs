using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadDB.TableRecord;
using CADPadServices.ApplicationServices;
using CADPadServices.Interfaces;
using System;


namespace CADPadServices.ESelection
{
    internal abstract class EntityRS
    {
        internal abstract bool Cross(Bounding bounding, Entity entity);
        internal virtual bool Window(Bounding bounding, Entity entity)
        {
            return bounding.Contains(entity.Bounding);
        }
    }
    public class EntityRSMgr
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static EntityRSMgr _instance = null;
        public static EntityRSMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EntityRSMgr();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Dictionary<Type, EntityRS> _type2EntityRS = new Dictionary<Type, EntityRS>();

        private EntityRSMgr()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterEntityRS(typeof(Line), new LineRS());
            //RegisterEntityRS(typeof(Xline), new XlineRS());
            //RegisterEntityRS(typeof(Ray), new RayRS());
            //RegisterEntityRS(typeof(Polyline), new PolylineRS());
            //RegisterEntityRS(typeof(Circle), new CircleRS());
            //RegisterEntityRS(typeof(Arc), new ArcRS());
            //RegisterEntityRS(typeof(Text), new TextRS());
        }

        private void RegisterEntityRS(Type type, EntityRS entityRS)
        {
            _type2EntityRS[type] = entityRS;
        }

        /// <summary>
        /// Select
        /// </summary>
        public bool Cross(Bounding selectBound, Entity entity)
        {
            if (_type2EntityRS.ContainsKey(entity.GetType()))
            {
                return _type2EntityRS[entity.GetType()].Cross(selectBound, entity);
            }

            return false;
        }

        public bool Window(Bounding selectBound, Entity entity)
        {
            if (_type2EntityRS.ContainsKey(entity.GetType()))
            {
                return _type2EntityRS[entity.GetType()].Window(selectBound, entity);
            }

            return false;
        }
    }

    public class SelectRectangleBase
    {
        private IDrawing _presenter = null;

        /// <summary>
        /// 选择模式
        /// </summary>
        public enum SelectMode
        {
            // 闭合框选,物体完全在选择矩形框内则选中物体
            Window = 1,
            // 交叉框选,物体与选择矩形框有交集则选中物体
            Cross = 2,
        }

        /// <summary>
        /// 矩形对角起点
        /// Canvas CSYS
        /// </summary>
        protected CADPoint _startPoint = new CADPoint(0, 0);
        public CADPoint startPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        /// <summary>
        /// 矩形对角终点
        /// Canvas CSYS
        /// </summary>
        protected CADPoint _endPoint = new CADPoint(0, 0);
        public CADPoint endPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        /// <summary>
        /// 选择模式
        /// </summary>
        public SelectMode selectMode
        {
            get
            {
                if (_endPoint.X >= _startPoint.X)
                {
                    return SelectMode.Window;
                }
                else
                {
                    return SelectMode.Cross;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SelectRectangleBase(IDrawing presenter)
        {
            _presenter = presenter;
        }

      

        /// <summary>
        /// 选择
        /// </summary>
        public List<Selection> Select(Block block)
        {
            Bounding selectBound = new Bounding(
                _presenter.CanvasToModel(_startPoint),
                _presenter.CanvasToModel(_endPoint));

            List<Selection> sels = new List<Selection>();
            SelectMode selMode = this.selectMode;
            foreach (Entity entity in block)
            {
                bool selected = false;
                if (selMode == SelectMode.Cross)
                {
                    selected = EntityRSMgr.Instance.Cross(selectBound, entity);
                }
                else if (selMode == SelectMode.Window)
                {
                    selected = EntityRSMgr.Instance.Window(selectBound, entity);
                }

                if (selected)
                {
                    Selection selection = new Selection();
                    selection.objectId = entity.id;
                    sels.Add(selection);
                }
            }

            return sels;
        }

        //private bool IsLineIn(IPresenter presenter, Line line, SelectMode selMode)
        //{
        //    if (selMode == SelectMode.Window)
        //    {
        //        Vector2 pnt1 = presenter.CanvasToModel(_startPoint);
        //        Vector2 pnt2 = presenter.CanvasToModel(_endPoint);
        //        LitMath.Rectangle2 selRect = new LitMath.Rectangle2(pnt1, pnt2);

        //        if (MathUtils.IsPointInRectangle(line.startPoint, selRect)
        //            && MathUtils.IsPointInRectangle(line.endPoint, selRect))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else if (selMode == SelectMode.Cross)
        //    {
        //        Vector2 pnt1 = presenter.CanvasToModel(_startPoint);
        //        Vector2 pnt2 = presenter.CanvasToModel(_endPoint);

        //        Bounding selectBound = new Bounding(pnt1, pnt2);
        //        Bounding lineBound = line.bounding;
        //        if (selectBound.Contains(lineBound))
        //        {
        //            return true;
        //        }

        //        LitMath.Rectangle2 selRect = new LitMath.Rectangle2(pnt1, pnt2);

        //        LitMath.Line2 rectLine1 = new LitMath.Line2(selRect.leftBottom, selRect.leftTop);
        //        LitMath.Line2 rectLine2 = new LitMath.Line2(selRect.leftTop, selRect.rightTop);
        //        LitMath.Line2 rectLine3 = new LitMath.Line2(selRect.rightTop, selRect.rightBottom);
        //        LitMath.Line2 rectLine4 = new LitMath.Line2(selRect.rightBottom, selRect.leftBottom);
        //        LitMath.Line2 line2 = new LitMath.Line2(line.startPoint, line.endPoint);

        //        Vector2 intersection = new Vector2();
        //        if (LitMath.Line2.Intersect(rectLine1, line2, ref intersection)
        //            || LitMath.Line2.Intersect(rectLine2, line2, ref intersection)
        //            || LitMath.Line2.Intersect(rectLine3, line2, ref intersection)
        //            || LitMath.Line2.Intersect(rectLine4, line2, ref intersection))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private bool IsCircleIn(IPresenter presenter, Circle circle, SelectMode selMode)
        //{
        //    if (selMode == SelectMode.Window)
        //    {
        //        Vector2 pnt1 = presenter.CanvasToModel(_startPoint);
        //        Vector2 pnt2 = presenter.CanvasToModel(_endPoint);
        //        Bounding selectBound = new Bounding(pnt1, pnt2);
        //        Bounding circleBound = circle.bounding;

        //        if (selectBound.Contains(circleBound))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else if (selMode == SelectMode.Cross)
        //    {
        //        Vector2 pnt1 = presenter.CanvasToModel(_startPoint);
        //        Vector2 pnt2 = presenter.CanvasToModel(_endPoint);
        //        Bounding selectBound = new Bounding(pnt1, pnt2);

        //        return MathUtils.BoundingCross(selectBound, circle);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private bool IsPolylineIn(IPresenter presenter, Polyline polyline, SelectMode selMode)
        //{
        //    switch (selMode)
        //    {
        //        case SelectMode.Window:
        //            {
        //                Vector2 pnt1 = presenter.CanvasToModel(_startPoint);
        //                Vector2 pnt2 = presenter.CanvasToModel(_endPoint);
        //                Bounding selectBound = new Bounding(pnt1, pnt2);
        //                Bounding polylineBound = polyline.bounding;

        //                if (selectBound.Contains(polylineBound))
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }

        //        case SelectMode.Cross:
        //            {
        //                Vector2 pnt1 = presenter.CanvasToModel(_startPoint);
        //                Vector2 pnt2 = presenter.CanvasToModel(_endPoint);
        //                Bounding selectBound = new Bounding(pnt1, pnt2);
        //                Bounding polylineBound = polyline.bounding;

        //                if (selectBound.Contains(polylineBound))
        //                {
        //                    return true;
        //                }

        //                LitMath.Rectangle2 selRect = new LitMath.Rectangle2(pnt1, pnt2);
        //                LitMath.Line2 rectLine1 = new LitMath.Line2(selRect.leftBottom, selRect.leftTop);
        //                LitMath.Line2 rectLine2 = new LitMath.Line2(selRect.leftTop, selRect.rightTop);
        //                LitMath.Line2 rectLine3 = new LitMath.Line2(selRect.rightTop, selRect.rightBottom);
        //                LitMath.Line2 rectLine4 = new LitMath.Line2(selRect.rightBottom, selRect.leftBottom);

        //                for (int i = 1; i < polyline.NumberOfVertices; ++i)
        //                {
        //                    Vector2 spnt = polyline.GetPointAt(i - 1);
        //                    Vector2 epnt = polyline.GetPointAt(i);
        //                    LitMath.Line2 line2 = new LitMath.Line2(spnt, epnt);
        //                    Vector2 intersection = new Vector2();
        //                    if (LitMath.Line2.Intersect(rectLine1, line2, ref intersection)
        //                        || LitMath.Line2.Intersect(rectLine2, line2, ref intersection)
        //                        || LitMath.Line2.Intersect(rectLine3, line2, ref intersection)
        //                        || LitMath.Line2.Intersect(rectLine4, line2, ref intersection))
        //                    {
        //                        return true;
        //                    }
        //                }

        //                return false;
        //            }

        //        default:
        //            return false;
        //    }
        //}
    }
}