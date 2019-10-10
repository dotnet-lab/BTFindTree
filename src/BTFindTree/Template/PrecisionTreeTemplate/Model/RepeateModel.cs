using System;
using System.Collections.Generic;
using System.Text;

namespace BTFindTree.Template.PrecisionTreeTemplate.Model
{


    public struct RepeateModel
    {
        public RepeateModel(int startIndex = 0)
        {
            StartIndex = startIndex;
            Length = 1;
            MatchCount = 0;
        }
        public int StartIndex;
        public int Length;
        public int MatchCount;
    }




    public enum MatchOrder
    {
        LeftToRight,
        RightToLeft
    }
    
}
