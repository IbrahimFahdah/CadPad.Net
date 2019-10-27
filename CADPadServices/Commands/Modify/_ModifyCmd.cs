using CADPadDB.CADEntity;

namespace CADPadServices.Commands.Modify
{
    public abstract class ModifyCmd : Command
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.Pointer.isShowAnchor = false;
        }

        /// <summary>
        /// 结束
        /// </summary>
        public override void Terminate()
        {
            _mgr.presenter.selections.Clear();
            base.Terminate();
        }
    }
}
