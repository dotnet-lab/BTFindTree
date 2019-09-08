using System;
using System.Collections.Generic;

namespace BTFindTree
{

    public class HashBTFTree
    {


        public readonly HashBTFTree LssTree;
        public readonly HashBTFTree GtrTree;
        public readonly KeyValuePair<string,string>[] Buckets;
        public readonly int CompareCode;


        public HashBTFTree(Memory<T> values,int slice)
        {

            if (values.Length<=slice)
            {

                Buckets = values.Slice(0, values.Length).ToArray();

            }
            else
            {

                var left = values.Length >> 1;
                CompareCode = values.Span[left].GetHashCode();


                LssTree = new HashBTFTree<T>(values.Slice(0, left), slice);
                GtrTree = new HashBTFTree<T>(values.Slice(left, values.Length - left), slice);

            }
        }
    }
}
