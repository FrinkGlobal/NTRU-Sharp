using System;
using System.Runtime.InteropServices;
using NTRU.ffi;
using NTRU.Params;

namespace NTRU.types
{
    public static class types
    {
        public const uint MAX_DEGREE = (1499 +1);

        const uint INT_POLOY_SIZE = ((MAX_DEGREE + 16 +7) & 0xFFF8);

        public const uint MAX_ONES = 499;



        [StructLayout(LayoutKind.Sequential)]
        public struct IntPoly {

            [MarshalAs(UnmanagedType.U2)]
            ushort n;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)MAX_DEGREE)]
            short[] coeffs;


            public IntPoly (short[] coeffs) {
            n = (ushort)coeffs.Length;
            this.coeffs = coeffs;
            }

            public short[] get_coeffs { get { return coeffs; } }

            public void set_coeffs(short[] value) { coeffs = value; }

            public void set_coeff(uint index, short value) { coeffs[index] = value; }

            public void mod_mask(ushort mod_mask) { }

            //public byte[] to_arr_32()
        }
    }
   

}