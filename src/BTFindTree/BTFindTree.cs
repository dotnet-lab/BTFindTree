using System;


namespace BTFindTree
{

    public class BTFTree<T>
    {


        public readonly BTFTree<T> LssTree;
        public readonly BTFTree<T> GtrTree;
        public readonly T[] Buckets;
        public readonly int CompareCode;


        public BTFTree(Memory<T> values,int slice)
        {

            if (values.Length<=slice)
            {

                Buckets = values.Slice(0, values.Length).ToArray();

            }
            else
            {

                var left = values.Length >> 1;
                CompareCode = values.Span[left].GetHashCode();


                LssTree = new BTFTree<T>(values.Slice(0, left), slice);
                GtrTree = new BTFTree<T>(values.Slice(left, values.Length - left), slice);

            }
        }
    }
}
