using System;
using System.Runtime.InteropServices;
using NTRU.rand;

namespace NTRU
{
    public static class ffi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct CNtruRandContext {
            public IntPtr rand_gen;

            public IntPtr seed;
            [MarshalAs(UnmanagedType.U2)]
            public ushort seed_len;

            public IntPtr state;

            public CNtruRandContext (IntPtr rand_gen, IntPtr seed, ushort seed_len, IntPtr state) {
                this.rand_gen = rand_gen;
                this.seed = seed;
                this.seed_len = seed_len;
                this.state = state;
            }

            public CNtruRandContext (RandGen rand_gen, IntPtr seed, ushort seed_len, IntPtr state) {
                IntPtr rand_gen_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_gen));
                Marshal.StructureToPtr(rand_gen, rand_gen_ptr, false);
                this.rand_gen = rand_gen_ptr;
                this.seed = seed;
                this.seed_len = seed_len;
                this.state = state;
                ///Marshal.FreeHGlobal(rand_gen_ptr);  Does This break the struct? or does not doing this cause a memory leak
            }
        }

        [DllImport("ntru")]
        public static extern byte ntru_gen_key_pair (IntPtr param, IntPtr kp, IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_gen_key_pair_multi (IntPtr param, IntPtr priv, IntPtr pub, IntPtr rand_ctx, uint num_pub);
        [DllImport("ntru")]
        public static extern byte ntru_gen_pub (IntPtr param, IntPtr priv, IntPtr pub, IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_encrypt (IntPtr msg, ushort msg_len, IntPtr pub, IntPtr param, IntPtr rand_ctx, IntPtr enc);
        [DllImport("ntru")]
        public static extern byte ntru_decrypt (IntPtr enc, IntPtr kp, IntPtr param, IntPtr dec, out IntPtr dec_len);
        [DllImport("ntru")]
        public static extern void ntru_sha1 (IntPtr input, ushort input_len, IntPtr digest);
        [DllImport("ntru")]
        public static extern void ntru_sha1_4way (IntPtr input, ushort input_len, IntPtr digest);
        [DllImport("ntru")]
        public static extern void ntru_sha256 (IntPtr input, ushort input_len, IntPtr digest);
        [DllImport("ntru")]
        public static extern void ntru_sha256_4way (IntPtr input, ushort input_len, IntPtr digest);
        [DllImport("ntru")]
        public static extern byte ntru_rand_init (out IntPtr rand_ctx, IntPtr rand_gen);
        [DllImport("ntru")]
        public static extern byte ntru_rand_init_det (IntPtr rand_ctx, IntPtr rand_gen, IntPtr seed, ushort seed_len);
        [DllImport("ntru")]
        public static extern byte ntru_rand_generate (IntPtr rand_data, ushort len, IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_rand_release (IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_rand_default_init (IntPtr rand_ctx, IntPtr rand_gen);
        [DllImport("ntru")]
        public static extern byte ntru_rand_default_generate (IntPtr rand_data, ushort len, IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_rand_default_release (IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_rand_igf2_init (IntPtr rand_ctx, IntPtr rand_gen);
        [DllImport("ntru")]
        public static extern byte ntru_rand_igf2_generate (IntPtr rand_data, ushort len, IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_rand_igf2_release (IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_rand_tern (ushort n, ushort num_ones, ushort num_neg_ones, IntPtr poly, IntPtr rand_ctx);
        [DllImport("ntru")]
        public static extern byte ntru_mult_tern (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
        [DllImport("ntru")]
        public static extern byte ntru_mult_tern_32 (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
        [DllImport("ntru")]
        public static extern byte ntru_mult_tern_64 (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
        [DllImport("ntru")]
        public static extern byte ntru_mult_tern_sse (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
        [DllImport("ntru")]
        public static extern byte ntru_mult_prod (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
        [DllImport("ntru")]
        public static extern byte ntru_mult_priv (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
        [DllImport("ntru")]
        public static extern byte ntru_mult_int (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
        [DllImport("ntru")]
        public static extern byte ntru_multi_int_16 (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
        [DllImport("ntru")]
        public static extern byte ntru_multi_int_64 (IntPtr a, IntPtr b, IntPtr c, ushort mod_mask);
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
        public static extern void ntru_to_arr_32 (IntPtr p, ushort q, IntPtr a);
        [DllImport("ntru")]
        public static extern void ntru_to_arr_64 (IntPtr p, ushort q, IntPtr a);
        [DllImport("ntru")]
        public static extern void ntru_to_arr_sse_2048 (IntPtr p, IntPtr a);
        [DllImport("ntru")]
        public static extern void ntru_from_arr (IntPtr arr, ushort n, ushort q, IntPtr p);
        [DllImport("ntru")]
        public static extern byte ntru_invert (IntPtr a, ushort mod_mask, IntPtr fq);
        [DllImport("ntru")]
        public static extern byte ntru_invert_32 (IntPtr a, ushort mod_mask, IntPtr fq);
        [DllImport("ntru")]
        public static extern byte ntru_invert_64 (IntPtr a, ushort mod_mask, IntPtr fq); 
        [DllImport("ntru")]
        public static extern void ntru_export_pub (IntPtr key, IntPtr arr);
        [DllImport("ntru")]
        public static extern ushort ntru_import_pub (IntPtr arr, IntPtr key);
        [DllImport("ntru")]
        public static extern ushort ntru_export_priv (IntPtr key, IntPtr arr);
        [DllImport("ntru")]
        public static extern void ntru_import_priv (IntPtr arr, IntPtr key);
        [DllImport("ntru")]
        public static extern byte ntru_params_from_priv_key (IntPtr key, IntPtr param);

    }
}