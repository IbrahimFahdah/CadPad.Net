using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.ApplicationServices;
using CADPadServices.Commands;
using CADPadServices.Enums;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class PointerContoller: IPointerContoller
    {
        protected IDrawing _presenter = null;

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


        public CADPoint  currentSnapPoint { get; protected set; } = new CADPoint(0, 0);


        protected PickupBox.PickupBox _pickupBox = null;
        protected SelectRectangleBase _selRect = null;

        //protected LocateCross _locateCross = null;

        protected SnapNodesMgrBase _snapNodesMgr = null;
        protected AnchorsMgr _anchorMgr = null;

        internal int pickupBoxSide
        {
            get { return _pickupBox.side; }
            set
            {
                if (_pickupBox.side != value)
                {
                    _pickupBox.side = value;
                }
            }
        }

        //internal int locateCrossLength
        //{
        //    get { return _locateCross.length; }
        //    set
        //    {
        //        if (_locateCross.length != value)
        //        {
        //            _locateCross.length = value;
        //            UpdateBitmap();
        //        }
        //    }
        //}

        protected bool _isShowAnchor = true;
        public bool isShowAnchor
        {
            get { return _isShowAnchor; }
            set
            {
                //if (_isShowAnchor != value)
                //{
                //    _anchorMgr.Clear();
                //    _isShowAnchor = value;
                //    if (_isShowAnchor)
                //    {
                //        _anchorMgr.Update();
                //    }
                //}
            }
        }



        public PointerContoller(IDrawing presenter)
        {

            _presenter = presenter;

            _pickupBox = new PickupBox.PickupBox(_presenter);
            _pickupBox.side = 20;

            // _locateCross = new LocateCross(_presenter);
            //  _locateCross.length = 60;

            _snapNodesMgr = new SnapNodesMgrBase(_presenter);
            _anchorMgr = new AnchorsMgr(_presenter);

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
                                List<Selection> sels = _pickupBox.Select(_presenter.CurrentBlock);
                                if (sels.Count > 0)
                                {
                                    //user directly clicked on entities.
                                    if (e.IsShiftKeyDown())
                                    {
                                        (_presenter.Document as Document).selections.Remove(sels);
                                    }
                                    else
                                    {
                                        (_presenter.Document as Document).selections.Add(sels);
                                    }

                                    DrawSelection(sels, e.IsShiftKeyDown());
                                }
                                else
                                {
                                    //start a selection box 
                                    _selRect = _presenter.CreateSelectRectangle();
                                    _selRect.startPoint = _selRect.endPoint = _pos;
                                }
                            }
                            else
                            {
                                Database db = (_presenter.Document as Document).database;
                                Entity entity = db.GetObject(_anchorMgr.currentGripEntityId) as Entity;
                                if (entity != null)
                                {
                                    //GripPointMoveCmd gripMoveCmd = new GripPointMoveCmd(
                                    //    entity, _anchorMgr.currentGripPointIndex, _anchorMgr.currentGripPoint);
                                    //cmd = gripMoveCmd;
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
                            List<Selection> sels = _pickupBox.Select(_presenter.CurrentBlock);
                            if (sels.Count > 0)
                            {
                                if (e.IsShiftKeyDown())
                                {
                                    (_presenter.Document as Document).selections.Remove(sels);
                                }
                                else
                                {
                                    (_presenter.Document as Document).selections.Add(sels);
                                }
                            }
                            else
                            {
                                _selRect = _presenter.CreateSelectRectangle();
                                _selRect.startPoint = _selRect.endPoint = _pos;
                            }
                        }
                    }
                    #endregion
                    break;

                case PointerModes.Locate:
                    currentSnapPoint = _snapNodesMgr.Snap(_pos);
                    break;

                case PointerModes.Drag:
                    break;

                default:
                    break;
            }

            return null;// return cmd;
        }

        public IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            if (e.IsLeftPressed)
            {
                if (_selRect != null)
                {
                    List<Selection> sels = _selRect.Select(_presenter.CurrentBlock);
                    if (sels.Count > 0)
                    {
                        if (e.IsShiftKeyDown())
                        {
                            (_presenter.Document as Document).selections.Remove(sels);
                        }
                        else
                        {
                            (_presenter.Document as Document).selections.Add(sels);
                        }

                    }
                }
                _selRect = null;
            }

            return null;
        }

        public IEventResult OnMouseMove(IMouseEventArgs e)
        {
            _pos.X = e.X;
            _pos.Y = e.Y;

            switch (mode)
            {
                case PointerModes.Default:
                    if (_selRect != null)
                    {
                        _selRect.endPoint = _pos;
                    }
                    else
                    {
                        currentSnapPoint = _anchorMgr.Snap(_pos);
                    }
                    break;

                case PointerModes.Select:
                    if (_selRect != null)
                    {
                        _selRect.endPoint = _pos;
                    }
                    break;

                case PointerModes.Locate:
                    currentSnapPoint = _snapNodesMgr.Snap(_pos);
                    break;

                case PointerModes.Drag:
                    break;

                default:
                    break;
            }

            return null;
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
                        //    //List<Selection> sels = _pickupBox.Select(_presenter.currentBlock);
                        //    //if (sels.Count > 0)
                        //    //{
                        //    //    foreach (Selection sel in sels)
                        //    //    {
                        //    //        DBObject dbobj = (_presenter.Document as Document).database.GetObject(sel.objectId);
                        //    //        if (dbobj != null && dbobj is MediaTypeNames.Text)
                        //    //        {
                        //    //            (_presenter.Document as Document).selections.Clear();
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
            //if (_isShowAnchor)
            //{
            //    _anchorMgr.Update();
            //}
        }

        public  void UpdateGripPoints()
        {
            //_anchorMgr.Clear();
            //if (_isShowAnchor)
            //{
            //    _anchorMgr.Update();
            //}
        }

        private void DrawSelection(List<Selection> sels, bool isShiftKeyDown)
        {
            foreach (var sel in sels)
            {
                var obj = _presenter.Document.database.GetObject(sel.objectId);
                if (obj is Entity en)
                {
                    if (en.DrawingVisual != null) en.DrawingVisual.Selected = !isShiftKeyDown;
                    en.Draw();
                }

            }
        }
       

     
    }
}
