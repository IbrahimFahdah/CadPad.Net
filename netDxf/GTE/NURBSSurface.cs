﻿// This is a translation to C# from the original C++ code of the Geometric Tool Library
// Original license
// David Eberly, Geometric Tools, Redmond WA 98052
// Copyright (c) 1998-2022
// Distributed under the Boost Software License, Version 1.0.
// https://www.boost.org/LICENSE_1_0.txt
// https://www.geometrictools.com/License/Boost/LICENSE_1_0.txt
// Version: 6.0.2022.01.06

namespace netDxf.GTE
{
    public class NURBSSurface :
        ParametricSurface
    {
        private readonly BasisFunction[] basisFunctions;
        private readonly int[] numControls;
        private readonly Vector3[] controls;
        private readonly double[] weights;

        // Construction.  If the input controls is non-null, a copy is made of
        // the controls.  To defer setting the control points or weights, pass
        // null pointers and later access the control points or weights via
        // GetControls(), GetWeights(), SetControl(), or SetWeight() member
        // functions.  The 'controls' and 'weights' must be stored in
        // row-major order, attribute[i0 + numControls0*i1].  As a 2D array,
        // this corresponds to attribute2D[i1][i0].
        public NURBSSurface(BasisFunctionInput input0, BasisFunctionInput input1, Vector3[] controls, double[] weights)
            : base(0-0, 1.0, 0.0, 1.0, true)
        {
            this.basisFunctions = new[] {new BasisFunction(input0), new BasisFunction(input1)};
            this.numControls = new[] {input0.NumControls, input1.NumControls};

            // The mBasisFunction stores the domain but so does ParametricSurface.
            this.uMin = this.basisFunctions[0].MinDomain;
            this.uMax = this.basisFunctions[0].MaxDomain;
            this.vMin = this.basisFunctions[1].MinDomain;
            this.vMax = this.basisFunctions[1].MaxDomain;

            // The replication of control points for periodic splines is
            // avoided by wrapping the i-loop index in Evaluate.
            this.controls = new Vector3[this.numControls[0] * this.numControls[1]];
            if (controls != null)
            {
                controls.CopyTo(this.controls, 0);
            }

            this.weights = new double[this.numControls[0] * this.numControls[1]];
            if (weights != null)
            {
                weights.CopyTo(this.weights, 0);
            }

            this.isConstructed = true;
        }

        // Member access.  The index 'dim' must be in {0,1}.
        public BasisFunction BasisFunction(int dim)
        {
            return this.basisFunctions[dim];
        }

        public int NumControls(int dim)
        {
            return this.numControls[dim];
        }

        public Vector3[] Controls
        {
            get { return this.controls; }
        }

        public double[] Weights
        {
            get { return this.weights; }
        }

        public void SetControl(int i0, int i1, Vector3 control)
        {
            if (0 <= i0 && i0 < this.NumControls(0) && 0 <= i1 && i1 < this.NumControls(1))
            {
                this.controls[i0 + this.numControls[0] * i1] = control;
            }
        }

        public Vector3 GetControl(int i0, int i1)
        {
            if (0 <= i0 && i0 < this.NumControls(0) && 0 <= i1 && i1 < this.NumControls(1))
            {
                return this.controls[i0 + this.numControls[0] * i1];
            }

            return this.controls[0];
        }

        public void SetWeight(int i0, int i1, double weight)
        {
            if (0 <= i0 && i0 < this.NumControls(0) && 0 <= i1 && i1 < this.NumControls(1))
            {
                this.weights[i0 + this.numControls[0] * i1] = weight;
            }
        }

        public double GetWeight(int i0, int i1)
        {
            if (0 <= i0 && i0 < this.NumControls(0) && 0 <= i1 && i1 < this.NumControls(1))
            {
                return this.weights[i0 + this.numControls[0] * i1];
            }

            return this.weights[0];
        }

        // Evaluation of the surface.  The function supports derivative
        // calculation through order 2; that is, order <= 2 is required.  If
        // you want only the position, pass in order of 0.  If you want the
        // position and first-order derivatives, pass in order of 1, and so
        // on.  The output array 'jet' must have enough storage to support the
        // maximum order.  The values are ordered as: position X; first-order
        // derivatives dX/du, dX/dv; second-order derivatives d2X/du2,
        // d2X/dudv, d2X/dv2.
        public override void Evaluate(double u, double v, int order, out Vector3[] jet)
        {
            const int supOrder = SUP_ORDER;
            jet = new Vector3[supOrder];

            if (!this.isConstructed || order >= supOrder)
            {
                // Return a zero-valued jet for invalid state.
                return;
            }

            this.basisFunctions[0].Evaluate(u, order, out int iumin, out int iumax);
            this.basisFunctions[1].Evaluate(v, order, out int ivmin, out int ivmax);

            // Compute position.
            this.Compute(0, 0, iumin, iumax, ivmin, ivmax, out Vector3 x, out double w);
            double invW = 1.0 / w;
            jet[0] = invW * x;

            if (order >= 1)
            {
                // Compute first-order derivatives.
                this.Compute(1, 0, iumin, iumax, ivmin, ivmax, out Vector3 xDerU, out double wDerU);
                jet[1] = invW * (xDerU - wDerU * jet[0]);

                this.Compute(0, 1, iumin, iumax, ivmin, ivmax, out Vector3 xDerV, out double wDerV);
                jet[2] = invW * (xDerV - wDerV * jet[0]);

                if (order >= 2)
                {
                    // Compute second-order derivatives.
                    this.Compute(2, 0, iumin, iumax, ivmin, ivmax, out Vector3 xDerUu, out double wDerUU);
                    jet[3] = invW * (xDerUu - 2.0 * wDerU * jet[1] - wDerUU * jet[0]);

                    this.Compute(1, 1, iumin, iumax, ivmin, ivmax, out Vector3 xDerUv, out double wDerUV);
                    jet[4] = invW * (xDerUv - wDerU * jet[2] - wDerV * jet[1] - wDerUV * jet[0]);

                    this.Compute(0, 2, iumin, iumax, ivmin, ivmax, out Vector3 xDerVv, out double wDerVV);
                    jet[5] = invW * (xDerVv - 2.0 * wDerV * jet[2] - wDerVV * jet[0]);
                }
            }
        }

    
        // Support for Evaluate(...).
        protected void Compute(int uOrder, int vOrder, int iumin, int iumax, int ivmin, int ivmax, out Vector3 x, out double  w)
        {
            // The j*-indices introduce a tiny amount of overhead in order to handle
            // both aperiodic and periodic splines.  For aperiodic splines, j* = i*
            // always.

            int numControls0 = this.numControls[0];
            int numControls1 = this.numControls[1];
            x = Vector3.Zero;
            w = 0.0;
            for (int iv = ivmin; iv <= ivmax; iv++)
            {
                double tmpv = this.basisFunctions[1].GetValue(vOrder, iv);
                int jv = (iv >= numControls1 ? iv - numControls1 : iv);
                for (int iu = iumin; iu <= iumax; iu++)
                {
                    double tmpu = this.basisFunctions[0].GetValue(uOrder, iu);
                    int ju = (iu >= numControls0 ? iu - numControls0 : iu);
                    int index = ju + numControls0 * jv;
                    double tmp = tmpu * tmpv * this.weights[index];
                    x += tmp * this.controls[index];
                    w += tmp;
                }
            }
        }
    };
}
