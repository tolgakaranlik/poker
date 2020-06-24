using System;
using System.Collections.Generic;
using System.Text;

namespace T1Runtime
{
    public enum T1InstructionType {
        FunctionDeclaration,
        FunctionCall,
        Assignment,
        ExpressionEvaluation,
        ConditionalJump,
        Jump,
        LabelSet,
        VariableDeclaration,
        Import,
        Return
    }

    [Serializable]
    public class T1Instruction
    {
        protected T1InstructionType type;
        protected Object operand;

        public T1InstructionType Type
        {
            get
            {
                return type;
            }
        }

        public Object Operand
        {
            get
            {
                return operand;
            }
        }

        public T1Instruction(T1InstructionType type, Object operand)
        {
            this.type = type;
            this.operand = operand;
        }
    }

    // Instruction: SetLabel
    [Serializable]
    public class T1InstructionLabelSet : T1Instruction
    {
        private int id;

        public int Id
        {
            get
            {
                return id;
            }
        }

        public T1InstructionLabelSet(int id) : base(T1InstructionType.LabelSet, id)
        {
            this.id = id;
        }
    }

    // Instruction: Import
    [Serializable]
    public class T1InstructionImport : T1Instruction
    {
        private string libraryName;

        public string LibraryName
        {
            get
            {
                return libraryName;
            }
        }

        public T1InstructionImport(string libraryName) : base(T1InstructionType.Import, libraryName)
        {
            this.libraryName = libraryName;
        }
    }

    // Instruction: Jump
    [Serializable]
    public class T1InstructionJump : T1Instruction
    {
        private int labelId;

        public int LabelId
        {
            get
            {
                return labelId;
            }
        }

        public T1InstructionJump(int labelId) : base(T1InstructionType.Jump, labelId)
        {
            this.labelId = labelId;
        }
    }

    // Instruction: EvaluateExpression
    [Serializable]
    public enum T1Operator
    {
        DirectByte,
        DirectInt,
        DirectString,
        DirectNumeric,
        Dereference,
        Add,
        Subtract,
        Divide,
        Multiply,
        Mod,
        Power,
        ShiftLeft,
        ShiftRight,
        AndLogical,
        OrLogical,
        AndBitwise,
        OrBitwise,
        Not,
        Concatanate
    }

    [Serializable]
    public enum T1OperandType
    {
        Constant,
        Variable,
        Expression,
        Function
    }

    [Serializable]
    public class T1ExpressionOperand
    {
        private T1OperandType type;
        private Object value;

        public T1OperandType Type
        {
            get
            {
                return type;
            }
        }

        public Object Value
        {
            get
            {
                return value;
            }
        }

        public T1ExpressionOperand(T1OperandType type, Object value)
        {
            this.type = type;
            this.value = value;
        }
    }

    [Serializable]
    public class T1ExpressionItem
    {
        private T1ExpressionOperand operandLeft;
        private T1ExpressionOperand operandRight;
        private T1Operator theOperator;

        public T1ExpressionOperand OperandLeft
        {
            get
            {
                return operandLeft;
            }
        }
        public T1ExpressionOperand OperandRight
        {
            get
            {
                return operandRight;
            }
        }
        public T1Operator Operator
        {
            get
            {
                return theOperator;
            }
        }

        public T1ExpressionItem(T1ExpressionOperand operandLeft, T1ExpressionOperand operandRight, T1Operator theOperator)
        {
            this.operandLeft = operandLeft;
            this.operandRight = operandRight;
            this.theOperator = theOperator;
        }
    }

    [Serializable]
    public class T1InstructionEvaluateExpression : T1Instruction
    {
        private T1ExpressionItem expression;

        public T1ExpressionItem Expression
        {
            get
            {
                return expression;
            }
        }

        public T1InstructionEvaluateExpression(T1ExpressionItem expression) : base(T1InstructionType.ExpressionEvaluation, expression)
        {
            this.expression = expression;
        }
    }

    // Assignment
    [Serializable]
    public class T1InstructionAssignment : T1Instruction
    {
        private T1RuntimeVairableReference variable;
        private T1ExpressionItem expression;

        public T1RuntimeVairableReference Variable
        {
            get
            {
                return variable;
            }
        }

        public T1ExpressionItem Expression
        {
            get
            {
                return expression;
            }
        }

        public T1InstructionAssignment(T1RuntimeVairableReference variable, T1ExpressionItem expression) : base(T1InstructionType.Assignment, variable)
        {
            this.variable = variable;
            this.expression = expression;
        }
    }

    // Variable Declaration
    public enum T1VariableType
    {
        Void,
        Int,
        Byte,
        Double,
        String
    }

    [Serializable]
    public class T1InstructionVariableDeclaration : T1Instruction
    {
        private T1VariableType variableType;

        public T1VariableType VariableType
        {
            get
            {
                return variableType;
            }
        }

        public T1InstructionVariableDeclaration(T1VariableType variableType) : base(T1InstructionType.VariableDeclaration, variableType)
        {
            this.variableType = variableType;
        }
    }

    // Conditional Jump
    [Serializable]
    public class T1InstructionConditionalJump : T1InstructionJump
    {
        private T1ExpressionItem expression;

        public T1ExpressionItem Expression
        {
            get
            {
                return expression;
            }
        }

        public T1InstructionConditionalJump(int labelId, T1ExpressionItem expression) : base(labelId)
        {
            type = T1InstructionType.ConditionalJump;

            this.expression = expression;
        }
    }

    // Function declaration
    [Serializable]
    public class T1InstructionFunctionDeclaration : T1InstructionLabelSet
    {
        private List<T1InstructionVariableDeclaration> arguments;
        private T1VariableType returnType;
        private int functionId;
        private T1Scope functionScope;

        public List<T1InstructionVariableDeclaration> Arguments
        {
            get
            {
                return arguments;
            }
        }
        public T1VariableType ReturnType
        {
            get
            {
                return returnType;
            }
        }
        public int FunctionId
        {
            get
            {
                return functionId;
            }
        }
        public T1Scope FunctionScope
        {
            get
            {
                return functionScope;
            }
        }

        public T1InstructionFunctionDeclaration(List<T1InstructionVariableDeclaration> arguments, T1VariableType returnType, int functionId, T1Scope functionScope) : base(functionId)
        {
            type = T1InstructionType.FunctionDeclaration;

            this.arguments = arguments;
            this.returnType = returnType;
            this.functionId = functionId;
            this.functionScope = functionScope;
        }
    }

    // Function call
    [Serializable]
    public class T1InstructionFunctionCall : T1Instruction
    {
        private List<T1InstructionVariableDeclaration> arguments;
        private int functionId;

        public List<T1InstructionVariableDeclaration> Arguments
        {
            get
            {
                return arguments;
            }
        }
        public int FunctionId
        {
            get
            {
                return functionId;
            }
        }

        public T1InstructionFunctionCall(List<T1InstructionVariableDeclaration> arguments, int functionId) : base(T1InstructionType.FunctionCall, functionId)
        {
            type = T1InstructionType.FunctionCall;

            this.arguments = arguments;
            this.functionId = functionId;
        }
    }
}
