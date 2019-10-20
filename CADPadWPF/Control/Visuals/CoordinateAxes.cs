using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CADPadServices.Interfaces;

namespace CADPadWPF.Control.Visuals
{
    internal class CoordinateAxes : DrawingVisual, ICoordinateAxes
    {

        public void Draw(IDrawing drawing)
        {
            if (drawing == null)
                return;

            using (DrawingContext thisDC = this.RenderOpen())
            {

                Brush br = new SolidColorBrush(CanvasPalette.WPFColor(drawing.AxesColor));
                Pen _pen = new Pen(br, drawing.AxesThickness);

                //
                // Draw arrows
                double rArrLen = drawing.AxesLength * 0.15;
                double rArrWidth = drawing.AxesLength * 0.03;

                // Dont apply scaling on coordinate axes.
                // They must have constant size.
                Point zeroPnt = new Point(0.0, 0.0);
                zeroPnt = drawing.ModelToCanvas(zeroPnt.AsCAD(), false).AsWPF();

                Point xPnt = new Point(drawing.AxesLength - rArrLen, 0.0);
                xPnt = drawing.ModelToCanvas(xPnt.AsCAD(), false).AsWPF();

                Point yPnt = new Point(0.0, drawing.AxesLength - rArrLen);
                yPnt = drawing.ModelToCanvas(yPnt.AsCAD(), false).AsWPF();

                thisDC.DrawLine(_pen, zeroPnt, xPnt);
                thisDC.DrawLine(_pen, zeroPnt, yPnt);

                // X arrow
                PathFigure arrow_X = new PathFigure();
                arrow_X.IsClosed = true;
                Point xPnt0 = new Point(drawing.AxesLength, 0);
                xPnt0 = drawing.ModelToCanvas(xPnt0.AsCAD(), false).AsWPF();
                arrow_X.StartPoint = xPnt0;
                Point xPnt1 = new Point(drawing.AxesLength - rArrLen, -rArrWidth);
                xPnt1 = drawing.ModelToCanvas(xPnt1.AsCAD(), false).AsWPF();
                arrow_X.Segments.Add(new LineSegment(xPnt1, true));
                Point xPnt2 = new Point(drawing.AxesLength - rArrLen, rArrWidth);
                xPnt2 = drawing.ModelToCanvas(xPnt2.AsCAD(), false).AsWPF();
                arrow_X.Segments.Add(new LineSegment(xPnt2, true));

                // Y arrow
                PathFigure arrow_Y = new PathFigure();
                arrow_Y.IsClosed = true;
                Point yPnt0 = new Point(0, drawing.AxesLength);
                yPnt0 = drawing.ModelToCanvas(yPnt0.AsCAD(), false).AsWPF();
                arrow_Y.StartPoint = yPnt0;
                Point yPnt1 = new Point(-rArrWidth, drawing.AxesLength - rArrLen);
                yPnt1 = drawing.ModelToCanvas(yPnt1.AsCAD(), false).AsWPF();
                arrow_Y.Segments.Add(new LineSegment(yPnt1, true));
                Point yPnt2 = new Point(rArrWidth, drawing.AxesLength - rArrLen);
                yPnt2 = drawing.ModelToCanvas(yPnt2.AsCAD(), false).AsWPF();
                arrow_Y.Segments.Add(new LineSegment(yPnt2, true));

                PathGeometry pg = new PathGeometry(new[] { arrow_X, arrow_Y });

                thisDC.DrawGeometry(br, _pen, pg);

                //
                // Draw text
                FontFamily ff = new FontFamily("Arial");
                Typeface tf = new Typeface(ff, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

                FormattedText text_X = new FormattedText("X", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tf, drawing.AxesTextSize, br);
                // Positive Y direction because default WPF axes have Y directed to bottom.
                // Here we draw in default WPF system.
                // Read coom in SimpleCAD.ModelToCanvas().
                thisDC.DrawText(text_X, xPnt + new Vector(0, 3));

                FormattedText text_Y = new FormattedText("Y", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tf, drawing.AxesTextSize, br);
                // Negative Y direction because default WPF axes have Y directed to bottom.
                // Here we draw in default WPF system.
                // Read coom in SimpleCAD.ModelToCanvas().
                thisDC.DrawText(text_Y, yPnt + new Vector(-drawing.AxesTextSize, -drawing.AxesTextSize));

            }
        }
    }
}
