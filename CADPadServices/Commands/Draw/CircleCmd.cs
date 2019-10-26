using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Draw
{
    public class CircleCmd : DrawCmd
    {
        private Circle _circle = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Circle[1] { _circle }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyCenter = 1,
            Step2_SpecityRadius = 2,
        }
        private Step _step = Step.Step1_SpecifyCenter;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifyCenter;
            this.Pointer.mode = PointerModes.Locate;
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            if (_step == Step.Step1_SpecifyCenter)
            {
                if (e.IsLeftPressed)
                {
                    _circle = Drawing.AppendEntity(new Circle(), DBObjectState.BeingConstructed);
                    _circle.center = this.Pointer.CurrentSnapPoint;
                    _circle.radius = 0;
                    _circle.LayerId = this.document.currentLayerId;
                    _circle.Color = this.document.currentColor;

                    _step = Step.Step2_SpecityRadius;
                }
            }
            else if (_step == Step.Step2_SpecityRadius)
            {
                if (e.IsLeftPressed)
                {
                    _circle.radius = (_circle.center - this.Pointer.CurrentSnapPoint).Length;
                    _circle.LayerId = this.document.currentLayerId;
                    _circle.Color = this.document.currentColor;

                    _mgr.FinishCurrentCommand();
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

            if (_circle != null)
            {
                _circle.radius = (_circle.center - this.Pointer.CurrentSnapPoint).Length;
                _circle.Draw();
            }

            return EventResult.Handled;
        }

     
    }
}
