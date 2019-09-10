using System;
using System.Collections.Generic;
using System.Text;

namespace SelfTest
{
    class Class1
    {
        public unsafe int A(string name)
        {
            fixed (char* c = name)
            {
                switch (*(int*)(c + 2))
                {
                    case 6422625:
                        switch (*(int*)(c + 4))
                        {
                            case 3211313:
                                switch (*(int*)(c + 8))
                                {
                                    case 99:
                                        return 1;
                                    case 100:
                                        return 2;
                                }
                                break;
                            case 3276850:
                                switch (*(int*)(c + 8))
                                {
                                    case 101:
                                        return 3;
                                    case 7471205:
                                        var a = 1 + 1;
                                        break;
                                }
                                break;
                            case 3276851:
                                return 4;
                        }
                        break;
                    case 6553699:
                        return 5;
                }
            }
            return default;

        }


        public int B(string name)
        {
            switch (name.GetHashCode())
            {
                case -397903024:
                    return 1;
                case 259107356:
                    return 2;
                case -810005667:
                    return 3;
                case 1043528272:
                    var a = 1 + 1;
                    break;
                case 1436709466:
                    return 4;
                case 1834649015:
                    return 5;
                case -1327692258:
                    return 6;
                case -1655823507:
                    return 7;
                case 1432890083:
                    return 8;
                case -292139003:
                    return 9;
            }
            return default;

        }
    }
}
