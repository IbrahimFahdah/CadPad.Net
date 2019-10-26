using System;
using System.Collections.Generic;
using System.Text;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class EventResult: IEventResult
    {
        public EventResultStatus status  { get; set; }=EventResultStatus.Invalid;

        public object data { get; set; } = null;

        public static IEventResult Unhandled
        {
            get
            {
                EventResult eRet = new EventResult();
                eRet.status = EventResultStatus.Unhandled;
                return eRet;
            }
        }

        public static IEventResult Handled
        {
            get
            {
                EventResult eRet = new EventResult();
                eRet.status = EventResultStatus.Handled;
                return eRet;
            }
        }
    }


    public enum EventResultStatus
    {
 
        Invalid = 0,
     
        Handled = 1,

        Unhandled = 2,
    }
}
