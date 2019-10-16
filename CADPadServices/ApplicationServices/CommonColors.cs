using System.Collections.Generic;
using CADPadDB.Colors;

namespace CADPadServices.ApplicationServices
{
    /// <summary>
    /// 常用颜色集合
    /// </summary>
    public class CommonColors : IEnumerable<CADColor>
    {
        private Dictionary<CADColor, string> _predefinedColors = new Dictionary<CADColor, string>();
        private List<CADColor> _commonColors = new List<CADColor>();

        public CommonColors()
        {
            InitPredefinedColors();
        }

        /// <summary>
        /// Initialize predefined colors
        /// </summary>
        private void InitPredefinedColors()
        {
            _predefinedColors.Add(CADColor.ByLayer, CADColor.ByLayer.Name);
            _predefinedColors.Add(CADColor.ByBlock, CADColor.ByBlock.Name);

            // Red
            _predefinedColors.Add(CADColor.FromArgb(255, 0, 0), "红");
            // Yellow
            _predefinedColors.Add(CADColor.FromArgb(255, 255, 0), "黄");
            // Green
            _predefinedColors.Add(CADColor.FromArgb(0, 255, 0), "绿");
            // Cyan
            _predefinedColors.Add(CADColor.FromArgb(0, 255, 255), "青");
            // Blue
            _predefinedColors.Add(CADColor.FromArgb(0, 0, 255), "蓝");
            // Magenta
            _predefinedColors.Add(CADColor.FromArgb(255, 0, 255), "洋红");
            // White
            _predefinedColors.Add(CADColor.FromArgb(255, 255, 255), "白");
        }

        public string GetColorName(CADColor color)
        {
            if (_predefinedColors.ContainsKey(color))
            {
                return _predefinedColors[color];
            }
            else
            {
                return color.Name;
            }
        }

        public bool Add(CADColor color)
        {
            if (_predefinedColors.ContainsKey(color)
                || _commonColors.Contains(color))
            {
                return false;
            }
            else
            {
                _commonColors.Add(color);
                return true;
            }
        }

        public void Clear()
        {
            _commonColors.Clear();
        }

        #region IEnumerable<CADColor>
        public IEnumerator<CADColor> GetEnumerator()
        {
            List<CADColor> colors = new List<CADColor>(_predefinedColors.Count + _commonColors.Count);
            foreach (CADColor color in _predefinedColors.Keys)
            {
                colors.Add(color);
            }
            foreach (CADColor color in _commonColors)
            {
                colors.Add(color);
            }

            return colors.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
