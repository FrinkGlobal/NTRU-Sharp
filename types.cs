using System;
using System.Runtime.InteropServices;
using NTRU.Params;
using NTRU.rand;

namespace NTRU.types
{
    public static class types
    {
        /// Max N Value for all param sets; +1 for ntru_invert_...()
        public const uint MAX_DEGREE = (1499 + 1);
        /// (Max #coefficients + 16) rounded to multiple of 8
        public const uint INT_POLY_SIZE = ((MAX_DEGREE + 16 +7) & 0xFFF8);
        /// max (df1, df2, df3, dg)
        public const uint MAX_ONES = 499;      

    }

        /// A polynomial with integer coefficients
        [StructLayout(LayoutKind.Sequential)]
        public struct IntPoly {
            /// The number of coefficients
            [MarshalAs(UnmanagedType.U2)]
            ushort n;
            /// The coefficients
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)types.INT_POLY_SIZE)]
            short[] coeffs;
            
            /// Create a new IntPoly
            public IntPoly (short[] coeffs) {
            n = (ushort)coeffs.Length;
            this.coeffs = coeffs;
            }

            /// Creates a new IntPoly
            public IntPoly (ushort n, short[] coeffs) {
                this.n = n;
                this.coeffs = coeffs;
            }
           
            public static IntPoly rand(ushort n, ushort pow2q, RandContext rand_ctx) {
                byte[] rand_data = rand_ctx.get_rng().generate((ushort)(n * 2), rand_ctx);

                short[] coeffs = new short[types.INT_POLY_SIZE];
                ushort shift = (ushort)(16 - pow2q);
                for (int i = 0; i < rand_data.Length; i++) {
                    coeffs[i] = (short)(rand_data[i] >> shift);
                }

                return new IntPoly(n, coeffs);

            }

            public static IntPoly from_arr (byte[] arr, ushort n, ushort q) {
                IntPoly p = IntPoly.Default();
                IntPtr poly_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(p));
                Marshal.StructureToPtr(p, poly_ptr, false);
                IntPtr arr_ptr = Marshal.AllocHGlobal(arr.Length);
                Marshal.Copy(arr, 0, arr_ptr, arr.Length);
                ffi.ntru_from_arr(arr_ptr, n, q, poly_ptr);
                Marshal.PtrToStructure(poly_ptr, p);
                Marshal.FreeHGlobal(poly_ptr);
                Marshal.FreeHGlobal(arr_ptr);
                return p;
            }

            public static IntPoly Default () {
                return new IntPoly(0, new short[types.INT_POLY_SIZE]);             
            }

            public IntPoly clone() {
                short[] new_coeffs = new short[types.INT_POLY_SIZE];
                new_coeffs = this.coeffs;
                return new IntPoly(this.n, new_coeffs);
            }

            public void add(IntPoly rhs) {
                IntPtr this_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, this_ptr, false);
                IntPtr other_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rhs));
                Marshal.StructureToPtr(rhs, other_ptr, false);
                ffi.ntru_add(this_ptr, other_ptr);
                Marshal.PtrToStructure(this_ptr, this);
                Marshal.FreeHGlobal(this_ptr);
                Marshal.FreeHGlobal(other_ptr);
            }

             public void sub(IntPoly rhs) {
                IntPtr this_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, this_ptr, false);
                IntPtr other_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rhs));
                Marshal.StructureToPtr(rhs, other_ptr, false);
                ffi.ntru_sub(this_ptr, other_ptr);
                Marshal.PtrToStructure(this_ptr, this);
                Marshal.FreeHGlobal(this_ptr);
                Marshal.FreeHGlobal(other_ptr);
            }

            public short[] get_coeffs { get { return coeffs; } }

            public void set_coeffs(short[] value) { coeffs = value; }

            public void set_coeff(uint index, short value) { coeffs[index] = value; }

            public void mod_mask(ushort mod_mask) {
                IntPtr this_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, this_ptr, false);
                ffi.ntru_mod_mask(this_ptr, mod_mask);
                Marshal.FreeHGlobal(this_ptr);
             }

            public byte[] to_arr_32(EncParams param) {
                IntPtr this_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, this_ptr, false);
                byte[] a = new byte[param.enc_len()];
                IntPtr a_byte = Marshal.AllocHGlobal(param.enc_len());
                ffi.ntru_to_arr_32(this_ptr, param.get_q(), a_byte);
                Marshal.Copy(a_byte, a, 0, param.enc_len());
                Marshal.FreeHGlobal(this_ptr);
                Marshal.FreeHGlobal(a_byte);
                return a;
            }

            public byte[] to_arr_64(EncParams param) {
                IntPtr this_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, this_ptr, false);
                byte[] a = new byte[param.enc_len()];
                IntPtr a_byte = Marshal.AllocHGlobal(param.enc_len());
                ffi.ntru_to_arr_64(this_ptr, param.get_q(), a_byte);
                Marshal.Copy(a_byte, a, 0, param.enc_len());
                Marshal.FreeHGlobal(this_ptr);
                Marshal.FreeHGlobal(a_byte);
                return a;
            }

            public byte[] to_arr_sse_2048 (EncParams param) {
                IntPtr this_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, this_ptr, false);
                byte[] a = new byte[param.enc_len()];
                IntPtr a_byte = Marshal.AllocHGlobal(param.enc_len());
                ffi.ntru_to_arr_sse_2048(this_ptr, a_byte);
                Marshal.Copy(a_byte, a, 0, param.enc_len());
                Marshal.FreeHGlobal(this_ptr);
                Marshal.FreeHGlobal(a_byte);
                return a;
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TernPoly
        {
            [MarshalAs(UnmanagedType.U2)]
            ushort n;
            [MarshalAs(UnmanagedType.U2)]
            ushort num_ones;
            [MarshalAs(UnmanagedType.U2)]
            ushort num_neg_ones;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)types.MAX_ONES)]
            ushort[] ones;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)types.MAX_ONES)]
            ushort[] neg_ones;


            public TernPoly (ushort n, ushort num_ones, ushort num_neg_ones, ushort[] ones, ushort[] neg_ones) {
                this.n = n;
                this.num_ones = num_ones;
                this.num_neg_ones = num_neg_ones;
                this.ones = ones;
                this.neg_ones = neg_ones;
            }

            public TernPoly clone() {
                ushort[] new_ones = new ushort[types.MAX_ONES];
                ushort[] new_neg_ones = new ushort[types.MAX_ONES];
                new_ones = this.ones;
                new_neg_ones = this.neg_ones;
                return new TernPoly(this.n, this.num_ones, this.num_neg_ones, new_ones, new_neg_ones);
            }

            public ushort get_n() {
                return this.n;
            }

            public ushort[] get_ones() {
                return this.ones;
            }

            public ushort[] get_neg_ones() {
                return this.neg_ones;
            }

            // public IntPoly to_int_poly() {

            // }
        }
   
}