
using CADPadDB.Maths;

namespace CADPadDB
{

    public struct Bounding
    {

        public CADVector center
        {
            get
            { 
                return new CADVector(
                    (_left + _right) / 2,
                    (_bottom + _top) / 2); 
            }
        }


        private double _width;
        public double width
        {
            get { return _width; }
            //set { _width = value; }
        }


        private double _height;
        public double height
        {
            get { return _height; }
            //set { _height = value; }
        }


        private double _left;
        public double left
        {
            get { return _left; }
        }


        private double _right;
        public double right
        {
            get { return _right; }
        }


        private double _top;
        public double top
        {
            get { return _top; }
        }


        private double _bottom;
        public double bottom
        {
            get { return _bottom; }
        }


        public Bounding(CADPoint point1, CADPoint point2)
        {
            if (point1.X < point2.X)
            {
                _left = point1.X;
                _right = point2.X;
            }
            else
            {
                _left = point2.X;
                _right = point1.X;
            }

            if (point1.Y < point2.Y)
            {
                _bottom = point1.Y;
                _top = point2.Y;
            }
            else
            {
                _bottom = point2.Y;
                _top = point1.Y;
            }

            _width = _right - _left;
            _height = _top - _bottom;
        }

        public Bounding(CADPoint center, double width, double height)
        {
            _left = center.X - width / 2;
            _right = center.X + width / 2;
            _bottom = center.Y - height / 2;
            _top = center.Y + height / 2;
            _width = _right - _left;
            _height = _top - _bottom;
        }

        /// <summary>
        /// Check whether contains Bounding
        /// </summary>
        public bool Contains(Bounding bounding)
        {
            return this.Contains(bounding.left, bounding.bottom)
                && this.Contains(bounding.right, bounding.top);
        }

        /// <summary>
        /// Check whether contains point
        /// </summary>
        public bool Contains(CADPoint point)
        {
            return this.Contains(point.X, point.Y);
        }

        /// <summary>
        /// Check whether contains point: (x, y)
        /// </summary>
        public bool Contains(double x, double y)
        {
            return x >= this.left
                && x <= this.right
                && y >= this.bottom
                && y <= this.top;
        }

        /// <summary>
        /// Check whether intersect with Bounding
        /// </summary>
        public bool IntersectWith(Bounding bounding)
        {
            bool b1 = (bounding.left >= this.left && bounding.left <= this.right)
                || (bounding.right >= this.left && bounding.right <= this.right)
                || (bounding.left <= this.left && bounding.right >= this.right);

            if (b1)
            {
                bool b2 = (this.bottom >= bounding.bottom && this.bottom <= bounding.top)
                    || (this.top >= bounding.bottom && this.top <= bounding.top)
                    || (this.bottom <= this.bottom && this.top >= bounding.top);
                if (b2)
                {
                    return true;
                }
            }
            

            return false;
        }

        private bool ValueInRange(double value, double min, double max)
        {
            return value >= min && value <= max;
        }
    }
}
