﻿using System;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Colors;
using CADPadDB.Maths;
using CADPadDB.TableRecord;
using CADPadServices.ApplicationServices;
using CADPadServices.Commands;
using CADPadServices.Commands.Draw;
using CADPadServices.ESelection;
using CADPadServices.Interfaces;

namespace CADPadServices
{
    public class Drawing : IDrawing
    {
        public Drawing(Document doc)
        {
            Document = doc;
            doc.selections.changed += this.OnSelectionChanged;

            Pointer = new PointerContoller(this);
            PanContoller = new PanContoller(this);
            ZoomContoller = new ZoomContoller(this);

            _cmdsMgr = new CommandsMgr(this);
            _cmdsMgr.commandFinished += this.OnCommandFinished;
            _cmdsMgr.commandCanceled += this.OnCommandCanceled;

        }

        public string Name { get; set; } = string.Empty;
        public ICanvas Canvas { get; set; }
        public Document Document { get; private set; }

        public Block CurrentBlock => Document.database.blockTable[Document.currentBlockName] as Block;

        public IPointerContoller Pointer { get; set; }
        public SelectRectangleBase CreateSelectRectangle()
        {
            return new SelectRectangleBase(this);
        }

        public IPanContoller PanContoller { get; set; }
        public IZoomContoller ZoomContoller { get; set; }
        public Selections selections => Document.selections;

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
            Pointer.OnMouseDown(e);
            if (_cmdsMgr.CurrentCmd != null)
            {
                _cmdsMgr.OnMouseDown(e);
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

        public T AppendEntity<T>(T entity, DBObjectState state = DBObjectState.Default) where T : Entity
        {
            Block modelSpace = Document.database.blockTable["ModelSpace"] as Block;
            entity.State = state;
            modelSpace.AppendEntity(entity);

            var drawingVisual = Canvas.CreateGeometryWraper();
            Canvas.AddGeometry(drawingVisual);
            drawingVisual.Entity = entity;
            entity.DrawingVisual = drawingVisual;


            return entity;
        }

        public void RemoveEntity<T>(T entity) where T : Entity
        {
            Canvas.RemoveGeometry(entity.DrawingVisual);
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
            
        }
    }
}