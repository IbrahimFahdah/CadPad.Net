using System;
using System.Collections.Generic;
using System.Text;
using CADPadDB.Maths;

namespace CADPadServices.Interfaces
{
    public interface IPanContoller:IMouseKeyReceiver
    {
        CADVector GetOffset();
        void MoveBy(CADVector offset);
        void MoveTo(CADVector offset);
        void MoveBy(CADPoint point);
        void MoveTo(CADPoint point);
        void Reset();
    }
}
