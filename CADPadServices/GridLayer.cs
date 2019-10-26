using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using CADPadDB.Colors;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class GridLayer : IGridLayer
    {
        private IDrawing _drawing = null;

      

        IGridLayerVisual _gridLayerVisual;

        public GridLayer(IDrawing drawing)
        {
            _drawing = drawing;
            CrossSize = 10;
            SpacingX = SpacingY = 10;
            MinSize = 15;
            GridStyle = GridStyle.Lines;
            Color = CADColor.FromArgb(255, 0, 0);
        }


        public int SpacingX { get; set; }
        public int SpacingY { get; set; }

        public int MinSize { get; set; }

        public GridStyle GridStyle { get; set; }

        public CADColor Color { get; set; }

        public double CrossSize { get; set; }

        public IGridLayerVisual GridLayerVisual
        {
            get
            {
                if (_gridLayerVisual == null)
                {
                    _gridLayerVisual = _drawing.GetGridLayerVisual();
                }
                return _gridLayerVisual;
            }
            set => _gridLayerVisual = value;
        }

        public void Draw()
        {
            GridLayerVisual.Draw(this);
        }


    }
}
