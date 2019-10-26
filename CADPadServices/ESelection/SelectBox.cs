using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadDB.TableRecord;
using CADPadServices.Interfaces;


namespace CADPadServices.ESelection
{
    public class SelectBox: ISelectBox
    {
        private IDrawing _drawing = null;

        private ISelectBoxVisual _selectionBoxVisual = null;

        public enum SelectMode
        {
            Window = 1,
            Cross = 2,
        }

        /// <summary>
        /// Canvas CSYS
        /// </summary>
        protected CADPoint _startPoint = new CADPoint(0, 0);
        public CADPoint StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        /// <summary>
        /// Canvas CSYS
        /// </summary>
        protected CADPoint _endPoint = new CADPoint(0, 0);

      

        public CADPoint EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        public SelectMode SelectionMode
        {
            get
            {
                if (_endPoint.X >= _startPoint.X)
                {
                    return SelectMode.Window;
                }
                else
                {
                    return SelectMode.Cross;
                }
            }
        }

        public SelectBox(IDrawing drawing)
        {
            _drawing = drawing;
        }


        public ISelectBoxVisual SelectionBoxVisual
        {
            get
            {
                if (_selectionBoxVisual == null)
                {
                    _selectionBoxVisual = _drawing.GetSelectionBoxVisual();
                }
                return _selectionBoxVisual;
            }
            set => _selectionBoxVisual = value;
        }

        
        public bool Active { get; set; }    

        public List<Selection> Select(Block block)
        {
            Bounding selectBound = new Bounding(
                _drawing.CanvasToModel(_startPoint),
                _drawing.CanvasToModel(_endPoint));

            List<Selection> sels = new List<Selection>();
            SelectMode selMode = SelectionMode;
            foreach (Entity entity in block)
            {
                bool selected = false;
                if (selMode == SelectMode.Cross)
                {
                    selected = EntityRSMgr.Instance.Cross(selectBound, entity);
                }
                else if (selMode == SelectMode.Window)
                {
                    selected = EntityRSMgr.Instance.Window(selectBound, entity);
                }

                if (selected)
                {
                    Selection selection = new Selection();
                    selection.objectId = entity.id;
                    sels.Add(selection);
                }
            }

            return sels;
        }

        public void Reset()
        {
            Active = false;
            StartPoint = EndPoint=new CADPoint( );
            Draw();
        }
        public void Draw()
        {
            if(StartPoint==EndPoint )
                SelectionBoxVisual.Clear();
            SelectionBoxVisual.Draw(this);
        }

        
    }
}