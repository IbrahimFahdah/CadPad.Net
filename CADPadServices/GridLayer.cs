using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using CADPadDB;
using CADPadDB.Colors;
using CADPadDB.Maths;
using CADPadServices.Enums;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class GridLayer : IGridLayer
    {
        private IDrawing _drawing = null;

        List<ObjectSnapPoint> _snapPoints = new List<ObjectSnapPoint>();

        IGridLayerVisual _gridLayerVisual;

        public GridLayer(IDrawing drawing)
        {
            _drawing = drawing;
            CrossSize = 10;
            SpacingX = SpacingY = 10;
            MinSize = 15;
            GridStyle = ((Drawing)_drawing).GridEnabled ? GridStyle.Lines : GridStyle.None;
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

        public List<ObjectSnapPoint> GetSnapPoints(CADPoint posInModel)
        {
            _snapPoints.Clear();

            int spx = SpacingX * (posInModel.X > 0 ? 1 : -1);
            int spy = SpacingY * (posInModel.Y > 0 ? 1 : -1);

            var nx = (int)posInModel.X / spx;
            int ny = (int)posInModel.Y / spy;

            _snapPoints.Add(new ObjectSnapPoint(ObjectSnapMode.Grid, new CADPoint(nx * spx, ny * spy)));
            _snapPoints.Add(new ObjectSnapPoint(ObjectSnapMode.Grid, new CADPoint((nx + 1) * spx, ny * spy)));
            _snapPoints.Add(new ObjectSnapPoint(ObjectSnapMode.Grid, new CADPoint(nx * spx, (ny + 1) * spy)));
            _snapPoints.Add(new ObjectSnapPoint(ObjectSnapMode.Grid, new CADPoint((nx + 1) * spx, (ny + 1) * spy)));

            return _snapPoints;
        }

    }
}
