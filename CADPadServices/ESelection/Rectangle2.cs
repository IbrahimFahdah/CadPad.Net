using CADPadDB.Maths;

namespace CADPadServices.ESelection
{
    public struct Rectangle2
    {

        public CADPoint location;


        public double width;
        public double height;

        public CADPoint leftBottom
        {
            get { return location; }
        }

        public CADPoint leftTop
        {
            get
            {
                return new CADPoint(this.location.X, this.location.Y + height);
            }
        }

        public CADPoint rightTop
        {
            get
            {
                return new CADPoint(this.location.X + this.width,
                    this.location.Y + this.height);
            }
        }

        public CADPoint rightBottom
        {
            get
            {
                return new CADPoint(this.location.X + this.width, this.location.Y);
            }
        }

        public Rectangle2(CADPoint location, double width, double height)
        {
            this.location = location;
            this.width = width;
            this.height = height;
        }

        public Rectangle2(CADPoint point1, CADPoint point2)
        {
            double minX = point1.X < point2.X ? point1.X : point2.X;
            double minY = point1.Y < point2.Y ? point1.Y : point2.Y;
            this.location = new CADPoint(minX, minY);
            this.width = System.Math.Abs(point2.X - point1.X);
            this.height = System.Math.Abs(point2.Y - point1.Y);
        }

        public override string ToString()
        {
            return string.Format("Rectangle2(({0}, {1}), {2}, {3})",
                this.location.X,
                this.location.Y,
                this.width,
                this.height);
        }
    }
}
