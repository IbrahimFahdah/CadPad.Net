using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CADPadDB.Colors;

namespace CADPadWPF.Control
{
    public static class CanvasPalette
    {
        public static Dictionary<string, Color> Colors=new Dictionary<string, Color>();


        public static Color WPFColor(CADColor color)
        {
            if (Colors.ContainsKey(color.GUID))
                return Colors[color.GUID];

            var wpf = ConvertToWPF(color);
            Colors.Add(color.GUID, wpf);

            return wpf;
        }

        public static Color ConvertToWPF(CADColor color)
        {
            return Color.FromRgb(color.r, color.g, color.b);
        }

        public static CADColor ConvertToCAD(Color color)
        {
            return CADColor.FromArgb(color.R, color.G , color.B );
        }
    }
}
