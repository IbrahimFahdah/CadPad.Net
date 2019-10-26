using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Draw
{
    public class RayCmd : DrawCmd
    {
        private List<Ray> _xlines = new List<Ray>();
        private Ray _currXline = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return _xlines.ToArray(); }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyBasePoint = 1,
            Step2_SpecifyOtherPoint = 2,
        }
        private Step _step = Step.Step1_SpecifyBasePoint;

        private void GotoStep(Step step)
        {
            switch (step)
            {
                case Step.Step1_SpecifyBasePoint:
                    {
                        this.Pointer.mode = PointerModes.Locate;
                  //      _pointInput.Message = "指定点: ";
                  //      this.dynamicInputer.StartInput(_pointInput);
                    }
                    break;

                case Step.Step2_SpecifyOtherPoint:
                    {
                        this.Pointer.mode = PointerModes.Locate;
                    //    _pointInput.Message = "指定通过点: ";
                 //       this.dynamicInputer.StartInput(_pointInput);
                    }
                    break;
            }

            _step = step;
        }

        /// <summary>
        /// 点动态输入控件
        /// </summary>
     //   private IDynInputPoint _pointInput = null;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

           // _pointInput = presenter.dynamicInputer.CreateDynInputPoint(this.presenter, new Vector2(0, 0));
          //  _pointInput.finish += this.OnPointInputReturn;
         //   _pointInput.cancel += this.OnPointInputReturn;

            this.GotoStep(Step.Step1_SpecifyBasePoint);
        }

        /// <summary>
        /// 结束
        /// </summary>
        public override void Terminate()
        {
          //  _pointInput.finish -= this.OnPointInputReturn;
         //   _pointInput.cancel -= this.OnPointInputReturn;

            base.Terminate();
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SpecifyBasePoint:
                    if (e.IsLeftPressed)
                    {
                        _currXline = presenter.AppendEntity(new Ray(), DBObjectState.BeingConstructed);
                        _currXline.basePoint = this.Pointer.CurrentSnapPoint;
                        _currXline.LayerId = this.document.currentLayerId;
                        _currXline.Color = this.document.currentColor;

                        this.GotoStep(Step.Step2_SpecifyOtherPoint);
                    }
                    break;

                case Step.Step2_SpecifyOtherPoint:
                    if (e.IsLeftPressed)
                    {
                        CADVector dir = (this.Pointer.CurrentSnapPoint
                            - _currXline.basePoint).normalized;
                        if (dir.X != 0 || dir.Y != 0)
                        {
                            _currXline.direction = dir;
                            _currXline.LayerId = this.document.currentLayerId;
                            _currXline.Color = this.document.currentColor;
                            _xlines.Add(_currXline);

                            _currXline = Drawing.AppendEntity((Ray)_currXline.Clone(), DBObjectState.BeingConstructed);
                            _currXline.Draw();
                            //_currXline = _currXline.Clone() as Ray;
                        }
                    }
                    break;
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

            if (_currXline != null
                && _step == Step.Step2_SpecifyOtherPoint)
            {
                CADVector dir = (this.Pointer.CurrentSnapPoint
                            - _currXline.basePoint).normalized;
                if (dir.X != 0 || dir.Y != 0)
                {
                    _currXline.direction = dir;
                    _currXline.Draw();
                }
            }

            return EventResult.Handled;
        }

        public override IEventResult OnKeyDown(IKeyEventArgs e)
        {
            if (e.IsEscape)
            {
                if (_xlines.Count > 0)
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
        //    if (xyRet != null
        //        && xyRet.status == DynInputStatus.OK)
        //    {
        //        switch (_step)
        //        {
        //            case Step.Step1_SpecifyBasePoint:
        //                {
        //                    _currXline = new Ray();
        //                    _currXline.basePoint = xyRet.value;
        //                    _currXline.LayerId = this.document.currentLayerId;
        //                    _currXline.Color = this.document.currentColor;

        //                    this.GotoStep(Step.Step2_SpecifyOtherPoint);
        //                }
        //                break;

        //            case Step.Step2_SpecifyOtherPoint:
        //                {
        //                    Vector2 dir = (xyRet.value
        //                        - _currXline.basePoint).normalized;
        //                    if (dir.x != 0 || dir.y != 0)
        //                    {
        //                        _currXline.direction = dir;
        //                        _currXline.LayerId = this.document.currentLayerId;
        //                        _currXline.Color = this.document.currentColor;
        //                        _xlines.Add(_currXline);

        //                        _currXline = _currXline.Clone() as Ray;
        //                    }

        //                    this.GotoStep(Step.Step2_SpecifyOtherPoint);
        //                }
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        if (_xlines.Count > 0)
        //        {
        //            _mgr.FinishCurrentCommand();
        //        }
        //        else
        //        {
        //            _mgr.CancelCurrentCommand();
        //        }
        //    }
        //}
    }
}
