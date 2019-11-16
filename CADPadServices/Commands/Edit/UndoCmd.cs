namespace CADPadServices.Commands.Edit
{
    public class UndoCmd : Command
    {
        public override void Initialize()
        {
            base.Initialize();

            _mgr.FinishCurrentCommand();
        }
    }
}
