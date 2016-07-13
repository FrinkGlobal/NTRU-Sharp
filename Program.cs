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
           
            byte[] encrypted = NTRUWrapper.encrypt(msg, kp.get_public(), EncParamSets.DEFAULT_PARAMS_256_BITS, rand_ctx);
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

            byte[] exportedPriv = kp.get_private().export(EncParamSets.DEFAULT_PARAMS_256_BITS);
            Console.WriteLine ("ENC PARAMS Private Key Length: " + EncParamSets.DEFAULT_PARAMS_256_BITS.private_len() + " Byte Array: " + exportedPriv.Length);
           
            for (int i = 0; i < exportedPriv.Length; i++) {
                Console.Write(exportedPriv[i]);
            }
            Console.Write("\n");           
            byte[] exportedPub = kp.get_public().export(EncParamSets.DEFAULT_PARAMS_256_BITS);
            Console.WriteLine ("ENC PARAMS PublicKey Key Length: " + EncParamSets.DEFAULT_PARAMS_256_BITS.public_len() + " Byte Array: " + exportedPub.Length);
            
            for (int i = 0; i < exportedPub.Length; i++) {
                Console.Write(exportedPub[i]);
            }         
            Console.Write("\n");      
            PublicKey pub = PublicKey.import(exportedPub);
            PrivateKey priv = PrivateKey.import(exportedPriv);

            KeyPair newKP = new KeyPair(priv, pub);

            if(newKP == kp) {
                Console.WriteLine("Importing  / Exporting Key Test Succeded!");
            }
            else
            {
                Console.WriteLine("Importing  / Exporting Key Test Failed!");
            }
            
            byte[] msg2 = Encoding.UTF8.GetBytes("Hello from NTRU! 2");

            byte[] encrypted2 = NTRUWrapper.encrypt(msg2, newKP.get_public(), EncParamSets.DEFAULT_PARAMS_256_BITS, rand_ctx);
            byte[] decrypted2 = NTRUWrapper.decrypt(encrypted2, newKP, EncParamSets.DEFAULT_PARAMS_256_BITS);
            Console.WriteLine(Encoding.UTF8.GetString(decrypted2));
            if (Encoding.UTF8.GetString(decrypted2) == "Hello from NTRU! 2")
            {
                Console.WriteLine("Ecryption / Decryption Test 2 Succeded!");
            }
            else
            {
                Console.WriteLine("Ecryption / Decryption Test 2 Failed!");
            }
        }
    }
}
