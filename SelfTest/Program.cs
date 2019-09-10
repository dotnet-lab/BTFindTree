using BTFindTree;
using System;
using System.Collections.Generic;

namespace SelfTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine((int)char.MaxValue);
            Console.WriteLine(short.MaxValue);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["abab1123c"] = "return 1;";
            dict["abab1123d"] = "return 2;";

            dict["abab2213e"] = "return 3;";
            dict["abab2213er"] = "var a = 1 + 1;";

            dict["abab3213f"] = "return 4;";
            dict["abcdeff"] = "return 5;";
            dict["abcdefg"] = "return 6;";
            dict["abcdefi"] = "return 7;";
            dict["abcdefh"] = "return 8;";
            dict["abcdefj"] = "return 9;";
            //Console.WriteLine(BTFTemplate.GetPointBTFScript(dict));
            Console.WriteLine(BTFTemplate.GetHashBTFScript(dict));
            Console.ReadKey();

        }
    }
}
