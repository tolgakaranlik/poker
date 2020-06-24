using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1RuntimeTests
{
    public interface T1RuntimeTest
    {
        string GetName();
        string GetDescription();
        bool Run();
    }
}
