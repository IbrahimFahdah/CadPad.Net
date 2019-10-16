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
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //_mgr.presenter.selections.Clear();
            //this.Pointer.isShowAnchor = false;
        }

        /// <summary>
        /// 结束
        /// </summary>
        public override void Terminate()
        {
           // _mgr.presenter.selections.Clear();

            base.Terminate();
        }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        protected override void Commit()
        {
            foreach (Entity entity in this.newEntities)
            {
                entity.State = DBObjectState.Default;
                //this.presenter.AppendEntity(entity);
            }
        }

        /// <summary>
        /// 回滚撤销
        /// </summary>
        protected override void Rollback()
        {
            foreach (Entity entity in this.newEntities)
            {
                entity.Erase();
            }
        }
    }
}
