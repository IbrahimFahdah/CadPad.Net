using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CADPadWPF.Helpers
{
    /// <summary>
    /// A TextBlock like control that provides special text trimming logic
    /// designed for a file or folder path.
    /// </summary>
    public class PathBlock : UserControl
    {
        readonly TextBlock textBlock;

        public PathBlock()
        {
            textBlock = new TextBlock();
            AddChild(textBlock);
        }


        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);

            // This is where the control requests to be as large
            // as is needed while fitting within the given bounds
            var meas = TrimToFit(Path, constraint);

            // Update the text
            textBlock.Text = meas.Item1;

            return meas.Item2;
        }

        /// <summary>
        /// Trims the given path until it fits within the given constraints.
        /// </summary>
        /// <param name="path">The path to trim.</param>
        /// <param name="constraint">The size constraint.</param>
        /// <returns>The trimmed path and its size.</returns>
        Tuple<string, Size> TrimToFit(string path, Size constraint)
        {
            if (path == null)
            {
                path = "";
            }

            // If the path does not need to be trimmed
            // then return immediately
            Size size = MeasureString(path);
            if (size.Width < constraint.Width)
            {
                return new Tuple<string, Size>(path, size);
            }

            // Do not perform trimming if the path is not valid
            // because the below algoritm will not work
            // if we cannot separate the filename from the directory
            string filename;
            string directory;
            try
            {
                filename = System.IO.Path.GetFileName(path);
                directory = System.IO.Path.GetDirectoryName(path);
            }
            catch (Exception)
            {
                return new Tuple<string, Size>(path, size);
            }

            while (true)
            {
                path = String.Format(@"{0}...\{1}", directory, filename);
                size = MeasureString(path);

                if (size.Width <= constraint.Width)
                {
                    // If size is within constraints
                    // then stop trimming
                    break;
                }

                // Shorten the directory component of the path
                // and continue
                directory = directory.Substring(0, directory.Length - 1);

                // If the directory component is completely gone
                // then replace it with ellipses and stop
                if (directory.Length == 0)
                {
                    path = @"...\" + filename;
                    size = MeasureString(path);
                    break;
                }

            }

            return new Tuple<string, Size>(path, size);
        }

        /// <summary>
        /// Returns the size of the given string if it were to be rendered.
        /// </summary>
        /// <param name="str">The string to measure.</param>
        /// <returns>The size of the string.</returns>
        Size MeasureString(string str)
        {
            var typeFace = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            var text = new FormattedText(str, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, FontSize, Foreground);

            return new Size(text.Width, text.Height);
        }


        /// <summary>
        /// Gets or sets the path to display.
        /// The text that is actually displayed will be trimmed appropriately.
        /// </summary>
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(PathBlock), new UIPropertyMetadata("", OnPathChanged));
        static void OnPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            PathBlock @this = (PathBlock)o;

            // This element will be re-measured
            // The text will be updated during that process
            @this.InvalidateMeasure();
        }
    }
}