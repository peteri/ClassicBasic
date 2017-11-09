// <copyright file="Input.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the INPUT command.
    /// </summary>
    public class Input : Token, ICommand
    {
        private readonly IRunEnvironment _runEnvironment;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IVariableRepository _variableRepository;
        private readonly ITeletype _teletype;
        private readonly ReadInputParser _readInputParser;
        private string _prompt;
        private bool _firstLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Input"/> class.
        /// </summary>
        /// <param name="runEnvironment">Run time environment.</param>
        /// <param name="expressionEvaluator">Expression evaluator.</param>
        /// <param name="variableRepository">Variable repository.</param>
        /// <param name="teletype">Teletype to use.</param>
        public Input(
            IRunEnvironment runEnvironment,
            IExpressionEvaluator expressionEvaluator,
            IVariableRepository variableRepository,
            ITeletype teletype)
            : base("INPUT", TokenType.ClassStatement)
        {
            _runEnvironment = runEnvironment;
            _expressionEvaluator = expressionEvaluator;
            _variableRepository = variableRepository;
            _teletype = teletype;
            _readInputParser = new ReadInputParser(ReadLine);
        }

        /// <summary>
        /// Executes the INPUT command.
        /// </summary>
        public void Execute()
        {
            var variableReferences = new List<VariableReference>();

            // Not valid in immediate mode.
            if (!_runEnvironment.CurrentLine.LineNumber.HasValue)
            {
                throw new Exceptions.IllegalDirectException();
            }

            // Set the prompt.
            _prompt = "?";
            var token = _runEnvironment.CurrentLine.NextToken();
            if (token.TokenClass == TokenType.ClassString)
            {
                _prompt = token.Text;
                if (_runEnvironment.CurrentLine.NextToken().Seperator != TokenType.Semicolon)
                {
                    throw new Exceptions.SyntaxErrorException();
                }
            }
            else
            {
                _runEnvironment.CurrentLine.PushToken(token);
            }

            do
            {
                variableReferences.Add(_expressionEvaluator.GetLeftValue());
                token = _runEnvironment.CurrentLine.NextToken();
            }
            while (token.Seperator == TokenType.Comma);

            _runEnvironment.CurrentLine.PushToken(token);

            if (variableReferences.Count == 0)
            {
                throw new Exceptions.SyntaxErrorException();
            }

            bool reenterInput = false;
            do
            {
                _firstLine = true;
                _readInputParser.Clear();
                try
                {
                    reenterInput = false;
                    _readInputParser.ReadVariables(variableReferences);
                }
                catch (Exceptions.SyntaxErrorException)
                {
                    _teletype.Write("?RENTER");
                    _teletype.Write(Environment.NewLine);
                    reenterInput = true;
                }
            }
            while (reenterInput);
            if (_readInputParser.HasExtraData)
            {
                _teletype.Write("?EXTRA IGNORED");
                _teletype.Write(Environment.NewLine);
            }
        }

        private string ReadLine()
        {
            _teletype.Write(_firstLine ? _prompt : "??");
            _firstLine = false;
            return _teletype.Read();
        }
    }
}
