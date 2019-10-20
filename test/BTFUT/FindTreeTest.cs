using BTFindTree;
using Natasha;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BenchmarkTest
{


    public class FindTreeTest
    {

        Func<string, int> HashDelegate;
        Func<string, int> FuzzyDelegate;
        Func<string, int> PrecisionDelegate;
        readonly Dictionary<string, int> Dict;
        readonly Dictionary<string, string> ScriptDict;

        public FindTreeTest()
        {

            Dict = new Dictionary<string, int>();
            Dict["NullClass"] = 0;
            Dict["Test2"] = 1;
            Dict["abab1123"] = 0;
            Dict["abab1123c"] = 1;
            Dict["abab1123d"] = 2;
            Dict["abab2213e"] = 3;
            Dict["abab2213er"] = 4;
            Dict["abab3213f"] = 5;
            Dict["abcdeff"] = 6;
            Dict["abcdefg"] = 7;
            Dict["abcdefi"] = 8;
            Dict["abcdef"] = 90;
            Dict["abcdefcidefg"] = 91;
            Dict["abcdefh"] = 9;
            Dict["abcdefj"] = 10;
            Dict["a"] = 11;
            Dict["Age"] = 220;
            Dict["Name"] = 221;
            Dict["Temp"] = 122;
            Dict["Money"] = 121;
            Dict["acbc1"] = 21;
            Dict["afdc2"] = 22;
            Dict["a73c5"] = 23;
            Dict["a7"] = 24;
            Dict["a74d5"] = 25;
            ScriptDict = new Dictionary<string, string>(Dict.Select(item => KeyValuePair.Create(item.Key, "return " + item.Value.ToString() + ";")));
            //HashDelegate = NFunc<string, int>.UnsafeDelegate(BTFTemplate.GetHashBTFScript(ScriptDict)+"return default;");
            //FuzzyDelegate = NFunc<string, int>.UnsafeDelegate(BTFTemplate.GetFuzzyPointBTFScript(ScriptDict) + "return default;");
            //PrecisionDelegate = NFunc<string, int>.UnsafeDelegate(BTFTemplate.GetPrecisionPointBTFScript(ScriptDict) + "return default;");
        }


        [Fact(DisplayName = "哈希查找树")]
        public void HashFindTree()
        {
            HashDelegate = NFunc<string, int>.UnsafeDelegate(BTFTemplate.GetHashBTFScript(ScriptDict) + "return default;");
            foreach (var item in Dict)
            {
                Assert.Equal(item.Value, HashDelegate(item.Key));
            }

        }

        [Fact(DisplayName = "空-初始化")]
        public void RunNull()
        {

            Assert.Equal(1, 1);

        }

        [Fact(DisplayName = "模糊指针查找树")]
        public void FuzzyFindTree()
        {
            FuzzyDelegate = NFunc<string, int>.UnsafeDelegate(BTFTemplate.GetFuzzyPointBTFScript(ScriptDict) + "return default;");
            foreach (var item in Dict)
            {
                Assert.Equal(item.Value, FuzzyDelegate(item.Key));
            }
        }



        [Fact(DisplayName = "归并最小权查找树")]
        public void PrecisionFindTree()
        {
            PrecisionDelegate = NFunc<string, int>.UnsafeDelegate(BTFTemplate.GetPrecisionPointBTFScript(ScriptDict) + "return default;");
            foreach (var item in Dict)
            {
                Assert.Equal(item.Value, PrecisionDelegate(item.Key));
            }
        }

    }
}
