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
                switch (*(short*)(c + 2))
                {
                    case 97:
                        switch (*(short*)(c + 4))
                        {
                            case 49:
                                switch (*(short*)(c + 8))
                                {
                                    case 0:
                                        return 0;
                                    case 99:
                                        return 1;
                                    case 100:
                                        return 2;
                                }
                                break;
                            case 50:
                                switch (*(short*)(c + 9))
                                {
                                    case 0:
                                        return 3;
                                    case 114:
                                        var a = 1 + 1;
                                        break;
                                }
                                break;
                            case 51:
                                return 4;
                        }
                        break;
                    case 99:
                        switch (*(short*)(c + 6))
                        {
                            case 102:
                                return 5;
                            case 103:
                                return 6;
                            case 105:
                                return 7;
                            case 104:
                                return 8;
                            case 106:
                                return 9;
                        }
                        break;
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



        public unsafe int C(string name)
        {
            fixed (char* c = name)
            {
                switch (*(ulong*)(c + 0))
                {
                    case 27584964335894625:
                        if (*(ulong*)(c + 4) == 14355434268917810)
                        {
                            if (*(ushort*)(c + 8) == 101)
                            {
                                switch (*(ushort*)(c + 9))
                                {
                                    case 114:
                                        var a = 1 + 1;
                                        break;
                                    case 0:
                                        return 3;
                                }
                            }
                        }
                        break;
                    case 28147922879250529:
                        if (*(ulong*)(c + 4) == 442388316261)
                        {
                            return 6;

                        }
                        break;
                }
            }
            return default;
        }
    }
}
