using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1Runtime;

namespace T1RuntimeTests
{
    public class TestInstructionJump : T1RuntimeTest
    {
        public string GetName()
        {
            return "InstructionJump";
        }

        public string GetDescription()
        {
            return "Test whether instruction jump runs flawlessly";
        }

        private T1Scope mainScope;

        public void Init()
        {
            mainScope = new T1Scope();

            mainScope.AddInstruction(new T1InstructionLabelSet(0));
            mainScope.AddInstruction(new T1InstructionJump(1));
            mainScope.AddInstruction(new T1InstructionJump(0));
            mainScope.AddInstruction(new T1InstructionLabelSet(1));
        }

        public bool Run()
        {
            Init();

            mainScope.Run();
            return mainScope.InstructionPointer == 4;
        }
    }
}
