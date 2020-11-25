using BTFindTree.Template.PrecisionTreeTemplate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTFindTree
{
    public class BTFTemplate
    {

        public static string GetCustomerBTFScript<T>(IDictionary<T, string> pairs, string switchCode, Func<T, string> caseCode = null)
        {

            if (pairs == default || pairs.Count == 0)
            {
                return default;
            }


            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append($"switch({switchCode}){{");


            foreach (var item in pairs)
            {

                scriptBuilder.AppendLine($"case {caseCode?.Invoke(item.Key)}:");
                scriptBuilder.AppendLine(item.Value);
                //if (!item.Value.Contains("return "))
                //{
                //    scriptBuilder.AppendLine("break;");
                //}

            }


            scriptBuilder.Append("}");
            return scriptBuilder.ToString();

        }




        public static string GetHashBTFScript<T>(IDictionary<T, string> pairs, string parameterName = "arg")
        {

            if (pairs == default || pairs.Count == 0)
            {
                return default;
            }


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



        public static string GetGroupFuzzyPointBTFScript(IDictionary<string, string> pairs, string parameterName = "arg")
        {

            if (pairs == default || pairs.Count == 0)
            {
                return default;
            }

            Dictionary<int, Dictionary<string, string>> groups = new Dictionary<int, Dictionary<string, string>>();
            foreach (var item in pairs)
            {
                int length = item.Key.Length;
                if (!groups.ContainsKey(length))
                {
                    groups[length] = new Dictionary<string, string>();
                }
                groups[length][item.Key] = item.Value;

            }

            StringBuilder scriptBuilder = new StringBuilder();
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (var item in groups)
            {

                result[item.Key] = GetFuzzyPointBTFScript(item.Value, parameterName) + "break;";

            }
            scriptBuilder.AppendLine(GetCustomerBTFScript(result, "btfParameterLength", item => item.ToString()));


            scriptBuilder.Insert(0, $"int btfParameterLength = {parameterName}.Length;");
            return scriptBuilder.ToString();

        }



        public static string GetFuzzyPointBTFScript(IDictionary<string, string> pairs, string parameterName = "arg")
        {

            if (pairs == default || pairs.Count == 0)
            {
                return default;
            }

            StringBuilder scriptBuilder = new StringBuilder();
            if (pairs.Count == 1)
            {
                foreach (var item in pairs)
                {
                    scriptBuilder.AppendLine($"if({parameterName}[0] == '{item.Key[0]}'){{ {item.Value} }}");
                }
                
            }
            else
            {
                scriptBuilder.AppendLine($"fixed (char* c =  {parameterName}){{");


                FuzzyPointTree tree = new FuzzyPointTree(pairs);
                scriptBuilder.Append(ForeachFuzzyTree(tree));


                scriptBuilder.Append("}");
            }

           
            
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


                //value==default 不为空则 Nodes 定有值
                //如果是单节点，则直接优化掉，节点层数+1
                while (node.Nodes != default && node.Nodes.Count == 1)
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
                if (tree.Nodes == default)
                {

                    scriptBuilder.Append(ForeachFuzzyTree(node));

                }
                else
                {

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

                }

                scriptBuilder.Append('}');

            }
            return scriptBuilder;

        }




        public static string GetGroupPrecisionPointBTFScript(IDictionary<string, string> pairs, string parameterName = "arg")
        {

            if (pairs == default || pairs.Count == 0)
            {
                return default;
            }

            Dictionary<int, Dictionary<string, string>> groups = new Dictionary<int, Dictionary<string, string>>();
            foreach (var item in pairs)
            {
                int temp = item.Key.Length;
                if (!groups.ContainsKey(temp))
                {
                    groups[temp] = new Dictionary<string, string>();
                }
                groups[temp][item.Key] = item.Value;

            }

            StringBuilder scriptBuilder = new StringBuilder();
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (var item in groups)
            {

                result[item.Key] = GetPrecisionPointBTFScript(item.Value, parameterName) + "break;";

            }
            scriptBuilder.AppendLine(GetCustomerBTFScript(result, "btfParameterLength", item => item.ToString()));


            scriptBuilder.Insert(0,$"int btfParameterLength = {parameterName}.Length;");
            return scriptBuilder.ToString();

        }




        public static string GetPrecisionPointBTFScript(IDictionary<string, string> pairs, string parameterName = "arg")
        {

            if (pairs == default || pairs.Count == 0)
            {
                return default;
            }


            StringBuilder scriptBuilder = new StringBuilder();
            if (pairs.Count == 1)
            {
                foreach (var item in pairs)
                {
                    scriptBuilder.AppendLine($"if({parameterName} == \"{item.Key}\"){{ {item.Value} }}");
                }

            }
            else
            {
                scriptBuilder.AppendLine($"fixed (char* c =  {parameterName}){{");


                PrecisionMinPriorityTree tree = new PrecisionMinPriorityTree(pairs.Keys.ToArray());
                scriptBuilder.AppendLine(ForeachPrecisionTree(tree.GetPriorityTrees(), pairs));


                scriptBuilder.AppendLine("}");
            }
            
           
            return scriptBuilder.ToString();

        }




        /*            n1                                   n2
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
        private static string ForeachPrecisionTree(List<PriorityTreeModel> nodes, IDictionary<string, string> parirs)
        {


            StringBuilder switchBuilder = new StringBuilder();
            StringBuilder caseBuilder = new StringBuilder();
            StringBuilder defaultBuilder = new StringBuilder();

            for (int i = 0; i < nodes.Count; i += 1)
            {

                var node = nodes[i];
                if (node.IsDefaultNode)
                {

                    defaultBuilder.AppendLine($"default:");
                    if (node.Next.Count == 1 && node.Next[0].IsEndNode)
                    {

                        var returnStr = parirs[node.FullValue];
                        defaultBuilder.AppendLine($"{returnStr}");
                        if (!returnStr.Contains("return "))
                        {

                            defaultBuilder.AppendLine("break;");

                        }

                    }
                    else
                    {

                        defaultBuilder.AppendLine(ForeachPrecisionTree(node.Next, parirs));
                        defaultBuilder.AppendLine("break;");

                    }

                }
                else if (node.IsEndNode)
                {

                    if (node.IsZeroNode)
                    {

                        caseBuilder.AppendLine($"case 0:");
                        var returnStr = parirs[node.FullValue];
                        caseBuilder.AppendLine($"{returnStr}");
                        if (!returnStr.Contains("return "))
                        {
                            caseBuilder.AppendLine("break;");
                        }

                    }
                    else
                    {

                        var (compareBuilder, code) = node.Value.GetCompareBuilder(node.Length, node.Offset);
                        if (switchBuilder.Length == 0)
                        {
                            switchBuilder.AppendLine($"switch({compareBuilder}){{");
                        }

                        caseBuilder.AppendLine($"case {code}:");
                        var returnStr = parirs[node.FullValue];
                        caseBuilder.AppendLine($"{returnStr}");
                        if (!returnStr.Contains("return "))
                        {
                            caseBuilder.AppendLine("break;");
                        }

                    }

                }
                else
                {

                    var (compareBuilder, code) = node.Value.GetCompareBuilder(node.Length, node.Offset);
                    if (switchBuilder.Length == 0)
                    {
                        switchBuilder.AppendLine($"switch({compareBuilder}){{");
                    }


                    if (node.IsZeroNode)
                    {

                        caseBuilder.AppendLine($"case {code}:");
                        var returnStr = parirs[node.FullValue];
                        caseBuilder.AppendLine($"{returnStr}");
                        if (!returnStr.Contains("return "))
                        {
                            caseBuilder.AppendLine("break;");
                        }

                    }
                    else if (node.Next.Count > 0)
                    {

                        caseBuilder.AppendLine($"case {code}:");
                        var tempNode = node.Next[0];
                        if (node.Next.Count == 1 && tempNode.IsEndNode && tempNode.IsZeroNode)
                        {

                            var returnStr = parirs[tempNode.FullValue];
                            caseBuilder.AppendLine($"{returnStr}");
                            if (!returnStr.Contains("return "))
                            {
                                caseBuilder.AppendLine("break;");
                            }

                        }
                        else
                        {

                            caseBuilder.AppendLine(ForeachPrecisionTree(node.Next, parirs));
                            caseBuilder.AppendLine("break;");

                        }

                    }

                }

            }

            var result = new StringBuilder();
            result.Append(switchBuilder);
            result.Append(caseBuilder);
            result.Append(defaultBuilder);
            if (switchBuilder.Length > 0)
            {
                result.Append('}');
            }
            return result.ToString();

        }

    }

}
