using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Draw
{
    public class EllipseCmd : DrawCmd
    {
        private Ellipse _ellipse = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Ellipse[1] { _ellipse }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyCenter = 1,
            Step2_SpecityRadiusX = 2,
            Step3_SpecityRadiusY = 3,
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
                    _ellipse = Drawing.AppendEntity(new Ellipse(), DBObjectState.BeingConstructed);
                    _ellipse.center = this.Pointer.CurrentSnapPoint;
                    _ellipse.RadiusX = 0;
                    _ellipse.RadiusY = 0;
                    _ellipse.LayerId = this.document.currentLayerId;
                    _ellipse.Color = this.document.currentColor;

                    _step = Step.Step2_SpecityRadiusX;
                }
            }
            else if (_step == Step.Step2_SpecityRadiusX)
            {
                if (e.IsLeftPressed)
                {
                    _ellipse.RadiusX = (_ellipse.center - this.Pointer.CurrentSnapPoint).Length;
                    _ellipse.RadiusY = _ellipse.RadiusX;
                    _step = Step.Step3_SpecityRadiusY;
                }
            }
            else if (_step == Step.Step3_SpecityRadiusY)
            {
                if (e.IsLeftPressed)
                {
                    _ellipse.RadiusY = (_ellipse.center - this.Pointer.CurrentSnapPoint).Length;

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

            switch (_step)
            {
                case Step.Step1_SpecifyCenter:
                    break;

                case Step.Step2_SpecityRadiusX:
                    _ellipse.RadiusX = (_ellipse.center - this.Pointer.CurrentSnapPoint).Length;
                    _ellipse.RadiusY = _ellipse.RadiusX;
                    _ellipse.Draw();
                    break;

                case Step.Step3_SpecityRadiusY:
                    _ellipse.RadiusY = (_ellipse.center - this.Pointer.CurrentSnapPoint).Length;
                    _ellipse.Draw();
                    break;
            }

            return EventResult.Handled;
        }

    }
}
