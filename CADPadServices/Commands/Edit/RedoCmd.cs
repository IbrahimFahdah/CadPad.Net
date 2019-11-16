namespace CADPadServices.Commands.Edit
{
    public class RedoCmd : Command
    {
        public override void Initialize()
        {
            base.Initialize();

            _mgr.FinishCurrentCommand();
        }
    }
}
