using System;
using System.Runtime.InteropServices;
using NTRU.types;
using NTRU.Params;

namespace NTRU
{
    public static class ffi
    {
        [DllImport("ntru")]
        public static extern byte ntru_gen_key_pair (IntPtr param, IntPtr kp, IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_gen_key_pair_multi (IntPtr param, IntPtr priv, IntPtr pub, IntPtr rand_ctx, uint num_pub);
        [DllImport("ntru")]
        public static extern byte ntru_gen_pub (IntPtr param, IntPtr priv, IntPtr pub, IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_encrypt (IntPtr msg, ushort msg_len, IntPtr pub, IntPtr param, IntPtr rand_ctx, IntPtr enc);
        [DllImport("ntru")]
        public static extern byte ntru_decrypt (IntPtr enc, IntPtr kp, IntPtr param, IntPtr dec, IntPtr dec_len);
        [DllImport("ntru")]
        public static extern void ntru_sha1 (IntPtr input, ushort input_len, IntPtr digest);
        [DllImport("ntru")]
        public static extern void ntru_sha1_4way (IntPtr input, ushort input_len, IntPtr digest);
        [DllImport("ntru")]
        public static extern void ntru_add (IntPtr a, IntPtr b);
        [DllImport("ntru")]
        public static extern void ntru_sub (IntPtr a, IntPtr b);
        [DllImport("ntru")]
        public static extern void ntru_mod_mask (IntPtr p, ushort mod_mask);
        [DllImport("ntru")]
        public static extern void ntru_mult_fac (IntPtr a, short factor);
        [DllImport("ntru")]
        public static extern void ntru_mod_center (IntPtr p, ushort modulus);
        [DllImport("ntru")]
        public static extern void ntru_mod3 (IntPtr p);
        [DllImport("ntru")]
        public static extern void ntru_to_arr_32 (IntPtr p, ushort q, byte a);
        [DllImport("ntru")]
        public static extern void ntru_to_arr_64 (IntPtr p, ushort q, byte a);

    }
}