using System;
using System.Collections.Generic;
using System.Text;
using CADPadDB.Maths;

namespace CADPadServices.Interfaces
{
    public interface IPanContoller:IMouseKeyReceiver
    {
        CADVector GetOffset();
    }
}
