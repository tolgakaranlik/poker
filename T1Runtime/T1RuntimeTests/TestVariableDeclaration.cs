using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1Runtime;

namespace T1RuntimeTests
{
    public class TestVariableDeclaration : T1RuntimeTest
    {
        public string GetName()
        {
            return "VariableDeclaration";
        }

        public string GetDescription()
        {
            return "Tests whether a variable can be declared and its value can be assigned correctly";
        }

        private T1Scope mainScope;

        public void Init()
        {
            mainScope = new T1Scope();

            mainScope.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Int));
            mainScope.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Double));
            mainScope.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Byte));
            mainScope.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.String));

            mainScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(mainScope, 0, T1VariableType.Int), new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 44), null, T1Operator.DirectInt)));
            mainScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(mainScope, 1, T1VariableType.Double), new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 999.91), null, T1Operator.DirectNumeric)));
            mainScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(mainScope, 2, T1VariableType.Byte), new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, (byte)241), null, T1Operator.DirectByte)));
            mainScope.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(mainScope, 3, T1VariableType.String), new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, "The new valUe"), null, T1Operator.DirectString)));
        }

        public bool Run()
        {
            Init();
            mainScope.Run();

            try
            {
                if ((int)mainScope.VariableTable[0].Value != 44)
                {
                    return false;
                }

                if ((double)mainScope.VariableTable[1].Value != 999.91)
                {
                    return false;
                }

                if ((double)mainScope.VariableTable[1].Value == 119.19)
                {
                    return false;
                }

                if ((byte)mainScope.VariableTable[2].Value != 241)
                {
                    return false;
                }

                if ((string)mainScope.VariableTable[3].Value != "The new valUe")
                {
                    return false;
                }
            } catch
            {
                return false;
            }

            return true;
        }
    }
}
