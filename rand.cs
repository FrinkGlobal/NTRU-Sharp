using System;
using System.Runtime.InteropServices;

namespace NTRU.rand
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RandomContext {
        public ffi.CNtruRandomContext rand_ctx;


        public RandomContext (ffi.CNtruRandomContext rand_ctx) {
            this.rand_ctx = rand_ctx;
        }

        public static RandomContext Default() {
            return new RandomContext(new ffi.CNtruRandomContext(
                rand.RNG_DEFAULT,
                IntPtr.Zero,
                0,
                IntPtr.Zero
            ));  
        }

        public ffi.CNtruRandomContext get_c_rand_ctx() {
            return rand_ctx;
        }

        public byte[] get_seed() {
            byte[] slice = new byte[rand_ctx.seed_len];
            Marshal.Copy(rand_ctx.seed, slice, 0, rand_ctx.seed_len);
            return slice;
        }

        public void set_seed(byte[] seed) {
            rand_ctx.seed_len = (ushort)seed.Length;
            IntPtr seed_ptr = Marshal.AllocHGlobal(seed.Length);
            Marshal.Copy(seed, 0, seed_ptr, seed.Length);
            rand_ctx.seed = seed_ptr;
            Marshal.FreeHGlobal(seed_ptr);
        }

        public RandGen get_rng() {
            RandGen get_rng = rand.RNG_DEFAULT;
            Marshal.PtrToStructure(rand_ctx.rand_gen, get_rng);
            return get_rng;
        }

        public void drop() {
            IntPtr rand_ctx_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_ctx));
            Marshal.StructureToPtr(rand_ctx, rand_ctx_ptr, false);
            var result = ffi.ntru_rand_release(rand_ctx_ptr);
            if(result != 0)
                Console.WriteLine("Error: Failed to drop random context pointer");
            Marshal.FreeHGlobal(rand_ctx_ptr);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RandGen {
        Func<IntPtr, IntPtr, byte> init_fn;

        Func<IntPtr, ushort, IntPtr, byte> generate_fn;

        Func<IntPtr, byte> release_fn;

        public RandGen (Func<IntPtr, IntPtr, byte> init_fn, Func<IntPtr, ushort, IntPtr, byte> generate_fn, Func<IntPtr, byte> release_fn) {
            this.init_fn = init_fn;
            this.generate_fn = generate_fn;
            this.release_fn = release_fn;
        }

        public RandomContext init(RandGen rand_gen) {
            RandomContext rand_ctx = RandomContext.Default();
            IntPtr rand_ctx_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_ctx));
            IntPtr rand_gen_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_gen));
            Marshal.StructureToPtr(rand_ctx, rand_ctx_ptr, false);
            Marshal.StructureToPtr(rand_gen, rand_gen_ptr, false);
            var result = this.init_fn(rand_ctx_ptr, rand_gen_ptr);
            if (result == 0)
                Console.WriteLine("Error: Failed to Initialize RandContext");
            
            Marshal.PtrToStructure(rand_ctx_ptr, rand_ctx);
            Marshal.FreeHGlobal(rand_ctx_ptr);
            Marshal.FreeHGlobal(rand_gen_ptr);
            return rand_ctx;
        }

        public byte[] generate(ushort length, RandomContext rand_ctx) {
            byte[] plain = new byte[length];
            IntPtr rand_ctx_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_ctx.rand_ctx));
            Marshal.StructureToPtr(rand_ctx.rand_ctx, rand_ctx_ptr, false);
            IntPtr plain_ptr = Marshal.AllocHGlobal(length);
            var result = this.generate_fn(plain_ptr, length, rand_ctx_ptr);
             if (result == 0)
                Console.WriteLine("Error: Failed to Generate plain");
            
            Marshal.Copy(plain_ptr, plain, 0, length);
            Marshal.FreeHGlobal(rand_ctx_ptr);
            Marshal.FreeHGlobal(plain_ptr);
            return plain;
        }
    }

    public static class rand {

        public static RandGen RNG_DEFAULT = new RandGen (
            ffi.ntru_rand_default_init,
            ffi.ntru_rand_default_generate,
            ffi.ntru_rand_default_release
        );

        public static RandGen RNG_IGF2 = new RandGen (
            ffi.ntru_rand_igf2_init,
            ffi.ntru_rand_igf2_generate,
            ffi.ntru_rand_igf2_release
        );


        public static RandomContext init (RandGen rand_gen) {
            RandomContext rand_ctx = RandomContext.Default();
            IntPtr rand_ctx_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_ctx.rand_ctx));
            Marshal.StructureToPtr(rand_ctx, rand_ctx_ptr, false);
            IntPtr rand_gen_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rand_gen));
            Marshal.StructureToPtr(rand_gen, rand_ctx_ptr, false);
            var result = ffi.ntru_rand_init (rand_ctx_ptr, rand_gen_ptr);
             if (result == 0)
                Console.WriteLine("Error: Failed to Initialize RandomContext");
            Marshal.PtrToStructure(rand_ctx_ptr, rand_ctx);
            Marshal.FreeHGlobal(rand_gen_ptr);
            Marshal.FreeHGlobal(rand_ctx_ptr);
            return rand_ctx;
        }

    }
}