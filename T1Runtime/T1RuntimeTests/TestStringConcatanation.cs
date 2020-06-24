using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1Runtime;

namespace T1RuntimeTests
{
    public class TestStringConcatanation : T1RuntimeTest
    {
        public string GetDescription()
        {
            return "Tests whether strings are concatanted as they should be";
        }

        public string GetName()
        {
            return "StringConcatanation";
        }

        private T1Scope mainScope;

        private void Init()
        {
            mainScope = new T1Scope();

            T1ExpressionItem s0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, "Quiere un botella"), null, T1Operator.DirectString);
            T1ExpressionItem s1 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, " o "), null, T1Operator.DirectString);
            T1ExpressionItem s2 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, "una vaso?"), null, T1Operator.DirectString);

            T1ExpressionItem e1 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Expression, s0), new T1ExpressionOperand(T1OperandType.Expression, s1), T1Operator.Concatanate);
            T1ExpressionItem e2 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Expression, e1), new T1ExpressionOperand(T1OperandType.Expression, s2), T1Operator.Concatanate);

            mainScope.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.String));
            mainScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(mainScope, 0, T1VariableType.String), new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Expression, e2), null, T1Operator.DirectString)));
        }

        public bool Run()
        {
            Init();
            mainScope.Run();

            try
            {
                if ((string)mainScope.VariableTable[0].Value != "Quiere un botella o una vaso?")
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
