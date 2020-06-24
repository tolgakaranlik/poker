using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1Runtime;

namespace T1RuntimeTests
{
    public class TestFunctionDeclaration : T1RuntimeTest
    {
        public string GetDescription()
        {
            return "Tests whether function declarations can be made with success";
        }

        public string GetName()
        {
            return "FunctionDeclaration";
        }

        private T1Scope mainScope;
        private T1Scope functionScope;

        private void Init()
        {
            mainScope = new T1Scope();
            functionScope = new T1Scope();

            List<T1InstructionVariableDeclaration> arguments = new List<T1InstructionVariableDeclaration>();
            arguments.Add(new T1InstructionVariableDeclaration(T1VariableType.Int));
            arguments.Add(new T1InstructionVariableDeclaration(T1VariableType.Int));

            T1ExpressionItem a0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 4), null, T1Operator.DirectInt);
            mainScope.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Int));
            mainScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(mainScope, 0, T1VariableType.Int), a0));

            // (functionScope 0) = (functionScope 0) ^ (functionScope 1)
            T1ExpressionItem c0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 8), null, T1Operator.DirectInt);
            T1ExpressionItem c1 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 3), null, T1Operator.DirectInt);
            functionScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(functionScope, 0, T1VariableType.Int), c0));
            functionScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(functionScope, 1, T1VariableType.Int), c1));

            T1ExpressionItem f0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Variable, new T1RuntimeVairableReference(functionScope, 0, T1VariableType.Int)), new T1ExpressionOperand(T1OperandType.Variable, new T1RuntimeVairableReference(functionScope, 1, T1VariableType.Int)), T1Operator.Power);
            functionScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(functionScope, 0, T1VariableType.Int), f0));

            // (mainScope 0) = (functionScope 0) / (mainScope 0)
            T1ExpressionItem f1 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Variable, new T1RuntimeVairableReference(functionScope, 0, T1VariableType.Int)), new T1ExpressionOperand(T1OperandType.Variable, new T1RuntimeVairableReference(mainScope, 0, T1VariableType.Int)), T1Operator.Divide);
            functionScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(functionScope, 1, T1VariableType.Int), f1));

            mainScope.AddInstruction(new T1InstructionFunctionDeclaration(arguments, T1VariableType.Int, 0, functionScope));
            mainScope.AddInstruction(new T1InstructionFunctionCall(arguments, 0));
        }

        public bool Run()
        {
            Init();
            mainScope.Run();

            // mainScope: v0 => 4
            // functionScope: v0 => 512
            // functionScope: v1 => 128

            if ((int)mainScope.SubScopes[0].VariableTable[0].Value != 512)
            {
                return false;
            }

            if ((int)mainScope.SubScopes[0].VariableTable[1].Value != 128)
            {
                return false;
            }

            if ((int)mainScope.VariableTable[0].Value != 4)
            {
                return false;
            }

            return true;
        }
    }
}
