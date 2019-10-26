using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadServices;
using CADPadServices.Commands.Draw;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Draw
{
    public class PolylineCmd : DrawCmd
    {
        private Polyline _polyline = null;
        private Line _line = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get{ return new Polyline[1] { _polyline }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyStartPoint = 1,
            Step2_SpecifyOtherPoint = 2,
        }
        private Step _step = Step.Step1_SpecifyStartPoint;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifyStartPoint;
            this.Pointer.mode = PointerModes.Locate;
        }

        public override void Terminate()
        {
            if(_line!=null)
            {
                Drawing.RemoveEntity(_line);
            }
            base.Terminate();
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SpecifyStartPoint:
                    if (e.IsLeftPressed)
                    {
                        _polyline = presenter.AppendEntity(new Polyline(), DBObjectState.BeingConstructed);
                        _polyline.AddVertexAt(_polyline.NumberOfVertices, this.Pointer.CurrentSnapPoint);

                        _line = presenter.AppendEntity(new Line(), DBObjectState.BeingConstructed);
                        _line.startPoint = _line.endPoint = this.Pointer.CurrentSnapPoint;
                        _line.Draw();
                        _step = Step.Step2_SpecifyOtherPoint;
                    }
                    break;

                case Step.Step2_SpecifyOtherPoint:
                    if (e.IsLeftPressed)
                    {
                        _polyline.AddVertexAt(_polyline.NumberOfVertices, this.Pointer.CurrentSnapPoint);
                        _polyline.Draw();

                        _line.startPoint = this.Pointer.CurrentSnapPoint;
                        _line.Draw();
                       
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
                case Step.Step1_SpecifyStartPoint:
                    break;

                case Step.Step2_SpecifyOtherPoint:
                    if (_line != null)
                    {
                        _line.endPoint = this.Pointer.CurrentSnapPoint;
                        _line.Draw();
                    }
                    break;
            }
            
            return EventResult.Handled;
        }

        public override IEventResult OnKeyDown(IKeyEventArgs e)
        {
            if (e.IsEscape)
            {
                if (_polyline.NumberOfVertices > 1)
                {
                    _polyline.LayerId = this.document.currentLayerId;
                    _polyline.Color = this.document.currentColor;
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

   
    }
}
