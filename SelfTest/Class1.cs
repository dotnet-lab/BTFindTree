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
    }
}
