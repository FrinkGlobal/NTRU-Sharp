using System;

namespace NTRU.rand
{
    public static class rand
    {
       
    }

    public struct RandomContext {
        ffi.CNtruRandomContext rand_ctx;


        // public static RandomContext Default() {
            
        // }        
    }

    public struct RandGen
    {
        Action<IntPtr, IntPtr> init_fn;

        Action<IntPtr, ushort, IntPtr> generate_fn;

        Action<IntPtr> release_fn;

 
    }
}