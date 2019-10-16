using System;

namespace CADPadDB.Colors
{
    public struct CADColor
    {

      

        public ColorMethod colorMethod { get; private set; }

        public byte r { get; private set; }

        public byte g { get; private set; }

        public byte b { get; private set; }

        public string GUID { get; private set; }

        private CADColor(byte r, byte g, byte b)
        {
            colorMethod = ColorMethod.ByColor;
            this.r = r;
            this.g = g;
            this.b = b;
            GUID = Guid.NewGuid().ToString();
        }

        public string Name
        {
            get
            {
                switch (colorMethod)
                {
                    case ColorMethod.ByLayer:
                        return "ByLayer";

                    case ColorMethod.ByBlock:
                        return "ByBlock";

                    case ColorMethod.None:
                        return "None";

                    case ColorMethod.ByColor:
                        return string.Format("{0},{1},{2}", r, g, b);

                    default:
                        return "";
                }
            }
        }

        public static CADColor FromArgb(byte r, byte g, byte b)
        {
            return new CADColor(r, g, b);
        }

        //public static CADColor FromColor(System.Drawing.CADColor color)
        //{
        //    return new CADColor(color.R, color.G, color.B);
        //}

        public static CADColor ByLayer
        {
            get
            {
                CADColor color = new CADColor();
                color.colorMethod = ColorMethod.ByLayer;
                color.r = 255;
                color.g = 255;
                color.b = 255;
                return color;
            }
        }

        public static CADColor ByBlock
        {
            get
            {
                CADColor color = new CADColor();
                color.colorMethod = ColorMethod.ByBlock;
                color.r = 255;
                color.g = 255;
                color.b = 255;
                return color;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1},{2},{3}", colorMethod.ToString(), r, g, b);
        }

        internal static bool TryParse(string str, out CADColor result)
        {
            string[] arr = str.Split(':');
            if (arr.Length != 2)
            {
                result = CADColor.ByLayer;
                return false;
            }

            //
            ColorMethod colorMethod = (ColorMethod)Enum.Parse(typeof(ColorMethod), arr[0], true);

            //
            string[] rgb = arr[1].Split(',');
            if (rgb.Length != 3)
            {
                result = CADColor.ByLayer;
                return false;
            }

            byte red = 0;
            byte green = 0;
            byte blue = 0;
            if (byte.TryParse(rgb[0], out red)
                && byte.TryParse(rgb[1], out green)
                && byte.TryParse(rgb[2], out blue))
            {
                result = new CADColor();
                result.colorMethod = colorMethod;
                result.r = red;
                result.g = green;
                result.b = blue;

                return true;
            }
            else
            {
                result = CADColor.ByLayer;
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CADColor))
                return false;

            return Equals((CADColor)obj);
        }

        public bool Equals(CADColor rhs)
        {
            if (colorMethod != rhs.colorMethod)
            {
                return false;
            }

            switch (colorMethod)
            {
                case ColorMethod.ByColor:
                    return r == rhs.r 
                        && g == rhs.g 
                        && b == rhs.b;

                case ColorMethod.ByBlock:
                case ColorMethod.ByLayer:
                case ColorMethod.None:
                default:
                    return true;
            }
        }

        public override int GetHashCode()
        {
            return colorMethod.GetHashCode();
        }

        public static bool operator ==(CADColor lhs, CADColor rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(CADColor lhs, CADColor rhs)
        {
            return !(lhs == rhs);
        }
    }
}
