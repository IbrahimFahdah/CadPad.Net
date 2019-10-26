namespace CADPadServices.Interfaces
{
    public interface IEventResult
    {
        object data { get; set; }
        EventResultStatus status { get; set; }
    }
}