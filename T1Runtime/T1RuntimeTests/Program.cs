using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1RuntimeTests
{
    class Program
    {
        static void Main(string[] args)
        {
            List<T1RuntimeTest> testList = new List<T1RuntimeTest>();

            testList.Add(new TestInstructionJump());
            testList.Add(new TestVariableDeclaration());
            testList.Add(new TestEvaluateExpression());
            testList.Add(new TestStringConcatanation());
            testList.Add(new TestFunctionDeclaration());
            testList.Add(new TestConditionalJump());
            testList.Add(new TestParseScript());
            // prebuilt functions
            // parse script from text

            int success = 0;
            int fail = 0;
            for (int i = 0; i < testList.Count; i++)
            {
                Console.WriteLine("Current test: " + testList[i].GetName());
                Console.WriteLine(testList[i].GetDescription());

                if(testList[i].Run())
                {
                    Console.WriteLine("SUCCEEDED :)\n");
                    success++;
                } else
                {
                    Console.WriteLine("FAILED X(\n");
                    fail++;
                }
            }

            Console.WriteLine("\n All Done\n=======================================\n Succeeded: "+ success+ "\n Failed: "+ fail);
            Console.ReadKey();
        }
    }
}
