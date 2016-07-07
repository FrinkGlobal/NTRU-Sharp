using System;
using System.Runtime.InteropServices;
using NTRU.Params;
using NTRU.rand;
using NTRU.types;

namespace NTRU
{
    public static class NTRUWrapper {

        public static KeyPair generate_key_pair(EncParams param, RandContext rand_context) {
            KeyPair kp = KeyPair.Default();
            IntPtr key_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(kp));
            Marshal.StructureToPtr(kp, key_ptr, false);
            IntPtr param_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, param_ptr, false);
            IntPtr rand_ctx_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_context));
            Marshal.StructureToPtr(rand_context, rand_ctx_ptr, false);
            var result = ffi.ntru_gen_key_pair(param_ptr, key_ptr, rand_ctx_ptr);
            if (result != 0)
                    Console.WriteLine("Error: Failed to Generate KeyPair");
            Marshal.PtrToStructure(key_ptr, kp);
            Marshal.FreeHGlobal(key_ptr);
            Marshal.FreeHGlobal(param_ptr);
            Marshal.FreeHGlobal(rand_ctx_ptr);
            return kp;
        }

        public static byte[] encrypt(byte[] msg, PublicKey pub, EncParams param, RandContext rand_ctx) {
            byte[] enc = new byte[param.enc_len()];
            IntPtr enc_ptr = Marshal.AllocHGlobal(param.enc_len());
            IntPtr msg_ptr = Marshal.AllocHGlobal(msg.Length);
            Marshal.Copy(msg, 0, msg_ptr, msg.Length);
            IntPtr pub_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(pub));
            Marshal.StructureToPtr(pub, pub_ptr, false);
            IntPtr param_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, param_ptr, false);
            IntPtr rand_ctx_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_ctx.get_c_rand_ctx()));
            Marshal.StructureToPtr(rand_ctx.get_c_rand_ctx(), rand_ctx_ptr, false);
            var result = ffi.ntru_encrypt(msg_ptr, (ushort)msg.Length, pub_ptr, param_ptr, rand_ctx_ptr, enc_ptr);
            if (result != 0)
                    Console.WriteLine("Error: Failed to Encrypt Message");
            Marshal.Copy(enc_ptr, enc, 0, enc.Length);
            Marshal.FreeHGlobal(msg_ptr);
            Marshal.FreeHGlobal(pub_ptr);
            Marshal.FreeHGlobal(param_ptr);
            Marshal.FreeHGlobal(rand_ctx_ptr);
            Marshal.FreeHGlobal(enc_ptr);
            return enc;
        }

        public static byte[] decrypt(byte[] enc, KeyPair kp, EncParams param) {
            byte[] dec = new byte[param.max_msg_len()];
            ushort dec_len = 0;
            IntPtr dec_len_ptr = new IntPtr(dec_len);
            IntPtr enc_ptr = Marshal.AllocHGlobal(enc.Length);
            Marshal.Copy(enc, 0, enc_ptr, enc.Length);
            IntPtr dec_ptr = Marshal.AllocHGlobal(dec.Length);
            IntPtr key_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(kp));
            Marshal.StructureToPtr(kp, key_ptr, false);
            IntPtr param_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, param_ptr, false);
            var result = ffi.ntru_decrypt(enc_ptr, key_ptr, param_ptr, dec_ptr, out dec_len_ptr);
            if (result != 0)
                    Console.WriteLine("Error: Failed to Decrypt Message");
            byte[] final_dec = new byte[dec_len_ptr.ToInt32()];
            Marshal.Copy(dec_ptr, final_dec, 0, final_dec.Length);
            Marshal.FreeHGlobal(enc_ptr);
            Marshal.FreeHGlobal(dec_ptr);
            Marshal.FreeHGlobal(key_ptr);
            Marshal.FreeHGlobal(param_ptr);
            return final_dec;
        }

    }
}