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

        Action<IntPtr, ushort, IntPtr> hash;

        Action<IntPtr, ushort, IntPtr> hash_4way;
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

        public EncParams (byte[] name, ushort n, ushort q, byte product_flag, ushort df1, ushort df2, ushort df3, ushort dg, ushort dm0, ushort db, ushort c, ushort min_calls_r, ushort min_calls_mask, byte hash_seed, byte[] oid, Action<IntPtr, ushort, IntPtr> hash, Action<IntPtr, ushort, IntPtr> hash_4way, ushort hlen, ushort pklen) {
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
    }

    public static class EncParamSets
    {
        
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

    }
}