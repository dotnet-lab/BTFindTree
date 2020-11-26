using BTFindTree;
using System;
using System.Collections.Generic;

namespace SelfTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["abcdefh"] = "return 9;";
            dict["abcdefj"] = "return 10;";
            dict["a"] = "return 11;";

            //dict["abab1123"] = "return 0;";
            //dict["abab1123c"] = "return 1;";
            //dict["abab1123d"] = "return 2;";

            //dict["abab2213e"] = "return 3;";
            //dict["abab2213er"] = "var a = 1 + 1;";

            //dict["abab3213f"] = "return 4;";
            //dict["abcdeff"] = "return 5;";
            //dict["abcdefg"] = "return 6;";
            //dict["abcdefi"] = "return 7;";
            //dict["abcdefh"] = "return 8;";
            //dict["abcdefj"] = "return 9;";
            //Console.WriteLine(BTFTemplate.GetFuzzyPointBTFScript(dict));
            Console.WriteLine(BTFTemplate.GetGroupPrecisionPointBTFScript(dict));
            //Console.WriteLine(BTFTemplate.GetHashBTFScript(dict));
            Class1 a = new Class1();
            Console.WriteLine(a.C("abab2213e"));
            Console.ReadKey();

        }
    }
}
