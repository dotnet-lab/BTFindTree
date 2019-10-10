using System.Text;

namespace BTFindTree
{
    public static class StringExtension
    {

        public unsafe static ulong GetULong(this string value,int index = 0)
        {
            fixed (char* c = value)
            {
                return *(ulong*)(c+index*4);
            }
        }




        public unsafe static uint GetUInt(this string value, int index = 0)
        {
            fixed (char* c = value)
            {
                return *(uint*)(c + index * 2);
            }
        }



        public unsafe static ushort GetUShort(this string value, int index = 0)
        {
            fixed (char* c = value)
            {
                return *(ushort*)(c + index);
            }
        }


        public unsafe static (StringBuilder compareBuilder, ulong code) GetCompareBuilder(this string value, int length, int index)
        {
           
            ulong code;
            string type;
            switch (length)
            {
                case 1:
                    code = GetUShort(value);
                    type = "ushort";
                    break;
                case 2:
                    code = GetUInt(value);
                    type = "uint";
                    break;
                default:
                    code = GetULong(value);
                    type = "ulong";
                    break;
            }


            StringBuilder builder = new StringBuilder();
            builder.Append($"*({ type}*)(c + {index}) ");
            return (builder, code) ;

        }

    }
}
