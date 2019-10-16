using System;
using System.Collections.Generic;
using System.Text;

namespace BTFindTree.Template.PrecisionTreeTemplate.Model
{

    public class FrequencyModel
    {

        // str: abcd
        public string Value;

        /*   
         *    char               :   a - b - c - d
         *    index              :   0 - 1 - 2 - 3
         *    Frequency    :   1 - 1 - 3 - 4
         */
        public List<int> RepeateCache;


        public FrequencyModel(string value)
        {

            Value = value;
            RepeateCache = new List<int>(value.Length) { };

            for (int i = 0; i < value.Length; i += 1)
            {

                RepeateCache.Add(1);

            }

        }


        public int this[int index]
        {

            get { return RepeateCache[index]; }
            set { RepeateCache[index] = value; }

        }


        public List<RepeateModel> GetByMatchCount(int matchCount)
        {

            var models = new List<RepeateModel>();
            var node = new RepeateModel();
            bool isFirst = true;
            for (int i = 0; i < RepeateCache.Count; i++)
            {

                if (RepeateCache[i] >= matchCount)
                {

                    node.Length += 1;
                    if (isFirst)
                    {
                        node.StartIndex = i;
                        isFirst = false;
                    }

                }
                else
                {

                    if (!isFirst)
                    {
                        models.Add(node);
                        node = new RepeateModel();
                        isFirst = true;
                    }

                }
            }
            if (models.Count == 0)
            {
                models.Add(node);
            }
            return models;
        }

    }
}
