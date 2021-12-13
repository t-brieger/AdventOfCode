using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;

namespace AdventOfCode.Solutions
{
    public class Year2016Day05 : Solution
    {
        private static void Part1Kernel(Index1D ix, ArrayView<byte> data, ulong constant)
        {
            byte[] message = new byte[64];
            message[0] = (byte) ((constant & 0xFF00000000000000L) >> 56);
            message[1] = (byte) ((constant & 0x00FF000000000000L) >> 48);
            message[2] = (byte) ((constant & 0x0000FF0000000000L) >> 40);
            message[3] = (byte) ((constant & 0x000000FF00000000L) >> 32);
            message[4] = (byte) ((constant & 0x00000000FF000000L) >> 24);
            message[5] = (byte) ((constant & 0x0000000000FF0000L) >> 16);
            message[6] = (byte) ((constant & 0x000000000000FF00L) >> 8);
            message[7] = (byte) ((constant & 0x00000000000000FFL) >> 0);
            byte j = 8;
            Index1D oldIx = ix;

            if (ix == 0)
            {
                j = 9;
                message[8] = (byte) '0';
            }

            while (ix > 0)
            {
                message[j++] = (byte) ((ix % 10) + '0');
                ix /= 10;
            }

            ix = oldIx;

            byte numLength = (byte) (j - 8);

            j--;
            // reverse the number
            for (int l = 0; l < numLength / 2 + (numLength % 2 == 0 ? 0 : 1); l++)
                (message[8 + l], message[j - l]) = (message[j - l], message[8 + l]);

            // message is now 8 bytes of seed, then numLength bytes of the string-ified int "ix".
            // fill it up to 64 bytes (or rather, 56, and then 8 for original message length)
            for (int l = 8 + numLength; l < 63; l++)
                message[l] = 0;
            message[8 + numLength] = 0x80;
            message[56] = (byte) (64 + numLength * 8);

            
            
            uint md5 = IlGpuKernels.KernelFirstMd5Int(message);
            if ((0xF0FFFF & md5) == 0)
            {
                data[ix] = (byte) ((0xF0000 & md5) >> 16);
            }
            else
                data[ix] = 0xFF;
        }

        public override string Part1(string input)
        {
            int numberOfHashes = 7_000_000;

            using Context context = Context.CreateDefault();
            Accelerator a = context.CreateCPUAccelerator(0);
            try
            {
                a = context.CreateCudaAccelerator(0);
            }
            catch (Exception)
            {
                try
                {
                    a = context.CreateCLAccelerator(0);
                }
                catch (Exception)
                {
                    // if this doesn't work it just stays a CPUAccelerator
                }
            }

            Action<Index1D, ArrayView<byte>, ulong> kern =
                a.LoadAutoGroupedStreamKernel<Index1D, ArrayView<byte>, ulong>(Part1Kernel);

            using MemoryBuffer1D<byte, Stride1D.Dense> buff = a.Allocate1D<byte>(numberOfHashes);

            ulong ul =
                ((ulong) input[0] << 56) |
                ((ulong) input[1] << 48) |
                ((ulong) input[2] << 40) |
                ((ulong) input[3] << 32) |
                ((ulong) input[4] << 24) |
                ((ulong) input[5] << 16) |
                ((ulong) input[6] <<  8) |
                ((ulong) input[7] <<  0);
            
            kern((Index1D) buff.Length, buff.View, ul);

            a.Synchronize();

            byte[] data = buff.GetAsArray1D();

            string output = "";
            
            for (int i = 0; output.Length < 8; i++)
            {
                if (data[i] > 0x0F)
                    continue;
                output += data[i].ToString("x")[0];
            }

            a.Dispose();

            return new string(output);
        }

        private static void Part2Kernel(Index1D ix, ArrayView<byte> data, ulong constant)
        {
            byte[] message = new byte[64];
            message[0] = (byte) ((constant & 0xFF00000000000000L) >> 56);
            message[1] = (byte) ((constant & 0x00FF000000000000L) >> 48);
            message[2] = (byte) ((constant & 0x0000FF0000000000L) >> 40);
            message[3] = (byte) ((constant & 0x000000FF00000000L) >> 32);
            message[4] = (byte) ((constant & 0x00000000FF000000L) >> 24);
            message[5] = (byte) ((constant & 0x0000000000FF0000L) >> 16);
            message[6] = (byte) ((constant & 0x000000000000FF00L) >> 8);
            message[7] = (byte) ((constant & 0x00000000000000FFL) >> 0);
            byte j = 8;
            Index1D oldIx = ix;

            if (ix == 0)
            {
                j = 9;
                message[8] = (byte) '0';
            }

            while (ix > 0)
            {
                message[j++] = (byte) ((ix % 10) + '0');
                ix /= 10;
            }

            ix = oldIx;

            byte numLength = (byte) (j - 8);

            j--;
            // reverse the number
            for (int l = 0; l < numLength / 2 + (numLength % 2 == 0 ? 0 : 1); l++)
                (message[8 + l], message[j - l]) = (message[j - l], message[8 + l]);

            // message is now 8 bytes of seed, then numLength bytes of the string-ified int "ix".
            // fill it up to 64 bytes (or rather, 56, and then 8 for original message length)
            for (int l = 8 + numLength; l < 63; l++)
                message[l] = 0;
            message[8 + numLength] = 0x80;
            message[56] = (byte) (64 + numLength * 8);

            uint md5 = IlGpuKernels.KernelFirstMd5Int(message);
            if ((0xF0FFFF & md5) == 0)
            {
                data[ix] = (byte) (((0xF0000 & md5) >> 12) | ((0xF0000000 & md5) >> 28));
            }
            else
                data[ix] = 0xff;
        }
        
        public override string Part2(string input)
        {
            // never needs above 26m iterations in all the inputs ive checked - lets be very safe and say 30.
            int numberOfHashes = 30_000_000;

            using Context context = Context.CreateDefault();
            Accelerator a = context.CreateCPUAccelerator(0);
            try
            {
                a = context.CreateCudaAccelerator(0);
            }
            catch (Exception)
            {
                try
                {
                    a = context.CreateCLAccelerator(0);
                }
                catch (Exception)
                {
                    // if this doesn't work it just stays a CPUAccelerator
                }
            }

            Action<Index1D, ArrayView<byte>, ulong> kern =
                a.LoadAutoGroupedStreamKernel<Index1D, ArrayView<byte>, ulong>(Part2Kernel);

            using MemoryBuffer1D<byte, Stride1D.Dense> buff = a.Allocate1D<byte>(numberOfHashes);

            ulong ul =
                ((ulong) input[0] << 56) |
                ((ulong) input[1] << 48) |
                ((ulong) input[2] << 40) |
                ((ulong) input[3] << 32) |
                ((ulong) input[4] << 24) |
                ((ulong) input[5] << 16) |
                ((ulong) input[6] <<  8) |
                ((ulong) input[7] <<  0);
            
            kern((Index1D) buff.Length, buff.View, ul);

            a.Synchronize();

            byte[] data = buff.GetAsArray1D();

            char[] output = {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'};

            for (int i = 0; i < data.Length; i++)
            {
                if ((data[i] & 0xF0) > 0x70)
                    continue;
                byte ix = (byte) ((data[i] & 0xF0) >> 4);
                byte val = (byte) (data[i] & 0x0F);
                if (output[ix] == '\0')
                    output[ix] = val.ToString("x")[0];
            }

            a.Dispose();

            return new string(output);

            /*using MD5 md5Hash = MD5.Create();

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

            return new string(output); */
        }
    }
}