using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Interfaces;

namespace CADPadServices.Controllers
{
    public class SnapNodesController : IMouseKeyReceiver
    {
        protected IDrawing _drawing = null;
        private IDrawingVisual tmpVisual = null;
        protected double _threshold = 8;
        protected ObjectSnapPoint _currentObjectSnapPoint = null;

        public IDrawingVisual SnapVisual
        {
            get
            {
                if(tmpVisual==null)
                {
                    tmpVisual = _drawing.CreateTempVisual();
                }
                return tmpVisual;
            }
            set => tmpVisual = value;
        }

        public SnapNodesController(IDrawing drawing)
        {
            _drawing = drawing;
            
        }

        public CADPoint Snap(CADPoint posInCanvas)
        {
            var posInModel = _drawing.CanvasToModel(new CADPoint(posInCanvas.X, posInCanvas.Y));

            foreach (Entity entity in _drawing.CurrentBlock)
            {
                List<ObjectSnapPoint> snapPnts = entity.GetSnapPoints();
                if (snapPnts == null || snapPnts.Count == 0 || entity.State==DBObjectState.BeingConstructed)
                {
                    continue;
                }
                foreach (ObjectSnapPoint snapPnt in snapPnts)
                {
                    double dis = (snapPnt.Position - posInModel).Length;
                    double disInCanvas = _drawing.ModelToCanvas(dis);
                    if (disInCanvas <= _threshold)
                    {
                        _currentObjectSnapPoint = snapPnt;

                        return snapPnt.Position;
                    }
                }
            }

            Clear();
            return new CADPoint(posInModel.X, posInModel.Y) ;
        }

        public void Clear()
        {
            _currentObjectSnapPoint = null;
            _drawing.RemoveTempVisual(tmpVisual);
            tmpVisual = null;
        }

        #region events
        public IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            return null;
        }

        public IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            Clear();
            return null;
        }

        public IEventResult OnMouseMove(IMouseEventArgs e)
        {
            if (_currentObjectSnapPoint != null)
            {
                CADPoint posInCanvas = _drawing.ModelToCanvas(_currentObjectSnapPoint.Position);

                switch (_currentObjectSnapPoint.Type)
                {
                    case ObjectSnapMode.End:
                        {
                            SnapVisual.Open();
                            SnapVisual.DrawRectangle(new CADPoint(posInCanvas.X - _threshold, posInCanvas.Y - _threshold),
                                _threshold * 2, _threshold * 2, false);
                            SnapVisual.Close();
                        }
                        break;

                    case ObjectSnapMode.Mid:
                        {
                            CADPoint offset = new CADPoint(0, -_threshold * 1.2);
                            CADPoint point1 = posInCanvas + offset;
                            offset = (CADPoint)CADVector.Rotate((CADVector)offset, 120);
                            CADPoint point2 = posInCanvas + offset;
                            offset = (CADPoint)CADVector.Rotate((CADVector)offset, 120);
                            CADPoint point3 = posInCanvas + offset;
                            SnapVisual.Open();
                            SnapVisual.DrawLine(point1, point2, false);
                            SnapVisual.DrawLine(point2, point3, false);
                            SnapVisual.DrawLine(point3, point1, false);
                            SnapVisual.Close();
                        }
                        break;

                    case ObjectSnapMode.Center:
                        {
                         //   gd.DrawCircle(posInCanvas, _threshold, false);
                        }
                        break;

                    default:
                        {
                            SnapVisual.Open();
                            SnapVisual.DrawRectangle(new CADPoint(posInCanvas.X - _threshold, posInCanvas.Y - _threshold),
                                _threshold * 2, _threshold * 2, false);
                            SnapVisual.Close();
                        }
                        break;
                }
            }
            return null;
        }

        public IEventResult OnMouseWheel(IMouseWheelEventArgs e)
        {
            return null;
        }

        public IEventResult OnMouseDoubleClick(IMouseEventArgs e)
        {
            return null;
        }

        public IEventResult OnKeyDown(IKeyEventArgs KeyEventArgs)
        {
            return null;
        }

        public IEventResult OnKeyUp(IKeyEventArgs KeyEventArgs)
        {
            return null;
        }

        #endregion
    }
}
