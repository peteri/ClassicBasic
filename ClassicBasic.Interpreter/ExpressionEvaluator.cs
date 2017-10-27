﻿// <copyright file="ExpressionEvaluator.cs" company="Peter Ibbotson">
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

            var indexes = new List<short>();
            token = _runEnvironment.CurrentLine.NextToken();
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

            return _variableRepository.GetOrCreateVariable(name, indexes.ToArray());
        }

        /// <summary>
        /// If the next token can be evaluated as line number returns the
        /// line number. If not returns null and puts the token back.
        /// </summary>
        /// <returns>null or a line number.</returns>
        public int? GetLineNumber()
        {
            var token = _runEnvironment.CurrentLine.NextToken();
            if (token.TokenClass != TokenType.ClassNumber)
            {
                _runEnvironment.CurrentLine.PushToken(token);
                return null;
            }

            if (int.TryParse(token.Text, out int lineNumber))
            {
                return lineNumber;
            }

            _runEnvironment.CurrentLine.PushToken(token);
            return null;
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
                    double doubleResult;
                    if (double.TryParse(token.Text, out doubleResult))
                    {
                        return new Accumulator(doubleResult);
                    }

                    break;
                case TokenType.ClassString:
                    return new Accumulator(token.Text);
                case TokenType.ClassVariable:
                    _runEnvironment.CurrentLine.PushToken(token);
                    var variable = GetLeftValue();
                    return variable.GetValue();
                case TokenType.ClassFunction:
                    throw new NotImplementedException();
                default:
                    break;
            }

            throw new Exceptions.SyntaxErrorException();
        }
    }
}
