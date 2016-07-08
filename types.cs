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
        /// Size of the union in 16 bit words
        public const uint PRIVUNION_SIZE = 3004;      

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
                p = (IntPoly)Marshal.PtrToStructure(poly_ptr, typeof(IntPoly));
                //Marshal.FreeHGlobal(poly_ptr);
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
                this = (IntPoly)Marshal.PtrToStructure(this_ptr, typeof(IntPoly));
                //Marshal.FreeHGlobal(this_ptr);
                Marshal.FreeHGlobal(other_ptr);
            }

             public void sub(IntPoly rhs) {
                IntPtr this_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, this_ptr, false);
                IntPtr other_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rhs));
                Marshal.StructureToPtr(rhs, other_ptr, false);
                ffi.ntru_sub(this_ptr, other_ptr);
                this = (IntPoly)Marshal.PtrToStructure(this_ptr, typeof(IntPoly));
                //Marshal.FreeHGlobal(this_ptr);
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

            public TernPoly (ushort n, ushort[] ones, ushort[] neg_ones) {
                this.n = n;
                this.num_ones = (ushort)ones.Length;
                this.num_neg_ones = (ushort)neg_ones.Length;
                this.ones = ones;
                this.neg_ones = neg_ones;
            }

            public TernPoly clone() {
                ushort[] new_ones = new ushort[types.MAX_ONES];
                ushort[] new_neg_ones = new ushort[types.MAX_ONES];
                new_ones = this.ones;
                new_neg_ones = this.neg_ones;
                return new TernPoly(this.n, new_ones, new_neg_ones);
            }

            public static TernPoly Default() {
                return new TernPoly (0, 0, 0, new ushort[types.MAX_ONES], new ushort[types.MAX_ONES]);
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

            public static TernPoly rand(ushort n, ushort num_ones, ushort num_neg_ones, RandContext rand_ctx) {
                TernPoly poly = TernPoly.Default();
                IntPtr poly_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(poly));
                Marshal.StructureToPtr(poly, poly_ptr, false);
                IntPtr rand_ctx_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_ctx.rand_ctx));
                Marshal.StructureToPtr(rand_ctx.rand_ctx, rand_ctx_ptr, false);
                var result = ffi.ntru_rand_tern(n, num_ones, num_neg_ones, poly_ptr, rand_ctx_ptr);
                if (result == 0)
                    Console.WriteLine("Error: Failed to Generate Random TernPoly");
                poly = (TernPoly)Marshal.PtrToStructure(poly_ptr, typeof(TernPoly));
                //Marshal.FreeHGlobal(poly_ptr);
                Marshal.FreeHGlobal(rand_ctx_ptr);
                return poly;
            }

            public IntPoly to_int_poly() {

                short[] coeffs = new short[types.INT_POLY_SIZE];

                for (int i = 0; i < this.num_ones; i++) {
                    coeffs[this.ones[i]] = 1;
                }

                 for (int i = 0; i < this.num_neg_ones; i++) {
                    coeffs[this.neg_ones[i]] = 1;
                }
                
                return new IntPoly(this.n, coeffs);
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProdPoly {
            [MarshalAs(UnmanagedType.U2)]
            ushort n;

            TernPoly f1;

            TernPoly f2;

            TernPoly f3;

            public ProdPoly (ushort n, TernPoly f1, TernPoly f2, TernPoly f3) {
                this.n = n;
                this.f1 = f1;
                this.f2 = f2;
                this.f3 = f3;
            }

            public static ProdPoly Default() {
                return new ProdPoly (0, TernPoly.Default(), TernPoly.Default(), TernPoly.Default());
            }

            public static ProdPoly rand(ushort n, ushort df1, ushort df2, ushort df3_ones, ushort df3_neg_ones, RandContext rand_ctx) {
                TernPoly f1 = TernPoly.rand(n, df1, df1, rand_ctx);
                TernPoly f2 = TernPoly.rand(n, df2, df2, rand_ctx);
                TernPoly f3 = TernPoly.rand(n, df3_ones, df3_neg_ones, rand_ctx);
                return new ProdPoly(n, f1, f2, f3);
            }

        }


        [StructLayout(LayoutKind.Sequential)]
        public struct PrivUnion {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)types.PRIVUNION_SIZE)]
            ushort[] data;

            public PrivUnion(ushort[] data) {
               this.data = data; 
            }

            public static PrivUnion Default() {
                ushort[] new_data = new ushort[types.PRIVUNION_SIZE];
                return new PrivUnion(new_data);
            }

            public PrivUnion clone() {
                return new PrivUnion(this.data);
            }


        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PrivPoly {
            [MarshalAs(UnmanagedType.U1)]
            byte product_flag;

            PrivUnion poly;

            public PrivPoly (byte product_flag, PrivUnion poly) {
                this.product_flag = product_flag;
                this.poly = poly;
            }

            public static PrivPoly Default() {
                return new PrivPoly(0, PrivUnion.Default());
            }
        
            public static PrivPoly new_with_prod_poly (ProdPoly poly) {
                throw new NotImplementedException();
            }

            public static PrivPoly new_with_tern_poly (TernPoly poly) {
                throw new NotImplementedException();
            } 

            public bool is_product() {
                return (this.product_flag == 1);
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PrivateKey {
            [MarshalAs(UnmanagedType.U2)]
            ushort q;

            PrivPoly t;


            public PrivateKey (ushort q, PrivPoly t) {
                this.q = q;
                this.t = t;
            }

            public static PrivateKey Default() {
                return new PrivateKey(0, PrivPoly.Default());
            }

            public ushort get_q() {
                return q;
            }

            public PrivPoly get_t() {
                return t;
            }

            public EncParams get_params() {
                EncParams param = EncParams.Default();
                IntPtr key_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, key_ptr, false);
                IntPtr param_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(param));
                Marshal.StructureToPtr(param, param_ptr, false);
                var result = ffi.ntru_params_from_priv_key(key_ptr, param_ptr);
                 if (result != 0)
                    Console.WriteLine("Error: Failed to Get Encryption Params from private key");
                param = (EncParams)Marshal.PtrToStructure(param_ptr, typeof(EncParams));
                //Marshal.FreeHGlobal(param_ptr);
                //Marshal.FreeHGlobal(key_ptr);
                return param;
            }

            public static PrivateKey import(byte[] arr) {
                PrivateKey key = PrivateKey.Default();
                IntPtr key_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(key));
                Marshal.StructureToPtr(key, key_ptr, false);
                IntPtr arr_ptr = Marshal.AllocHGlobal(arr.Length);
                Marshal.Copy(arr, 0, arr_ptr, arr.Length);
                ffi.ntru_import_priv(arr_ptr, key_ptr);
                key = (PrivateKey)Marshal.PtrToStructure(key_ptr, typeof(PrivateKey));
                //Marshal.FreeHGlobal(key_ptr);
                Marshal.FreeHGlobal(arr_ptr);
                return key;
            }

            public byte[] export(EncParams param) {
                byte[] arr = new byte[param.private_len()];
                IntPtr key_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, key_ptr, false);
                IntPtr arr_ptr = Marshal.AllocHGlobal(arr.Length);
                ffi.ntru_export_priv(key_ptr, arr_ptr);
                Marshal.Copy(arr_ptr, arr, 0, arr.Length);
                Marshal.FreeHGlobal(key_ptr);
                Marshal.FreeHGlobal(arr_ptr);
                return arr;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PublicKey {
            [MarshalAs(UnmanagedType.U2)]
            ushort q;

            IntPoly h;


            public PublicKey (ushort q, IntPoly h) {
                this.q = q;
                this.h = h;
            }

            public static PublicKey Default() {
                return new PublicKey(0, IntPoly.Default());
            }

            public ushort get_q() {
                return q;
            }

            public IntPoly get_h() {
                return h;
            }

            public static PublicKey import(byte[] arr) {
                PublicKey key = PublicKey.Default();
                IntPtr key_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(key));
                IntPtr arr_ptr = Marshal.AllocHGlobal(arr.Length);
                Marshal.Copy(arr, 0, arr_ptr, arr.Length);
                ffi.ntru_import_pub(arr_ptr, key_ptr);
                key = (PublicKey)Marshal.PtrToStructure(key_ptr, typeof(PublicKey));
                Marshal.FreeHGlobal(arr_ptr);
                return key;
            }

            public byte[] export(EncParams param) {
                byte[] arr = new byte[param.enc_len()];
                IntPtr arr_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(arr.Length));
                IntPtr key_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this));
                Marshal.StructureToPtr(this, key_ptr, false);
                ffi.ntru_export_pub(key_ptr, arr_ptr);
                Marshal.Copy(arr_ptr, arr, 0, arr.Length);
                Marshal.FreeHGlobal(key_ptr);
                Marshal.FreeHGlobal(arr_ptr);
                return arr;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CNtruKeyPair
        {
            IntPtr Private;

            IntPtr Public;

            public CNtruKeyPair (IntPtr Private, IntPtr Public) {
                this.Private = Private;
                this.Public = Public;
            }

            public static CNtruKeyPair Default() {
                IntPtr def_priv_key = Marshal.AllocHGlobal(Marshal.SizeOf(PrivateKey.Default()));
                Marshal.StructureToPtr(PrivateKey.Default(), def_priv_key, false);
                IntPtr def_pub_key = Marshal.AllocHGlobal(Marshal.SizeOf(PublicKey.Default()));
                Marshal.StructureToPtr(PublicKey.Default(), def_pub_key, false);
                return new CNtruKeyPair(def_priv_key, def_pub_key);
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct KeyPair {
            
            public PrivateKey Private;

            public PublicKey Public;

            public KeyPair (PrivateKey Private, PublicKey Public) {
                this.Private = Private;
                this.Public = Public;
            }

            public static KeyPair Default() {
                return new KeyPair (PrivateKey.Default(), PublicKey.Default());
            }

            public EncParams get_params() {
                return Private.get_params();
            }

            public PrivateKey get_private() {
                return Private;
            }

            public PublicKey get_public() {
                return Public;
            }
        }

}