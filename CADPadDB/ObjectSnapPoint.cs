using CADPadDB.Maths;

namespace CADPadDB
{

    public class ObjectSnapPoint
    {

        public ObjectSnapMode Type { get; set; } = ObjectSnapMode.Undefined;


        public CADPoint Position { get; set; } = new CADPoint(0, 0);


        public ObjectSnapPoint(ObjectSnapMode type, CADPoint pos)
        {
            Type = type;
            Position = pos;
        }
    }
}
