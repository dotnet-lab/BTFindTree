using System;
using System.Collections.Generic;
using System.Text;

namespace BTFindTree.Template
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
            scriptBuilder.Append($"switch({tree.PointCode}){{");
            scriptBuilder.Append(ForeachPointTree(tree));
            scriptBuilder.Append('}');
            return scriptBuilder.ToString();

        }




        public static StringBuilder ForeachPointTree(PointBTFindTree tree)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"case {tree.PointCode}:");
            if (tree.Value!=default)
            {

                scriptBuilder.AppendLine(tree.Value);
                if (!tree.Value.Contains("return "))
                {
                    scriptBuilder.AppendLine("break;");
                }

            }
            else
            {

                foreach (var item in tree.Nodes)
                {
                    scriptBuilder.Append(ForeachPointTree(item));
                }

            }
            scriptBuilder.AppendLine("break;");
            return scriptBuilder;

        }
    }
}
