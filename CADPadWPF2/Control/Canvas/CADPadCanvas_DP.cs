using System.Windows;
using System.Windows.Media;
using CADPadServices.Interfaces;
using Drawing = CADPadServices.Drawing;

namespace CADPadWPF.Control.Canvas
{
    public partial class CADPadCanvas
    {
        public static readonly DependencyProperty DrawingProperty = DependencyProperty.Register(
            "Drawing",
            typeof(IDrawing),
            typeof(Canvas.CADPadCanvas),
            new FrameworkPropertyMetadata(null));
        public IDrawing Drawing
        {
            get => (Drawing)GetValue(DrawingProperty);
            set
            {
                value.Canvas = this;
                SetValue(DrawingProperty, value);
            }
        }
        //=============================================================================
        public static readonly DependencyProperty AxesColorProperty = DependencyProperty.Register(
            "AxesColor",
            typeof(Color),
            typeof(Canvas.CADPadCanvas),
            new FrameworkPropertyMetadata(Colors.Black, On_AxesColor_Changed));
        public Color AxesColor
        {
            get => (Color)GetValue(AxesColorProperty);
            set
            {
                Drawing.AxesColor = CanvasPalette.ConvertToCAD(value);
                SetValue(AxesColorProperty, value);
            }
        }
        private static void On_AxesColor_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas.CADPadCanvas dh)
            {
                dh.RedrawCoordinateAxes();
            }
        }
        //=============================================================================
        public static readonly DependencyProperty AxesThicknessProperty = DependencyProperty.Register(
            "AxesThickness",
            typeof(double),
            typeof(Canvas.CADPadCanvas),
            new FrameworkPropertyMetadata(2.0, On_AxesThickness_Changed));
        public double AxesThickness
        {
            get => (double)GetValue(AxesThicknessProperty);
            set
            {
                Drawing.AxesThickness = value;
                SetValue(AxesThicknessProperty, value);
            }
        }
        private static void On_AxesThickness_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas.CADPadCanvas dh)
            {
                dh.RedrawCoordinateAxes();
            }
        }
        //=============================================================================
        public static readonly DependencyProperty AxesLengthProperty = DependencyProperty.Register(
            "AxesLength",
            typeof(double),
            typeof(Canvas.CADPadCanvas),
            new FrameworkPropertyMetadata(50.0, On_AxesLength_Changed));
        public double AxesLength
        {
            get => (double)GetValue(AxesLengthProperty);
            set
            {
                Drawing.AxesLength = value;
                SetValue(AxesLengthProperty, value);
            }
        }

        private static void On_AxesLength_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas.CADPadCanvas dh)
            {
                dh.RedrawCoordinateAxes();
            }
        }

        //=============================================================================
        public static readonly DependencyProperty AxesTextSizeProperty = DependencyProperty.Register(
            "AxesTextSize",
            typeof(double),
            typeof(Canvas.CADPadCanvas),
            new FrameworkPropertyMetadata(12.0, On_AxesTextSize_Changed));
        public double AxesTextSize
        {
            get => (double)GetValue(AxesTextSizeProperty);
            set
            {
                Drawing.AxesTextSize = value;
                SetValue(AxesTextSizeProperty, value);
            }
        }

        private static void On_AxesTextSize_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas.CADPadCanvas dh)
            {
                dh.RedrawCoordinateAxes();
            }
        }
        //=============================================================================
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale",
            typeof(double),
            typeof(Canvas.CADPadCanvas),
            new FrameworkPropertyMetadata(2.0, On_Scale_Changed));

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set
            {
                Drawing.Scale = value;
                SetValue(ScaleProperty, value);
            }
        }
        private static void On_Scale_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CADPadCanvas dh)
            {
                dh.Redraw();
            }
        }

        //=============================================================================
        public void RedrawCoordinateAxes()
        {
            if (m_axes != null)
                m_axes.Draw(Drawing);
        }
    }
}