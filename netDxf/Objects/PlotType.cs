#region netDxf library licensed under the MIT License
// 
//                       netDxf library
// Copyright (c) 2019-2021 Daniel Carvajal (haplokuon@gmail.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
#endregion

namespace netDxf.Objects
{
    /// <summary>
    /// Defines the portion of paper space to output to the media.
    /// </summary>
    public enum PlotType
    {
        /// <summary>
        /// Last screen display
        /// </summary>
        LastScreenDisplay = 0,

        /// <summary>
        /// Drawing extents.
        /// </summary>
        DrawingExtents = 1,

        /// <summary>
        /// Drawing limits.
        /// </summary>
        DrawingLimits = 2,

        /// <summary>
        /// View specified by the plot view name.
        /// </summary>
        View = 3,

        /// <summary>
        /// Window specified by the upper-right and bottom-left window corners.
        /// </summary>
        Window = 4,

        /// <summary>
        /// Layout information.
        /// </summary>
        LayoutInformation = 5
    }
}