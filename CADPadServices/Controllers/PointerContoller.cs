using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Commands;
using CADPadServices.Enums;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices.Controllers
{
    public class PointerContoller : IPointerContoller
    {
        protected IDrawing _drawing = null;
        protected SnapNodesController _snapNodesMgr = null;
        protected GripPointsController _anchorMgr = null;
        protected PickupBox _pickupBox = null;


        public PointerModes mode { get; set; } = PointerModes.Default;

        /// <summary>
        ///  Canvas CSYS
        /// </summary>
        protected CADPoint _pos = new CADPoint(0, 0);
        internal CADPoint position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        /// <summary>
        /// Defined using Model sys.
        /// </summary>
        public CADPoint CurrentSnapPoint { get; protected set; } = new CADPoint(0, 0);
        public CADPoint _loc { get; protected set; } = new CADPoint(0, 0);

        protected SelectBox _selRect = null;

        internal SelectBox SelRect
        {
            get
            {
                if (_selRect == null)
                {
                    _selRect = _drawing.CreateSelectRectangle();
                }
                return _selRect;
            }
        }

        protected Cursor _cursor = null;


        protected bool _isShowAnchor = true;
        public bool isShowAnchor
        {
            get { return _isShowAnchor; }
            set
            {
                if (_isShowAnchor != value)
                {
                    _anchorMgr.Clear();
                    _isShowAnchor = value;
                    if (_isShowAnchor)
                    {
                        _anchorMgr.Update();
                    }
                }
            }
        }

        public PointerContoller(IDrawing drawing)
        {

            _drawing = drawing;

            _pickupBox = new PickupBox(_drawing);
            _pickupBox.side = 20;

            _cursor = new Cursor(_drawing);
            _cursor.Length = 60;

            _snapNodesMgr = new SnapNodesController(_drawing);
            _anchorMgr = new GripPointsController(_drawing);

        }

        #region events
        public IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            _pos.X = e.X;
            _pos.Y = e.Y;
            Command cmd = null;
            switch (mode)
            {
                case PointerModes.Default:
                    #region
                    {
                        if (e.IsLeftPressed)
                        {
                            if (_anchorMgr.currentGripPoint == null)
                            {

                                _pickupBox.center = _pos;
                                List<Selection> sels = _pickupBox.Select(_drawing.CurrentBlock);
                                if (sels.Count > 0)
                                {
                                    //user directly clicked on entities.
                                    if (e.IsShiftKeyDown())
                                    {
                                        (_drawing.Document as Document).Selections.Remove(sels);
                                    }
                                    else
                                    {
                                        (_drawing.Document as Document).Selections.Add(sels);
                                    }

                                    DrawSelection(sels, e.IsShiftKeyDown());
                                }
                                else
                                {
                                    //start a selection box 
                                    SelRect.Active = true;
                                    SelRect.StartPoint = SelRect.EndPoint = _pos;
                                }
                            }
                            else
                            {
                                //user selected a grip point

                                Database db = (_drawing.Document as Document).Database;
                                Entity entity = db.GetObject(_anchorMgr.currentGripEntityId) as Entity;
                                if (entity != null)
                                {
                                    GripPointMoveCmd gripMoveCmd = new GripPointMoveCmd(
                                        entity, _anchorMgr.currentGripPointIndex, _anchorMgr.currentGripPoint);

                                    cmd = gripMoveCmd;
                                }
                            }
                        }
                    }
                    #endregion
                    break;

                case PointerModes.Select:
                    #region
                    {
                        if (e.IsLeftPressed)
                        {
                            _pickupBox.center = _pos;
                            List<Selection> sels = _pickupBox.Select(_drawing.CurrentBlock);
                            if (sels.Count > 0)
                            {
                                if (e.IsShiftKeyDown())
                                {
                                    (_drawing.Document as Document).Selections.Remove(sels);
                                }
                                else
                                {
                                    (_drawing.Document as Document).Selections.Add(sels);
                                }
                                DrawSelection(sels, e.IsShiftKeyDown());
                            }
                            else
                            {
                                SelRect.Active = true;
                                SelRect.StartPoint = SelRect.EndPoint = _pos;
                            }
                        }
                    }
                    #endregion
                    break;

                case PointerModes.Locate:
                    CurrentSnapPoint = _snapNodesMgr.Snap(_pos);
                    break;

                case PointerModes.Drag:
                    break;

                default:
                    break;
            }

            return new EventResult() { data = cmd };
        }

        public IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Released)
            {
                if (SelRect.Active)
                {
                    List<Selection> sels = SelRect.Select(_drawing.CurrentBlock);
                    if (sels.Count > 0)
                    {
                        if (e.IsShiftKeyDown())
                        {
                            (_drawing.Document as Document).Selections.Remove(sels);
                        }
                        else
                        {
                            (_drawing.Document as Document).Selections.Add(sels);
                        }
                        DrawSelection(sels, e.IsShiftKeyDown());
                    }
                }

                SelRect.Reset();
                Draw();
            }
            _snapNodesMgr.OnMouseUp(e);
            return null;
        }

        public IEventResult OnMouseMove(IMouseEventArgs e)
        {
            _pos.X = e.X;
            _pos.Y = e.Y;
            _loc = _drawing.CanvasToModel(_pos);
            switch (mode)
            {
                case PointerModes.Default:
                    if (SelRect.Active)
                    {
                        SelRect.EndPoint = _pos;
                    }
                    else
                    {
                        CurrentSnapPoint = _anchorMgr.Snap(_pos);
                        _loc = CurrentSnapPoint;
                    }
                    break;

                case PointerModes.Select:
                    if (SelRect.Active)
                    {
                        SelRect.EndPoint = _pos;
                    }
                    else
                    {
                        _loc = _drawing.CanvasToModel(_pos);
                    }
                    break;

                case PointerModes.Locate:
                    CurrentSnapPoint = _snapNodesMgr.Snap(_pos);
                    _loc = CurrentSnapPoint;
                    _snapNodesMgr.OnMouseMove(e);
                    break;

                case PointerModes.Drag:
                    break;

                default:
                    break;
            }

            Draw();
            return null;
        }

        private void Draw()
        {
            _cursor.Draw(mode, _loc, _pickupBox.side, !SelRect.Active);

            SelRect.Draw();

        }

        public IEventResult OnMouseDoubleClick(IMouseEventArgs e)
        {
            switch (mode)
            {
                case PointerModes.Default:
                    if (e.IsLeftPressed)
                    {
                        //if (_anchorMgr.currentGripPoint == null)
                        //{
                        //    //_pickupBox.center = _pos;
                        //    //List<Selection> sels = _pickupBox.Select(_drawing.currentBlock);
                        //    //if (sels.Count > 0)
                        //    //{
                        //    //    foreach (Selection sel in sels)
                        //    //    {
                        //    //        DBObject dbobj = (_drawing.Document as Document).database.GetObject(sel.objectId);
                        //    //        if (dbobj != null && dbobj is MediaTypeNames.Text)
                        //    //        {
                        //    //            (_drawing.Document as Document).selections.Clear();
                        //    //        }
                        //    //    }
                        //    //}
                        //}
                    }
                    break;

                default:
                    break;
            }

            return null;
        }

        public IEventResult OnMouseWheel(IMouseWheelEventArgs e)
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

        public void OnSelectionChanged()
        {
            if (_isShowAnchor)
            {
                _anchorMgr.Update();
            }
        }

        public void UpdateGripPoints()
        {
            _anchorMgr.Clear();
            if (_isShowAnchor)
            {
                _anchorMgr.Update();
            }
        }

        private void DrawSelection(List<Selection> sels, bool isShiftKeyDown)
        {
            foreach (var sel in sels)
            {
                var obj = _drawing.Document.Database.GetObject(sel.objectId);
                if (obj is Entity en)
                {
                    if (en.DrawingVisual != null) en.DrawingVisual.Selected = !isShiftKeyDown;
                    en.Draw();
                }

            }

            if (_isShowAnchor)
            {
                _anchorMgr.Update();
            }
        }



    }
}
