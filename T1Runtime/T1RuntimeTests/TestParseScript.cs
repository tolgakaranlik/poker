using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1Runtime;

namespace T1RuntimeTests
{
    public class TestParseScript : T1RuntimeTest
    {
        public string GetDescription()
        {
            return "Tests whether T1 scripts can be parsed with success";
        }

        public string GetName()
        {
            return "ParseScript";
        }

        private T1Scope mainScope;

        private void Init()
        {
            mainScope = new T1Scope();
        }

        public bool Run()
        {
            Init();

            try
            {
                mainScope.LoadScript("script1.t1");
            } catch //(Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
