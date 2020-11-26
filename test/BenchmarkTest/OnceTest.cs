using BenchmarkDotNet.Attributes;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace BenchmarkTest
{
    [MemoryDiagnoser, CoreJob, MarkdownExporter, RPlotExporter]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class OnceTest
    {
        private string _target;
        private readonly byte[] _bytes;
        public OnceTest()
        {
            _target = "shjshdshdjshj";
            _bytes = Encoding.UTF8.GetBytes(_target);

        }
        //[Benchmark]
        //public unsafe void Unsafe()
        //{

        //    fixed (byte* ptr = &_bytes[0])
        //    {
        //        var result = *(ulong*)ptr;
        //    }

        //}
        //[Benchmark]
        //public void Safe()
        //{

        //    BinaryPrimitives.ReadUInt64LittleEndian(_bytes);

        //}

        //[Benchmark]
        //public unsafe void Unsafe2()
        //{

        //    fixed (char* ptr = _target)
        //    {
        //        var result = *(ulong*)ptr;
        //        result = *(ulong*)(ptr+8);
        //    }

        //}

        [Benchmark]
        public unsafe void Unsafe2()
        {

            fixed (char* ptr = _target)
            {
                var result = *(ushort*)ptr;
                if (result == 115)
                {
                    result = *(ushort*)(ptr + 1);
                    if (result != 104)
                    {
                        throw new Exception("值不相等！");
                    }
                }
            }

        }

        [Benchmark]
        public void Safe2()
        {

            ref char charRef = ref MemoryMarshal.GetReference(_target.AsSpan());
            ref byte byteRef = ref Unsafe.As<char, byte>(ref charRef);
            var result = Unsafe.ReadUnaligned<ushort>(ref byteRef);
            if (result == 115)
            {
                byteRef = ref Unsafe.Add(ref byteRef, 2);
                result = Unsafe.ReadUnaligned<ushort>(ref byteRef);
                if (result != 104)
                {
                    throw new Exception("值不相等！");
                }

            }
            
            


        }
        [Benchmark]
        public void Safe3()
        {
            ref char charRef = ref MemoryMarshal.GetReference(_target.AsSpan());
            ref ushort shortRef = ref Unsafe.As<char, ushort>(ref charRef);
            if (shortRef == 115)
            {
                shortRef = ref Unsafe.Add(ref shortRef, 1);
                if (shortRef != 104)
                {
                    throw new Exception("值不相等！");
                }
            }
           
        }
    }
}
