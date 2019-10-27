using System.Drawing;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands
{
    /// <summary>
    /// 夹点移动命令
    /// </summary>
    public class GripPointMoveCmd : Command
    {
        /// <summary>
        /// 夹点
        /// </summary>
        protected GripPoint _gripPoint = null;
        protected int _index = -1;

        protected CADPoint _originalGripPos;
        protected CADPoint _resultGripPos;

        /// <summary>
        /// 图元
        /// </summary>
        protected Entity _entity = null;
        protected Entity _entityCopy = null;
        private IDrawingVisual tmpLine = null;
        /// <summary>
        /// 鼠标位置(世界坐标系)
        /// </summary>
        protected CADPoint _mousePosInWorld;

        /// <summary>
        /// 构造函数
        /// </summary>
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

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            presenter.AppendEntity(_entityCopy, DBObjectState.Unconfirmed);
            tmpLine = presenter.CreateTempVisual();
            this.Pointer.isShowAnchor = true;
            this.Pointer.mode = PointerModes.Locate;
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public override void Undo()
        {
            base.Undo();
            _entity.SetGripPointAt(_index, _gripPoint, _originalGripPos);
        }

        /// <summary>
        /// 重做
        /// </summary>
        public override void Redo()
        {
            base.Redo();
            _entity.SetGripPointAt(_index, _gripPoint, _resultGripPos);
        }

        /// <summary>
        /// 完成
        /// </summary>
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

        /// <summary>
        /// 撤销
        /// </summary>
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
            //this.anchor.OnDraw(_mgr.presenter, g, Color.Red);
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
