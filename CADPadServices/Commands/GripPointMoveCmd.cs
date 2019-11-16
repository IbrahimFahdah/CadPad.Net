using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands
{

    public class GripPointMoveCmd : Command
    {

        protected GripPoint _gripPoint = null;
        protected int _index = -1;

        protected CADPoint _originalGripPos;
        protected CADPoint _resultGripPos;


        protected Entity _entity = null;
        protected Entity _entityCopy = null;
        private IDrawingVisual tmpLine = null;

        protected CADPoint _mousePosInWorld;


        public GripPointMoveCmd(Entity entity, int index, GripPoint gripPoint)
        {
            _entity = entity;
            _entityCopy = entity.Clone() as Entity;
            _index = index;
            _gripPoint = gripPoint;

            _originalGripPos = _gripPoint.Position;
            _resultGripPos = _gripPoint.Position;
            _mousePosInWorld = _gripPoint.Position;
        }


        public override void Initialize()
        {
            base.Initialize();

            presenter.AppendEntity(_entityCopy, DBObjectState.Unconfirmed);
            tmpLine = presenter.CreateTempVisual();
            this.Pointer.isShowAnchor = true;
            this.Pointer.mode = PointerModes.Locate;
        }


        public override void Undo()
        {
            base.Undo();
            _entity.SetGripPointAt(_index, _gripPoint, _originalGripPos);
            _entity.Draw();
            Pointer.UpdateGripPoints();
        }


        public override void Redo()
        {
            base.Redo();
            _entity.SetGripPointAt(_index, _gripPoint, _resultGripPos);
            _entity.Draw();
            Pointer.UpdateGripPoints();
        }

        public override void Finish()
        {
            _resultGripPos = _mousePosInWorld;
            _entity.SetGripPointAt(_index, _gripPoint, _resultGripPos);
            _entity.Draw();
            presenter.RemoveEntity(_entityCopy);
            presenter.RemoveTempVisual(tmpLine);
            Pointer.UpdateGripPoints();
            base.Finish();
        }


        public override void Cancel()
        {
            base.Cancel();
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            if (e.IsLeftPressed)
            {
                _mousePosInWorld = this.Pointer.CurrentSnapPoint;
                _mgr.FinishCurrentCommand();
            }

            return EventResult.Handled;
        }

        public override IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            return EventResult.Handled;
        }

        public override IEventResult OnMouseMove(IMouseEventArgs e)
        {
            _mousePosInWorld = this.Pointer.CurrentSnapPoint;
            _entityCopy.SetGripPointAt(_index, _gripPoint, _mousePosInWorld);
            Draw();
            return EventResult.Handled;
        }

        public override IEventResult OnKeyDown(IKeyEventArgs e)
        {
            if (e.IsEscape)
            {
                presenter.RemoveEntity(_entityCopy);
                _mgr.CancelCurrentCommand();
            }

            return EventResult.Handled;
        }

        public override IEventResult OnKeyUp(IKeyEventArgs e)
        {
            return EventResult.Handled;
        }

        public void Draw()
        {
            this.DrawPath();
            _entityCopy.Draw();
        }

        protected virtual void DrawPath()
        {
            tmpLine.Open();
            tmpLine.DrawLine( _originalGripPos, _mousePosInWorld);
            tmpLine.Close();
        }
    }
}
