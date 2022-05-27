using System.Collections.Generic;
using System.Windows.Media;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Interfaces;

namespace CADPadDrawing.Visuals
{
    public class CADEnitiyVisual : CanvasDrawingVisual, ICADEnitiyVisual
    {
        private Pen _spen;//, _pen;
        private bool selected;
      
        public CADEnitiyVisual(IDrawing owner) : base(owner)
        {

        }

        public Entity Entity { get; set; } = null;
        public bool Selected { get; set; }
        public List<GripPoint> GetGripPoints()
        {
            if (Selected)
                return Entity.GetGripPoints();
            return new List<GripPoint>();
        }

        public Pen SelectPen
        {
            get
            {
                if (_spen == null)
                {
                    var b = new SolidColorBrush(Colors.Red);
                    RenderOptions.SetCachingHint(b, CachingHint.Cache);
                    b.Freeze();
                    _spen = new Pen(b, 1);
                    //critical for good performance
                    _spen.Freeze();
                }
                return _spen;
            }
        }

        public override Pen Pen
        {
            get
            {
                if (Selected)
                    return SelectPen;
                if (_pen == null)
                {
                    var b = new SolidColorBrush(Colors.White);

                    //RenderOptions.SetCachingHint(b, CachingHint.Cache);
                    //b.Freeze();
                    _pen = new Pen(b, 1);
                    //critical for good performance
                    //_pen.Freeze();
                }
                return _pen;
            }
            set
            {
                _pen = value;
            }
        }

        //public override void DrawLine(CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true)
        //{
        //    DrawLine(_drawing, thisDC, Selected ? SelectPen : Pen, startPoint, endPoint);
        //}

        public virtual void Draw()
        {
            Entity?.Draw(this);
        }
    }
}
