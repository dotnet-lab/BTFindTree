using System;
using System.Collections.Generic;
using System.Text;

namespace BTFindTree.Template.PrecisionTreeTemplate.Model
{

    public class PriorityTreeModel
    {

        public List<PriorityTreeModel> Next;
        public PriorityTreeModel()
        {
            Next = new List<PriorityTreeModel>();
        }


        public string Value;
        public string FullValue;
        public int Length;
        public int Offset;
        public bool IsDefaultNode;
        public bool IsZeroNode;

    }

}
