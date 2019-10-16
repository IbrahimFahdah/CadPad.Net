using CADPadDB;
using CADPadDB.Maths;

namespace CADPadServices.ESelection
{
    public struct Selection
    {
        private ObjectId _objectId;
        public ObjectId objectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }

        private CADPoint _position;
        public CADPoint position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Selection(ObjectId objectId, CADPoint pickPosition)
        {
            _objectId = objectId;
            _position = pickPosition;
        }
    }
}
