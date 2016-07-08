using System;
using System.Text;
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
            KeyPair kp = NTRUWrapper.generate_key_pair(EncParamSets.DEFAULT_PARAMS_256_BITS, rand_ctx);
            Console.WriteLine("Successfully Generated Keys!!");
            byte[] msg = Encoding.UTF8.GetBytes("Hello from NTRU!");
           
            byte[] encrypted = NTRUWrapper.encrypt(msg, kp.Public, EncParamSets.DEFAULT_PARAMS_256_BITS, rand_ctx);
            byte[] decrypted = NTRUWrapper.decrypt(encrypted, kp, EncParamSets.DEFAULT_PARAMS_256_BITS);
            Console.WriteLine(Encoding.UTF8.GetString(decrypted));
            if (Encoding.UTF8.GetString(decrypted) == "Hello from NTRU!")
            {
                Console.WriteLine("Ecryption / Decryption Test Succeded!");
            }
            else
            {
                Console.WriteLine("Ecryption / Decryption Test Failed!");
            }

        }
    }
}
