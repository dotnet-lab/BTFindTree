using System.Collections.Generic;
using System.Text;

namespace BTFindTree
{
    public class BTFTemplate
    {

        public static string GetHashBTFScript<T>(IDictionary<T,string> pairs)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append("switch(btf.GetHashCode()){");


            foreach (var item in pairs)
            {

                scriptBuilder.AppendLine($"case {item.Key.GetHashCode()}:");
                scriptBuilder.AppendLine(item.Value);
                if (!item.Value.Contains("return "))
                {
                    scriptBuilder.AppendLine("break;");
                }
                
            }


            scriptBuilder.Append("}return default;");
            return scriptBuilder.ToString();

        }




        public static string GetPointBTFScript(IDictionary<string,string> parirs)
        {

            if (parirs == default)
            {
                return default;
            }


            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($@"fixed (char* c = name){{");


            PointBTFindTree tree = new PointBTFindTree(parirs);
            scriptBuilder.Append(ForeachPointTree(tree));


            scriptBuilder.Append("} return default;");
            return scriptBuilder.ToString();

        }




        public static StringBuilder ForeachPointTree(PointBTFindTree tree)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            
            
            
            if (tree.Value!=default)
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
                scriptBuilder.Append($"switch (*({PointBTFindTree.OfferType}*)(c+{PointBTFindTree.OfferSet * tree.Layer})){{");
                //此时Nodes一定不是单节点，而是具有兄弟的节点
                foreach (var item in tree.Nodes)
                {

                    //如果当前子节点不是叶节点
                    if (item.Value==default)
                    {

                        //证明当前节点分支一定还需要再判断
                        scriptBuilder.AppendLine($"case {item.PointCode}:");
                        scriptBuilder.Append(ForeachPointTree(item));
                        scriptBuilder.Append("break;");

                    }
                    else
                    {

                        //叶节点再次交给递归处理
                        scriptBuilder.Append(ForeachPointTree(item));

                    }
                   
                }
                scriptBuilder.Append('}');                

            }
            return scriptBuilder;

        }

    }

}
