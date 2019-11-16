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

    public class MirrorCmd : ModifyCmd
    {

        private List<Entity> _entities = new List<Entity>();

        private List<Entity> _resultEntities = new List<Entity>();


        private bool _isSrcDeleted = false;


        private IDrawingVisual _mirrorLine = null;
        protected CADPoint startPoint;
        protected CADPoint endPoint;


        private enum Step
        {
            Step1_SelectObject = 1,
            Step2_SpecifyMirrorLinePoint1st = 2,
            Step3_SpecifyMirrorLinePoint2nd = 3,
            Step4_WhetherDelSrc = 4,
        }
        private Step _step = Step.Step1_SelectObject;

        public override void Initialize()
        {
            base.Initialize();

            if (presenter.selections.Count > 0)
            {
                foreach (Selection sel in presenter.selections)
                {
                    Entity entity = database.GetObject(sel.objectId) as Entity;
                    if (entity != null)
                    {
                        _entities.Add(entity);
                    }
                }

            }

            if (_entities.Count > 0)
            {
                this.Pointer.mode = PointerModes.Locate;
                _step = Step.Step2_SpecifyMirrorLinePoint1st;
            }
            else
            {
                presenter.selections.Clear();
                _step = Step.Step1_SelectObject;
                this.Pointer.mode = PointerModes.Select;
            }  
            
            _mirrorLine = presenter.CreateTempVisual(); 

        }


        protected override void Commit()
        {
            foreach (Entity item in _resultEntities)
            {
                item.State = DBObjectState.Default;
               // _mgr.presenter.AppendEntity(item);
            }
        }

        public override void Redo()
        {

            foreach (Entity entity in this._resultEntities)
            {
                this.presenter.AppendEntity(entity, reUseVisual: true);
                entity.Draw();
            }

            base.Redo();
        }


        protected override void Rollback()
        {
            foreach (Entity item in _resultEntities)
            {
                presenter.RemoveEntity(item);
               // item.Erase();
            }
        }

        public override void Terminate()
        {
        
            presenter.RemoveTempVisual(_mirrorLine);
            presenter.RemoveUnconfirmed();
            base.Terminate();
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SelectObject:
                    break;

                case Step.Step2_SpecifyMirrorLinePoint1st:
                    if (e.IsLeftPressed)
                    {
                       // _mirrorLine = new Line();
                         startPoint = this.Pointer.CurrentSnapPoint;
                         endPoint = startPoint;
                        DrawMirrorLine();
                        _step = Step.Step3_SpecifyMirrorLinePoint2nd;
                    }
                    break;

                case Step.Step3_SpecifyMirrorLinePoint2nd:
                    if (e.IsLeftPressed)
                    {
                        endPoint = this.Pointer.CurrentSnapPoint;
                        DrawMirrorLine();
                        UpdateResultEntities();

                        _step = Step.Step4_WhetherDelSrc;
                        _mgr.FinishCurrentCommand();
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
                case Step.Step1_SelectObject:
                    if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Released)
                    {
                        if (presenter.selections.Count > 0)
                        {
                            foreach (Selection sel in _mgr.presenter.selections)
                            {
                                DBObject dbobj = database.GetObject(sel.objectId);
                                Entity entity = dbobj as Entity;
                                _entities.Add(entity);
                            }

                            this.Pointer.mode = PointerModes.Locate;
                            _step = Step.Step2_SpecifyMirrorLinePoint1st;
                        }
                        else
                        {
                            _mgr.CancelCurrentCommand();
                        }
                    }
                    break;

                case Step.Step2_SpecifyMirrorLinePoint1st:
                    break;

                case Step.Step3_SpecifyMirrorLinePoint2nd:
                    break;

                case Step.Step4_WhetherDelSrc:
                    break;

                default:
                    break;
            }

            return EventResult.Handled;
        }

        public override IEventResult OnMouseMove(IMouseEventArgs e)
        {
            if (_step == Step.Step3_SpecifyMirrorLinePoint2nd)
            {
                endPoint = this.Pointer.CurrentSnapPoint;
                DrawMirrorLine();
                UpdateResultEntities();
            }

            return EventResult.Handled;
        }

        public override IEventResult OnKeyDown(IKeyEventArgs e)
        {
            if (e.IsEscape)
            {
                _mgr.CancelCurrentCommand();
            }

            return EventResult.Handled;
        }

        public override IEventResult OnKeyUp(IKeyEventArgs e)
        {
            return EventResult.Handled;
        }

        private void DrawMirrorLine()
        {
            _mirrorLine .Open();
            _mirrorLine.DrawLine(startPoint, endPoint);
            _mirrorLine.Close();
        }

        Matrix3 _mirrorMatrix;
        private void UpdateResultEntities()
        {
            Matrix3 mirrorMatrix = MathUtils.MirrorMatrix(
                        new Line2(startPoint, endPoint));

            if(_resultEntities.Count==0)
            {
                foreach (Entity entity in _entities)
                {
                    var copy = _mgr.presenter.AppendEntity((Entity)entity.Clone(), DBObjectState.Unconfirmed);
                    _resultEntities.Add(copy);
                    copy.Draw();
                }
           
            }

           
            foreach (Entity entity in _resultEntities)
            {
                // Entity copy = entity.Clone() as Entity;
                //   var copy=_mgr.presenter.AppendEntity((Entity)entity.Clone(), DBObjectState.BeingConstructed);
   
                entity.TransformBy(_mirrorMatrix.inverse);
                entity.TransformBy(mirrorMatrix);
                // _resultEntities.Add(copy);
                entity.Draw();
            }
            _mirrorMatrix = mirrorMatrix;

        }
    }
}
