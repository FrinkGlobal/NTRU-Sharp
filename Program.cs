using System;
using NTRU.types;
using NTRU.Params;
using NTRU.rand;

namespace NTRU
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RandContext rand_ctx = rand.rand.init(rand.rand.RNG_DEFAULT);
            Console.WriteLine("Successfully Created RandContext!");
        }
    }
}
