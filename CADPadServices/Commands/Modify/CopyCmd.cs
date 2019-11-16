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

    public class CopyCmd : ModifyCmd
    {

        private List<Entity> _items = new List<Entity>();
        private List<Entity> _tempItemsToDraw = new List<Entity>();
        private Line _pathLine = null;
        private void InitializeItemsToCopy()
        {
            Document doc = _mgr.presenter.Document as Document;
            foreach (Selection sel in _mgr.presenter.selections)
            {
                DBObject dbobj = doc.Database.GetObject(sel.objectId);
                if (dbobj != null && dbobj is Entity)
                {
                    Entity entity = dbobj as Entity;
                    _items.Add(entity);

                    Entity tempEntity = _mgr.presenter.AppendEntity((Entity)entity.Clone(), DBObjectState.Unconfirmed);  
                    _tempItemsToDraw.Add(tempEntity);
                }
            }

            _pathLine = presenter.AppendEntity(new Line(), DBObjectState.Unconfirmed);
        }

       
        private class CopyAction
        {
            public List<Entity> copyItems = new List<Entity>();
            public Line pathLine ;
        }
        private List<CopyAction> _actions = new List<CopyAction>();
        private void FinishOneCopyAction()
        {
            CopyAction copyAction = new CopyAction();

            foreach (Entity item in _items)
            {
               // Entity copyItem = item.Clone() as Entity;
               // copyItem.Translate(translation);
                Entity copyItem = _mgr.presenter.AppendEntity((Entity)item.Clone());
                copyItem.Translate(translation);
                copyItem.Draw();
                copyAction.copyItems.Add(copyItem);
            }
            copyAction.pathLine = _pathLine;

            _actions.Add(copyAction);
        }


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
            if (presenter.selections.Count > 0)
            {
                _step = Step.Step2_SpecifyBasePoint;
                InitializeItemsToCopy();
                this.Pointer .mode = PointerModes.Locate;
            }
            else
            {
                _step = Step.Step1_SelectObjects;
                this.Pointer.mode = PointerModes.Select;
            }
        }

        protected override void Commit()
        {
            //foreach (CopyAction action in _actions)
            //{
            //    foreach (Entity copyItem in action.copyItems)
            //    {
            //        copyItem.State = DBObjectState.Default;
            //       // _mgr.presenter.AppendEntity(copyItem);
            //    }
            //}
        }

        public override void Redo()
        {
            foreach (CopyAction action in _actions)
            {
                foreach (Entity copyItem in action.copyItems)
                {
                    this.presenter.AppendEntity(copyItem, reUseVisual: true);
                    copyItem.Draw();
                }
            }

            base.Redo();
        }


        protected override void Rollback()
        {
            foreach (CopyAction action in _actions)
            {
                foreach (Entity copyItem in action.copyItems)
                {
                    _mgr.presenter.RemoveEntity(copyItem);
                    //copyItem.Erase();
                }
            }
        }

        public override void Terminate()
        {
            if (_pathLine != null)
            {
                presenter.RemoveEntity(_pathLine);
            }
            foreach (Entity copy in _tempItemsToDraw)
            {
                presenter.RemoveEntity(copy);
            }
            base.Terminate();
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SelectObjects:
                    break;

                case Step.Step2_SpecifyBasePoint:
                    if (e.IsLeftPressed)
                    {
                        _pathLine.startPoint = this.Pointer.CurrentSnapPoint;
                        _pathLine.endPoint = _pathLine.startPoint;
                        _pathLine.Draw();

                        _step = Step.Step3_SpecifySecondPoint;
                    }
                    break;

                case Step.Step3_SpecifySecondPoint:
                    if (e.IsLeftPressed)
                    {
                        _pathLine.endPoint = this.Pointer.CurrentSnapPoint;

                        FinishOneCopyAction();
                    }
                    break;

                default:
                    break;
            }

            return EventResult.Handled;
        }

        public override IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SelectObjects:
                    if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Released)
                    {
                        if (_mgr.presenter.selections.Count > 0)
                        {
                            _step = Step.Step2_SpecifyBasePoint;
                            InitializeItemsToCopy();

                            this.Pointer.mode = PointerModes.Locate;
                        }
                        else
                        {
                            _mgr.CancelCurrentCommand();
                        }
                    }
                    break;

                case Step.Step2_SpecifyBasePoint:
                    break;

                case Step.Step3_SpecifySecondPoint:
                    break;

                default:
                    break;
            }

            return EventResult.Handled;
        }

        public override IEventResult OnMouseMove(IMouseEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SelectObjects:
                    break;

                case Step.Step2_SpecifyBasePoint:
                    break;

                case Step.Step3_SpecifySecondPoint:
                    CADVector preTranslation = translation;
                    _pathLine.endPoint = this.Pointer.CurrentSnapPoint;

                    CADVector offset = translation - preTranslation;
                    foreach (Entity tempEntity in _tempItemsToDraw)
                    {
                        tempEntity.Translate(offset);
                        tempEntity.Draw();
                    }
                    //foreach (CopyAction action in _actions)
                    //{
                    //    foreach (Entity entity in action.copyItems)
                    //    {
                    //        entity.Draw();
                    //    }
                    //}

                    _pathLine.endPoint = this.Pointer.CurrentSnapPoint;
                    _pathLine.Draw();
                    break;

                default:
                    break;
            }

            return EventResult.Handled;
        }

        public override IEventResult OnKeyDown(IKeyEventArgs e)
        {
            if (e.IsEscape)
            {
                //if (_step == Step.Step3_SpecifySecondPoint)
                //{
                if (_actions.Count > 0)
                {
                    _mgr.FinishCurrentCommand();
                }
                else
                {
                    _mgr.CancelCurrentCommand();
                }
                //}
            }

            return EventResult.Handled;
        }

        public override IEventResult OnKeyUp(IKeyEventArgs e)
        {
            return EventResult.Handled;
        }

    
    }
}
