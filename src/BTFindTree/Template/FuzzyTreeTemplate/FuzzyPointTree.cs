﻿using System.Collections.Generic;
using System.Linq;

namespace BTFindTree
{

    public class FuzzyPointTree
    {

        public List<FuzzyPointTree> Nodes;
        public readonly string Value;
        public readonly ushort PointCode;
        public int Layer;


        public const int OfferSet = 1;
        public const string OfferType = "ushort";


        public FuzzyPointTree(KeyValuePair<string, string> value, int layer = 0)
        {

            //当前结果值
            Value = value.Value;
            //当前匹配值
            PointCode = value.Key.GetUShort(layer);
            //当前层数
            Layer = layer;

        }




        public FuzzyPointTree(IDictionary<string, string> values, int layer = 0, ushort pCode = 0)
        {

            Layer = layer;
            PointCode = pCode;

            if (values.Count == 1 && layer != 0)
            {

                //如果递归集合中只剩一个元素，那么认为它的路径已经确认。
                foreach (var value in values)
                {

                    //直接设置当前节点信息
                    Value = value.Value;
                    
                }

            }
            else
            {

                //初始化节点
                Nodes = new List<FuzzyPointTree>();
                var valuesDict = new Dictionary<ushort, Dictionary<string, string>>();


                foreach (var item in values)
                {

                    //如果长度小于指针偏移量则视为叶子节点
                    //如果 Key总长度 <= 当前指针偏移量
                    if (item.Key.Length <= layer * OfferSet)
                    {

                        //         2
                        //  1 -
                        //         2
                        //         3
                        //  1 节点的 Nodes 集合里将存放 节点 2/2/3
                        Nodes.Add(new FuzzyPointTree(item, layer));

                    }
                    else
                    {

                        //获取当前元素Key值偏移量的指针数据
                        ushort pcode = item.Key.GetUShort(layer);
                        if (!valuesDict.ContainsKey(pcode))
                        {

                            //如果缓存中不存在指针值则以该值创建字典缓存
                            valuesDict[pcode] = new Dictionary<string, string>();

                        }

                        //将当前值以及元素添加到缓存
                        valuesDict[pcode][item.Key] = item.Value;
                        //code - key - value
                        //code - key1 - value1
                        //code - key2 - value2

                    }

                }


                foreach (var item in valuesDict)
                {

                    //将指针值以及改指针值下面的元素继续递归处理
                    //以当前元素为节点，字典向下传递
                    //pcode 即 item.Key  向下传递               
                    Nodes.Add(new FuzzyPointTree(item.Value, layer + 1, item.Key));

                }

            }

        }

    }

}
