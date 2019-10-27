using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadServices.Enums;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Modify
{

    public class DeleteCmd : ModifyCmd
    {

        private List<Entity> _items = new List<Entity>();
        private void InitializeItemsToDelete()
        {
            Document doc = _mgr.presenter.Document as Document;
            foreach (Selection sel in _mgr.presenter.selections)
            {
                DBObject dbobj = doc.database.GetObject(sel.objectId);
                if (dbobj != null && dbobj is Entity)
                {
                    Entity entity = dbobj as Entity;
                    _items.Add(entity);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            //
            if (_mgr.presenter.selections.Count > 0)
            {
                InitializeItemsToDelete();
                _mgr.FinishCurrentCommand();
            }
            else
            {
                this.Pointer.mode = PointerModes.Select;
            }
        }


        protected override void Commit()
        {
            foreach (Entity item in _items)
            {
                _mgr.presenter.RemoveEntity(item);
               // item.Erase();
            }
        }


        protected override void Rollback()
        {
            foreach (Entity item in _items)
            {
                _mgr.presenter.AppendEntity(item);
            }
        }

        public override IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            return EventResult.Handled;
        }

        public override IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            if (e.ChangedButton==MouseButton.Right && e.ButtonState==MouseButtonState.Released)
            {
                if (_mgr.presenter.selections.Count > 0)
                {
                    InitializeItemsToDelete();
                    _mgr.FinishCurrentCommand();
                }
                else
                {
                    _mgr.CancelCurrentCommand();
                }
            }

            return EventResult.Handled;
        }

        public override IEventResult OnMouseMove(IMouseEventArgs e)
        {
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

       
    }
}
