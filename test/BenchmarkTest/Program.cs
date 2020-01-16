using BenchmarkDotNet.Running;
using System;
using System.Runtime.CompilerServices;

namespace BenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //FindTreeTest a = new FindTreeTest();
            //var b = a.UseUnsafe2("abcdefj");
            //Console.WriteLine(b);
            //b = a.UseUnsafe2("abcdefj");
            //Console.WriteLine(b);
            //b = a.UseUnsafe2("abcdefj");
            //Console.WriteLine(b);
            BenchmarkRunner.Run<FindTreeTest>();

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
