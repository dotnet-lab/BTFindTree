using BTFindTree.Template.PrecisionTreeTemplate.Model;
using System.Collections.Generic;
using System.Linq;

namespace BTFindTree
{

    public class PrecisionMinPriorityTree
    {

        private readonly int MaxLength;
        private int MaxMatchCount;
        public readonly List<string> Values;
        private List<RepeateModel> PriorityCache;
        private readonly Dictionary<string, FrequencyModel> TripModels;


        public PrecisionMinPriorityTree(params string[] strs)
        {

            TripModels = new Dictionary<string, FrequencyModel>();
            for (int i = 0; i < strs.Length; i += 1)
            {

                if (MaxLength < strs[i].Length)
                {
                    MaxLength = strs[i].Length;
                }
                TripModels[strs[i]] = new FrequencyModel(strs[i]);

            }
            Values = new List<string>(strs);
        }




        public void GetSingleCharFrequency()
        {

            //统计每个字符在当前位置出现的次数
            for (int i = 0; i < MaxLength; i += 1)
            {

                Dictionary<char, int> charsCache = new Dictionary<char, int>();
                foreach (var item in TripModels)
                {

                    if (item.Key.Length > i)
                    {

                        var tempChar = item.Key[i];
                        if (charsCache.ContainsKey(tempChar))
                        {

                            charsCache[tempChar] += 1;

                        }
                        else
                        {

                            charsCache[tempChar] = 1;

                        }

                    }

                }

                //把之前位置对应的字符出现的频率，写入缓存中
                foreach (var itemChar in charsCache)
                {

                    foreach (var item in TripModels)
                    {

                        if (item.Key.Length > i)
                        {

                            if (item.Key[i] == itemChar.Key)
                            {

                                item.Value.RepeateCache[i] = itemChar.Value;
                                if (MaxMatchCount < itemChar.Value)
                                {

                                    MaxMatchCount = itemChar.Value;

                                }

                            }

                        }

                    }

                }

            }


            //foreach (var item in TripModels)
            //{

            //    Console.WriteLine(item.Value.Value);
            //    for (int i = 0; i < item.Value.RepeateCache.Count; i++)
            //    {
            //        Console.WriteLine("\t 字符：{0}  \t出现{1}次", item.Value.Value[i], item.Value[i]);
            //    }

            //}


        }

        //public void Show(int length)
        //{
        //    Console.WriteLine("获取出现次数在{0}次以上的字符串", length);
        //    foreach (var item in TripModels)
        //    {
        //        var list = item.Value.GetByMatchCount(length);
        //        Console.WriteLine(item.Key);
        //        foreach (var itemList in list)
        //        {
        //            Console.WriteLine("起始位置在：{0}，长度为：{1}；字符串为：{2}", itemList.StartIndex, itemList.Length, item.Key.Substring(itemList.StartIndex, itemList.Length));
        //        }
        //    }
        //}

        public void GetAllWordsFrequency()
        {

            int priority = int.MaxValue;
            for (int i = 1; i <= MaxMatchCount; i++)
            {

                foreach (var item in TripModels)
                {

                    //找到连续的相同位置的字符串 i 是能匹配到的频次
                    var list = item.Value.GetByMatchCount(i + 1);
                    //Console.WriteLine();
                    //Console.Write("字符串 ");
                    //Console.ForegroundColor = ConsoleColor.Magenta;
                    //Console.Write(item.Key);
                    //Console.ForegroundColor = ConsoleColor.DarkGray;
                    //Console.WriteLine(" 分析结果：");
                    //Console.WriteLine();


                    int offset = 0;
                    List<RepeateModel> modelCache = new List<RepeateModel>();
                    foreach (var itemList in list)
                    {
                        if (i==6)
                        {

                        }
                        //记录上一次偏移量，如果有间隔，则添加间隔
                        if (offset != itemList.StartIndex)
                        {
                            modelCache.AddRange(GetFromSpace(itemList.StartIndex, ref offset));
                        }

                        offset = itemList.StartIndex + itemList.Length;
                        var str = item.Key.Substring(itemList.StartIndex, itemList.Length);


                        //进行高频比对，先找到高频为4出现最多的，如果再中间，则两头比对
                        //结果应该是当前比对字串的分割集合
                        var models = GetHighFrequency(str, itemList.StartIndex);
                        for (int j = 0; j < models.Count; j++)
                        {

                            var temp = models[j];
                            if (models[j].MatchCount > 0)
                            {

                                //增加偏移量，使用当前的偏移量
                                temp.StartIndex += itemList.StartIndex;
                                modelCache.Add(temp);

                            }

                        }

                    }


                    modelCache.AddRange(GetFromSpace(MaxLength, ref offset));
                    int result = GetPriority(modelCache);
                    if (priority > result)
                    {
                        priority = result;
                        PriorityCache = modelCache;
                    }

                }

            }
        }




        public List<PriorityTreeModel> GetPriorityTrees()
        {

            GetSingleCharFrequency();
            GetAllWordsFrequency();
            var list = new List<RepeateModel>(from result in PriorityCache
                                              orderby result.MatchCount descending
                                              select result);
            return GetTrees(Values, list);

        }




        private static List<PriorityTreeModel> GetTrees(IEnumerable<string> strs, List<RepeateModel> models, int deepth = 0)
        {

            HashSet<string> cache = new HashSet<string>(strs);
            List<PriorityTreeModel> lists = new List<PriorityTreeModel>();
            if (deepth < models.Count)
            {

                Dictionary<string, PriorityTreeModel> sets = new Dictionary<string, PriorityTreeModel>();
                Dictionary<PriorityTreeModel, List<string>> dict = new Dictionary<PriorityTreeModel, List<string>>();
                //找到当前的拾取节点
                var model = models[deepth];
                //遍历字符串
                foreach (var item in strs)
                {

                    //如果当前拾取起点已经超过字符串的最大值则跳过
                    if (item.Length > model.StartIndex)
                    {

                        string node;
                        //如果拾取长度大于字符串的总长度则只截取字符串剩余的部分
                        if (item.Length <= model.StartIndex + model.Length)
                        {

                            node = item.Substring(model.StartIndex, item.Length - model.StartIndex);
                            //生成截取节点
                            if (!sets.ContainsKey(node))
                            {

                                PriorityTreeModel priority = new PriorityTreeModel();
                                dict[priority] = new List<string>();
                                priority.Value = node;
                                priority.FullValue = item;
                                priority.Offset = model.StartIndex;
                                priority.Length = model.Length;
                                if (deepth != models.Count - 1)
                                {
                                    priority.Next.Add(new PriorityTreeModel()
                                    {
                                        Value = default,
                                        FullValue = item
                                    });
                                }
                                lists.Add(priority);
                                sets[node] = priority;

                            }

                        }
                        else
                        {

                            node = item.Substring(model.StartIndex, model.Length);
                            //生成截取节点
                            if (!sets.ContainsKey(node))
                            {

                                PriorityTreeModel priority = new PriorityTreeModel();
                                dict[priority] = new List<string>();
                                priority.Value = node;
                                priority.FullValue = item;
                                priority.Offset = model.StartIndex;
                                priority.Length = model.Length;
                                lists.Add(priority);
                                sets[node] = priority;

                            }

                        }



                        dict[sets[node]].Add(item);
                        cache.Remove(item);

                    }

                }

                int nextDeepth = deepth + 1;
                foreach (var item in dict)
                {

                    item.Key.Next.AddRange(GetTrees(item.Value, models, nextDeepth));

                }

                lists.AddRange(GetTrees(cache, models, nextDeepth));

            }

            return lists;
        }




        private static List<RepeateModel> GetFromSpace(int index, ref int offset)
        {

            var list = new List<RepeateModel>();
            if (offset < index)
            {

                //如果当前偏移量比前一偏移量多3个长度
                while (index - offset > 3)
                {

                    RepeateModel model = new RepeateModel
                    {
                        StartIndex = offset,
                        Length = 4
                    };
                    offset += 4;
                    list.Add(model);

                }


                //如果小玉3个长度
                if (offset != index)
                {

                    RepeateModel model2 = new RepeateModel();
                    if (index - offset == 3)
                    {

                        model2.StartIndex = offset - 1;
                        model2.Length = 4;


                    }
                    else
                    {

                        model2.StartIndex = offset;
                        model2.Length = index - offset;

                    }
                    //偏移量增加
                    offset = index;
                    list.Add(model2);

                }

            }
            return list;
        }




        private int GetPriority(List<RepeateModel> models)
        {

            int priority = 0;

            foreach (var model in models)
            {
                HashSet<string> sets = new HashSet<string>();
                foreach (var item in TripModels)
                {
                    string temp;
                    int diff = item.Key.Length - model.StartIndex;
                    if (diff < model.Length)
                    {
                        if (diff > 0)
                        {
                            temp = item.Key.Substring(model.StartIndex, diff);
                        }
                        else
                        {
                            continue;
                        }

                    }
                    else
                    {
                        temp = item.Key.Substring(model.StartIndex, model.Length);

                    }
                    if (!sets.Contains(temp))
                    {
                        priority += 1;
                        sets.Add(temp);
                    }

                }
            }

            return priority;

        }




        private List<RepeateModel> GetHighFrequency(string str, int index, int offset = 0)
        {

            List<RepeateModel> result = new List<RepeateModel>();
            RepeateModel model;

            if (str.Length == 1)
            {

                model = GetHightFrequencyByIndex(str, index, 1);

            }else if (str.Length == 2)
            {

                model = GetHightFrequencyByIndex(str, index, 2);

            }
            else
            {

                model = GetHightFrequencyByIndex(str, index, 4);

            }


            int preIndex = model.StartIndex;
            model.StartIndex += offset;


            //Console.WriteLine("{3}类型，偏移量为：{2}，频次最高为：{0}，对应的字符串：{1}", model.MatchCount, source.Substring(model.StartIndex, 4), model.StartIndex, type);
            if (str.Length == model.Length || model.Length == 0)
            {

                result.Add(model);
                return result;

            }
            else
            {

                if (preIndex != 0)
                {

                    var leftStr = str.Substring(0, preIndex);
                    if (leftStr != default)
                    {

                        if (leftStr.Length == 1)
                        {

                            leftStr = str.Substring(0, preIndex + 1);
                            var leftModel1 = GetHightFrequencyByIndex(leftStr, index, 1, true, MatchOrder.LeftToRight);
                            var leftModel2 = GetHightFrequencyByIndex(leftStr, index, 2, true, MatchOrder.LeftToRight);


                            if (leftModel1.MatchCount < leftModel2.MatchCount)
                            {

                                leftModel1 = leftModel2;

                            }
                            leftModel1.StartIndex += offset;
                            result.Add(leftModel1);

                        }
                        else if (leftStr.Length == 2)
                        {

                            var leftModel2 = GetHightFrequencyByIndex(leftStr, index, 2, true, MatchOrder.LeftToRight);
                            leftModel2.StartIndex += offset;
                            result.Add(leftModel2);

                        }
                        else if (leftStr.Length == 3)
                        {

                            leftStr = str.Substring(0, preIndex + 1);
                            var leftModel1 = GetHightFrequencyByIndex(leftStr, index, 2, true, MatchOrder.LeftToRight);
                            var leftModel2 = GetHightFrequencyByIndex(leftStr, index, 4, true, MatchOrder.LeftToRight);

                            if (leftModel1.MatchCount < leftModel2.MatchCount)
                            {

                                leftModel1 = leftModel2;

                            }
                            leftModel1.StartIndex += offset;
                            result.Add(leftModel1);

                        }
                        else
                        {

                            var tempResult = GetHighFrequency(leftStr, index, 0);
                            for (int i = 0; i < tempResult.Count; i++)
                            {
                                var temp = tempResult[i];
                                temp.StartIndex += preIndex;
                                result.Add(temp);
                            }

                        }
                    }

                }

                result.Add(model);

                int tempIndex = preIndex + model.Length;
                if (tempIndex < str.Length)
                {

                    int RIGHTINDEX = index + tempIndex;
                    var rightStr = str.Substring(tempIndex, str.Length - tempIndex);
                    if (rightStr != default)
                    {

                        if (rightStr.Length == 1)
                        {

                            rightStr = str.Substring(tempIndex - 1, str.Length - tempIndex + 1);
                            RIGHTINDEX -= 1;
                            var rightModel1 = GetHightFrequencyByIndex(rightStr, RIGHTINDEX, 1, true, MatchOrder.RightToLeft);
                            var rightModel2 = GetHightFrequencyByIndex(rightStr, RIGHTINDEX, 2, true, MatchOrder.RightToLeft);


                            if (rightModel1.MatchCount < rightModel2.MatchCount)
                            {

                                rightModel1 = rightModel2;

                            }
                            rightModel1.StartIndex += tempIndex - 1;
                            result.Add(rightModel1);

                        }
                        else if (rightStr.Length == 2)
                        {

                            var rightModel1 = GetHightFrequencyByIndex(rightStr, RIGHTINDEX, 2, true, MatchOrder.RightToLeft);
                            rightModel1.StartIndex += tempIndex;
                            result.Add(rightModel1);

                        }
                        else if (rightStr.Length == 3)
                        {

                            rightStr = str.Substring(tempIndex - 1, str.Length - tempIndex + 1);
                            RIGHTINDEX -= 1;
                            var rightModel1 = GetHightFrequencyByIndex(rightStr, RIGHTINDEX, 4, true, MatchOrder.RightToLeft);
                            var rightModel2 = GetHightFrequencyByIndex(rightStr, RIGHTINDEX, 2, true, MatchOrder.RightToLeft);


                            if (rightModel1.MatchCount < rightModel2.MatchCount)
                            {

                                rightModel1 = rightModel2;

                            }
                            rightModel1.StartIndex += tempIndex - 1;
                            result.Add(rightModel1);

                        }
                        else
                        {

                            var tempResult = GetHighFrequency(rightStr, RIGHTINDEX, 0);
                            for (int i = 0; i < tempResult.Count; i++)
                            {
                                var temp = tempResult[i];
                                temp.StartIndex += tempIndex;
                                result.Add(temp);
                            }

                        }
                    }

                }

            }

            return result;
        }




        private RepeateModel GetHightFrequencyByIndex(string str, int index, int length = 4, bool once = false, MatchOrder order = MatchOrder.LeftToRight)
        {

            RepeateModel model = default;
            if (order == MatchOrder.LeftToRight)
            {

                int totle = str.Length - length;
                if (once)
                {

                    totle = 0;

                }


                if (totle<0)
                {
                    model.Length = length;
                    model.StartIndex = 0;
                }


                for (int i = 0; i <= totle; i++)
                {

                    var tempStr = str.Substring(i, length);
                    int frequency = 0;
                    int offset = index + i;


                    foreach (var item in TripModels)
                    {

                        if (offset + length <= item.Key.Length)
                        {

                            var matchStr = item.Key.Substring(offset, length);
                            if (tempStr == matchStr)
                            {

                                frequency += 1;

                            }

                        }

                    }


                    if (frequency > 0)
                    {

                        model = new RepeateModel
                        {
                            StartIndex = i,
                            Length = length,
                            MatchCount = frequency
                        };
                        return model;

                    }

                }

            }
            else
            {

                int total = str.Length - length;
                int start = 0;
                if (once)
                {

                    start = total;

                }


                for (int i = total; i >= start; i -= 1)
                {

                    var tempStr = str.Substring(i, length);
                    int frequency = 0;
                    int offset = index + i;
                    foreach (var item in TripModels)
                    {

                        if (offset + length <= item.Key.Length)
                        {
                            var matchStr = item.Key.Substring(offset, length);
                            if (tempStr == matchStr)
                            {

                                frequency += 1;

                            }
                        }

                    }


                    if (frequency > 0)
                    {

                        model = new RepeateModel
                        {
                            StartIndex = i,
                            Length = length,
                            MatchCount = frequency
                        };
                        return model;

                    }

                }
            }
            return model;
        }
    }

}
