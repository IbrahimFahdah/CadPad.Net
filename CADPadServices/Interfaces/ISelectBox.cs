using CADPadDB.Maths;
using static CADPadServices.ESelection.SelectBox;

namespace CADPadServices.Interfaces
{
    public interface ISelectBox 
    {
        SelectMode SelectionMode { get;  }
        CADPoint EndPoint { get; }
        CADPoint StartPoint { get; }
    }
}