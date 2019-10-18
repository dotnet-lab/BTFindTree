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
        private readonly Dictionary<string, FrequencyModel> TripCache;


        public PrecisionMinPriorityTree(params string[] strs)
        {

            TripCache = new Dictionary<string, FrequencyModel>();
            for (int i = 0; i < strs.Length; i += 1)
            {

                if (MaxLength < strs[i].Length)
                {
                    MaxLength = strs[i].Length;
                }
                TripCache[strs[i]] = new FrequencyModel(strs[i]);

            }
            Values = new List<string>(strs);
        }




        private void GetSingleCharFrequency()
        {

            //统计每个字符在当前位置出现的次数
            for (int i = 0; i < MaxLength; i += 1)
            {

                //遍历trip字符缓存
                Dictionary<char, int> charsCache = new Dictionary<char, int>();
                foreach (var item in TripCache)
                {
                    //如果当前字符串的长度大于索引
                    if (item.Key.Length > i)
                    {

                        //获取字符
                        var tempChar = item.Key[i];
                        if (charsCache.ContainsKey(tempChar))
                        {
                            //如果缓存里已经存在了字符，那么缓存计数+1
                            charsCache[tempChar] += 1;

                        }
                        else
                        {

                            //否则设置缓存字符，计数默认为1
                            charsCache[tempChar] = 1;

                        }

                    }

                }

                //把之前位置对应的字符出现的频率，写入缓存中
                //再一次遍历trip字符缓存
                foreach (var item in TripCache)
                {

                    //如果当前字符串的长度大于索引
                    if (item.Key.Length > i)
                    {

                        //设置当前层字符的匹配频次
                        int matchCount = charsCache[item.Key[i]];
                        item.Value.RepeateCache[i] = matchCount;
                        //记录频次最大值
                        if (MaxMatchCount < matchCount)
                        {

                            MaxMatchCount = matchCount;

                        }

                    }

                }

            }

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

            //将权值设置为最大
            int priority = int.MaxValue;

            //从1次开始匹配一直到最高频次
            for (int i = 1; i <= MaxMatchCount; i++)
            {

                //循环遍历trip缓存
                foreach (var item in TripCache)
                {

                    //找到连续的相同位置的字符串 i 是能匹配到的频次
                    var list = item.Value.GetByFrequency(i);


                    //设置当前偏移量
                    int offset = 0;
                    List<RepeateModel> modelCache = new List<RepeateModel>();
                    foreach (var itemList in list)
                    {

                        //记录上一次偏移量，如果有间隔，则添加间隔
                        //如果当前偏移量和连续节点的偏移量不相等
                        //说明连续节点有了跳跃
                        //  x x x x x x x - - - - - - - - - index - - - - -
                        //  | ____offset____|_GetFromSpace__|         |__length__|
                        //
                        if (offset != itemList.StartIndex)
                        {
                            //搜集中间被跳过字符串的分割策略
                            //
                            modelCache.AddRange(GetFromSpace(itemList.StartIndex, ref offset));
                        }


                        //获取这部分字符
                        var str = item.Key.Substring(itemList.StartIndex, itemList.Length);


                        //进行高频比对，先找到高频为4出现最多的，如果再中间，则两头比对
                        //结果应该是当前比对字串的分割集合
                        //针对上部分字符，进行高频分解
                        var models = GetHighFrequency(str, itemList.StartIndex);
                        for (int j = 0; j < models.Count; j++)
                        {

                            //因为是结构体，所以要单独拿出来操作
                            var temp = models[j];
                            //增加偏移量，使用当前的偏移量
                            temp.StartIndex += offset;
                            //添加结构体
                            modelCache.Add(temp);

                        }


                        //偏移量继续递增，跳到当前连续节点的后面
                        //  x x x x x x x x x x x x x x x x x index - - - - -
                        //                                                                 |__length__|
                        //  |_____________________________offset___________________________________|
                        //  
                        offset += itemList.Length;

                    }


                    //偏移量继续递增，跳到当前连续节点的后面
                    //  x x x x x x x x x x x x x x x x x xxxxx - - - - -
                    //  |_____________________________offset____________________|
                    //  |_____________________________MaxLength___________________________|
                    modelCache.AddRange(GetFromSpace(MaxLength, ref offset));

                    //获取最小权
                    int result = GetPriority(modelCache);
                    //选取最小权分配节点
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



        /*            n1                                  n2
        *        /    \      \                          /     \ 
        *       /      \      \                        /       \
        *     n3e      n4      d4                     n5       n6e
        *               |     /  \                    /\
        *              n7e  n8e  n9e               n10e  n11e
        *             
        *   n：节点
        *   d: 无数据的空白节点
        *   e: 末尾
        */
        private List<PriorityTreeModel> GetTrees(IEnumerable<string> strs, List<RepeateModel> models, int deepth = 0)
        {

            //创建字符串集合缓存
            HashSet<string> cache = new HashSet<string>(strs);
            int startLength = cache.Count;
            //创建叶节点集合
            List<PriorityTreeModel> lists = new List<PriorityTreeModel>();

            //如果当前深度小于集合数量
            if (deepth < models.Count)
            {

                //创建字符映射叶子节点
                Dictionary<string, PriorityTreeModel> StringToModelMapping = new Dictionary<string, PriorityTreeModel>();
                //创建叶节点映射全字符串
                Dictionary<PriorityTreeModel, List<string>> ModelToStringsMapping = new Dictionary<PriorityTreeModel, List<string>>();


                //找到当前层的分割节点
                var model = models[deepth];
                //遍历字符串，起初为传入构造函数的集合
                foreach (var item in strs)
                {

                    //如果分割起点 小于 字符串的最大值
                    if (item.Length > model.StartIndex)
                    {

                        string node;
                        //如果 拾取长度 大等于 字符串的总长度 
                        //则只截取字符串剩余的部分
                        if (item.Length < model.StartIndex + model.Length)
                        {

                            //截取当前剩余字串
                            node = item.Substring(model.StartIndex, item.Length - model.StartIndex);
                            //如果字符串已经被耗尽，则停止对它的解析
                            TripCache[item].Length -= item.Length - model.StartIndex;


                            //创建一个方案节点
                            PriorityTreeModel priority = new PriorityTreeModel
                            {
                                Value = node,
                                FullValue = item,
                                Offset = model.StartIndex,
                                Length = model.Length
                            };


                            //添加到叶节点集合中
                            lists.Add(priority);
                            //创建与该方案相关的字串集合
                            ModelToStringsMapping[priority] = new List<string>();
                            //相同的截取字符串和方案节点添加到缓存
                            StringToModelMapping[node] = priority;


                            if (TripCache[item].Length == 0)
                            {

                                priority.IsEndNode = true;

                            }
                            else
                            {

                                //如果字符串还未耗尽，则继续解析
                                ModelToStringsMapping[priority].Add(item);

                            }

                        }
                        else
                        { 

                            //如果 拾取长度 小于 字符串的总长度 ，说明可以充分截取
                            node = item.Substring(model.StartIndex, model.Length);
                            TripCache[item].Length -= model.Length;


                            //如果缓存中还没有这个截取方案
                            //则生成截取方案节点
                            if (!StringToModelMapping.ContainsKey(node))
                            {

                                //仅仅是匹配类型的节点
                                PriorityTreeModel priority = new PriorityTreeModel
                                {
                                    Value = node,
                                    Offset = model.StartIndex,
                                    Length = model.Length
                                };


                                //添加到叶节点集合中
                                lists.Add(priority);
                                //创建与该方案相关的字串集合
                                ModelToStringsMapping[priority] = new List<string>();
                                //相同的截取字符串和方案节点添加到缓存
                                StringToModelMapping[node] = priority;

                            }


                            if (TripCache[item].Length == 0 && deepth == models.Count - 1)
                            {

                                StringToModelMapping[node].FullValue = item;
                                StringToModelMapping[node].IsZeroNode = true;

                            }
                            else
                            {

                                //如果字符串还未耗尽，则继续解析
                                ModelToStringsMapping[StringToModelMapping[node]].Add(item);

                            }

                            

                        }

                        
                        cache.Remove(item);

                    }
                    else if (item.Length == model.StartIndex)
                    {

                        // abcdef
                        // abcd
                        if (TripCache[item].Length == 0)
                        {

                            //创建一个方案节点
                            PriorityTreeModel priority = new PriorityTreeModel
                            {

                                FullValue = item,
                                IsEndNode = true,
                                IsZeroNode = true

                            };
                            //添加到叶节点集合中
                            lists.Add(priority);
                            cache.Remove(item);

                        }

                        // 1abcdef
                        // 3abcdefg
                        // 4abcdegga
                        // 2abcd
                        // 5abev
                        // 1 -- abcd
                        //   -- abev
                        // 2 -- ef
                        //   -- eg

                    }

                }


                int nextDeepth = deepth + 1;
                foreach (var item in ModelToStringsMapping)
                {

                    item.Key.Next.AddRange(GetTrees(item.Value, models, nextDeepth));

                }

                //如果此次有未参与处理的字串，那么在下一层时将进行处理
                if (cache.Count > 0)
                {

                    PriorityTreeModel priority = new PriorityTreeModel
                    {
                        IsDefaultNode = true
                    };
                    priority.Next.AddRange(GetTrees(cache, models, nextDeepth));
                    lists.Add(priority);

                }

            }

            return lists;
        }




        /// <summary>
        /// 搜集offset到index中间的分割策略
        /// </summary>
        /// <param name="target">目标偏移量</param>
        /// <param name="offset">当前偏移量</param>
        /// <returns></returns>
        private static List<RepeateModel> GetFromSpace(int target, ref int offset)
        {

            var list = new List<RepeateModel>();

            //如果当前偏移量比前一偏移量多4个长度，那么以4个增长点上增
            while (target - offset > 3)
            {

                RepeateModel model = new RepeateModel
                {
                    StartIndex = offset,
                    Length = 4
                };
                offset += 4;
                list.Add(model);

            }


            //如果处理完之后偏移量仍然有剩余
            if (offset != target)
            {

                RepeateModel model2 = new RepeateModel();
                //如果相差3个长度
                if (target - offset == 3)
                {

                    //提前借一位，并取4个长度
                    model2.StartIndex = offset;
                    model2.Length = 4;

                }
                else
                {

                    model2.StartIndex = offset;
                    model2.Length = target - offset;

                }
                //偏移量增加
                list.Add(model2);
                //最后处理完应该offset = target;
                offset = target;

            }

            return list;
        }




        private int GetPriority(List<RepeateModel> models)
        {

            //默认最小权为0
            int priority = 0;

            //遍历分割节点
            foreach (var model in models)
            {

                //匹配字符串缓存
                HashSet<string> sets = new HashSet<string>();
                foreach (var item in TripCache)
                {

                    string temp;
                    //获取当前节点到当前字符串最后一个字符的距离
                    int diff = item.Key.Length - model.StartIndex;
                    if (diff < 0)
                    {
                        //如果分割偏移量已经超过当前字符串的长度
                        //则进行下一个循环
                        continue;

                    }
                    if (diff < model.Length)
                    {

                        //如果差值在分割长度的范围内
                        //获取剩余的分割字符
                        temp = item.Key.Substring(model.StartIndex, diff);

                    }
                    else
                    {

                        //获取当前的分割字符
                        temp = item.Key.Substring(model.StartIndex, model.Length);

                    }

                    //如果分割字符不在缓存里
                    if (!sets.Contains(temp))
                    {
                        //权值+1
                        priority += 1;
                        sets.Add(temp);

                    }

                }

            }

            return priority;

        }



        /// <summary>
        /// 对一段字符串进行高频解析
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="index">字符串之前的偏移量</param>
        /// <param name="offset">当前偏移量</param>
        /// <returns></returns>
        private List<RepeateModel> GetHighFrequency(string str, int index, int offset = 0, MatchOrder order = MatchOrder.None, string other = default)
        {

            List<RepeateModel> result = new List<RepeateModel>();
            RepeateModel model;

            if (str.Length < 3 || str.Length == 4)
            {

                model = GetFrequencyByOffsetAndIndex(str, index, str.Length);
                model.StartIndex += offset;
                result.Add(model);

            }
            else if (str.Length == 3)
            {

                // 3个字符分割成 2 + 1 分别求权值
                int tempPriority1 = 0;
                var model1 = GetFrequencyByOffsetAndIndex(str.Substring(0, 2), index, 2);
                tempPriority1 += model1.MatchCount;
                var model2 = GetFrequencyByOffsetAndIndex(str.Substring(2, 1), index + 2, 1);
                tempPriority1 += model2.MatchCount;


                //3个字符分割成 1 + 2 分别求权值
                int tempPriority2 = 0;
                var model3 = GetFrequencyByOffsetAndIndex(str.Substring(0, 1), index, 1);
                tempPriority2 += model3.MatchCount;
                var model4 = GetFrequencyByOffsetAndIndex(str.Substring(1, 2), index + 1, 2);
                tempPriority2 += model4.MatchCount;


                //3个字符借位求权
                int tempPriority3 = 0;
                if (order == MatchOrder.LeftToRight)
                {
                    model = GetFrequencyByOffsetAndIndex(other, index, 4);
                    tempPriority3 += model.MatchCount;
                }
                else if (order == MatchOrder.RightToLeft)
                {
                    model = GetFrequencyByOffsetAndIndex(other, index, 4);
                    tempPriority3 += model.MatchCount;
                }


                //匹配权比较
                if (tempPriority1 >= tempPriority2)
                {

                    if (tempPriority1 >= tempPriority3)
                    {
                        model1.StartIndex += offset;
                        result.Add(model1);
                        model2.StartIndex += offset + 2;
                        result.Add(model2);
                    }
                    else
                    {
                        model3.StartIndex += offset - 1;
                        result.Add(model3);
                    }

                }
                else
                {

                    if (tempPriority2 >= tempPriority3)
                    {
                        model3.StartIndex += offset;
                        result.Add(model3);
                        model4.StartIndex += offset + 1;
                        result.Add(model4);
                    }
                    else
                    {
                        model3.StartIndex += offset - 1;
                        result.Add(model3);
                    }

                }

            }
            else
            {

                //如果是4个或者4个以上的, 那么找到4个字符为一组的，匹配最多的那组
                model = GetMaxFrequencyModel(str, index);
                

                //如果该组左侧有字符，那么递归处理左侧字符
                if (model.StartIndex > 0)
                {
                    var source = str.Substring(0, model.StartIndex);
                    if (source.Length == 3)
                    {
                        result.AddRange(GetHighFrequency(source, index, offset, MatchOrder.LeftToRight, str.Substring(0, 4)));
                    }
                    else
                    {
                        result.AddRange(GetHighFrequency(source, index, offset));
                    }

                }

                //如果该组右侧有字符，那么递归处理右侧字符
                if (model.StartIndex + 4 < str.Length)
                {

                    int tempOffset = model.StartIndex + 4;
                    var source = str.Substring(model.StartIndex, str.Length - tempOffset);
                    if (source.Length == 3)
                    {
                        result.AddRange(GetHighFrequency(source, index + 4, tempOffset + offset, MatchOrder.RightToLeft, str.Substring(model.StartIndex + 3, 4)));
                    }
                    else
                    {
                        result.AddRange(GetHighFrequency(source, index + 4, tempOffset + offset));
                    }

                }


                model.StartIndex += offset;
                result.Add(model);
            }

            return result;
        }


        private RepeateModel GetFrequencyByOffsetAndIndex(string str, int index, int length)
        {
            //如果是1位，则直接取1位
            var model = new RepeateModel()
            {

                Length = length,
                StartIndex = 0

            };

            foreach (var item in TripCache)
            {

                if (index + length <= item.Key.Length)
                {

                    var matchStr = item.Key.Substring(index, length);
                    if (str == matchStr)
                    {

                        model.MatchCount += 1;

                    }

                }

            }
            return model;
        }


        private RepeateModel GetMaxFrequencyModel(string str, int index)
        {

            int frequency = 0;
            int result = 0;
            for (int i = 0; i < str.Length - 4; i += 1)
            {
                string temp = str.Substring(i, 4);
                int tempFrequency = 0;
                foreach (var item in TripCache)
                {

                    if (item.Key.Length > index + i + 4)
                    {

                        string matchStr = item.Key.Substring(index + i, 4);
                        if (temp == matchStr)
                        {
                            tempFrequency += 1;

                        }

                    }

                }
                if (frequency < tempFrequency)
                {
                    frequency = tempFrequency;
                    result = i;
                }

            }

            return new RepeateModel
            {
                MatchCount = frequency,
                StartIndex = result,
                Length = 4
            };


        }


    }

}
