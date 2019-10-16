using CADPadDB;
using CADPadDB.Maths;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    /// <summary>
    /// 捕捉节点管理器
    /// </summary>
    public class SnapNodesMgrBase
    {
        protected IDrawing _presenter = null;

        protected ObjectSnapPoint _currObjectSnapPoint = null;
        internal ObjectSnapPoint currentObjectSnapPoint
        {
            get { return _currObjectSnapPoint; }
        }

        public SnapNodesMgrBase(IDrawing presenter)
        {
            _presenter = presenter;
        }

        public CADPoint Snap(double x, double y)
        {
            return this.Snap(new CADPoint(x, y));
        }

        public CADPoint Snap(CADPoint posInCanvas)
        {
            var posInModel = _presenter.CanvasToModel(new CADPoint(posInCanvas.X, posInCanvas.Y));

            //foreach (Entity entity in _presenter.currentBlock)
            //{
            //    List<ObjectSnapPoint> snapPnts = entity.GetSnapPoints();
            //    if (snapPnts == null || snapPnts.Count == 0)
            //    {
            //        continue;
            //    }
            //    foreach (ObjectSnapPoint snapPnt in snapPnts)
            //    {
            //        double dis = (snapPnt.Position - posInModel).length;
            //        double disInCanvas = _presenter.ModelToCanvas(dis);
            //        if (disInCanvas <= _threshold)
            //        {
            //            _currObjectSnapPoint = snapPnt;
            //            return snapPnt.Position;
            //        }
            //    }
            //}

            _currObjectSnapPoint = null;
            return new CADPoint(posInModel.X, posInModel.Y) ;
        }

        public void Clear()
        {
            _currObjectSnapPoint = null;
        }

        protected double _threshold = 8;
        //public virtual void OnPaint(IGraphicsContext canvasDraw)
        //{
         
        //}
    }
}
