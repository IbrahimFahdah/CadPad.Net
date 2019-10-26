using CADPadDB.Maths;
using CADPadServices.Commands;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices.Interfaces
{
    public interface IPointerContoller: IMouseKeyReceiver
    {
        bool isShowAnchor { get; set; }
        PointerModes mode { get; set; }

        CADPoint  CurrentSnapPoint { get; }

   
        void OnSelectionChanged();

        void UpdateGripPoints();
    }
}