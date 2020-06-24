using System;
using System.Collections.Generic;
using System.IO;

namespace T1Runtime
{
    public class T1ScriptInterface
    {
        private T1Scope MainScope;

        public T1ScriptInterface()
        {
        }

        public void RunFile(string fileName)
        {
            MainScope = new T1Scope();
            string scriptText = "";
            using (StreamReader sr = new StreamReader(fileName))
            {
                scriptText = sr.ReadToEnd();
            }

            if (!CompileScript(scriptText))
            {
                throw new Exception("Compile not successful");
            }

            RunInstructions();
        }

        private void ParseInstructions()
        {
            
        }

        public void RunInstructions()
        {
            MainScope.Run();
        }

        public void RunScript(string scriptText)
        {
            if(!CompileScript(scriptText))
            {
                throw new Exception("Compile not successful");
            }

            RunInstructions();
        }

        private bool CompileScript(string scriptText)
        {

            return true;
        }
    }
}
