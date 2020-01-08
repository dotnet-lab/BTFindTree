using System.Collections.Generic;

namespace BTFindTree.Template.PrecisionTreeTemplate.Model
{

    public class FrequencyModel
    {

        // str: abcd
        public string Value;
        public int Length;
        /*   
         *    char         :   a - b - c - d
         *    index        :   0 - 1 - 2 - 3
         *    Frequency    :   1 - 1 - 3 - 4
         */
        public int[] RepeateCache;


        public FrequencyModel(string value)
        {

            Value = value;
            Length = value.Length;
            //字符串的每个字符进行缓存
            RepeateCache = new int[value.Length];

        }


        public int this[int index]
        {

            get { return RepeateCache[index]; }
            set { RepeateCache[index] = value; }

        }

        


        /// <summary>
        /// 获取匹配次数的连续字符串中间段
        /// </summary>
        /// <param name="frequency">匹配频次</param>
        /// <returns></returns>
        public List<RepeateModel> GetByFrequency(int frequency)
        {

            var models = new List<RepeateModel>();
            var node = new RepeateModel();
            bool isFirst = true;
            for (int i = 0; i < RepeateCache.Length; i++)
            {

                //如果当前频次大于等于指定频次
                if (RepeateCache[i] >= frequency)
                {

                    node.Length += 1;
                    if (isFirst)
                    {
                        //如果第一次，则设置索引
                        node.StartIndex = i;
                        //标识重置
                        isFirst = false;
                    }

                }
                else
                {

                    //如果出现了中断
                    //如果之前正在连续计数
                    if (!isFirst)
                    {

                        models.Add(node);
                        node = new RepeateModel();
                        //标识设置为连续字符的开始
                        isFirst = true;

                    }

                }
            }

            return models;
        }

    }
}
