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
            dict["abab1123c"] = "return 1;";
            dict["abab1123d"] = "return 2;";

            dict["abab2213e"] = "return 3;";
            dict["abab2213er"] = "var a = 1 + 1;";

            dict["abab3213f"] = "return 4;";
            dict["abcdeff"] = "return 5;";

            Console.WriteLine(BTFTemplate.GetPointBTFScript(dict));
            Console.ReadKey();

        }
    }
}
