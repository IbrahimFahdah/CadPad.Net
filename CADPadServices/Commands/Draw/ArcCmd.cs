using System.Collections.Generic;
using System.Drawing;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Draw
{
    /// <summary>
    /// 绘制圆弧命令
    /// </summary>
    public class ArcCmd : DrawCmd
    {
        /// <summary>
        /// 绘制的圆弧
        /// </summary>
        private Arc _arc = null;
        private Line _line = null;
        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Arc[1] { _arc }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyCenter = 1,
            Step2_SpecityStartPoint = 2,
            Step3_SpecifyEndPoint = 3,
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

        public override void Terminate()
        {
            if (_line != null)
            {
                Drawing.RemoveEntity(_line);
            }
            base.Terminate();
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SpecifyCenter:
                    if (e.IsLeftPressed)
                    {
                        _arc = presenter.AppendEntity(new Arc(),DBObjectState.Unconfirmed);
                        _arc.center = this.Pointer.CurrentSnapPoint;
                        _arc.radius = 0;
                        _arc.LayerId = this.document.currentLayerId;
                        _arc.Color = this.document.currentColor;

                        _step = Step.Step2_SpecityStartPoint;

                        _line = presenter.AppendEntity(new Line(), DBObjectState.Unconfirmed);
                        _line.startPoint = _arc.center;
                        _line.endPoint = this.Pointer.CurrentSnapPoint;
                        _line.Draw();
                    }
                    break;

                case Step.Step2_SpecityStartPoint:
                    if (e.IsLeftPressed)
                    {
                        _arc.radius = (_arc.center - this.Pointer.CurrentSnapPoint).Length;
                        _arc.LayerId = this.document.currentLayerId;
                        _arc.Color = this.document.currentColor;

                        double startAngle = CADVector.SignedAngleInRadian(
                            new CADVector(1, 0),
                            this.Pointer.CurrentSnapPoint - _arc.center);
                        startAngle = MathUtils.NormalizeRadianAngle(startAngle);
                        _arc.startAngle = startAngle;
                        _arc.endAngle = startAngle;

                        _step = Step.Step3_SpecifyEndPoint;
                    }
                    break;

                case Step.Step3_SpecifyEndPoint:
                    if (e.IsLeftPressed)
                    {
                        double endAngle = CADVector.SignedAngleInRadian(
                            new CADVector(1, 0),
                            this.Pointer.CurrentSnapPoint - _arc.center);
                        endAngle = MathUtils.NormalizeRadianAngle(endAngle);
                        _arc.endAngle = endAngle;

                        _arc.Draw();
                        _mgr.FinishCurrentCommand();
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

            switch (_step)
            {
                case Step.Step1_SpecifyCenter:
                    break;

                case Step.Step2_SpecityStartPoint:
                    _line.endPoint = this.Pointer.CurrentSnapPoint;
                    _line.Draw();
                    break;

                case Step.Step3_SpecifyEndPoint:
                    double endAngle = CADVector.SignedAngleInRadian(
                            new CADVector(1, 0),
                            this.Pointer.CurrentSnapPoint - _arc.center);
                    endAngle = MathUtils.NormalizeRadianAngle(endAngle);
                    _arc.endAngle = endAngle;
                    _arc.Draw();

                    _line.endPoint = this.Pointer.CurrentSnapPoint;
                    _line.Draw();
                    break;
            }

            return EventResult.Handled;
        }
    }
}
