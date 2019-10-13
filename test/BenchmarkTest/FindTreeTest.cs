using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            Dict["abab2213er"] =2;

            Dict["abab3213f"] = 4;
            Dict["abcdeff"] = 5;
            Dict["abcdefg"] = 6;
            Dict["abcdefi"] = 7;
            Dict["abcdefh"] = 8;
            Dict["abcdefj"] = 9;
            ConDict = new ConcurrentDictionary<string, int>(Dict);
        }
        [Benchmark]
        public void HashFindTree()
        {
            var result = UseHash("abcdefj");
        }



        [Benchmark]
        public void IntPtrFindTree()
        {
            var result = UsePoint("abcdefj");
        }



        [Benchmark]
        public void ShortPtrFindTree()
        {
            var result = UseShort("abcdefj");
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


        [Benchmark]
        public void ConcurrentDict()
        {
            var result = ConDict["abcdefj"];
        }


        [Benchmark]
        public void NormalDict()
        {
            var result = Dict["abcdefj"];
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

        public unsafe int UsePoint(string arg)
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
                        switch (*(int*)(c + 6))
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
