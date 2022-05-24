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

using System.Collections.Generic;
using netDxf.Blocks;
using netDxf.Tables;

namespace netDxf.Entities
{
    /// <summary>
    /// Predefined shapes for dimension style arrowheads.
    /// </summary>
    /// <remarks>
    /// Arrowhead block names and its representation.<br/>
    /// "" = closed filled<br/>
    /// "_DOT" = dot<br/>
    /// "_DOTSMALL" = dot small<br/>
    /// "_DOTBLANK" = dot blank<br/>
    /// "_ORIGIN" = origin indicator<br/>
    /// "_ORIGIN2" = origin indicator 2<br/>
    /// "_OPEN" = open<br/>
    /// "_OPEN90" = open 90 (right angle)<br/>
    /// "_OPEN30" = open 30<br/>
    /// "_CLOSED" = closed<br/>
    /// "_SMALL" = dot small blank<br/>
    /// "_NONE" = none<br/>
    /// "_OBLIQUE" = oblique<br/>
    /// "_BOXFILLED" = box filled<br/>
    /// "_BOXBLANK" = box<br/>
    /// "_CLOSEDBLANK" = closed blank<br/>
    /// "_DATUMFILLED" = datum triangle filled<br/>
    /// "_DATUMBLANK" = datum triangle<br/>
    /// "_INTEGRAL" = integral<br/>
    /// "_ARCHTICK" = architectural tick<br/>
    /// </remarks>
    public static class DimensionArrowhead
    {
        #region predefined dimensions arrowheads

        /// <summary>
        /// Dot arrowhead.
        /// </summary>
        public static Block Dot
        {
            get
            {
                Block arrowhead = new Block("_DOT");

                List<Polyline2DVertex> vertexes = new List<Polyline2DVertex>
                {
                    new Polyline2DVertex(-0.25, 0.0, 1.0),
                    new Polyline2DVertex(0.25, 0.0, 1.0)
                };
                Polyline2D pol = new Polyline2D(vertexes, true)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock
                };
                pol.SetConstantWidth(0.5);
                arrowhead.Entities.Add(pol);

                Line line = new Line(new Vector3(-0.5, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line);

                return arrowhead;
            }
        }

        /// <summary>
        /// Small dot arrowhead.
        /// </summary>
        public static Block DotSmall
        {
            get
            {
                Block arrowhead = new Block("_DOTSMALL");

                List<Polyline2DVertex> vertexes = new List<Polyline2DVertex>
                {
                    new Polyline2DVertex(-0.0625, 0.0, 1.0),
                    new Polyline2DVertex(0.0625, 0.0, 1.0)
                };
                Polyline2D pol = new Polyline2D(vertexes, true)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock
                };
                pol.SetConstantWidth(0.5);
                arrowhead.Entities.Add(pol);

                return arrowhead;
            }
        }

        /// <summary>
        /// Blank dot arrowhead.
        /// </summary>
        public static Block DotBlank
        {
            get
            {
                Block arrowhead = new Block("_DOTBLANK");

                Circle circle = new Circle(new Vector3(0.0, 0.0, 0.0), 0.5)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(circle);

                Line line = new Line(new Vector3(-0.5, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line);

                return arrowhead;
            }
        }

        /// <summary>
        /// Origin indicator arrowhead.
        /// </summary>
        public static Block OriginIndicator
        {
            get
            {
                Block arrowhead = new Block("_ORIGIN");

                Circle circle = new Circle(new Vector3(0.0, 0.0, 0.0), 0.5)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(circle);

                Line line = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line);

                return arrowhead;
            }
        }

        /// <summary>
        /// Origin indicator 2 arrowhead.
        /// </summary>
        public static Block OriginIndicator2
        {
            get
            {
                Block arrowhead = new Block("_ORIGIN2");

                Circle circle1 = new Circle(new Vector3(0.0, 0.0, 0.0), 0.5)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(circle1);

                Circle circle2 = new Circle(new Vector3(0.0, 0.0, 0.0), 0.25)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(circle2);

                Line line = new Line(new Vector3(-0.5, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line);

                return arrowhead;
            }
        }

        /// <summary>
        /// Open arrowhead.
        /// </summary>
        public static Block Open
        {
            get
            {
                Block arrowhead = new Block("_OPEN");

                Line line1 = new Line(new Vector3(-1.0, 0.1666666666666666, 0.0), new Vector3(0.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line1);

                Line line2 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, -0.1666666666666666, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line2);

                Line line3 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line3);

                return arrowhead;
            }
        }

        /// <summary>
        /// Open 90 degree arrowhead (right angle arrowhead).
        /// </summary>
        public static Block Open90
        {
            get
            {
                Block arrowhead = new Block("_OPEN90");

                Line line1 = new Line(new Vector3(-0.5, 0.5, 0.0), new Vector3(0.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line1);

                Line line2 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-0.5, -0.5, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line2);

                Line line3 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line3);

                return arrowhead;
            }
        }

        /// <summary>
        /// Open 30 degree angle arrowhead.
        /// </summary>
        public static Block Open30
        {
            get
            {
                Block arrowhead = new Block("_OPEN30");

                Line line1 = new Line(new Vector3(-1.0, 0.26794919, 0.0), new Vector3(0.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line1);

                Line line2 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, -0.26794919, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line2);

                Line line3 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line3);

                return arrowhead;
            }
        }

        /// <summary>
        /// Closed arrowhead.
        /// </summary>
        public static Block Closed
        {
            get
            {
                Block arrowhead = new Block("_CLOSED");

                Line line1 = new Line(new Vector3(-1.0, 0.1666666666666666, 0.0), new Vector3(0.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line1);

                Line line2 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, -0.1666666666666666, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line2);

                Line line3 = new Line(new Vector3(-1.0, 0.1666666666666666, 0.0), new Vector3(-1.0, -0.1666666666666666, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line3);

                Line line4 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line4);

                return arrowhead;
            }
        }

        /// <summary>
        /// Small blank dot arrowhead.
        /// </summary>
        public static Block DotSmallBlank
        {
            get
            {
                Block arrowhead = new Block("_SMALL");

                Circle circle = new Circle(new Vector3(0.0, 0.0, 0.0), 0.25)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(circle);

                return arrowhead;
            }
        }

        /// <summary>
        /// Empty arrowhead.
        /// </summary>
        public static Block None
        {
            get
            {
                Block arrowhead = new Block("_NONE");
                return arrowhead;
            }
        }

        /// <summary>
        /// Oblique arrowhead.
        /// </summary>
        public static Block Oblique
        {
            get
            {
                Block arrowhead = new Block("_OBLIQUE");

                Line line = new Line(new Vector3(-0.5, -0.5, 0.0), new Vector3(0.5, 0.5, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line);

                return arrowhead;
            }
        }

        /// <summary>
        /// Filled box arrowhead.
        /// </summary>
        public static Block BoxFilled
        {
            get
            {
                Block arrowhead = new Block("_BOXFILLED");

                Solid solid = new Solid(new Vector2(-0.5, 0.5), new Vector2(0.5, 0.5), new Vector2(-0.5, -0.5), new Vector2(0.5, -0.5))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock
                };
                arrowhead.Entities.Add(solid);

                Line line = new Line(new Vector3(-0.5, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line);

                return arrowhead;
            }
        }

        /// <summary>
        /// Box arrowhead.
        /// </summary>
        public static Block Box
        {
            get
            {
                Block arrowhead = new Block("_BOXBLANK");

                Line line1 = new Line(new Vector3(-0.5, -0.5, 0.0), new Vector3(0.5, -0.5, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line1);

                Line line2 = new Line(new Vector3(0.5, -0.5, 0.0), new Vector3(0.5, 0.5, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line2);

                Line line3 = new Line(new Vector3(0.5, 0.5, 0.0), new Vector3(-0.5, 0.5, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line3);

                Line line4 = new Line(new Vector3(-0.5, 0.5, 0.0), new Vector3(-0.5, -0.5, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line4);

                Line line5 = new Line(new Vector3(-0.5, 0.0, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line5);

                return arrowhead;
            }
        }

        /// <summary>
        /// Blank closed arrowhead.
        /// </summary>
        public static Block ClosedBlank
        {
            get
            {
                Block arrowhead = new Block("_CLOSEDBLANK");

                Line line1 = new Line(new Vector3(-1.0, 0.1666666666666666, 0.0), new Vector3(0.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line1);

                Line line2 = new Line(new Vector3(0.0, 0.0, 0.0), new Vector3(-1.0, -0.1666666666666666, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line2);

                Line line3 = new Line(new Vector3(-1.0, 0.1666666666666666, 0.0), new Vector3(-1.0, -0.1666666666666666, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line3);

                return arrowhead;
            }
        }

        /// <summary>
        /// Filled datum triangle arrowhead.
        /// </summary>
        public static Block DatumTriangleFilled
        {
            get
            {
                Block arrowhead = new Block("_DATUMFILLED");

                Solid solid = new Solid(new Vector2(0.0, 0.57735027), new Vector2(-1.0, 0.0), new Vector2(0.0, -0.57735027))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock
                };
                arrowhead.Entities.Add(solid);

                return arrowhead;
            }
        }

        /// <summary>
        /// Datum triangle arrowhead.
        /// </summary>
        public static Block DatumTriangle
        {
            get
            {
                Block arrowhead = new Block("_DATUMBLANK");

                Line line1 = new Line(new Vector3(0.0, 0.5773502700000001, 0.0), new Vector3(-1.0, 0.0, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line1);

                Line line2 = new Line(new Vector3(-1.0, 0.0, 0.0), new Vector3(0.0, -0.5773502700000001, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line2);

                Line line3 = new Line(new Vector3(0.0, -0.5773502700000001, 0.0), new Vector3(0.0, 0.5773502700000001, 0.0))
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(line3);

                return arrowhead;
            }
        }

        /// <summary>
        /// Integral symbol arrowhead.
        /// </summary>
        public static Block Integral
        {
            get
            {
                Block arrowhead = new Block("_INTEGRAL");

                Arc arc1 = new Arc(new Vector3(0.44488802, -0.09133463, 0.0), 0.4541666700000001, 101.9999999980395, 167.9999999799193)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(arc1);

                Arc arc2 = new Arc(new Vector3(-0.44488802, 0.09133463, 0.0), 0.4541666700000001, 282.0000000215427, 348.0000000034225)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock,
                    Lineweight = Lineweight.ByBlock
                };
                arrowhead.Entities.Add(arc2);

                return arrowhead;
            }
        }

        /// <summary>
        /// Architectural tick arrowhead.
        /// </summary>
        public static Block ArchitecturalTick
        {
            get
            {
                Block arrowhead = new Block("_ARCHTICK");

                List<Polyline2DVertex> vertexes = new List<Polyline2DVertex>
                {
                    new Polyline2DVertex(-0.5, -0.5),
                    new Polyline2DVertex(0.5, 0.5)
                };
                Polyline2D pol = new Polyline2D(vertexes, false)
                {
                    Layer = Layer.Default,
                    Linetype = Linetype.ByBlock,
                    Color = AciColor.ByBlock
                };

                pol.SetConstantWidth(0.15);
                arrowhead.Entities.Add(pol);

                return arrowhead;
            }
        }

        #endregion
    }
}