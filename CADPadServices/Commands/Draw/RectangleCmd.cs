using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Draw
{
    public class RectangleCmd : DrawCmd
    {
        private Polyline _rectangle = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Polyline[1] { _rectangle }; }
        }

        // 起点+终点
        private CADPoint _point1st = new CADPoint(0, 0);
        private CADPoint _point2nd = new CADPoint(0, 0);

        private void UpdateRectangle()
        {
            if (_rectangle == null)
            {
                //_rectangle = new Polyline();
                _rectangle = presenter.AppendEntity(new Polyline(), DBObjectState.Unconfirmed);
                _rectangle.closed = true;
                for (int i = 0; i < 4; ++i)
                {
                    _rectangle.AddVertexAt(0, new CADPoint(0, 0));
                }
            }

            _rectangle.SetPointAt(0, _point1st);
            _rectangle.SetPointAt(1, new CADPoint(_point2nd.X, _point1st.Y));
            _rectangle.SetPointAt(2, _point2nd);
            _rectangle.SetPointAt(3, new CADPoint(_point1st.X, _point2nd.Y));
            _rectangle.LayerId = this.document.currentLayerId;
            _rectangle.Color = this.document.currentColor;
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyPoint1st = 1,
            Step2_SpecifyPoint2nd = 2,
        }
        private Step _step = Step.Step1_SpecifyPoint1st;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifyPoint1st;
            this.Pointer.mode = PointerModes.Locate;
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SpecifyPoint1st:
                    if (e.IsLeftPressed)
                    {
                        
                        _point1st = this.Pointer.CurrentSnapPoint;
                        _step = Step.Step2_SpecifyPoint2nd;
                    }
                    break;

                case Step.Step2_SpecifyPoint2nd:
                    if (e.IsLeftPressed)
                    {
                        _point2nd = this.Pointer.CurrentSnapPoint;
                        this.UpdateRectangle();
                        _rectangle.Draw();
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

            if (_step == Step.Step2_SpecifyPoint2nd)
            {
                _point2nd = this.Pointer.CurrentSnapPoint;
                this.UpdateRectangle();
                _rectangle.Draw();
            }

            return EventResult.Handled;
        }

    }
}
