using System;
using System.Collections.Generic;
using System.Linq;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Colors;
using CADPadDB.Maths;
using CADPadDB.TableRecord;
using CADPadServices.Commands;
using CADPadServices.Commands.Draw;
using CADPadServices.Commands.Modify;
using CADPadServices.Controllers;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class Drawing : IDrawing
    {
        public Drawing(Document doc)
        {
            Document = doc;
            doc.Selections.changed += this.OnSelectionChanged;

            ColorMethod = ColorMethod.ByLayer;

            Pointer = new PointerContoller(this);
            PanContoller = new PanContoller(this);
            ZoomContoller = new ZoomContoller(this);
            GridLayer = new GridLayer(this);

            _cmdsMgr = new CommandsMgr(this);
            _cmdsMgr.commandFinished += this.OnCommandFinished;
            _cmdsMgr.commandCanceled += this.OnCommandCanceled;

        }

        public string Name { get; set; } = string.Empty;
        public ICanvas Canvas { get; set; }
        public Document Document { get; private set; }

        public Block CurrentBlock => Document.Database.blockTable[Document.CurrentBlockName] as Block;

        public IPointerContoller Pointer { get; set; }
        public SelectBox CreateSelectRectangle()
        {
            return new SelectBox(this);
        }

        public IPanContoller PanContoller { get; set; }
        public IZoomContoller ZoomContoller { get; set; }
        public Selections selections => Document.Selections;
        public IGridLayer GridLayer { get; set; }

        public bool GridEnabled { get; set; } = false;
        public bool ZoomEnabled { get; set; } = true;
        public bool PointerEnabled { get; set; } = true;

        public ColorMethod ColorMethod { get => CADColor.GlobalColorMethod; set => CADColor.GlobalColorMethod = value; }

        protected CommandsMgr _cmdsMgr = null;

        public CADColor AxesColor { get; set; }
        public double AxesThickness { get; set; }
        public double AxesLength { get; set; }
        public double AxesTextSize { get; set; }

        public double Scale
        {
            get => ZoomContoller.Scale;
            set => ZoomContoller.Scale = value;
        }

        public ICoordinateAxes Axes { get; set; }

        /// <summary>
        /// Using model coordinates
        /// </summary>
        public CADPoint MousePoint { get; set; }

        /// <summary>
        /// Origin is in canvas/physical coordinates
        /// Offset of left top corner of plot from (0;0) point.
        /// </summary>
        public CADVector Origin = new CADVector(0.0, 0.0);

        public bool CanUndo => _cmdsMgr.canUndo;
        public bool CanRedo => _cmdsMgr.canRedo;

        public CADPoint CanvasToModel(CADPoint localPnt)
        {
            CADPoint tempPnt = localPnt;

            // reverse Y
            // read comment in ModelToCanvas()
            tempPnt.Y *= -1;


            tempPnt += PanContoller.GetOffset();

            tempPnt.X = tempPnt.X / Scale;
            tempPnt.Y = tempPnt.Y / Scale;
            return tempPnt;
        }

        public double CanvasToModel(double value)
        {

            return value / Scale;
        }

        public double ModelToCanvas(double value)
        {

            return value * Scale;
        }

        public CADPoint ModelToCanvas(CADPoint globalPnt, bool bWithScale = true)
        {
            CADPoint tempPnt = globalPnt;
            if (bWithScale)
            {
                tempPnt.X *= Scale;
                tempPnt.Y *= Scale;
            }

            tempPnt -= PanContoller.GetOffset();

            //
            // Default WPF coordinate system has Y-axis, directed to down.
            // So left top corner has(0, 0) coordinate.
            //
            // All graphics that i have seen use Y-axis, directed to up.
            // So left BOT corner has(0, 0) coordinate.
            //
            // Lets revert Y - axis.
            // So all coordinate properties will show default "human" coordinate system.
            //
            // ---------------------------------------------------------------------------------------
            // Another way to solve this problem is applly (Y=-1) render transform to CADPadCanvas control.
            // Something like this:
            //
            // this.RenderTransform = new ScaleTransform(1, -1);
            // this.RenderTransformOrigin = new Point(0.5, 0.5);
            //
            // It is much simplier but incorrect. Because all text will have correct (x,y)-point at which he should be drawn,
            // but it will be drawn with (Y=-1) scaling.
            tempPnt.Y *= -1;

            return tempPnt;
        }

        public IEventResult OnMouseDown(IMouseButtonEventArgs e)
        {
            PanContoller.OnMouseDown(e);
            var res = Pointer.OnMouseDown(e);
            if (_cmdsMgr.CurrentCmd != null)
            {
                _cmdsMgr.OnMouseDown(e);
            }
            else
            {
                if (res.data is Command cmd)
                {
                    _cmdsMgr.DoCommand(cmd);
                }
            }
            return null;// _cmdsMgr.CurrentCmd;
        }

        public IEventResult OnMouseUp(IMouseButtonEventArgs e)
        {
            Pointer.OnMouseUp(e);
            PanContoller.OnMouseUp(e);


            if (_cmdsMgr.CurrentCmd != null)
            {
                // ((LinesChainCmd)_cmdsMgr.CurrentCmd).DrawingContext = null;
                _cmdsMgr.OnMouseUp(e);
            }

            return null;
        }

        public IEventResult OnMouseMove(IMouseEventArgs e)
        {
            Pointer.OnMouseMove(e);
            PanContoller.OnMouseMove(e);

            MousePoint = CanvasToModel(new CADPoint(e.X, e.Y));



            if (_cmdsMgr.CurrentCmd != null)
            {
                _cmdsMgr.OnMouseMove(e);
                // RepaintCanvas();
            }

            return null;
        }
        public IEventResult OnMouseWheel(IMouseWheelEventArgs e)
        {
            ZoomContoller.OnMouseWheel(e);
            return null;
        }

        public IEventResult OnMouseDoubleClick(IMouseEventArgs e)
        {
            return null;
        }

        public IEventResult OnKeyDown(IKeyEventArgs e)
        {
            Pointer.OnKeyDown(e);
            PanContoller.OnKeyDown(e);
            if (_cmdsMgr.CurrentCmd != null)
            {
                _cmdsMgr.OnKeyDown(e);
            }
            else
            {
                if (e.IsEscape)
                {
                    Document.Selections.Clear();
                    //foreach (var g in Canvas.Geometries)
                    //{
                    //   if(g.Selected)
                    //   {
                    //        g.Selected = false;
                    //        g.Draw();
                    //   }
                    //}
                    //Canvas.ResetGrips();
                }
                //if (_dynamicInputer.StartCmd(e))
                //{
                //}
                //else if (e.IsEscape)
                //{
                //    _document.selections.Clear();
                //}
                else if (e.IsDelete)
                {
                    if ((Document as Document).Selections.Count > 0)
                    {
                        DeleteCmd cmd = new DeleteCmd();
                        this.OnCommand(cmd);
                    }
                }
            }

            return null;
        }

        public IEventResult OnKeyUp(IKeyEventArgs e)
        {
            Pointer.OnKeyUp(e);
            PanContoller.OnKeyUp(e);
            if (_cmdsMgr != null)
            {
                _cmdsMgr.OnKeyUp(e);
            }

            return null;
        }

        public T AppendEntity<T>(T entity, DBObjectState state = DBObjectState.Default, bool reUseVisual = false) where T : Entity
        {
            Block modelSpace = CurrentBlock;
            entity.State = state;
            modelSpace.AppendEntity(entity);

            var drawingVisual = entity.DrawingVisual;
            if (reUseVisual == false || entity.DrawingVisual == null)
            {
                drawingVisual = Canvas.CreateCADEnitiyVisual();

                drawingVisual.Entity = entity;
                entity.DrawingVisual = drawingVisual;
            }

            if (entity.DrawingVisual != null)
            {
                Canvas.AddVisual(drawingVisual);
            }

            return entity;
        }

        public void Rebuild()
        {
            Canvas.Clear();
            foreach (var entity in CurrentBlock)
            {
                var drawingVisual = Canvas.CreateCADEnitiyVisual();
                drawingVisual.Entity = entity;
                entity.DrawingVisual = drawingVisual;
                Canvas.AddVisual(drawingVisual);
            }
            Canvas.Redraw();
        }

        public void Clear()
        {
            var items = CurrentBlock.ToList();

            foreach (var entity in items)
            {
                RemoveEntity(entity);
            }
            _cmdsMgr.Clear();
        }

        public void RemoveEntity<T>(T entity) where T : Entity
        {
            Canvas.ClearVisualGrips(entity.DrawingVisual);
            Canvas.RemoveVisual(entity.DrawingVisual);
            entity.Erase();
        }

        public void OnCommand(ICommand cmd)
        {

            _cmdsMgr.DoCommand(cmd as Command);
            if (_cmdsMgr.CurrentCmd is DrawCmd cm)
            {
                cm.Drawing = this;
            }
        }

        public void OnCommandFinished(Command cmd)
        {
            //   this.RepaintCanvas(true);
        }

        public void OnCommandCanceled(Command cmd)
        {
            // this.RepaintCanvas(false);
        }

        public void OnSelectionChanged()
        {
            Pointer.OnSelectionChanged();
            if (selections.Count == 0)
            {
                foreach (var g in Canvas.Geometries)
                {
                    if (g.Selected)
                    {
                        g.Selected = false;
                        g.Draw();
                    }
                }
                Canvas.ResetGrips();
            }
            //Canvas.ResetGrips();
        }

        public void ResetGrips()
        {
            Canvas.ResetGrips();
        }

        public IDrawingVisual CreateTempVisual()
        {
            var drawingVisual = Canvas.CreateVisual();
            Canvas.AddVisual(drawingVisual);
            return drawingVisual;
        }

        public ICursorVisual GetCursorVisual()
        {
            return Canvas.CursorVisual;
        }

        public ISelectBoxVisual GetSelectBoxVisual()
        {
            return Canvas.SelectBoxVisual;
        }

        public IGridLayerVisual GetGridLayerVisual()
        {
            return Canvas.GridLayerVisual;
        }

        public void RemoveTempVisual(IDrawingVisual v)
        {
            Canvas.RemoveVisual(v);
        }

        public void RemoveUnconfirmed()
        {
            Block modelSpace = Document.Database.blockTable["ModelSpace"] as Block;

            var toRemove = modelSpace.Where(e => e.State == DBObjectState.Unconfirmed).ToList();
            foreach (Entity item in toRemove)
            {
                RemoveEntity(item);
            }
        }

        public void CenterInView(Bounding bounding)
        {
            Canvas.CenterCanvasToModelPoint(new CADPoint(-bounding.center.X, -bounding.center.Y));
        }

        public void ZoomToFit(double windowWidth, double windowHeight, Bounding bounding)
        {
            ZoomContoller.Scale = Math.Min(windowWidth / bounding.width, windowHeight / bounding.height);
        }

        public Bounding GetBounding(List<Entity> entities)
        {
            CADPoint topLeft = new CADPoint();
            CADPoint bottomRight = new CADPoint();

            if (entities == null || entities.Count == 0)
            {
                return new Bounding();
            }

            topLeft.X = entities[0].Bounding.left;
            topLeft.Y = entities[0].Bounding.top;
            bottomRight.X = entities[0].Bounding.right;
            bottomRight.Y = entities[0].Bounding.bottom;

            entities.ForEach(a =>
            {
                var bounding = a.Bounding;
                topLeft.X = Math.Min(topLeft.X, bounding.left);
                topLeft.Y = Math.Max(topLeft.Y, bounding.top);

                bottomRight.X = Math.Max(bottomRight.X, bounding.right);
                bottomRight.Y = Math.Min(bottomRight.Y, bounding.bottom);
            });

            return new Bounding(topLeft, bottomRight);
        }
    }
}
