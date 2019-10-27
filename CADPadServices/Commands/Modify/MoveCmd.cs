using System.Collections.Generic;
using System.Drawing;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Modify
{

    public class MoveCmd : ModifyCmd
    {

        private List<Entity> _items = new List<Entity>();
        private List<Entity> _copys = new List<Entity>();
        private Line _pathLine = null;
        private void InitItems()
        {
            Document doc = _mgr.presenter.Document as Document;
            foreach (Selection sel in _mgr.presenter.selections)
            {
                DBObject dbobj = doc.database.GetObject(sel.objectId);
                if (dbobj != null && dbobj is Entity)
                {
                    Entity entity = dbobj as Entity;
                    Entity copy = entity.Clone() as Entity;

                    _items.Add(entity);
                    _copys.Add(copy);
                    _mgr.presenter.AppendEntity(copy,DBObjectState.Unconfirmed);
                }
            }

            _pathLine = presenter.AppendEntity(new Line(), DBObjectState.Unconfirmed);
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SelectObjects = 1,
            Step2_SpecifyBasePoint = 2,
            Step3_SpecifySecondPoint = 3,
        }
        private Step _step = Step.Step1_SelectObjects;

        private CADVector translation
        {
            get
            {
                return _pathLine.endPoint - _pathLine.startPoint;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            //
            if (_mgr.presenter.selections.Count > 0)
            {
                _step = Step.Step2_SpecifyBasePoint;
                InitItems();
                this.Pointer.mode = PointerModes.Locate;
            }
            else
            {
                _step = Step.Step1_SelectObjects;
                this.Pointer.mode = PointerModes.Select;
            }
        }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        protected override void Commit()
        {
            foreach (Entity item in _items)
            {
                item.Translate(translation);
                item.Draw();
            }
        }

        /// <summary>
        /// 回滚撤销
        /// </summary>
        protected override void Rollback()
        {
            foreach (Entity item in _items)
            {
                item.Translate(-translation);
                item.Draw();
            }
        }

        public override void Terminate()
        {
            if (_pathLine != null)
            {
                presenter.RemoveEntity(_pathLine);
            }
            foreach (Entity copy in _copys)
            {
                presenter.RemoveEntity(copy);
            }
            base.Terminate();
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            if (_step == Step.Step1_SelectObjects)
            {
            }
            else if (_step == Step.Step2_SpecifyBasePoint)
            {
                if (e.IsLeftPressed)
                {
                   
                    _pathLine.startPoint = this.Pointer.CurrentSnapPoint;
                    _pathLine.endPoint = _pathLine.startPoint;
                    _pathLine.Draw();

                     _step = Step.Step3_SpecifySecondPoint;
                }
            }
            else if (_step == Step.Step3_SpecifySecondPoint)
            {
                if (e.IsLeftPressed)
                {
                    _pathLine.endPoint = this.Pointer.CurrentSnapPoint;

                    _mgr.FinishCurrentCommand();
                }
            }

            return EventResult.Handled;
        }

        public override IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            if (_step == Step.Step1_SelectObjects)
            {
                if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Released)
                {
                    if (_mgr.presenter.selections.Count > 0)
                    {
                        _step = Step.Step2_SpecifyBasePoint;
                        InitItems();

                        this.Pointer.mode = PointerModes.Locate;
                    }
                    else
                    {
                        _mgr.CancelCurrentCommand();
                    }
                }
            }
            else if (_step == Step.Step2_SpecifyBasePoint)
            {
            }
            else if (_step == Step.Step3_SpecifySecondPoint)
            {
            }

            return EventResult.Handled;
        }

        public override IEventResult OnMouseMove(IMouseEventArgs e)
        {
            if (e.IsRightPressed)
            {
                return EventResult.Unhandled;
            }

            if (_step == Step.Step1_SelectObjects)
            {
            }
            else if (_step == Step.Step2_SpecifyBasePoint)
            {
            }
            else if (_step == Step.Step3_SpecifySecondPoint)
            {
                CADVector preTranslation = translation;
                _pathLine.endPoint = this.Pointer.CurrentSnapPoint;
                CADVector offset = translation - preTranslation;
                foreach (Entity copy in _copys)
                {
                    copy.Translate(offset);
                    copy.Draw();
                }

                _pathLine.endPoint = this.Pointer.CurrentSnapPoint;
                _pathLine.Draw();
            }

            return EventResult.Handled;
        }

        public override IEventResult OnKeyDown(IKeyEventArgs e)
        {
            if (e.IsEscape)
            {
                if (_copys.Count > 0)
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

    }
}
