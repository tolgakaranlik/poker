using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace T1Runtime
{
    [Serializable]
    public class T1RuntimeVairableReference
    {
        private T1Scope scope;
        private int variableId;
        private T1VariableType variableType;

        public T1Scope Scope
        {
            get
            {
                return scope;
            }
        }
        public int VariableId
        {
            get
            {
                return variableId;
            }
        }
        public T1VariableType VariableType
        {
            get
            {
                return variableType;
            }
        }

        public T1RuntimeVairableReference(T1Scope scope, int variableId, T1VariableType variableType)
        {
            this.scope = scope;
            this.variableId = variableId;
            this.variableType = variableType;
        }
    }

    [Serializable]
    public class T1Variable
    {
        private T1VariableType variableType;
        private object value;

        public T1VariableType VariableType
        {
            get
            {
                return variableType;
            }
        }
        public object Value
        {
            get
            {
                return value;
            }
        }

        public T1Variable(T1VariableType variableType)
        {
            this.variableType = variableType;
            value = null;
        }

        public T1Variable(T1VariableType variableType, object value)
        {
            this.variableType = variableType;
            this.value = value;
        }
    }

    [Serializable]
    public class T1TableItem
    {
        private T1Scope scope;
        private string name;
        private int index;

        public T1Scope ScopeId
        {
            get
            {
                return scope;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public int Index
        {
            get
            {
                return index;
            }
        }

        public T1TableItem(string name, int index, T1Scope scope)
        {
            this.name = name;
            this.index = index;
            this.scope = scope;
        }
    }

    [Serializable]
    public class T1Scope : ICloneable
    {
        private T1Scope ParentScope;
        private List<T1Scope> subScopes;

        private List<T1Instruction> Instructions;
        private List<int> LabelTable;
        private List<T1Variable> variableTable;
        private int instructionPointer;
        private object ExpressionResult;
        private T1VariableType ExpressionType;
        private List<object> ExpressionStack;
        private List<string> Tokens = null;
        private int CurrentToken = 0;
        private int CurrentLine = 0;

        List<T1TableItem> VariableTableItems;
        List<T1TableItem> FunctionTableItems;

        public int InstructionPointer
        {
            get
            {
                return instructionPointer;
            }
        }

        public List<T1Variable> VariableTable
        {
            get
            {
                return variableTable;
            }
        }

        public List<T1Scope> SubScopes
        {
            get
            {
                return subScopes;
            }
        }

        public T1Scope()
        {
            Instructions = new List<T1Instruction>();
            subScopes = new List<T1Scope>();
            LabelTable = new List<int>();
            variableTable = new List<T1Variable>();
            ExpressionStack = new List<object>();

            ParentScope = null;
        }

        public T1Scope(T1Scope parentScope)
        {
            Instructions = new List<T1Instruction>();
            subScopes = new List<T1Scope>();
            LabelTable = new List<int>();
            variableTable = new List<T1Variable>();
            ExpressionStack = new List<object>();

            ParentScope = parentScope;
        }

        public object Clone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(ms, this);
            ms.Position = 0;
            object obj = bf.Deserialize(ms);
            ms.Close();

            return obj;
        }

        public void ParseScript(string scriptText)
        {
            Tokens = new List<string>();

            string[] lines = scriptText.Split('\n');
            string[] statements;
            string[] subStatements;
            string line;
            string statementAlphabet = "_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string c;
            bool found;

            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i].Replace("\r","").Replace("\t", "");
                string token;
                if(!line.Contains(";"))
                {
                    statements = new string[] { line };
                } else
                {
                    statements = line.Split(';');
                }

                for (int j = 0; j < statements.Length; j++)
                {
                    if (!statements[j].Contains(" "))
                    {
                        subStatements = new string[] { statements[j] };
                    }
                    else
                    {
                        subStatements = statements[j].Split(' ');
                    }

                    //tokens.Add(statements[j]);
                    for (int k = 0; k < subStatements.Length; k++)
                    {
                        found = true;
                        while (!String.IsNullOrEmpty(subStatements[k]) && found)
                        {
                            found = false;

                            for (int m = 0; m < subStatements[k].Length; m++)
                            {
                                c = subStatements[k].Substring(m, 1);
                                if (!statementAlphabet.Contains(c))
                                {
                                    if (m > 0)
                                    {
                                        token = subStatements[k].Substring(0, m);
                                        Tokens.Add(token);
                                    }

                                    Tokens.Add(subStatements[k].Substring(m, 1));

                                    subStatements[k] = subStatements[k].Substring(m + 1);
                                    found = true;
                                    break;
                                }
                            }
                        }

                        if(!String.IsNullOrEmpty(subStatements[k]))
                        {
                            Tokens.Add(subStatements[k]);
                        }
                    }

                    Tokens.Add(";");
                }

                Tokens.Add("\n");
            }
        }

        private bool TableContains(List<T1TableItem> list, string element)
        {
            element = element.Trim();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == element)
                {
                    return true;
                }
            }

            return false;
        }

        private void CompileScript()
        {
            List<string> primitiveTypes = new List<string>() { "void", "int", "string", "byte", "double" };
            List<string> statements = new List<string>() { "for", "if", "while", "return" };
            VariableTableItems = new List<T1TableItem>();
            FunctionTableItems = new List<T1TableItem>();
            T1Instruction CurrentInstruction = null;
            T1Scope CurrentScope = new T1Scope();

            CurrentLine = 1;
            CurrentToken = 0;

            string token;
            while (CurrentToken < Tokens.Count)
            {
                token = GetNextToken();

                if (primitiveTypes.Contains(token))
                {
                    string typeName = token;

                    // function or variable declaration
                    string identifierName = GetNextToken();

                    // test whether identifier stands in any of the tables
                    if(TableContains(VariableTableItems, identifierName) || TableContains(FunctionTableItems, identifierName))
                    {
                        throw new Exception("Identifier redeclared: " + identifierName + " [" + CurrentLine + "]");
                    }

                    token = GetNextToken();
                    T1VariableType variableType = GetVariableTypeFromToken(token);

                    if (token == "(")
                    {
                        // this seems to be a function declaration
                        FunctionTableItems.Add(new T1TableItem(identifierName, FunctionTableItems.Count - 1, CurrentScope));

                        token = GetNextToken();
                        List<T1InstructionVariableDeclaration> arguments = new List<T1InstructionVariableDeclaration>();

                        while (token != ")")
                        {
                            if(primitiveTypes.Contains(token))
                            {
                                typeName = token;
                                identifierName = GetNextToken();

                                arguments.Add(new T1InstructionVariableDeclaration(GetVariableTypeFromToken(typeName)));
                                if(TableContains(VariableTableItems, identifierName))
                                {
                                    throw new Exception("Argumant redeclared: " + identifierName + " [" + CurrentLine + "]");
                                }

                                token = GetNextToken();
                                if(token == ",")
                                {
                                    token = GetNextToken();
                                }
                            }

                            token = GetNextToken();
                        }

                        token = GetNextToken();
                        if (token != "{")
                        {
                            throw new Exception("{ expected [" + CurrentLine + "]");
                        }

                        T1Scope functionScope = new T1Scope();
                        AddInstruction(new T1InstructionFunctionDeclaration(arguments, T1VariableType.Int, 0, functionScope));

                        token = GetNextToken();
                        while(token != "}")
                        {
                            // is it a function call?
                            if (TableContains(FunctionTableItems, token))
                            {

                            }
                            // is it an assignment?
                            else if (TableContains(VariableTableItems, token))
                            {
                            }
                            // is it a variable declaration?
                            else if (primitiveTypes.Contains(token))
                            {
                            }
                            // is it a statement?
                            else if(statements.Contains(token))
                            {
                                if(token == "if")
                                {

                                } else if(token == "for")
                                {
                                    token = GetNextToken();
                                    if(token != "(")
                                    {
                                        throw new Exception("\"(\" expected but " + token + " found");
                                    }

                                } else if(token == "while")
                                {
                                    token = GetNextToken();
                                    if (token != "(")
                                    {
                                        throw new Exception("\"(\" expected but " + token + " found");
                                    }


                                }
                                else if(token == "return")
                                {

                                } else
                                {
                                    throw new Exception("Undexpected statement or keyword: " + token + " [" + CurrentLine + "]");
                                }
                            } else
                            {
                                throw new Exception("Unexpected statement starter: " + token + " [" + CurrentLine + "]");
                            }

                            token = GetNextToken();
                        }
                    }
                    else if(token == "=" || token == ";")
                    {
                        // this seems to be a variable declaration
                        if(token == "=")
                        {
                            CompileExpression();
                        } else
                        {
                            VariableTableItems.Add(new T1TableItem(identifierName, VariableTableItems.Count - 1, CurrentScope));

                            CurrentInstruction = new T1InstructionVariableDeclaration(variableType);
                            CurrentScope.AddInstruction(CurrentInstruction);
                        }
                    } else
                    {
                        throw new Exception("Unexpected identifier: " + token + " [" + CurrentLine + "]");
                    }
                }
                else if (token == "\n")
                {
                    CurrentLine++;
                }
                else if (token == ";")
                {
                    // skip to the next statement
                }
                else
                {
                    throw new Exception("Unexpected identifier: " + token + " [" + CurrentLine + "]");
                }

                CurrentToken++;
            }
        }

        private void CompileExpression()
        {
            throw new NotImplementedException();
        }

        private T1VariableType GetVariableTypeFromToken(string typeName)
        {
            T1VariableType variableType = T1VariableType.Int;

            if (typeName == "void")
            {
                variableType = T1VariableType.Void;
            }
            else if (typeName == "string")
            {
                variableType = T1VariableType.String;
            }
            else if (typeName == "byte")
            {
                variableType = T1VariableType.Byte;
            }
            else
            {
                variableType = T1VariableType.Double;
            }

            return variableType;
        }

        private string GetNextToken()
        {
            if(CurrentToken >= Tokens.Count)
            {
                throw new Exception("Unexpected end of file at line " + CurrentLine);
            }

            return Tokens[CurrentToken++];
        }

        public void RunScript(string scriptText)
        {
            ParseScript(scriptText);
            CompileScript();

            Run();
        }

        public void LoadScript(string fileName)
        {
            if(!File.Exists(fileName))
            {
                throw new Exception("Script file not found");
            }

            using (StreamReader sr = new StreamReader(fileName))
            {
                string fileContent = sr.ReadToEnd();
                RunScript(fileContent);
            }
        }

        public void Run()
        {
            instructionPointer = 0;
            ExpressionResult = null;
            ExpressionType = T1VariableType.Int;

            // pass 1: LabelSet
            while (instructionPointer < Instructions.Count)
            {
                if(Instructions[instructionPointer].Type == T1InstructionType.LabelSet)
                {
                    RunInstruction(Instructions[instructionPointer]);
                }

                instructionPointer++;
            }

            // pass 2: Run all
            instructionPointer = 0;
            while (instructionPointer < Instructions.Count)
            {
                if (Instructions[instructionPointer].Type == T1InstructionType.LabelSet)
                {
                    instructionPointer++;
                    continue;
                }

                RunInstruction(Instructions[instructionPointer++]);
            }
        }

        public void AddInstruction(T1Instruction instruction)
        {
            Instructions.Add(instruction);
        }

        private void RunInstruction(T1Instruction instruction)
        {
            switch (instruction.Type)
            {
                // Variable table should contain scope pointer as well as variable id
                case T1InstructionType.Assignment:
                    EvaluateExpression(((T1InstructionAssignment)instruction).Expression);
                    if(((T1InstructionAssignment)instruction).Variable.VariableType != ExpressionType)
                    {
                        throw new Exception("Runtime error: Variable type mismatch in expression");
                    }

                    ((T1InstructionAssignment)instruction).Variable.Scope.VariableTable[((T1InstructionAssignment)instruction).Variable.VariableId] = new T1Variable(((T1InstructionAssignment)instruction).Variable.VariableType, ExpressionResult);

                    break;
                case T1InstructionType.ConditionalJump:
                    EvaluateExpression(((T1InstructionConditionalJump)instruction).Expression);

                    if ((int)ExpressionResult != 0)
                    {
                        instructionPointer = LabelTable[((T1InstructionConditionalJump)instruction).LabelId] + 1;
                    }

                    break;
                case T1InstructionType.ExpressionEvaluation:
                    EvaluateExpression(((T1InstructionEvaluateExpression)instruction).Expression);
                    break;
                case T1InstructionType.FunctionCall:
                    T1Scope newScope = (T1Scope)SubScopes[((T1InstructionFunctionCall)instruction).FunctionId]; // .Clone();
                    newScope.ParentScope = this;

                    // Variable assignments for arguments
                    for (int i = 0; i < ((T1InstructionFunctionCall)instruction).Arguments.Count; i++)
                    {
                        newScope.VariableTable[i + 1] = new T1Variable(((T1InstructionFunctionCall)instruction).Arguments[i].VariableType, ((T1InstructionFunctionCall)instruction).Arguments[i].Operand);
                    }

                    newScope.Run();
                    break;
                case T1InstructionType.FunctionDeclaration:
                    T1Scope subScope = ((T1InstructionFunctionDeclaration)instruction).FunctionScope;

                    // Return value
                    T1Variable returnValue = null;
                    switch(((T1InstructionFunctionDeclaration)instruction).ReturnType)
                    {
                        case T1VariableType.Void:
                            returnValue = new T1Variable(T1VariableType.Void);
                            break;
                        case T1VariableType.Byte:
                            returnValue = new T1Variable(T1VariableType.Byte);
                            break;
                        case T1VariableType.Double:
                            returnValue = new T1Variable(T1VariableType.Double);
                            break;
                        case T1VariableType.Int:
                            returnValue = new T1Variable(T1VariableType.Int);
                            break;
                        case T1VariableType.String:
                            returnValue = new T1Variable(T1VariableType.String);
                            break;
                    }

                    subScope.VariableTable.Add(returnValue);

                    // Arguments
                    for (int i = 0; i < ((T1InstructionFunctionDeclaration)instruction).Arguments.Count; i++)
                    {
                        T1Variable newArgument = null;
                        switch (((T1InstructionFunctionDeclaration)instruction).Arguments[i].VariableType)
                        {
                            case T1VariableType.Void:
                                newArgument = new T1Variable(T1VariableType.Void);
                                break;
                            case T1VariableType.Byte:
                                newArgument = new T1Variable(T1VariableType.Byte);
                                break;
                            case T1VariableType.Double:
                                newArgument = new T1Variable(T1VariableType.Double);
                                break;
                            case T1VariableType.Int:
                                newArgument = new T1Variable(T1VariableType.Int);
                                break;
                            case T1VariableType.String:
                                newArgument = new T1Variable(T1VariableType.String);
                                break;
                        }

                        subScope.VariableTable.Add(newArgument);
                    }

                    SubScopes.Add(subScope);
                    break;
                case T1InstructionType.Import:
                    break;
                case T1InstructionType.Jump:
                    instructionPointer = LabelTable[((T1InstructionJump)instruction).LabelId] + 1;
                    break;
                case T1InstructionType.LabelSet:
                    LabelTable.Add(instructionPointer);
                    break;
                case T1InstructionType.VariableDeclaration:
                    T1Variable newVariable = null;

                    switch (((T1InstructionVariableDeclaration)instruction).VariableType)
                    {
                        case T1VariableType.Void:
                            newVariable = new T1Variable(T1VariableType.Void, VariableTable.Count);
                            break;
                        case T1VariableType.Byte:
                            newVariable = new T1Variable(T1VariableType.Byte, VariableTable.Count);
                            break;
                        case T1VariableType.Double:
                            newVariable = new T1Variable(T1VariableType.Double, VariableTable.Count);
                            break;
                        case T1VariableType.Int:
                            newVariable = new T1Variable(T1VariableType.Int, VariableTable.Count);
                            break;
                        case T1VariableType.String:
                            newVariable = new T1Variable(T1VariableType.String, VariableTable.Count);
                            break;
                    }

                    VariableTable.Add(newVariable);
                    break;
                case T1InstructionType.Return:
                    instructionPointer = Instructions.Count;

                    break;
            }
        }

        private void EvaluateExpression(object expression)
        {
            if (expression is T1ExpressionItem)
            {
                EvaluateExpressionItem((T1ExpressionItem)expression);
            } else if (expression is T1RuntimeVairableReference)
            {
                EvaluateExpressionReference((T1RuntimeVairableReference)expression);
            } else if(expression is T1ExpressionOperand)
            { 
                switch(((T1ExpressionOperand)expression).Type)
                {
                    case T1OperandType.Constant:
                        ExpressionResult = ((T1ExpressionOperand)expression).Value;
                        if(ExpressionResult.GetType().Name.Contains("Int32"))
                        {
                            ExpressionType = T1VariableType.Int;
                        } else if (ExpressionResult.GetType().Name.Contains("Double"))
                        {
                            ExpressionType = T1VariableType.Double;
                        }
                        else if (ExpressionResult.GetType().Name.Contains("Byte"))
                        {
                            ExpressionType = T1VariableType.Byte;
                        }
                        else if (ExpressionResult.GetType().Name.Contains("String"))
                        {
                            ExpressionType = T1VariableType.String;
                        } else
                        {
                            ExpressionType = T1VariableType.Void;
                        }

                        break;
                    case T1OperandType.Expression:
                        EvaluateExpressionItem((T1ExpressionItem)((T1ExpressionOperand)expression).Value);
                        break;
                    case T1OperandType.Variable:
                        EvaluateExpressionReference((T1RuntimeVairableReference)((T1ExpressionOperand)expression).Value);
                        break;
                    case T1OperandType.Function:
                        throw new Exception("not implemented 108");
                }
            } else
            {
                throw new Exception("Internal error 902");
            }
        }

        private void EvaluateExpressionReference(T1RuntimeVairableReference expression)
        {
            if(expression.Scope.VariableTable[expression.VariableId].VariableType != expression.VariableType)
            {
                throw new Exception("Runtime error: type mismatch");
            }

            ExpressionType = expression.VariableType;
            ExpressionResult = expression.Scope.VariableTable[expression.VariableId].Value;
        }

        private void EvaluateExpressionItem(T1ExpressionItem expression)
        {
            switch (expression.Operator)
            {
                case T1Operator.DirectByte:
                    if (expression.OperandLeft.Value is byte)
                    {
                        ExpressionResult = (byte)expression.OperandLeft.Value;
                    }
                    else if (expression.OperandLeft.Value is T1ExpressionItem)
                    {
                        EvaluateExpression(expression.OperandLeft);
                    }
                    else
                    {
                        throw new Exception("Runtime error: type mismatch");
                    }

                    ExpressionType = T1VariableType.Byte;
                    break;
                case T1Operator.DirectInt:
                    if (expression.OperandLeft.Value is int)
                    {
                        ExpressionResult = (int)expression.OperandLeft.Value;
                    }
                    else if (expression.OperandLeft.Value is T1ExpressionItem)
                    {
                        EvaluateExpression(expression.OperandLeft);
                    }
                    else
                    {
                        throw new Exception("Runtime error: type mismatch");
                    }

                    ExpressionType = T1VariableType.Int;
                    break;
                case T1Operator.DirectNumeric:
                    if (expression.OperandLeft.Value is double)
                    {
                        ExpressionResult = (double)expression.OperandLeft.Value;
                    }
                    else if (expression.OperandLeft.Value is T1ExpressionItem)
                    {
                        EvaluateExpression(expression.OperandLeft);
                    }
                    else
                    {
                        throw new Exception("Runtime error: type mismatch");
                    }

                    ExpressionType = T1VariableType.Double;
                    break;
                case T1Operator.DirectString:
                    if (expression.OperandLeft.Value is string)
                    {
                        ExpressionResult = (string)expression.OperandLeft.Value;
                    }
                    else if (expression.OperandLeft.Value is T1ExpressionItem)
                    {
                        EvaluateExpression(expression.OperandLeft);
                    }
                    else
                    {
                        throw new Exception("Runtime error: type mismatch");
                    }

                    ExpressionType = T1VariableType.String;
                    break;
                case T1Operator.Dereference:
                    T1Variable variable = ((T1RuntimeVairableReference)expression.OperandLeft.Value).Scope.VariableTable[((T1RuntimeVairableReference)expression.OperandLeft.Value).VariableId];

                    switch (variable.VariableType)
                    {
                        case T1VariableType.Byte:
                            ExpressionResult = (byte)variable.Value;
                            break;
                        case T1VariableType.Double:
                            ExpressionResult = (double)variable.Value;
                            break;
                        case T1VariableType.Int:
                            ExpressionResult = (int)variable.Value;
                            break;
                        case T1VariableType.String:
                            ExpressionResult = (string)variable.Value;
                            break;
                    }

                    ExpressionType = variable.VariableType;
                    break;
                case T1Operator.Concatanate:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    ExpressionResult = ExpressionStackPopString() + (string)ExpressionResult;
                    ExpressionType = T1VariableType.String;

                    break;
                case T1Operator.Add:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    switch (ExpressionType)
                    {
                        case T1VariableType.Int:
                            ExpressionResult = ExpressionStackPopInt() + (int)ExpressionResult;
                            break;
                        case T1VariableType.Byte:
                            ExpressionResult = ExpressionStackPopByte() + (byte)ExpressionResult;
                            break;
                        case T1VariableType.Double:
                            ExpressionResult = ExpressionStackPopDouble() + (double)ExpressionResult;
                            break;
                        default:
                            throw new Exception("Runtime error: Invalid type in math");
                    }

                    break;
                case T1Operator.Subtract:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    switch (ExpressionType)
                    {
                        case T1VariableType.Int:
                            ExpressionResult = ExpressionStackPopInt() - (int)ExpressionResult;
                            break;
                        case T1VariableType.Byte:
                            ExpressionResult = ExpressionStackPopByte() - (byte)ExpressionResult;
                            break;
                        case T1VariableType.Double:
                            ExpressionResult = ExpressionStackPopDouble() - (double)ExpressionResult;
                            break;
                        default:
                            throw new Exception("Runtime error: Invalid type in math");
                    }

                    break;
                case T1Operator.Multiply:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    switch (ExpressionType)
                    {
                        case T1VariableType.Int:
                            ExpressionResult = ExpressionStackPopInt() * (int)ExpressionResult;
                            break;
                        case T1VariableType.Byte:
                            ExpressionResult = ExpressionStackPopByte() * (byte)ExpressionResult;
                            break;
                        case T1VariableType.Double:
                            ExpressionResult = ExpressionStackPopDouble() * (double)ExpressionResult;
                            break;
                        default:
                            throw new Exception("Runtime error: Invalid type in math");
                    }

                    break;
                case T1Operator.Divide:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    switch (ExpressionType)
                    {
                        case T1VariableType.Int:
                            ExpressionResult = ExpressionStackPopInt() / (int)ExpressionResult;
                            break;
                        case T1VariableType.Byte:
                            ExpressionResult = ExpressionStackPopByte() / (byte)ExpressionResult;
                            break;
                        case T1VariableType.Double:
                            ExpressionResult = ExpressionStackPopDouble() / (double)ExpressionResult;
                            break;
                        default:
                            throw new Exception("Runtime error: Invalid type in math");
                    }

                    break;
                case T1Operator.Mod:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    switch (ExpressionType)
                    {
                        case T1VariableType.Int:
                            ExpressionResult = ExpressionStackPopInt() % (int)ExpressionResult;
                            break;
                        case T1VariableType.Byte:
                            ExpressionResult = ExpressionStackPopByte() % (byte)ExpressionResult;
                            break;
                        case T1VariableType.Double:
                            ExpressionResult = ExpressionStackPopDouble() % (double)ExpressionResult;
                            break;
                        default:
                            throw new Exception("Runtime error: Invalid type in math");
                    }

                    break;
                case T1Operator.ShiftLeft:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    ExpressionResult = ExpressionStackPopInt() << (int)ExpressionResult;

                    break;
                case T1Operator.Power:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    ExpressionResult = (int)Math.Pow(ExpressionStackPopInt(), (int)ExpressionResult);

                    break;
                case T1Operator.ShiftRight:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    ExpressionResult = ExpressionStackPopInt() >> (int)ExpressionResult;

                    break;
                case T1Operator.OrLogical:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    ExpressionResult = (ExpressionStackPopDouble() == 1) || ((double)ExpressionResult == 1);

                    break;
                case T1Operator.OrBitwise:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    ExpressionResult = (ExpressionStackPopDouble() == 1) | ((double)ExpressionResult == 1);

                    break;
                case T1Operator.AndLogical:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    ExpressionResult = (ExpressionStackPopDouble() == 1) && ((double)ExpressionResult == 1);

                    break;
                case T1Operator.AndBitwise:
                    EvaluateExpression(expression.OperandLeft);
                    ExpressionStackPush();
                    EvaluateExpression(expression.OperandRight);

                    ExpressionResult = (ExpressionStackPopDouble() == 1) & ((double)ExpressionResult == 1);

                    break;
            }
        }

        private void ExpressionStackPush()
        {
            ExpressionStack.Add(ExpressionResult);
        }

        private byte ExpressionStackPopByte()
        {
            byte result = (byte)ExpressionStack[ExpressionStack.Count - 1];
            ExpressionStack.RemoveAt(ExpressionStack.Count - 1);

            return result;
        }

        private int ExpressionStackPopInt()
        {
            int result = (int)ExpressionStack[ExpressionStack.Count - 1];
            ExpressionStack.RemoveAt(ExpressionStack.Count - 1);

            return result;
        }

        private double ExpressionStackPopDouble()
        {
            double result = (double)ExpressionStack[ExpressionStack.Count - 1];
            ExpressionStack.RemoveAt(ExpressionStack.Count - 1);

            return result;
        }

        private string ExpressionStackPopString()
        {
            string result = (string)ExpressionStack[ExpressionStack.Count - 1];
            ExpressionStack.RemoveAt(ExpressionStack.Count - 1);

            return result;
        }

    }
}
