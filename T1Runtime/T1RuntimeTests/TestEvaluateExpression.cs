using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1Runtime;

namespace T1RuntimeTests
{
    public class TestEvaluateExpression : T1RuntimeTest
    {
        public string GetDescription()
        {
            return "Tests whether epressions are evaluated correctly";
        }

        public string GetName()
        {
            return "EvaluateExpression";
        }

        private T1Scope mainScope;

        public void Init()
        {
            mainScope = new T1Scope();

            T1ExpressionItem c0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 5), null, T1Operator.DirectInt);
            T1ExpressionItem c1 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 14), null, T1Operator.DirectInt);
            T1ExpressionItem c2 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 14), null, T1Operator.DirectInt);
            T1ExpressionItem c3 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 8), null, T1Operator.DirectInt);
            T1ExpressionItem c4 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 1), null, T1Operator.DirectInt);

            T1ExpressionItem e1 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Expression, c0), new T1ExpressionOperand(T1OperandType.Expression, c1), T1Operator.Add);
            T1ExpressionItem e2 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Expression, c2), new T1ExpressionOperand(T1OperandType.Expression, c3), T1Operator.Subtract);
            T1ExpressionItem e0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Expression, e1), new T1ExpressionOperand(T1OperandType.Expression, e2), T1Operator.Multiply);

            T1ExpressionItem f0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Expression, c0), null, T1Operator.DirectInt);

            mainScope.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Int));
            mainScope.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Int));

            mainScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(mainScope, 0, T1VariableType.Int), new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Expression, e0), new T1ExpressionOperand(T1OperandType.Expression, c4), T1Operator.ShiftRight)));
            mainScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(mainScope, 1, T1VariableType.Int), f0));
        }

        public bool Run()
        {
            Init();
            mainScope.Run();

            try
            {
                if ((int)mainScope.VariableTable[0].Value != 57)
                {
                    return false;
                }

                if ((int)mainScope.VariableTable[1].Value != 5)
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
