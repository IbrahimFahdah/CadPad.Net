using System.Collections.Generic;
using System.Drawing;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.ApplicationServices;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices.Controllers
{
    public class GripPointsController
    {
        private IDrawing _drawing = null;

        private Dictionary<ObjectId, List<GripPoint>> _gripPnts = new Dictionary<ObjectId, List<GripPoint>>();
        public GripPoint currentGripPoint { get; private set; } = null;

        public ObjectId currentGripEntityId { get; private set; } = ObjectId.Null;

        public int currentGripPointIndex { get; private set; } = -1;

        public GripPointsController(IDrawing drawing)
        {
            _drawing = drawing;
        }

        public void Update()
        {
            Document doc = _drawing.Document as Document;
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

            _drawing.ResetGrips();
        }

        public void Clear()
        {
            _gripPnts.Clear();
        }

        //public void Draw()
        //{
        //    foreach (KeyValuePair<ObjectId, List<GripPoint>> kvp in _gripPnts)
        //    {
        //        foreach (GripPoint gripPnt in kvp.Value)
        //        {
        //            double width = 10;
        //            double height = 10;
        //            CADPoint posInCanvas = _drawing.ModelToCanvas(gripPnt.Position);
        //            posInCanvas.X -= width / 2;
        //            posInCanvas.Y -= height / 2;
        //            _drawing.FillRectangle(graphics, Color.Blue, posInCanvas, width, height, CSYS.Canvas);
        //        }
        //    }
        //}

        public CADPoint Snap(CADPoint posInCanvas)
        {
            CADPoint posInModel = _drawing.CanvasToModel(posInCanvas);

            foreach (KeyValuePair<ObjectId, List<GripPoint>> kvp in _gripPnts)
            {
                int index = -1;
                foreach (GripPoint gripPnt in kvp.Value)
                {
                    ++index;
                    double width = 10;
                    double height = 10;
                    CADPoint gripPosInCanvas = _drawing.ModelToCanvas(gripPnt.Position);
                    gripPosInCanvas.X -= width / 2;
                    gripPosInCanvas.Y -= height / 2;
                    Rectangle2 rect = new Rectangle2(gripPosInCanvas, width, height);

                    if (MathUtils.IsPointInRectangle(posInCanvas, rect))
                    {
                        currentGripPoint = gripPnt;
                        currentGripEntityId = kvp.Key;
                        currentGripPointIndex = index;
                        return gripPnt.Position;
                    }
                }
            }

            currentGripPoint = null;
            currentGripEntityId = ObjectId.Null;
            currentGripPointIndex = -1;
            return posInModel;
        }
    }
}