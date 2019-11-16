using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadServices.Interfaces;

namespace CADPadServices.Commands.Draw
{
    public abstract class DrawCmd : Command
    {
        protected abstract IEnumerable<Entity> newEntities { get; }

        public IDrawing Drawing;

        public override void Initialize()
        {
            base.Initialize();
            _mgr.presenter.selections.Clear();
            this.Pointer.isShowAnchor = false;
        }


        public override void Terminate()
        {
            _mgr.presenter.selections.Clear();
            base.Terminate();
        }


        public override void Redo()
        {

            foreach (Entity entity in this.newEntities)
            {
                this.presenter.AppendEntity(entity, reUseVisual: true);
                entity.Draw();
            }

            base.Redo();
        }

        protected override void Commit()
        {
            foreach (Entity entity in this.newEntities)
            {
                entity.State = DBObjectState.Default;
                //this.presenter.AppendEntity(entity);
            }
        }


        protected override void Rollback()
        {
            foreach (Entity entity in this.newEntities)
            {
                presenter.RemoveEntity(entity);
               // entity.Erase();
            }
        }
    }
}
