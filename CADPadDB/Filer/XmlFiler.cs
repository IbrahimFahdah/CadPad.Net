using CADPadDB.Colors;
using CADPadDB.Maths;


namespace CADPadDB.Filer
{

    public abstract class XmlFiler
    {

        public abstract bool Write(string name, string value);
        public abstract bool Write(string name, bool value);
        public abstract bool Write(string name, byte value);
        public abstract bool Write(string name, uint value);
        public abstract bool Write(string name, int value);
        public abstract bool Write(string name, double value);
        public abstract bool Write(string name, CADPoint value);
        public abstract bool Write(string name, CADColor color);
        public abstract bool Write(string name, ObjectId value);
        public abstract bool Write(string name, LineWeight value);


        public abstract bool Read(string name, out string value);
        public abstract bool Read(string name, out bool value);
        public abstract bool Read(string name, out byte value);
        public abstract bool Read(string name, out uint value);
        public abstract bool Read(string name, out int value);
        public abstract bool Read(string name, out double value);
        public abstract bool Read(string name, out CADPoint value);
        public abstract bool Read(string name, out CADColor color);
        public abstract bool Read(string name, out ObjectId value);
        public abstract bool Read(string name, out LineWeight value);
    }
}
