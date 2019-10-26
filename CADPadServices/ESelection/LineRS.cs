using System;
using System.Collections.Generic;
using System.Text;
using CADPadDB;
using CADPadDB.CADEntity;
using CADPadDB.Maths;

namespace CADPadServices.ESelection
{

    internal class LineRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Line line = entity as Line;
            if (line == null)
            {
                return false;
            }

            Bounding lineBound = line.Bounding;
            if (selectBound.Contains(lineBound))
            {
                return true;
            }

            Rectangle2 selRect = new Rectangle2(
                new CADPoint(selectBound.left, selectBound.bottom),
                new CADPoint(selectBound.right, selectBound.top));

            Line2 rectLine1 = new Line2(selRect.leftBottom, selRect.leftTop);
            Line2 rectLine2 = new Line2(selRect.leftTop, selRect.rightTop);
            Line2 rectLine3 = new Line2(selRect.rightTop, selRect.rightBottom);
            Line2 rectLine4 = new Line2(selRect.rightBottom, selRect.leftBottom);
            Line2 line2 = new Line2(line.startPoint, line.endPoint);

            CADPoint intersection = new CADPoint();
            if (Line2.Intersect(rectLine1, line2, ref intersection)
                || Line2.Intersect(rectLine2, line2, ref intersection)
                || Line2.Intersect(rectLine3, line2, ref intersection)
                || Line2.Intersect(rectLine4, line2, ref intersection))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //internal override bool Window(Bounding selectBound, Entity entity)
        //{
        //    Line line = entity as Line;
        //    if (line == null)
        //    {
        //        return false;
        //    }

        //    LitMath.Rectangle2 SelRect = new LitMath.Rectangle2(
        //        new LitMath.Vector2(selectBound.left, selectBound.bottom),
        //        new LitMath.Vector2(selectBound.right, selectBound.top));

        //    if (MathUtils.IsPointInRectangle(line.startPoint, SelRect)
        //        && MathUtils.IsPointInRectangle(line.endPoint, SelRect))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
