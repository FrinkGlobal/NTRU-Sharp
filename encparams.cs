using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NTRU.Params
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EncParams {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        byte[] name;
        [MarshalAs(UnmanagedType.U2)]
        ushort n;
        [MarshalAs(UnmanagedType.U2)]
        ushort q;
        [MarshalAs(UnmanagedType.U1)]
        byte product_flag;
        [MarshalAs(UnmanagedType.U2)]
        ushort df1;
        [MarshalAs(UnmanagedType.U2)]
        ushort df2;
        [MarshalAs(UnmanagedType.U2)]
        ushort df3;
        [MarshalAs(UnmanagedType.U2)]
        ushort dg;
        [MarshalAs(UnmanagedType.U2)]
        ushort dm0;
        [MarshalAs(UnmanagedType.U2)]
        ushort db;
        [MarshalAs(UnmanagedType.U2)]
        ushort c;
        [MarshalAs(UnmanagedType.U2)]
        ushort min_calls_r;
        [MarshalAs(UnmanagedType.U2)]
        ushort min_calls_mask;
        [MarshalAs(UnmanagedType.U1)]
        byte hash_seed;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        byte[] oid;
        [MarshalAsAttribute(UnmanagedType.FunctionPtr)]
        Action<IntPtr, IntPtr, IntPtr> hash;
        [MarshalAsAttribute(UnmanagedType.FunctionPtr)]
        Action<IntPtr, IntPtr, IntPtr> hash_4way;
        [MarshalAs(UnmanagedType.U2)]
        ushort hlen;
        [MarshalAs(UnmanagedType.U2)]
        ushort pklen;
        
        public String get_name() { return Encoding.UTF8.GetString(name); }

        public ushort get_n() { return n; }

        public ushort get_q() { return q; }

        public ushort get_db() { return db; }

        public byte max_msg_len() { return (byte)(n / 2 * 3 - 1 - db / 8); }

        public ushort enc_len() {
            if(q != 0 && (q-1) != 0)
            {
                return 0;
            }
            else
            {         
                ushort _q = q;
                ushort log = 0;
                while (_q > 1)
                {
                    _q /= 2;
                    log += 1;
                }
                ushort len_bits =(ushort)(n * log);

                return (ushort)((len_bits + 7) / 8);
            }
        }

        public ushort public_len() {  return (ushort)(4 + this.enc_len()); }

        public ushort private_len() {
            if(product_flag == 1) {
               return (ushort) (5 + 5 + 4 * df1 + 4 + 4 * df2 + 4 + 4 * df3); 
            }
            else
            {
                return (ushort) (5 + 4 + 4 * df1);
            }
        }

        public EncParams (byte[] name, ushort n, ushort q, byte product_flag, ushort df1, ushort df2, ushort df3, ushort dg, ushort dm0, ushort db, ushort c, ushort min_calls_r, ushort min_calls_mask, byte hash_seed, byte[] oid, Action<IntPtr, IntPtr, IntPtr> hash, Action<IntPtr, IntPtr, IntPtr> hash_4way, ushort hlen, ushort pklen) {
            this.name = name;
            this.n = n;
            this.q = q;
            this.product_flag = product_flag;
            this.df1 = df1;
            this.df2 = df2;
            this.df3 = df3;
            this.dg = dg;
            this.dm0 = dm0;
            this.db = db;
            this.c = c;
            this.min_calls_r = min_calls_r;
            this.min_calls_mask = min_calls_mask;
            this.hash_seed = hash_seed;
            this.oid = oid;
            this.hash = hash;
            this.hash_4way = hash_4way;
            this.hlen = hlen;
            this.pklen = pklen;
        }

        public static EncParams Default() {
            return new EncParams(
                new byte[11],
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                new byte[3],
                ffi.ntru_sha1,
                ffi.ntru_sha1_4way,
                0,
                0
            );
        }
    }

    public static class EncParamSets
    {
        /// An IEEE 1361.1 parameter set that gives 112 bits of security and is optimized for key size.
        public static readonly EncParams EES401EP1 = new EncParams (
            new byte[] {69, 69, 83, 52, 48, 49, 69, 80, 49, 0, 0 },
            401,
            2048,
            0,
            113,
            0,
            0,
            133,
            133,
            112,
            11,
            32,
            9,
            1,
            new byte[] {0, 2, 4},
            ffi.ntru_sha1,
            ffi.ntru_sha1_4way,
            20,
            114
        );

        /// An IEEE 1361.1 parameter set that gives 192 bits of security and is optimized for key size.
        public static readonly EncParams EES449EP1 = new EncParams (
            new byte[] {69, 69, 83, 54, 55, 55, 69, 80, 49, 0, 0},
            677,
            2048,
            0,
            157,
            0,
            0,
            225,
            157,
            192,
            11,
            27,
            9,
            1,
            new byte[] {0, 5, 3},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            192
        );

        /// An IEEE 1361.1 parameter set that gives 256 bits of security and is optimized for key size.
        public static readonly EncParams EES1087EP2 = new EncParams (
            new byte[] {69, 69, 83, 49, 48, 56, 55, 69, 80, 50, 0},
            1087,
            2048,
            0,
            120,
            0,
            0,
            362,
            120,
            256,
            13,
            25,
            14,
            1,
            new byte[] {0, 6, 3},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            256
        );

        /// An IEEE 1361.1 parameter set that gives 112 bits of security and is a tradeoff between key size
        /// and encryption/decryption speed.
        public static readonly EncParams EES541EP1 = new EncParams (
            new byte[] {69, 69, 83, 53, 52, 49, 69, 80, 49, 0, 0},
            541,
            2048,
            0,
            49,
            0,
            0,
            180,
            49,
            112,
            12,
            15,
            11,
            1,
            new byte[] {0, 2, 5},
            ffi.ntru_sha1,
            ffi.ntru_sha1_4way,
            20,
            112
        );

        /// An IEEE 1361.1 parameter set that gives 128 bits of security and is a tradeoff between key
        /// size and encryption/decryption speed.
        public static readonly EncParams EES613EP1 = new EncParams (
            new byte[] {69, 69, 83, 54, 49, 51, 69, 80, 49, 0, 0},
            613,
            2048,
            0,
            55,
            0,
            0,
            204,
            55,
            128,
            11,
            16,
            13,
            1,
            new byte[] {0, 3, 4},
            ffi.ntru_sha1,
            ffi.ntru_sha1_4way,
            20,
            128
        );

        /// An IEEE 1361.1 parameter set that gives 192 bits of security and is a tradeoff between key size
        /// and encryption/decryption speed.
        public static readonly EncParams EES887EP1 = new EncParams (
            new byte[] {69, 69, 83, 56, 56, 55, 69, 80, 49, 0, 0},
            887,
            2048,
            0,
            81,
            0,
            0,
            295,
            81,
            192,
            10,
            13,
            12,
            1,
            new byte[] {0, 5, 4},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            192
        );

        /// An IEEE 1361.1 parameter set that gives 256 bits of security and is a tradeoff between key size
        /// and encryption/decryption speed.
        public static readonly EncParams EES1171EP1 = new EncParams (
            new byte[] {69, 69, 83, 49, 55, 49, 69, 80, 49, 0},
            1171,
            2048,
            0,
            106,
            0,
            0,
            390,
            106,
            256,
            12,
            20,
            15,
            1,
            new byte[] {0, 6, 4},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            256
        );


        /// An IEEE 1361.1 parameter set that gives 112 bits of security and is optimized for
        /// encryption/decryption speed.
        public static readonly EncParams EES659EP1 = new EncParams (
            new byte[] {69, 69, 83, 54, 53, 57, 69, 80, 49, 0, 0},
            659,
            2048,
            0,
            38,
            0,
            0,
            219,
            38,
            112,
            11,
            11,
            14,
            1,
            new byte[] {0, 2, 6},
            ffi.ntru_sha1,
            ffi.ntru_sha1_4way,
            20,
            112
        );


        /// An IEEE 1361.1 parameter set that gives 128 bits of security and is optimized for
        /// encryption/decryption speed.
        public static readonly EncParams EES761EP1 = new EncParams (
            new byte[] {69, 69, 83, 55, 54, 49, 69, 80, 49, 0, 0},
            761,
            2048,
            0,
            42,
            0,
            0,
            253,
            42,
            128,
            12,
            13,
            16,
            1,
            new byte[] {0, 5, 3},
            ffi.ntru_sha1,
            ffi.ntru_sha1_4way,
            20,
            128
        );


        /// An IEEE 1361.1 parameter set that gives 192 bits of security and is optimized for
        /// encryption/decryption speed.
        public static readonly EncParams EES1087EP1 = new EncParams (
            new byte[] {69, 69, 83, 49, 48, 56, 55, 69, 80, 49, 0},
            1087,
            2048,
            0,
            63,
            0,
            0,
            362,
            63,
            192,
            13,
            13,
            14,
            1,
            new byte[] {0, 5, 5},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            192
        );


        /// An IEEE 1361.1 parameter set that gives 256 bits of security and is optimized for
        /// encryption/decryption speed.
        public static readonly EncParams EES1499EP1 = new EncParams (
            new byte[] {69, 69, 83, 49, 52, 57, 57, 69, 80, 49, 0},
            1499,
            2048,
            0,
            79,
            0,
            0,
            499,
            79,
            256,
            13,
            17,
            19,
            1,
            new byte[] {0, 6, 5},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            256
        );

        /// A product-form parameter set that gives 112 bits of security.
        public static readonly EncParams EES401EP2 = new EncParams (
            new byte[] {69, 69, 83, 52, 48, 49, 69, 80, 50, 0, 0},
            401,
            2048,
            1,
            8,
            8,
            6,
            133,
            101,
            112,
            11,
            10,
            6,
            1,
            new byte[] {0, 2, 16},
            ffi.ntru_sha1,
            ffi.ntru_sha1_4way,
            20,
            112
        );

        /// A product-form parameter set that gives 128 bits of security.
        public static readonly EncParams EES443EP1 = new EncParams (
            new byte[] {69, 69, 83, 52, 52, 51, 69, 80, 49, 0, 0},
            443,
            2048,
            1,
            9,
            8,
            5,
            148,
            115,
            128,
            9,
            8,
            5,
            1,
            new byte[] {0, 3, 17},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            128
        );

        /// A product-form parameter set that gives 192 bits of security.
        public static readonly EncParams EES587EP1 = new EncParams (
            new byte[] {69, 69, 83, 53, 56, 55, 69, 80, 49, 0, 0},
            587,
            2048,
            1,
            10,
            10,
            8,
            196,
            157,
            192,
            11,
            13,
            7,
            1,
            new byte[] {0, 5, 17},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            192
        );

        /// A product-form parameter set that gives 256 bits of security.
        public static readonly EncParams EES743EP1 = new EncParams (
            new byte[] {69, 69, 83, 55, 52, 51, 69, 80, 49, 0, 0},
            743,
            2048,
            1,
            11,
            11,
            15,
            247,
            204,
            256,
            13,
            12,
            7,
            1,
            new byte[] {0, 6, 16},
            ffi.ntru_sha256,
            ffi.ntru_sha256_4way,
            32,
            256
        );

        public static EncParams DEFAULT_PARAMS_112_BITS = EES401EP2;

        public static EncParams DEFAULT_PARAMS_128_BITS = EES443EP1;

        public static EncParams DEFAULT_PARAMS_192_BITS = EES587EP1;

        public static EncParams DEFAULT_PARAMS_256_BITS = EES743EP1;

    }
}