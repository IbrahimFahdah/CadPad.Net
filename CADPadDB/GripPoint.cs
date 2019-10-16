using CADPadDB.Maths;

namespace CADPadDB
{
    public class GripPoint
    {
        public GripPointType Type { get; set; } = GripPointType.Undefined;

        public CADPoint Position { get; set; } = new CADPoint(0, 0);

        public object xData1 { get; set; } = null;

        public object xData2 { get; set; } = null;

        public GripPoint(GripPointType type, CADPoint position)
        {
            this.Type = type;
            this.Position = position;
        }
    }
}
