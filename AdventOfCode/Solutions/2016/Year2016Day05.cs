using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions
{
    public class Year2016Day05 : Solution
    {
        private static string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new();

            foreach (byte b in data) sBuilder.Append(b.ToString("x2"));

            return sBuilder.ToString();
        }

        public override string Part1(string input)
        {
            using MD5 md5Hash = MD5.Create();

            string output = "";


            for (int i = 0; output.Length < 8; i++)
            {
                string h = GetMd5Hash(md5Hash, input + i);
                if (h[..5] == "00000")
                    output += h[5];
            }

            return output;
        }

        public override string Part2(string input)
        {
            //TODO run this on the gpu (and while you're at it, 2015/04 as well)
            // and maybe this time, don't break your graphics driver for 3 hours, only for opencl to still decide that
            // in fact, it fucking hates you. gpus are a pain.

            using MD5 md5Hash = MD5.Create();

            char[] output = { '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' };

            // just here so we don't do "output.ALl(c => c != '\0')" every loop - it gets expensive quick.
            int done = 0;
            
            for (int i = 0; done < 8; i++)
            {
                string h = GetMd5Hash(md5Hash, input + i);
                if (h[..5] == "00000" && h[5] is >= '0' and <= '7' &&output[h[5] - '0'] == '\0')
                {
                    done++;
                    output[h[5] - '0'] = h[6];
                }
            }

            return new string(output); 
        }
    }
}