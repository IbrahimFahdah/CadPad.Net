﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CADPadDB;
using CADPadDB.Maths;
using CADPadDrawing.Visuals;
using CADPadServices.Interfaces;
using Drawing = CADPadServices.Drawing;

namespace CADPadDrawing.Canvas
{
    public partial class CADPadCanvas : FrameworkElement, ICanvas
    {

        private CoordinateAxes m_axes = null;
        private List<CADGripVisual> m_grips = new List<CADGripVisual>();
        private List<IDrawingVisual> _tempVisuals = new List<IDrawingVisual>();


        protected bool _initialiseDrawing;
        static CADPadCanvas()
        {

            CADPadCanvas.SelectedGeometryProperty = DependencyProperty.Register(
                "SelectedGeometry",
                typeof(IDrawingVisual),
                typeof(CADPadCanvas),
                new FrameworkPropertyMetadata(null, On_SelectedGeometry_Changed));

            CADPadCanvas.GeometryToCreateProperty = DependencyProperty.Register(
                "GeometryToCreate",
                typeof(IDrawingVisual),
                typeof(CADPadCanvas),
                new FrameworkPropertyMetadata(null, On_GeometryToCreate_Changed));



            CADPadCanvas.MousePointPropertyKey = DependencyProperty.RegisterReadOnly(
                "MousePoint",
                typeof(Point),
                typeof(CADPadCanvas),
                new FrameworkPropertyMetadata(new Point(0.0, 0.0), FrameworkPropertyMetadataOptions.None));
            CADPadCanvas.MousePointProperty = MousePointPropertyKey.DependencyProperty;
        }

        #region Dependency properties







        //=============================================================================
        public static readonly DependencyProperty SelectedGeometryProperty;

        public IDrawingVisual SelectedGeometry
        {
            get { return (IDrawingVisual)GetValue(CADPadCanvas.SelectedGeometryProperty); }
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
        public IDrawingVisual GeometryToCreate
        {
            get { return (IDrawingVisual)GetValue(CADPadCanvas.GeometryToCreateProperty); }
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


        public CADPadCanvas()
        {
            this.Loaded += CADPadCanvas_Loaded;
            this.SizeChanged += CADPadCanvas_SizeChanged; ;
            this.Cursor = Cursors.None;

            RenderOptions.SetEdgeMode((DependencyObject)this, EdgeMode.Aliased);
        }

        public void InitialiseDrawing()
        {
            if (Drawing == null)
                return;

            m_axes = new CoordinateAxes();
            AddVisualChild(m_axes);
            AddLogicalChild(m_axes);

            GridLayerVisual = new GridLayerVisual(Drawing);
            AddVisualChild((Visual)GridLayerVisual);
            AddLogicalChild((Visual)GridLayerVisual);

            CursorVisual = new CADCursorVisual(Drawing);
            AddVisualChild((Visual)CursorVisual);
            AddLogicalChild((Visual)CursorVisual);

            SelectBoxVisual = new SelectBoxVisual(Drawing);
            AddVisual(SelectBoxVisual);

            //Set Drawing properties from Canvas
            Drawing.Canvas = this;
            Drawing.AxesColor = CanvasPalette.ConvertToCAD(AxesColor);
            Drawing.AxesLength = AxesLength;
            Drawing.AxesTextSize = AxesTextSize;
            Drawing.AxesThickness = AxesThickness;
            Drawing.Scale = Scale;
            Drawing.Axes = m_axes;
            Drawing.GridLayer.Color = CanvasPalette.ConvertToCAD(GridColor);

            _initialiseDrawing = true;
        }
   
        public List<ICADEnitiyVisual> Geometries { get; set; } = new List<ICADEnitiyVisual>();

        public ICursorVisual CursorVisual { get; set; }

        public ISelectBoxVisual SelectBoxVisual { get; set; }

        public IGridLayerVisual GridLayerVisual { get; set; }

        public void AddVisual(IDrawingVisual dv)
        {
            if (dv is ICADEnitiyVisual)
            {
                Geometries.Add((ICADEnitiyVisual)dv);
            }
            else
            {
                _tempVisuals.Add(dv);
            }

            AddVisualChild((Visual)dv);
            AddLogicalChild((Visual)dv);
        }

        public void RemoveVisual(IDrawingVisual dv)
        {
            RemoveVisualChild((Visual)dv);
            RemoveLogicalChild((Visual)dv);
            if (dv is ICADEnitiyVisual)
            {
                Geometries.Remove((ICADEnitiyVisual)dv);
            }
            else
            {
                _tempVisuals.Remove(dv);
            }
        }

        protected override int VisualChildrenCount
        {
            // 1 for m_axes
            get { return _tempVisuals.Count + Geometries.Count + m_grips.Count + 1 + 1 + 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            int offset = 0;

            if (index >= offset && index - offset < _tempVisuals.Count)
                return (Visual)_tempVisuals[index - offset];

            offset += _tempVisuals.Count;
            if (index >= offset && index - offset < m_grips.Count)
                return m_grips[index - offset];

            offset += m_grips.Count;
            if (index == offset)
                return (Visual)GridLayerVisual;

            offset += 1;
            if (index >= offset && index - offset < Geometries.Count)
                return (Visual)Geometries[index - offset];

            offset += Geometries.Count;
            if (index == offset)
                return m_axes;

            offset += 1;
            if (index == offset)
                return (Visual)CursorVisual;


            return null;
        }

        private void CADPadCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (Drawing == null)
                return;

            InitialiseDrawing();
            CenterCanvas();
        }

        public void CenterCanvas()
        {
            CenterCanvasToModelPoint(new CADPoint());
        }

        public void CenterCanvasToModelPoint(CADPoint modelPoint)
        {
            // place (0, 0) point at center of SimpleCAD
            // You should do it on Loaded event, otherwise ActualHeight and ActualWidth will be 0.
            double rOffset_X = this.ActualWidth / 2;
            double rOffset_Y = this.ActualHeight / 2;

            ((Drawing)Drawing).Origin.X = -rOffset_X;
            ((Drawing)Drawing).Origin.Y = -rOffset_Y;

            var offsetPoint = ((Drawing)Drawing).ModelToCanvas(modelPoint);
            ((Drawing)Drawing).Origin.X += -offsetPoint.X + rOffset_X;
            ((Drawing)Drawing).Origin.Y += -offsetPoint.Y - rOffset_Y;
            
            // read comment in ModelToCanvas()
            ((Drawing)Drawing).Origin.Y *= -1;

            Redraw();
        }

        private void CADPadCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_initialiseDrawing)
            {
                Redraw();
            }
        }


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
            var pp = e.GetPosition(this);
            base.OnMouseMove(e);

            if (!this.IsEnabled || pp.Y < 0)
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

        public void OnKeyDown2(KeyEventArgs e)
        {
            OnKeyDown(e);
        }
        public void OnKeyUp2(KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        public void Redraw()
        {

            if (m_axes != null)
                m_axes.Draw(Drawing);


            if (Drawing?.GridLayer != null)
            {
                Drawing.GridLayer.Draw();
            }

            foreach (var entity in Geometries)
            {
                entity.Draw();
            }

            foreach (var point in m_grips)
            {
                point.Draw();
            }
            // this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Unspecified );
        }

        public IDrawingVisual CreateVisual()
        {
            return new CanvasDrawingVisual(Drawing);
        }

        public ICADEnitiyVisual CreateCADEnitiyVisual()
        {
            var g = new CADEnitiyVisual(Drawing);
            return g;
        }

        private void ClearGrips()
        {
            foreach (CADGripVisual g in m_grips)
            {
                RemoveVisualChild(g);
                RemoveLogicalChild(g);
            }
            m_grips.Clear();
        }

        public void ClearVisualGrips(IDrawingVisual associatedVisual)
        {
            var toRemove = m_grips.Where(e => e.AssociatedVisual == associatedVisual).ToList();

            foreach (CADGripVisual g in toRemove)
            {
                RemoveVisualChild(g);
                RemoveLogicalChild(g);
                m_grips.Remove (g);
            }
        }

        public void ResetGrips()
        {
            ClearGrips();

            foreach (var g in Geometries)
            {
                List<GripPoint> pnts = g.GetGripPoints();
                if (g.Selected)
                {
                    foreach (GripPoint p in pnts)
                    {
                        CADGripVisual newGripVisual = new CADGripVisual(Drawing, p,g);
                        m_grips.Add(newGripVisual);
                        AddVisualChild(newGripVisual);
                        AddLogicalChild(newGripVisual);
                        newGripVisual.Draw();
                    }
                }
                //else
                //{
                //    foreach (GripPoint p in pnts)
                //    {
                //        CADGripVisual newGripVisual = new CADGripVisual(Drawing, p);
                //        m_grips.Remove(newGripVisual);
                //        RemoveVisualChild(newGripVisual);
                //        RemoveLogicalChild(newGripVisual);
                //    }
                //}
            }
        }

        public void Clear()
        {
            ClearGrips();
            foreach (Visual g in Geometries)
            {
                RemoveVisualChild(g);
                RemoveLogicalChild(g);       
            }
        }
    }
}
