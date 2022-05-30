using System.Collections.Generic;
using CADPadDB.Colors;
using CADPadDB.Maths;
using CADPadDB.TableRecord;

namespace CADPadDB.CADEntity
{
    public abstract class Entity : DBObject
    {        
        public override string ClassName { get; } = "Entity";

        public ICADEnitiyVisual DrawingVisual { get; set; }
        public abstract Bounding Bounding
        {
            get;
        }

        public CADColor Color { get; set; } = CADColor.ByLayer;

        public CADColor ColorValue
        {
            get
            {
                switch (Color.colorMethod)
                {
                    case ColorMethod.ByBlock:
                        if (this.parent != null
                            && this.parent is BlockReference)
                        {
                            BlockReference blockRef = this.parent as BlockReference;
                            return blockRef.ColorValue;
                        }
                        else
                        {
                            return CADColor.FromArgb(Color.r, Color.g, Color.b);
                        }

                    case ColorMethod.ByLayer:
                        Database db = this.database;
                        if (db != null
                            && db.layerTable.Has(this.Layer))
                        {
                            Layer layer = db.layerTable[this.Layer] as Layer;
                            return layer.colorValue;
                        }
                        else
                        {
                            return CADColor.FromArgb(Color.r, Color.g, Color.b);
                        }

                    case ColorMethod.ByColor:
                    case ColorMethod.None:
                    default:
                        return CADColor.FromArgb(Color.r, Color.g, Color.b);
                }
            }
        }

        public LineWeight LineWeight { get; set; } = LineWeight.ByLayer;

        public ObjectId LayerId { get; set; } = ObjectId.Null;

        public string Layer
        {
            get
            {
                Database db = this.database;
                if (db != null 
                    && LayerId != ObjectId.Null
                    && db.layerTable.Has(LayerId))
                {
                    Layer layerRecord = db.GetObject(LayerId) as Layer;
                    return layerRecord.name;
                }
                return "";
            }
            set
            {
                Database db = this.database;
                if (db != null
                    && db.layerTable.Has(value))
                {
                    LayerId = db.layerTable[value].id;
                }
            }
        }

        public virtual void Draw()
        {
            //if (!(this.parent.database.layerTable[Layer] as Layer).Visible) return;
            Draw(DrawingVisual);
        }

        public virtual void Draw(IDrawingVisual gd)
        {
        }

        public override object Clone()
        {
            Entity entity = base.Clone() as Entity;
            entity.Color = Color;
            entity.LayerId = LayerId;
            return entity;
        }


        public abstract void Translate(CADVector translation);


        public abstract void TransformBy(Matrix3 transform);

        protected override void _Erase()
        {
            if (_parent != null)
            {
                Block block = _parent as Block;
                block.RemoveEntity(this);
            }
        }

        public virtual List<ObjectSnapPoint> GetSnapPoints()
        {
            return null;
        }


        public virtual List<GripPoint> GetGripPoints()
        {
            return null;
        }

        public virtual void SetGripPointAt(int index, GripPoint gripPoint, CADPoint newPosition)
        {
        }


        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("Color", Color);
            filer.Write("LineWeight", LineWeight.ToString());
            filer.Write("Layer", LayerId);
        }

        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);
          
            filer.Read("Color", out CADColor color);
            filer.Read("LineWeight", out LineWeight lineWeight);
            filer.Read("Layer", out ObjectId layerId);

            this.Color = color;
            LineWeight = lineWeight;
            LayerId = layerId;
        }
    }
}
