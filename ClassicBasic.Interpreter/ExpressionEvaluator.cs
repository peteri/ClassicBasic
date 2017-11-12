// <copyright file="ExpressionEvaluator.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Expression evaluator.
    /// </summary>
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IVariableRepository _variableRepository;
        private readonly IRunEnvironment _runEnvironment;
        private int _depth;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="variableRepository">Variable repository.</param>
        /// <param name="runEnvironment">Run time environment, used mostly for the current line.</param>
        public ExpressionEvaluator(
            IVariableRepository variableRepository,
            IRunEnvironment runEnvironment)
        {
            _variableRepository = variableRepository;
            _runEnvironment = runEnvironment;
        }

        /// <summary>
        /// Gets an expression.
        /// </summary>
        /// <returns>Accumlator with either a string or double.</returns>
        public Accumulator GetExpression()
        {
            _depth = 0;
            return GetExpressionWithDepthCheck();
        }

        /// <summary>
        /// Gets a variable reference which can be used a LValue for assignments.
        /// </summary>
        /// <returns>Reference to a variable.</returns>
        public VariableReference GetLeftValue()
        {
            string name = GetVariableName();

            return _variableRepository.GetOrCreateVariable(name, GetIndexes());
        }

        /// <summary>
        /// Gets a variable name including the % or $
        /// </summary>
        /// <returns>Name of the variable.</returns>
        public string GetVariableName()
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            if (token.TokenClass != TokenType.ClassVariable)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var name = token.Text;
            token = _runEnvironment.CurrentLine.NextToken();
            name += (token.Seperator == TokenType.Dollar) ? "$" : string.Empty;
            name += (token.Seperator == TokenType.Percent) ? "%" : string.Empty;
            if ((token.Seperator != TokenType.Dollar) && (token.Seperator != TokenType.Percent))
            {
                _runEnvironment.CurrentLine.PushToken(token);
            }

            return name;
        }

        /// <summary>
        /// Parses an array of indexes from the current command stream.
        /// Eats the outer set of brackets.
        /// </summary>
        /// <returns>Array of indexes</returns>
        public short[] GetIndexes()
        {
            var indexes = new List<short>();
            var token = _runEnvironment.CurrentLine.NextToken();
            if (token.Seperator == TokenType.OpenBracket)
            {
                do
                {
                    indexes.Add(GetExpression().ValueAsShort());
                    token = _runEnvironment.CurrentLine.NextToken();
                }
                while (token.Seperator == TokenType.Comma);
                if (token.Seperator != TokenType.CloseBracket)
                {
                    throw new Exceptions.SyntaxErrorException();
                }
            }
            else
            {
                _runEnvironment.CurrentLine.PushToken(token);
            }

            return indexes.ToArray();
        }

        private IList<Accumulator> GetFunctionParameters()
        {
            var parameters = new List<Accumulator>();
            IToken token = _runEnvironment.CurrentLine.NextToken();
            if (token.Seperator != TokenType.OpenBracket)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            do
            {
                parameters.Add(GetExpression());
                token = _runEnvironment.CurrentLine.NextToken();
            }
            while (token.Seperator == TokenType.Comma);

            if (token.Seperator != TokenType.CloseBracket)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            return parameters;
        }

        private Accumulator GetExpressionWithDepthCheck()
        {
            _depth++;
            if (_depth > 64)
            {
                throw new Exceptions.FormulaTooComplex();
            }

            var result = AndExpression();
            while (true)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
                switch (token.Seperator)
                {
                    case TokenType.Or:
                        var expression = AndExpression().ValueAsDouble();
                        result.SetValue((result.ValueAsDouble() != 0.0 || expression != 0.0) ? 1.0 : 0.0);
                        break;
                    default:
                        _runEnvironment.CurrentLine.PushToken(token);
                        _depth--;
                        return result;
                }
            }
        }

        private Accumulator AndExpression()
        {
            var result = NotExpression();
            while (true)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
                switch (token.Seperator)
                {
                    case TokenType.And:
                        var expression = NotExpression().ValueAsDouble();
                        result.SetValue((result.ValueAsDouble() != 0.0 && expression != 0.0) ? 1.0 : 0.0);
                        break;
                    default:
                        _runEnvironment.CurrentLine.PushToken(token);
                        return result;
                }
            }
        }

        private Accumulator NotExpression()
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            switch (token.Seperator)
            {
                case TokenType.Not:
                    var result = NotExpression().ValueAsDouble();
                    return new Accumulator((result == 0.0) ? 1.0 : 0.0);
                default:
                    _runEnvironment.CurrentLine.PushToken(token);
                    return CompareExpression();
            }
        }

        private Accumulator CompareExpression()
        {
            var result = AddSubstractExpression();
            var first = _runEnvironment.CurrentLine.NextToken();
            IToken secondary = null;
            switch (first.Seperator)
            {
                case TokenType.Equal:
                    break;
                case TokenType.LessThan:
                case TokenType.GreaterThan:
                    secondary = _runEnvironment.CurrentLine.NextToken();
                    break;
                default:
                    _runEnvironment.CurrentLine.PushToken(first);
                    return result;
            }

            if (secondary != null)
            {
                switch (secondary.Seperator)
                {
                    case TokenType.GreaterThan:
                    case TokenType.Equal:
                        break;
                    default:
                        _runEnvironment.CurrentLine.PushToken(secondary);
                        secondary = null;
                        break;
                }
            }

            int comparisonResult;

            if (result.Type == typeof(string))
            {
                comparisonResult = result.ValueAsString().CompareTo(AddSubstractExpression().ValueAsString());
            }
            else
            {
                comparisonResult = result.ValueAsDouble().CompareTo(AddSubstractExpression().ValueAsDouble());
            }

            return new Accumulator(ComparisonFunction(first, secondary)(comparisonResult) ? 1.0 : 0.0);
        }

        private Func<int, bool> ComparisonFunction(IToken first, IToken second)
        {
            switch (first.Seperator)
            {
                case TokenType.GreaterThan:
                    if (second == null)
                    {
                        return comparisonResult => (comparisonResult > 0);
                    }

                    return comparisonResult => (comparisonResult >= 0);
                case TokenType.LessThan:
                    if (second == null)
                    {
                        return comparisonResult => (comparisonResult < 0);
                    }

                    if (second.Seperator == TokenType.Equal)
                    {
                        return comparisonResult => (comparisonResult <= 0);
                    }

                    return comparisonResult => (comparisonResult != 0);
            }

            // Must be equals
            return comparisonResult => (comparisonResult == 0);
        }

        private Accumulator AddSubstractExpression()
        {
            var result = MultiplyDivideExpression();
            while (true)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
                switch (token.Seperator)
                {
                    case TokenType.Plus:
                        if (result.Type == typeof(string))
                        {
                            result.SetValue(result.ValueAsString() + MultiplyDivideExpression().ValueAsString());
                        }
                        else
                        {
                            result.SetValue(result.ValueAsDouble() + MultiplyDivideExpression().ValueAsDouble());
                        }

                        break;
                    case TokenType.Minus:
                        result.SetValue(result.ValueAsDouble() - MultiplyDivideExpression().ValueAsDouble());
                        break;
                    default:
                        _runEnvironment.CurrentLine.PushToken(token);
                        return result;
                }
            }
        }

        private Accumulator MultiplyDivideExpression()
        {
            var result = NegateExpression();
            while (true)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
                switch (token.Seperator)
                {
                    case TokenType.Multiply:
                        result.SetValue(result.ValueAsDouble() * NegateExpression().ValueAsDouble());
                        break;
                    case TokenType.Divide:
                        var divisor = NegateExpression().ValueAsDouble();
                        if (divisor == 0.0)
                        {
                            throw new Exceptions.DivisionByZeroException();
                        }

                        result.SetValue(result.ValueAsDouble() / divisor);
                        break;
                    default:
                        _runEnvironment.CurrentLine.PushToken(token);
                        return result;
                }
            }
        }

        private Accumulator NegateExpression()
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            switch (token.Seperator)
            {
                case TokenType.Minus:
                    var result = PowerExpression().ValueAsDouble();
                    return new Accumulator(-1.0 * result);
                default:
                    _runEnvironment.CurrentLine.PushToken(token);
                    return PowerExpression();
            }
        }

        private Accumulator PowerExpression()
        {
            var result = BracketsExpression();
            while (true)
            {
                var token = _runEnvironment.CurrentLine.NextToken();
                switch (token.Seperator)
                {
                    case TokenType.Power:
                        result.SetValue(Math.Pow(result.ValueAsDouble(), BracketsExpression().ValueAsDouble()));
                        break;
                    default:
                        _runEnvironment.CurrentLine.PushToken(token);
                        return result;
                }
            }
        }

        private Accumulator BracketsExpression()
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            switch (token.Seperator)
            {
                case TokenType.OpenBracket:
                    var result = GetExpressionWithDepthCheck();
                    if (_runEnvironment.CurrentLine.NextToken().Seperator != TokenType.CloseBracket)
                    {
                        throw new Exceptions.SyntaxErrorException();
                    }

                    return result;
                default:
                    _runEnvironment.CurrentLine.PushToken(token);
                    return Value();
            }
        }

        private Accumulator Value()
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            switch (token.TokenClass)
            {
                case TokenType.ClassNumber:
                    return ParseNumber(token);
                case TokenType.ClassString:
                    return new Accumulator(token.Text);
                case TokenType.ClassVariable:
                    return GetVariableValue(token);
                case TokenType.ClassFunction:
                    var function = token as IFunction;
                    var parameters = GetFunctionParameters();
                    return function.Execute(parameters);
                case TokenType.ClassStatement:
                    return CheckForUserFunction(token);
                default:
                    break;
            }

            throw new Exceptions.SyntaxErrorException();
        }

        private Accumulator GetVariableValue(IToken token)
        {
            if (token.Text.Length > 2)
            {
                if (token.Text == "ERR")
                {
                    return new Accumulator((double)_runEnvironment.LastErrorNumber);
                }

                if (token.Text == "ERL")
                {
                    return new Accumulator((double)(_runEnvironment.LastErrorLine ?? 0));
                }
            }

            _runEnvironment.CurrentLine.PushToken(token);
            var variable = GetLeftValue();
            return variable.GetValue();
        }

        private Accumulator ParseNumber(IToken token)
        {
            string text = token.Text;
            if (text.EndsWith("E"))
            {
                var plusMinus = _runEnvironment.CurrentLine.NextToken();
                var exponent = _runEnvironment.CurrentLine.NextToken();
                if ((plusMinus.Seperator != TokenType.Plus && plusMinus.Seperator != TokenType.Minus)
                    || exponent.TokenClass != TokenType.ClassNumber)
                {
                    throw new Exceptions.SyntaxErrorException();
                }

                text = text + plusMinus.Text + exponent.Text;
            }

            if (double.TryParse(text, out double doubleResult))
            {
                return new Accumulator(doubleResult);
            }

            throw new Exceptions.SyntaxErrorException();
        }

        private Accumulator CheckForUserFunction(IToken token)
        {
            if (token.Statement != TokenType.Fn)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            var functionName = _runEnvironment.CurrentLine.NextToken();
            if (functionName.TokenClass != TokenType.ClassVariable)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            if (!_runEnvironment.DefinedFunctions.TryGetValue(functionName.Text, out UserDefinedFunction userDefinedFunction))
            {
                throw new Exceptions.UndefinedFunctionException();
            }

            var parameters = GetFunctionParameters();
            if (parameters.Count != 1)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            return userDefinedFunction.Execute(parameters[0], _runEnvironment, this, _variableRepository);
        }
    }
}
