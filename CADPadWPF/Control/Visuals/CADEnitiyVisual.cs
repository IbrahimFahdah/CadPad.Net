using System.Collections.Generic;
using System.Windows.Media;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;
using CADPadServices.Interfaces;

namespace CADPadWPF.Control.Visuals
{
    public class CADEnitiyVisual : CanvasDrawingVisual, ICADEnitiyVisual
    {
        private Pen _spen;
     

        public CADEnitiyVisual(IDrawing owner):base(owner)
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

        public Pen sPen
        {
            get
            {
                if (_spen == null)
                {
                    _spen = new Pen(new SolidColorBrush(Colors.Red), 2);
                }
                return _spen;
            }
        }

        public override void DrawLine(CADPoint startPoint, CADPoint endPoint, bool mdoelToCanvas = true)
        {
            DrawLine(_drawing, thisDC, Selected ? sPen : Pen, startPoint, endPoint);
        }

        public virtual void Draw()
        {
            Entity?.Draw(this);
        }

      
    }
}
