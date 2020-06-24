using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1Runtime;

namespace T1RuntimeTests
{
    public class TestConditionalJump : T1RuntimeTest
    {
        public string GetDescription()
        {
            return "Tests whether conditional jump works properly";
        }

        public string GetName()
        {
            return "ConditionalJump";
        }

        private T1Scope scope1;
        private T1Scope scope2;

        private void Init()
        {
            scope1 = new T1Scope();

            // set label 0
            // v[0] = 0
            // v[1] = 99
            // if(v[0] != 0) goto label 1
            // v[1] = 128
            // set label 1

            scope1.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Int));
            scope1.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Int));
            T1ExpressionItem a0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 1), null, T1Operator.DirectInt);
            T1ExpressionItem a1 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 99), null, T1Operator.DirectInt);
            T1ExpressionItem a2 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 128), null, T1Operator.DirectInt);

            scope1.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(scope1, 0, T1VariableType.Int), a0));
            scope1.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(scope1, 1, T1VariableType.Int), a1));

            scope1.AddInstruction(new T1InstructionLabelSet(0));

            scope1.AddInstruction(new T1InstructionConditionalJump(1, new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Variable, new T1RuntimeVairableReference(scope1, 0, T1VariableType.Int)), new T1ExpressionOperand(T1OperandType.Constant, 0), T1Operator.Subtract)));
            scope1.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(scope1, 1, T1VariableType.Int), a2));

            scope1.AddInstruction(new T1InstructionLabelSet(1));

            scope2 = new T1Scope();

            // set label 0
            // v[0] = 1
            // v[1] = 99
            // if(v[0] != 0) goto label 1
            // v[1] = 128
            // set label 1

            scope2.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Int));
            scope2.AddInstruction(new T1InstructionVariableDeclaration(T1VariableType.Int));
            a0 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 0), null, T1Operator.DirectInt);
            a1 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 99), null, T1Operator.DirectInt);
            a2 = new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Constant, 128), null, T1Operator.DirectInt);

            scope2.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(scope2, 0, T1VariableType.Int), a0));
            scope2.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(scope2, 1, T1VariableType.Int), a1));

            scope2.AddInstruction(new T1InstructionLabelSet(0));

            scope2.AddInstruction(new T1InstructionConditionalJump(1, new T1ExpressionItem(new T1ExpressionOperand(T1OperandType.Variable, new T1RuntimeVairableReference(scope2, 0, T1VariableType.Int)), new T1ExpressionOperand(T1OperandType.Constant, 0), T1Operator.Subtract)));
            scope2.AddInstruction(new T1InstructionAssignment(new T1RuntimeVairableReference(scope2, 1, T1VariableType.Int), a2));

            scope2.AddInstruction(new T1InstructionLabelSet(1));
        }

        public bool Run()
        {
            Init();
            scope1.Run();
            scope2.Run();

            if ((int)scope1.VariableTable[1].Value != 99)
            {
                return false;
            }

            if ((int)scope2.VariableTable[1].Value == 99)
            {
                return false;
            }

            return true;
        }
    }
}
