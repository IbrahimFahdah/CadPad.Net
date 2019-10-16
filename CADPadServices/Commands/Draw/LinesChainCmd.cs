using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Draw
{
    public class LinesChainCmd : DrawCmd
    {
        private List<Line> _lines = new List<Line>();
        private Line _currLine = null;


        protected override IEnumerable<Entity> newEntities
        {
            get { return _lines.ToArray(); }
        }


        private enum Step
        {
            Step1_SpecifyStartPoint = 1,
            Step2_SpecifyEndPoint = 2,
        }
        private Step _step = Step.Step1_SpecifyStartPoint;

        /// <summary>
        /// 点动态输入控件
        /// </summary>
        //private IDynInputPoint _pointInput = null;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifyStartPoint;
            this.Pointer.mode = PointerModes.Locate;

            //_pointInput = presenter.dynamicInputer.CreateDynInputPoint(this.presenter, new Vector2(0, 0));
            //_pointInput.Message = "指定第一个点: ";
            //this.dynamicInputer.StartInput(_pointInput);
            //_pointInput.finish += this.OnPointInputReturn;
            //_pointInput.cancel += this.OnPointInputReturn;
        }

        public override void Terminate()
        {
            // _pointInput.finish -= this.OnPointInputReturn;
            //  _pointInput.cancel -= this.OnPointInputReturn;

            Drawing.RemoveEntity(_currLine);
            base.Terminate();
        }

        #region Events
        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            if (_step == Step.Step1_SpecifyStartPoint)
            {
                if (e.IsLeftPressed)
                {
                    _currLine = Drawing.AppendEntity(new Line(), DBObjectState.BeingConstructed);
                    _currLine.startPoint = this.Pointer.currentSnapPoint;
                    _currLine.endPoint = this.Pointer.currentSnapPoint;
                    _currLine.LayerId = this.document.currentLayerId;
                    _currLine.Color = this.document.currentColor;

                    // _pointInput.Message = "指定下一点: ";
                    _step = Step.Step2_SpecifyEndPoint;
                    _currLine.Draw();


                }
            }
            else if (_step == Step.Step2_SpecifyEndPoint)
            {
                if (e.IsLeftPressed)
                {
                    _currLine.endPoint = this.Pointer.currentSnapPoint;
                    _currLine.LayerId = this.document.currentLayerId;
                    _currLine.Color = this.document.currentColor;
                    _lines.Add(_currLine);

                    _currLine = Drawing.AppendEntity(new Line(), DBObjectState.BeingConstructed);
                    _currLine.startPoint = this.Pointer.currentSnapPoint;
                    _currLine.endPoint = this.Pointer.currentSnapPoint;
                    _currLine.LayerId = this.document.currentLayerId;
                    _currLine.Color = this.document.currentColor;
                    _currLine.Draw();
                }
            }

            return EventResult.Handled;
        }

        public override IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            return EventResult.Handled;
        }

        public override IEventResult OnMouseMove(IMouseEventArgs e)
        {
            if (e.IsMiddlePressed)
            {
                return EventResult.Handled;
            }

            if (_currLine != null)
            {
                _currLine.endPoint = this.Pointer.currentSnapPoint;
                _currLine.Draw();
            }


            return EventResult.Handled;
        }

        public override IEventResult OnKeyDown(IKeyEventArgs e)
        {
            if (e.IsEscape)
            {
                if (_lines.Count > 0)
                {
                    _mgr.FinishCurrentCommand();
                }
                else
                {
                    _mgr.CancelCurrentCommand();
                }
            }
            return EventResult.Handled;
        }

        public override IEventResult OnKeyUp(IKeyEventArgs e)
        {
            return EventResult.Handled;
        }



        //private void OnPointInputReturn(IDynInputCtrl sender, DynInputResult retult)
        //{
        //    DynInputResult<Vector2> xyRet = retult as DynInputResult<Vector2>;
        //    if (xyRet == null 
        //        || xyRet.state == DynInputStatus.Cancel)
        //    {
        //        if (_lines.Count > 0)
        //        {
        //            _mgr.FinishCurrentCommand();
        //        }
        //        else
        //        {
        //            _mgr.CancelCurrentCommand();
        //        }

        //        return;
        //    }

        //    _pointInput.Message = "指定下一点: ";
        //    this.dynamicInputer.StartInput(_pointInput);

        //    switch (_step)
        //    {
        //        case Step.Step1_SpecifyStartPoint:
        //            {
        //                _currLine = new Line();
        //                _currLine.startPoint = xyRet.value;
        //                _currLine.endPoint = xyRet.value;
        //                _currLine.LayerId = this.Document.currentLayerId;
        //                _currLine.Color = this.Document.currentColor;

        //                _step = Step.Step2_SpecifyEndPoint;
        //            }
        //            break;

        //        case Step.Step2_SpecifyEndPoint:
        //            {
        //                _currLine.endPoint = xyRet.value;
        //                _currLine.LayerId = this.Document.currentLayerId;
        //                _currLine.Color = this.Document.currentColor;
        //                _lines.Add(_currLine);

        //                _currLine = new Line();
        //                _currLine.startPoint = xyRet.value;
        //                _currLine.endPoint = xyRet.value;
        //                _currLine.LayerId = this.Document.currentLayerId;
        //                _currLine.Color = this.Document.currentColor;
        //            }
        //            break;
        //    }
        //}

        #endregion
    }
}
