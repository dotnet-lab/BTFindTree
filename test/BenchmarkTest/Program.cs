using BenchmarkDotNet.Running;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace BenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ushort w1 = '我';
            ushort w2 = '爱';
            ushort w3 = 'n';
            ushort w4 = '啊';
            string str = "我爱n啊";
            //var bytes1 = Encoding.UTF8.GetBytes(str);
            ref char charRef = ref MemoryMarshal.GetReference(str.AsSpan());
            ref byte byteRef = ref Unsafe.As<char, byte>(ref charRef);
            ref ushort shortRef = ref Unsafe.As<char, ushort>(ref charRef);
            //Console.WriteLine(w1 == byteRef);
            //Console.WriteLine(w1 == Unsafe.ReadUnaligned<uint>(ref byteRef));
            var length = Unsafe.SizeOf<char>();
            Console.WriteLine(length) ;

            for (int i = 0; i < 10; i++)
            {
                byteRef = ref Unsafe.Add(ref byteRef, i);
                if (w1 == Unsafe.ReadUnaligned<ushort>(ref byteRef))
                {
                    Console.WriteLine("w1");
                    Console.WriteLine(i); //0 和 //48
                    Console.WriteLine(w1 == Unsafe.Add(ref shortRef, 0));
                    break;
                }

            }
            for (int i = length; i < 10; i++)
            {
                byteRef = ref Unsafe.Add(ref byteRef, i);
                if (w2 == Unsafe.ReadUnaligned<ushort>(ref byteRef))
                {
                    Console.WriteLine("w2");
                    Console.WriteLine(i); //40
                    Console.WriteLine(w2 == Unsafe.Add(ref shortRef, 1));
                    break;
                }

            }
            for (int i = length; i < 10; i++)
            {
                byteRef = ref Unsafe.Add(ref byteRef, i);
                if (w3 == Unsafe.ReadUnaligned<ushort>(ref byteRef))
                {
                    Console.WriteLine("w3");
                    Console.WriteLine(i); //32
                    Console.WriteLine(w3 == Unsafe.Add(ref shortRef, 2));
                    break;
                }

            }
            for (int i = length; i < 10; i++)
            {
                byteRef = ref Unsafe.Add(ref byteRef, i);
                if (w4 == Unsafe.ReadUnaligned<ushort>(ref byteRef))
                {
                    Console.WriteLine("w4");
                    Console.WriteLine(i); //24
                    Console.WriteLine(w4 == Unsafe.Add(ref shortRef, 3));
                    break;
                }

            }

            //for (int i = 0; i < 8; i++)
            //{

            //}

            //Console.WriteLine(w2 == Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref byteRef, 4)));
            //Console.WriteLine(byteRef);
            //Console.WriteLine(w3 == Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref byteRef, 0)));
            //Console.WriteLine(w4 == Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref byteRef, 0)));
            //FindTreeTest a = new FindTreeTest();
            //a.BytePointer();
            //a.ByteSpan();
            //var b = a.UseUnsafe2("abcdefj");
            //Console.WriteLine(b);
            //b = a.UseUnsafe2("abcdefj");
            //Console.WriteLine(b);
            //b = a.UseUnsafe2("abcdefj");
            //Console.WriteLine(b);
            //BenchmarkRunner.Run<FindTreeTest>();

            //Console.WriteLine(Unsafe.SizeOf<char>());
            //Test();
            Console.ReadKey();
        }
        public unsafe static void Test()
        {
            byte[] array = new byte[512];
            fixed (byte* pointer = array)
            {
                for (var i = 0; i < array.Length; i++)
                {
                    *(pointer + i) = (byte)i;
                    Console.Write(array[i]);
                }
            }
        }
    }
}
