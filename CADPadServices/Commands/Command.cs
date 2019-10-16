using CADPadDB;
using CADPadServices.ApplicationServices;
using CADPadServices.Enums;
using CADPadServices.Interfaces;
using ICommand = CADPadServices.Interfaces.ICommand;

namespace CADPadServices.Commands
{
    public  abstract class Command : ICommand,IMouseKeyReceiver
    {
        /// <summary>
        /// 命令管理器
        /// </summary>
        protected CommandsMgr _mgr = null;
        public CommandsMgr cmdMgr
        {
            get { return _mgr; }
            set { _mgr = value; }
        }

        public IDrawing presenter
        {
            get { return _mgr.presenter; }
        }

        public Document document
        {
            get { return _mgr.presenter.Document as Document; }
        }

        public Database database
        {
            get { return (_mgr.presenter.Document as Document).database; }
        }

        public IPointerContoller Pointer
        {
            get { return _mgr.presenter.Pointer; }
        }

        //public IDynamicInputer dynamicInputer
        //{
        //    get { return _mgr.presenter.dynamicInputer; }
        //}

        protected PointerModes _lastPointerMode;
        protected bool _lastShowAnchor;

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            _lastPointerMode = this.Pointer.mode;
            _lastShowAnchor = this.Pointer.isShowAnchor;

            this.Pointer.isShowAnchor = false;
        }

        /// <summary>
        /// 结束
        /// </summary>
        public virtual void Terminate()
        {
           this.Pointer.mode = _lastPointerMode;
           this.Pointer.isShowAnchor = _lastShowAnchor;
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public virtual void Undo()
        {
            this.Rollback();
        }

        /// <summary>
        /// 重做
        /// </summary>
        public virtual void Redo()
        {
            this.Commit();
        }

        /// <summary>
        /// 完成
        /// </summary>
        public virtual void Finish()
        {
            this.Commit();
            this.Terminate();
        }

        /// <summary>
        /// 取消
        /// </summary>
        public virtual void Cancel()
        {
            this.Terminate();
        }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        protected virtual void Commit()
        {
        }

        /// <summary>
        /// 回滚撤销
        /// </summary>
        protected virtual void Rollback()
        {
        }


        public virtual IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            return null;
        }


        public virtual IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            return null;
        }


        public virtual IEventResult OnMouseMove(IMouseEventArgs e)
        {
            return null;
        }


        public virtual IEventResult OnMouseWheel(IMouseWheelEventArgs e)
        {
            return null;
        }

        public IEventResult OnMouseDoubleClick(IMouseEventArgs e)
        {
            return null;
        }

        public virtual IEventResult OnKeyDown(IKeyEventArgs e)
        {
            return null;
        }


        public virtual IEventResult OnKeyUp(IKeyEventArgs e)
        {
            return null;
        }



  
    }
}
