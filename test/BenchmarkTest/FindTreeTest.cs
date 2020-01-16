using BenchmarkDotNet.Attributes;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace BenchmarkTest
{


    public class FindTreeTest
    {

        public readonly Dictionary<string, int> Dict;
        public readonly ConcurrentDictionary<string, int> ConDict;

        public FindTreeTest()
        {
            Dict = new Dictionary<string, int>();
            Dict["abab1123"] = 0;
            Dict["abab1123c"] = 1;
            Dict["abab1123d"] = 2;

            Dict["abab2213e"] = 3;
            Dict["abab2213er"] = 2;

            Dict["abab3213f"] = 4;
            Dict["abcdeff"] = 5;
            Dict["abcdefg"] = 6;
            Dict["abcdefi"] = 7;
            Dict["abcdefh"] = 8;
            Dict["abcdefj"] = 9;
            ConDict = new ConcurrentDictionary<string, int>(Dict);
            UseSpanPoint("abcdefj");
            UsePoint("abcdefj");
            UseMemoryMarshalPoint("abcdefj");
            UseUnsafe("abcdefj");
        }
        //[Benchmark]
        //public void HashFindTree()
        //{
        //    var result = UseHash("abcdefj");
        //}

        //[Benchmark]
        //public void IntPtrSpanFindTree()
        //{
        //    var result = UseSpanPoint("abcdefj");
        //}

        //[Benchmark]
        //public void IntPtrFindTree()
        //{
        //    var result = UsePoint("abcdefj");
        //}
        //[Benchmark]
        //public void IntPtrMemoryMarshalFindTree()
        //{
        //    var result = UseMemoryMarshalPoint("abcdefj");
        //}

        //[Benchmark]
        //public void UnsafeFindTree()
        //{
        //    var result = UseUnsafe2("abcdefj");
        //}

        public unsafe int UseMemoryMarshalPoint(string arg)
        {

            fixed (char* c = arg)
            {
                switch (Unsafe.ReadUnaligned<int>(c + 2))
                {
                    case 6422625:
                        switch (Unsafe.ReadUnaligned<int>(c + 4))
                        {
                            case 3211313:
                                switch (Unsafe.ReadUnaligned<int>(c + 8))
                                {
                                    case 99:
                                        return 1;
                                    case 100:
                                        return 2;
                                }
                                break;
                            case 3276850:
                                switch (*(int*)(c + 8))
                                {
                                    case 101:
                                        return 3;
                                    case 7471205:
                                        var a = 1 + 1;
                                        break;
                                }
                                break;
                            case 3276851:
                                return 4;
                        }
                        break;
                    case 6553699:
                        switch (Unsafe.ReadUnaligned<short>(c + 6))
                        {
                            case 102:
                                return 5;
                            case 103:
                                return 6;
                            case 105:
                                return 7;
                            case 104:
                                return 8;
                            case 106:
                                return 9;
                        }
                        break;
                }
            }
            return default;

        }


        public unsafe int UseUnsafe(string arg)
        {

            var bytes = MemoryMarshal.AsBytes(arg.AsSpan());
            ref var addr = ref Unsafe.AsRef(in bytes.GetPinnableReference());
            
            switch (Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref addr, 4)))
            {
                case 6422625:
                    switch (Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref addr, 8)))
                    {
                        case 3211313:
                            switch (Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref addr, 16)))
                            {
                                case 99:
                                    return 1;
                                case 100:
                                    return 2;
                            }
                            break;
                        case 3276850:
                            switch (Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref addr, 16)))
                            {
                                case 101:
                                    return 3;
                                case 7471205:
                                    var a = 1 + 1;
                                    break;
                            }
                            break;
                        case 3276851:
                            return 4;
                    }
                    break;
                case 6553699:
                    switch (Unsafe.ReadUnaligned<short>(ref Unsafe.Add(ref addr, 12)))
                    {
                        case 102:
                            return 5;
                        case 103:
                            return 6;
                        case 105:
                            return 7;
                        case 104:
                            return 8;
                        case 106:
                            return 9;
                    }
                    break;
            }

            return default;

        }

        //[Benchmark]
        //public void ShortPtrFindTree()
        //{
        //    var result = UseShort("abcdefj");
        //}
        public unsafe int UseUnsafe2(string arg)
        {

            //var bytes = MemoryMarshal.AsBytes(arg.AsSpan());
            //ref var addr = ref Unsafe.AsRef(in bytes.GetPinnableReference());
            //var span = arg.AsSpan();
            //ref var addr = ref Unsafe.AvPointer(arg.AsSpan().GetPinnableReference());
            ref char addr = ref Unsafe.AsRef(in arg.AsSpan().GetPinnableReference());

            switch (Unsafe.As<char, int>(ref Unsafe.Add(ref addr, 2)))
            {
                case 6422625:
                    switch (Unsafe.As<char, int>(ref Unsafe.Add(ref addr, 4)))
                    {
                        case 3211313:
                            switch (Unsafe.As<char, int>(ref Unsafe.Add(ref addr, 8)))
                            {
                                case 99:
                                    return 1;
                                case 100:
                                    return 2;
                            }
                            break;
                        case 3276850:
                            switch (Unsafe.As<char, int>(ref Unsafe.Add(ref addr, 8)))
                            {
                                case 101:
                                    return 3;
                                case 7471205:
                                    var a = 1 + 1;
                                    break;
                            }
                            break;
                        case 3276851:
                            return 4;
                    }
                    break;
                case 6553699:
                    switch (Unsafe.As<char, short>(ref Unsafe.Add(ref addr, 6)))
                    {
                        case 102:
                            return 5;
                        case 103:
                            return 6;
                        case 105:
                            return 7;
                        case 104:
                            return 8;
                        case 106:
                            return 9;
                    }
                    break;
            }

            return default;

        }



        public unsafe int UseShort(string arg)
        {
            fixed (char* c = arg)
            {
                switch (*(short*)(c + 2))
                {
                    case 97:
                        switch (*(short*)(c + 4))
                        {
                            case 49:
                                switch (*(short*)(c + 8))
                                {
                                    case 99:
                                        return 1;
                                    case 100:
                                        return 2;
                                }
                                break;
                            case 50:
                                switch (*(short*)(c + 9))
                                {
                                    case 0:
                                        return 3;
                                    case 114:
                                        var a = 1 + 1;
                                        break;
                                }
                                break;
                            case 51:
                                return 4;
                        }
                        break;
                    case 99:
                        switch (*(short*)(c + 6))
                        {
                            case 102:
                                return 5;
                            case 103:
                                return 6;
                            case 105:
                                return 7;
                            case 104:
                                return 8;
                            case 106:
                                return 9;
                        }
                        break;
                }
            }
            return default;
        }


        //[Benchmark]
        //public void ConcurrentDict()
        //{
        //    var result = ConDict["abcdefj"];
        //}


        //[Benchmark]
        public void NormalDict()
        {
            var result = Dict["abcdefj"];
        }


        private static readonly byte[] array = new byte[1024];

        

        [Benchmark]
        public unsafe void ByteSpan()
        {

            //var array = new byte[1024];
            var span = array.AsSpan();
            for (var i = 0; i < span.Length; i += 1)
            {
                span[i] = byte.MinValue;
            }

        }


        [Benchmark]
        public unsafe void BytePointer()
        {
            
            //var array = new byte[1024];
            fixed (byte* pointer = array)
            {
                for (var i = 0; i < array.Length; i += 1)
                {
                    *(pointer + i) = byte.MinValue;
                }
            }

        }

        //[Benchmark]
        public unsafe void BytePointer2()
        {

            //byte[] array = new byte[1024];
            fixed (byte* pointer = &array[0])
            {
                for (var i = 0; i < array.Length; i += 1)
                {
                    *(pointer + i) = *(pointer + i);
                }
            }

        }

        public int UseHash(string arg)
        {
            var a = arg.GetHashCode();
            a = 10;
            switch (a)
            {
                case 1663536773:
                    return 1;
                case -261059740:
                    return 2;
                case -817518664:
                    return 3;
                case -221347000:
                    var b = 1 + 1;
                    break;
                case 1485518367:
                    return 4;
                case 1059151160:
                    return 5;
                case -1970978525:
                    return 6;
                case -595701590:
                    return 7;
                case -538885107:
                    return 8;
                case 10:
                    return 9;
            }
            return default;
        }

        private unsafe int UsePoint(string arg)
        {
            fixed (char* c = arg)
            {
                switch (*(int*)(c + 2))
                {
                    case 6422625:
                        switch (*(int*)(c + 4))
                        {
                            case 3211313:
                                switch (*(int*)(c + 8))
                                {
                                    case 99:
                                        return 1;
                                    case 100:
                                        return 2;
                                }
                                break;
                            case 3276850:
                                switch (*(int*)(c + 8))
                                {
                                    case 101:
                                        return 3;
                                    case 7471205:
                                        var a = 1 + 1;
                                        break;
                                }
                                break;
                            case 3276851:
                                return 4;
                        }
                        break;
                    case 6553699:
                        switch (*(short*)(c + 6))
                        {
                            case 102:
                                return 5;
                            case 103:
                                return 6;
                            case 105:
                                return 7;
                            case 104:
                                return 8;
                            case 106:
                                return 9;
                        }
                        break;
                }
            }
            return default;
        }

        private unsafe int UseSpanPoint(string arg)
        {
            fixed (char* c = arg)
            {
                switch (*(int*)(c + 2))
                {
                    case 6422625:
                        switch (*(int*)(c + 4))
                        {
                            case 3211313:
                                switch (*(int*)(c + 8))
                                {
                                    case 99:
                                        return 1;
                                    case 100:
                                        return 2;
                                }
                                break;
                            case 3276850:
                                switch (*(int*)(c + 8))
                                {
                                    case 101:
                                        return 3;
                                    case 7471205:
                                        var a = 1 + 1;
                                        break;
                                }
                                break;
                            case 3276851:
                                return 4;
                        }
                        break;
                    case 6553699:
                        switch (*(short*)(c + 6))
                        {
                            case 102:
                                return 5;
                            case 103:
                                return 6;
                            case 105:
                                return 7;
                            case 104:
                                return 8;
                            case 106:
                                return 9;
                        }
                        break;
                }
            }
            return default;
        }
    }

}
