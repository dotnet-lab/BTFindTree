using BTFindTree.Template.PrecisionTreeTemplate.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTFindTree
{
    public class BTFTemplate
    {

        public static string GetHashBTFScript<T>(IDictionary<T, string> pairs,string parameterName = "arg")
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append($"switch({parameterName}.GetHashCode()){{");


            foreach (var item in pairs)
            {

                scriptBuilder.AppendLine($"case {item.Key.GetHashCode()}:");
                scriptBuilder.AppendLine(item.Value);
                if (!item.Value.Contains("return "))
                {
                    scriptBuilder.AppendLine("break;");
                }

            }


            scriptBuilder.Append("}");
            return scriptBuilder.ToString();

        }




        public static string GetFuzzyPointBTFScript(IDictionary<string, string> parirs, string parameterName = "arg")
        {

            if (parirs == default)
            {
                return default;
            }


            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"fixed (char* c =  {parameterName}){{");


            FuzzyPointTree tree = new FuzzyPointTree(parirs);
            scriptBuilder.Append(ForeachFuzzyTree(tree));


            scriptBuilder.Append("}");
            return scriptBuilder.ToString();

        }




        private static StringBuilder ForeachFuzzyTree(FuzzyPointTree tree)
        {

            StringBuilder scriptBuilder = new StringBuilder();



            if (tree.Value != default)
            {

                //如果是叶节点
                scriptBuilder.AppendLine($"case {tree.PointCode}:");
                scriptBuilder.AppendLine(tree.Value);
                if (!tree.Value.Contains("return "))
                {
                    scriptBuilder.AppendLine("break;");
                }

            }
            else
            {

                //设置头节点
                var node = tree;


                //value==default不为空则Nodes定有值
                //如果是单节点，则直接优化掉，节点层数+1
                while (node.Nodes.Count == 1)
                {
                    node = node.Nodes[0];
                    tree.Layer += 1;
                }


                //如果头节点移动了（允许优化）
                if (node != tree)
                {
                    //则头节点重新赋值
                    tree.Nodes = node.Nodes;
                }


                //一个集合必然已switch开头
                scriptBuilder.Append($"switch (*({FuzzyPointTree.OfferType}*)(c+{FuzzyPointTree.OfferSet * tree.Layer})){{");
                //此时Nodes一定不是单节点，而是具有兄弟的节点
                foreach (var item in tree.Nodes)
                {

                    //如果当前子节点不是叶节点
                    if (item.Value == default)
                    {

                        //证明当前节点分支一定还需要再判断
                        scriptBuilder.AppendLine($"case {item.PointCode}:");
                        scriptBuilder.Append(ForeachFuzzyTree(item));
                        scriptBuilder.Append("break;");

                    }
                    else
                    {

                        //叶节点再次交给递归处理
                        scriptBuilder.Append(ForeachFuzzyTree(item));

                    }

                }
                scriptBuilder.Append('}');

            }
            return scriptBuilder;

        }




        public static string GetPrecisionPointBTFScript(IDictionary<string, string> parirs, string parameterName = "arg")
        {

            if (parirs == default)
            {
                return default;
            }

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"fixed (char* c =  {parameterName}){{");

            PrecisionMinPriorityTree tree = new PrecisionMinPriorityTree(parirs.Keys.ToArray());
            scriptBuilder.AppendLine(ForeachPrecisionTree(tree.GetPriorityTrees(), parirs));

            scriptBuilder.AppendLine("}");
            return scriptBuilder.ToString();
        }


        private static string ForeachPrecisionTree(List<PriorityTreeModel> nodes, IDictionary<string, string> parirs, int deepth = 0)
        {

            if (nodes == default)
            {
                return default;
            }

            StringBuilder builder = new StringBuilder();
            StringBuilder append = new StringBuilder();
            int deep = deepth + 1;
            if (nodes.Count == 1)
            {

                var node = nodes[0];
                if (node.Value != default)
                {

                    var (compareBuilder, code) = node.Value.GetCompareBuilder(node.Length, node.Offset);
                    builder.AppendLine($"if({compareBuilder} == {code}){{");
                    if (node.Next == default || node.Next.Count == 0)
                    {

                        builder.AppendLine($"{parirs[node.FullValue]}");

                    }
                    else
                    {

                        builder.AppendLine(ForeachPrecisionTree(node.Next, parirs, deep));

                    }
                    builder.Append('}');

                }


            }
            else
            {

                bool hashAppendSwitch = false;


                foreach (var item in nodes)
                {
                    if (item.Value == default)
                    {

                        append.AppendLine($"case 0:");
                        var returnStr = parirs[item.FullValue];
                        append.AppendLine($"{returnStr}");
                        if (!returnStr.Contains("return "))
                        {
                            append.AppendLine("break;");
                        }

                    }
                    else
                    {

                        (StringBuilder compareBuilder, ulong code) = item.Value.GetCompareBuilder(item.Length,item.Offset);
                        if (!hashAppendSwitch)
                        {
                            builder.AppendLine($"switch({compareBuilder}){{");
                            hashAppendSwitch = true;
                        }


                        builder.AppendLine($"case {code}:");
                        if (item.Next == default || item.Next.Count == 0)
                        {

                            var returnStr = parirs[item.FullValue];
                            builder.AppendLine($"{returnStr}");
                            if (!returnStr.Contains("return "))
                            {
                                builder.AppendLine("break;");
                            }

                        }
                        else
                        {

                            if (item.Next.Count == 1 && item.Next[0].Value==default)
                            {
                                builder.AppendLine($"{parirs[item.Next[0].FullValue]}");
                            }
                            else
                            {
                                builder.AppendLine(ForeachPrecisionTree(item.Next, parirs, deep));
                                builder.AppendLine("break;");
                            }

                        }
                        
                    }


                }
                builder.Append(append);
                builder.Append('}');
            }
            return builder.ToString();

        }

    }

}
