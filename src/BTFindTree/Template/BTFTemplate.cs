using System;
using System.Collections.Generic;
using System.Text;

namespace BTFindTree
{
    public class BTFTemplate
    {

        public static string GetHashBTFScript<T>(IDictionary<T,string> pairs)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append("switch(btf.GetHasCode()){");
            foreach (var item in pairs)
            {
                scriptBuilder.AppendLine($"case {item.Key.GetHashCode()}:");
                scriptBuilder.AppendLine(item.Value);
                if (!item.Value.Contains("return "))
                {
                    scriptBuilder.AppendLine("break;");
                }
                
            }
            scriptBuilder.AppendLine("default:return default;");
            scriptBuilder.Append("}");
            return scriptBuilder.ToString();
        }




        public static string GetPointBTFScript(IDictionary<string,string> parirs)
        {

            PointBTFindTree tree = new PointBTFindTree(parirs);

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($@"fixed (char* c = name){{");
            var body = ForeachPointTree(tree);
            body.Length -= 6;
            scriptBuilder.Append(body);
            scriptBuilder.Append("} return default;");
            return scriptBuilder.ToString();

        }




        public static StringBuilder ForeachPointTree(PointBTFindTree tree)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            
            
            
            if (tree.Value!=default)
            {
                scriptBuilder.AppendLine($"case {tree.PointCode}:");
                scriptBuilder.AppendLine(tree.Value);
                if (!tree.Value.Contains("return "))
                {
                    scriptBuilder.AppendLine("break;");
                }

            }
            else
            {

                var node = tree;
                while (node.Nodes.Count == 1)
                {
                    node = node.Nodes[0];
                    tree.Layer += 1;
                }


                if (node != tree)
                {
                    tree.Nodes = node.Nodes;
                }


                scriptBuilder.Append($"switch (*({PointBTFindTree.OfferType}*)(c+{PointBTFindTree.OfferSet * tree.Layer})){{");
                foreach (var item in tree.Nodes)
                {
                    if (item.Value==default)
                    {
                        scriptBuilder.AppendLine($"case {item.PointCode}:");
                    }
                    scriptBuilder.Append(ForeachPointTree(item));
                }
                scriptBuilder.Append('}');
                scriptBuilder.Append("break;");

            }
            return scriptBuilder;

        }
    }
}
