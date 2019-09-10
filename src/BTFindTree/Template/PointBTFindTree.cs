using System.Collections.Generic;
using System.Linq;

namespace BTFindTree
{


#if AMD64

        public class PointBTFindTree
    {
        public List<PointBTFindTree> Nodes;
        public readonly string Value;
        public readonly long PointCode;


        public const int OfferSet = 4;
        public const string OfferType = "long";

        public PointBTFindTree(KeyValuePair<string, string> value, int layer = 0)
        {

            Value = value.Value;
            PointCode = value.Key.GetInt(layer);

        }




        public PointBTFindTree(IDictionary<string, string> values, int layer = 0, long pCode = 0)
        {
            if (values.Count == 1)
            {

                foreach (var value in values)
                {
                    Value = value.Value;
                    PointCode = pCode;
                }

            }
            else
            {

                PointCode = pCode;
                Nodes = new List<PointBTFindTree>();
                var valuesDict = new Dictionary<long, Dictionary<string, string>>();


                foreach (var item in values)
                {

                    if (item.Key.Length < layer * OfferSet)
                    {

                        Nodes.Add(new PointBTFindTree(item, layer));

                    }
                    else
                    {

                        int pcode = item.Key.GetLong(layer);
                        if (!valuesDict.ContainsKey(pcode))
                        {
                            valuesDict = new Dictionary<int, Dictionary<string, string>>();
                        }
                        valuesDict[pcode][item.Key] = item.Value;

                    }

                }


                foreach (var item in valuesDict)
                {

                    Nodes.Add(new PointBTFindTree(item.Value, layer + 1, item.Key));

                }

            }

        }

    }
#else
    public class PointBTFindTree
    {
        public List<PointBTFindTree> Nodes;
        public readonly string Value;
        public readonly long PointCode;
        public int Layer;


        public const int OfferSet = 1;
        public const string OfferType = "short";

        public PointBTFindTree(KeyValuePair<string, string> value, int layer = 0)
        {

            Value = value.Value;
            PointCode = value.Key.GetShort(layer);
            Layer = layer;

        }




        public PointBTFindTree(IDictionary<string, string> values, int layer = 0, long pCode = 0)
        {

            Layer = layer;
            if (values.Count == 1)
            {

                foreach (var value in values)
                {
                    Value = value.Value;
                    PointCode = pCode;
                }

            }
            else
            {

                PointCode = pCode;
                Nodes = new List<PointBTFindTree>();
                var valuesDict = new Dictionary<int, Dictionary<string, string>>();


                foreach (var item in values)
                {

                    if (item.Key.Length < layer * OfferSet)
                    {

                        Nodes.Add(new PointBTFindTree(item, layer));

                    }
                    else
                    {

                        int pcode = item.Key.GetShort(layer);
                        if (!valuesDict.ContainsKey(pcode))
                        {
                            valuesDict[pcode] = new Dictionary<string, string>();

                        }
                        valuesDict[pcode][item.Key] = item.Value;

                    }

                }


                foreach (var item in valuesDict)
                {

                    Nodes.Add(new PointBTFindTree(item.Value, layer + 1, item.Key));

                }

            }

        }

    }

#endif

}
