using System;
using System.Runtime.InteropServices;
using NTRU.Params;

namespace NTRU.types
{
    public static class types
    {
        /// Max N Value for all param sets; +1 for ntru_invert_...()
        public const uint MAX_DEGREE = (1499 + 1);
        /// (Max #coefficients + 16) rounded to multiple of 8
        const uint INT_POLOY_SIZE = ((MAX_DEGREE + 16 +7) & 0xFFF8);
        /// max (df1, df2, df3, dg)
        public const uint MAX_ONES = 499;

        /// A polynomial with integer coefficients
        [StructLayout(LayoutKind.Sequential)]
        public struct IntPoly {
            /// The number of coefficients
            [MarshalAs(UnmanagedType.U2)]
            ushort n;
            /// The coefficients
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)MAX_DEGREE)]
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
           
            public static IntPoly Default () {
                return new IntPoly(0, new short[INT_POLOY_SIZE]);             
            }

            public IntPoly clone() {
                short[] new_coeffs = new short[INT_POLOY_SIZE];
                for (int i = 0; i < this.n; i++) {
                    new_coeffs[i] = this.coeffs[i];
                }
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
    }
   
}