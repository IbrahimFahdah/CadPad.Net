using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CADPadDB;
using CADPadServices.Interfaces;
using CADPadWPF.Control.Canvas;
using Drawing = CADPadServices.Drawing;

namespace CADPadWPF.Control
{
    public partial class CADPadCanvas : FrameworkElement, ICanvas
    {

        private CoordinateAxes m_axes = null;
    

        protected bool _initialiseDrawing;
        static CADPadCanvas()
        {



            CADPadCanvas.SelectedGeometryProperty = DependencyProperty.Register(
                "SelectedGeometry",
                typeof(ICadGeometry),
                typeof(CADPadCanvas),
                new FrameworkPropertyMetadata(null, On_SelectedGeometry_Changed));

            CADPadCanvas.GeometryToCreateProperty = DependencyProperty.Register(
                "GeometryToCreate",
                typeof(ICadGeometry),
                typeof(CADPadCanvas),
                new FrameworkPropertyMetadata(null, On_GeometryToCreate_Changed));

         

            CADPadCanvas.MousePointPropertyKey = DependencyProperty.RegisterReadOnly(
                "MousePoint",
                typeof(Point),
                typeof(CADPadCanvas),
                new FrameworkPropertyMetadata(new Point(0.0, 0.0), FrameworkPropertyMetadataOptions.None));
            CADPadCanvas.MousePointProperty = MousePointPropertyKey.DependencyProperty;
        }

        
        public CADPadCanvas()
        {
            //OnUpdatePlotHandler += OnUpdatePlot;

            this.Loaded += CADPadCanvas_Loaded;
        }

        public List<IDrawingVisual> Geometries { get; set; } = new List<IDrawingVisual>();
        public IDrawingVisual CreateGeometryWraper()
        {
            return new GeometryWraper(Drawing);
        }

        public void AddGeometry(IDrawingVisual dv)
        {
            Geometries.Add(dv);
            AddVisualChild((Visual) dv);
            AddLogicalChild((Visual)dv);
        }

        public void RemoveGeometry(IDrawingVisual dv)
        {
            RemoveVisualChild((Visual)dv);
            RemoveLogicalChild((Visual)dv);
            Geometries.Remove(dv);
        }
        public void InitialiseDrawing()
        {
            if (Drawing == null)
                return;

            m_axes = new CoordinateAxes();
            AddVisualChild(m_axes);
            AddLogicalChild(m_axes);

            Drawing.Canvas = this;
            Drawing.AxesColor = CanvasPalette.ConvertToCAD(AxesColor);
            Drawing.AxesLength = AxesLength;
            Drawing.AxesTextSize = AxesTextSize;
            Drawing.AxesThickness = AxesThickness;
            Drawing.Scale = Scale;
            Drawing.Axes = m_axes;

            // m_axes.Draw();

            _initialiseDrawing = true;
        }
        protected override int VisualChildrenCount
        {
            // 1 for m_axes
            get { return 1 + Geometries.Count; }//  m_grips.Count; 
        }

      
        //=============================================================================
        protected override Visual GetVisualChild(int index)
        {
            int offset = 0;

            if (index == 0)
                return m_axes;

            offset += 1;

            IDrawingVisual geom = null;
            if (index >= offset && index - offset < Geometries.Count)
                geom = Geometries[index - offset];

            //offset += Geometries.Count;
            //if (index >= offset && index - offset < m_grips.Count)
            //    return m_grips[index - offset];

            return (Visual)geom;
        }
        private void CADPadCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if(Drawing==null)
                return;

            InitialiseDrawing();

            // place (0, 0) point at center of SimpleCAD
            // You should do it on Loaded event, otherwise ActualHeight and ActualWidth will be 0.
            double rOffset_X = this.ActualWidth / 2;
            double rOffset_Y = this.ActualHeight / 2;

            ((Drawing)Drawing).Origin.X = -rOffset_X;
            ((Drawing)Drawing).Origin.Y = -rOffset_Y;

            // read comment in ModelToCanvas()
            ((Drawing)Drawing).Origin.Y *= -1;

         
            Redraw();

        }

        #region Dependency properties
     

       
   

    

        //=============================================================================
        public static readonly DependencyProperty SelectedGeometryProperty;

        public ICadGeometry SelectedGeometry
        {
            get { return (ICadGeometry)GetValue(CADPadCanvas.SelectedGeometryProperty); }
            set { SetValue(CADPadCanvas.SelectedGeometryProperty, value); }
        }
        private static void On_SelectedGeometry_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //CADPadCanvas dh = d as CADPadCanvas;
            //if (dh != null)
            //    dh.ResetGrips();
        }

        //=============================================================================
        public static readonly DependencyProperty GeometryToCreateProperty;
        public ICadGeometry GeometryToCreate
        {
            get { return (ICadGeometry)GetValue(CADPadCanvas.GeometryToCreateProperty); }
            set { SetValue(CADPadCanvas.GeometryToCreateProperty, value); }
        }
        private static void On_GeometryToCreate_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CADPadCanvas dh = d as CADPadCanvas;
            if (dh != null)
                dh.On_GeometryToCreate_Changed();
        }
        public void On_GeometryToCreate_Changed()
        {
            //_Cancel();

            //_CloneGeom();

            //_OnInternalCommandEnded();
        }

        //=============================================================================
        /// <summary>
        /// DependencyProperty for <see cref="Background" /> property.
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background",
                typeof(Brush),
                typeof(CADPadCanvas),
                new FrameworkPropertyMetadata((Brush)null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));
        /// <summary>
        /// The Background property defines the brush used to fill the area between borders.
        /// </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        //=============================================================================

        //=============================================================================
        public static readonly DependencyProperty DisabledBackgroundProperty =
            DependencyProperty.Register("DisabledBackground",
                typeof(Brush),
                typeof(CADPadCanvas),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(190, 190, 190)),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));
        /// <summary>
        /// The Background property defines the brush used to fill the area between borders.
        /// </summary>
        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        //=============================================================================
        public static readonly DependencyPropertyKey MousePointPropertyKey;
        public static readonly DependencyProperty MousePointProperty;
        public Point MousePoint
        {
            get
            {
                return (Point)GetValue(CADPadCanvas.MousePointProperty);
            }
            protected set { SetValue(CADPadCanvas.MousePointPropertyKey, value); }
        }


        #endregion

        //=============================================================================
        /// <summary>
        ///     Fills in the background based on the Background property.
        /// </summary>
        protected override void OnRender(DrawingContext dc)
        {
            Brush background = Background;
            if (!this.IsEnabled)
                background = DisabledBackground;

            //
            // This code is copied from panel source.
            //
            if (background != null)
            {
                // Using the Background brush, draw a rectangle that fills the
                // render bounds of the panel.
                Size renderSize = RenderSize;
                dc.DrawRectangle(background,
                    null,
                    new Rect(0.0, 0.0, renderSize.Width, renderSize.Height));
            }
        }

        #region Mouse events
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!this.IsEnabled)
                return;


            Drawing.OnMouseMove(new CanvasMouseEventArgs(e, this));
            MousePoint = Drawing.MousePoint.AsWPF();
        }

        //=============================================================================
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (!this.IsEnabled)
                return;

            Drawing.OnMouseUp(new CanvasMouseButtonEventArgs(e, this));
        }

        //=============================================================================
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (!this.IsEnabled)
                return;

            Drawing.OnMouseDown(new CanvasMouseButtonEventArgs(e, this));
          
        }
        //=============================================================================

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!this.IsEnabled)
                return;

            Drawing.OnMouseWheel(new CanvasMouseWheelEventArgs(e, this));
        }

        //=============================================================================
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!this.IsEnabled)
                return;

            Drawing.OnKeyDown(new CanvasKeyEventArgs(e, this));
        }

        //=============================================================================
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (!this.IsEnabled)
                return;

            Drawing.OnKeyUp(new CanvasKeyEventArgs(e, this));
        }

        #endregion

        internal void OnKeyDown2(KeyEventArgs e)
        {
            OnKeyDown(e);
        }
        internal void OnKeyUp2(KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        public void Redraw()
        {
            if (m_axes != null)
                m_axes.Draw(Drawing);

            foreach (var entity in Geometries)
            {
                entity.Draw();
            }
        }

        //=============================================================================
        //private static event EventHandler OnUpdatePlotHandler;

        //public static void UpdatePlot()
        //{
        //    EventHandler handler = OnUpdatePlotHandler;
        //    if (handler != null)
        //        handler(null, EventArgs.Empty);
        //}



        //private void OnUpdatePlot(object sender, EventArgs e)
        //{
        //    if (m_axes != null)
        //        m_axes.Draw(Drawing);

        //    //if (Geometries != null)
        //    //{
        //    //    foreach (ICadGeometry g in Geometries)
        //    //        g.Draw(this, null);
        //    //}

        //    //if (m_grips != null)
        //    //{
        //    //    foreach (cadGrip g in m_grips)
        //    //        g.Update();
        //    //}
        //}





    }
}
