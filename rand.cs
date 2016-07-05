using System;
using System.Runtime.InteropServices;

namespace NTRU.rand
{

    public struct RandomContext {
        ffi.CNtruRandomContext rand_ctx;


        public RandomContext (ffi.CNtruRandomContext rand_ctx) {
            this.rand_ctx = rand_ctx;
        }

        public static RandomContext Default() {
            //return new RandomContext();
            throw new NotImplementedException();    
        }

        public byte[] get_seed() {
            throw new NotImplementedException();
        }

        public void set_seed(byte[] seed) {
            rand_ctx.seed_len = (ushort)seed.Length;
            IntPtr seed_ptr = Marshal.AllocHGlobal(seed.Length);
            Marshal.Copy(seed, 0, seed_ptr, seed.Length);
            rand_ctx.seed = seed_ptr;
        }

        public RandGen get_rng() {
            //Marshal.PtrToStructure()
            throw new NotImplementedException();

        }

    }

    public struct RandGen {
        Action<IntPtr, IntPtr> init_fn;

        Action<IntPtr, ushort, IntPtr> generate_fn;

        Action<IntPtr> release_fn;
     
    }
}