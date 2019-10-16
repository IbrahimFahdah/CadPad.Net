using System.Collections.Generic;
using System.Drawing;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.ApplicationServices;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class AnchorsMgr
    {
        private IDrawing _presenter = null;

        private Dictionary<ObjectId, List<GripPoint>> _gripPnts = new Dictionary<ObjectId, List<GripPoint>>();
        private GripPoint _currGripPoint = null;
        public GripPoint currentGripPoint
        {
            get { return _currGripPoint; }
        }
        private ObjectId _currGripEntityId = ObjectId.Null;
        public ObjectId currentGripEntityId
        {
            get { return _currGripEntityId; }
        }
        private int _currGripPointIndex = -1;
        public int currentGripPointIndex
        {
            get { return _currGripPointIndex; }
        }

        public AnchorsMgr(IDrawing presenter)
        {
            _presenter = presenter;
        }

        public void Update()
        {
            Document doc = _presenter.Document as Document;
            if (doc.selections.Count == 0)
            {
                _gripPnts.Clear();
                return;
            }

            Dictionary<ObjectId, List<GripPoint>> oldGripPnts = _gripPnts;
            _gripPnts = new Dictionary<ObjectId, List<GripPoint>>();
            foreach (Selection sel in doc.selections)
            {
                if (sel.objectId == ObjectId.Null)
                {
                    continue;
                }
                if (oldGripPnts.ContainsKey(sel.objectId))
                {
                    _gripPnts[sel.objectId] = oldGripPnts[sel.objectId];
                    continue;
                }

                DBObject dbobj = doc.database.GetObject(sel.objectId);
                if (dbobj == null)
                {
                    continue;
                }
                Entity entity = dbobj as Entity;
                if (entity == null)
                {
                    continue;
                }

                List<GripPoint> entGripPnts = entity.GetGripPoints();
                if (entGripPnts != null && entGripPnts.Count > 0)
                {
                    _gripPnts[sel.objectId] = entGripPnts;
                }
            }
        }

        public void Clear()
        {
            _gripPnts.Clear();
        }

        //public void OnPaint(IGraphicsContext graphics)
        //{
        //    foreach (KeyValuePair<ObjectId, List<GripPoint>> kvp in _gripPnts)
        //    {
        //        foreach (GripPoint gripPnt in kvp.Value)
        //        {
        //            double width = 10;
        //            double height = 10;
        //            CADPoint posInCanvas = _presenter.ModelToCanvas(gripPnt.Position);
        //            posInCanvas.X -= width / 2;
        //            posInCanvas.Y -= height / 2;
        //            _presenter.FillRectangle(graphics, Color.Blue, posInCanvas, width, height, CSYS.Canvas);
        //        }
        //    }
        //}

        public CADPoint Snap(CADPoint posInCanvas)
        {
            CADPoint posInModel = _presenter.CanvasToModel(posInCanvas);

            foreach (KeyValuePair<ObjectId, List<GripPoint>> kvp in _gripPnts)
            {
                int index = -1;
                foreach (GripPoint gripPnt in kvp.Value)
                {
                    ++index;
                    double width = 10;
                    double height = 10;
                    CADPoint gripPosInCanvas = _presenter.ModelToCanvas(gripPnt.Position);
                    gripPosInCanvas.X -= width / 2;
                    gripPosInCanvas.Y -= height / 2;
                    Rectangle2 rect = new Rectangle2(gripPosInCanvas, width, height);

                    if (MathUtils.IsPointInRectangle(posInCanvas, rect))
                    {
                        _currGripPoint = gripPnt;
                        _currGripEntityId = kvp.Key;
                        _currGripPointIndex = index;
                        return gripPnt.Position;
                    }
                }
            }

            _currGripPoint = null;
            _currGripEntityId = ObjectId.Null;
            _currGripPointIndex = -1;
            return posInModel;
        }
    }
}