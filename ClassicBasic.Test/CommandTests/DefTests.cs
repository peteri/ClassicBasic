﻿// <copyright file="DefTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the DEF FN statement.
    /// </summary>
    [TestClass]
    public class DefTests
    {
        private IRunEnvironment _runEnvironment;
        private ExpressionEvaluator _expressionEvaluator;
        private VariableRepository _variableRepository;

        /// <summary>
        /// Checks DEF works and doesn't eat a trailing colon.
        /// </summary>
        [TestMethod]
        public void DefDefinesFunctionAndDoesntEatColon()
        {
            var tokens = new List<IToken>
            {
                new Token("FN", TokenClass.Statement, TokenType.Fn),
                new Token("NAME"),
                new Token("(", TokenClass.Seperator, TokenType.OpenBracket),
                new Token("A"),
                new Token(")", TokenClass.Seperator, TokenType.CloseBracket),
                new Token("=", TokenClass.Seperator, TokenType.Equal),
                new Token("A"),
                new Token("*", TokenClass.Seperator, TokenType.Multiply),
                new Token("A"),
                new Token(":", TokenClass.Seperator, TokenType.Colon)
            };

            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _expressionEvaluator = new ExpressionEvaluator(_variableRepository, _runEnvironment);
            var programLine = new ProgramLine(10, tokens);
            _runEnvironment.CurrentLine = programLine;

            var sut = new Def(_runEnvironment, _expressionEvaluator);

            sut.Execute();

            Assert.AreEqual(TokenType.Colon, _runEnvironment.CurrentLine.NextToken().Seperator);
            var definition = _runEnvironment.DefinedFunctions["NAME"];
            Assert.AreEqual("NAME", definition.FunctionName);
            Assert.AreEqual("A", definition.VariableName);
            Assert.AreEqual(6, definition.LineToken);
            Assert.AreEqual(programLine, definition.Line);
        }

        /// <summary>
        /// Checks DEF Throws syntax error on bad token.
        /// </summary>
        /// <param name="insertRemAt">Location in token list to insert REM at.</param>
        /// <param name="nextToken">Value next token should return. (leaks implementation)</param>
        /// <param name="message">Message for Assert error.</param>
        [DataTestMethod]
        [DataRow(0, "(", "FN")]
        [DataRow(1, "(", "Function Name")]
        [DataRow(2, "(", "OpenBracket")]
        [DataRow(3, "A", "Parameter Name")]
        [DataRow(4, "=", "Close bracket")]
        [DataRow(5, "=", "Equals")]
        public void DefDefinesFunctionThrowsOnBadToken(int insertRemAt, string nextToken, string message)
        {
            var tokens = new List<IToken>
            {
                new Token("FN", TokenClass.Statement, TokenType.Fn),
                new Token("NAME"),
                new Token("(", TokenClass.Seperator, TokenType.OpenBracket),
                new Token("A"),
                new Token(")", TokenClass.Seperator, TokenType.CloseBracket),
                new Token("=", TokenClass.Seperator, TokenType.Equal),
                new Token("A"),
                new Token("*", TokenClass.Seperator, TokenType.Multiply),
                new Token("A"),
                new Token(":", TokenClass.Seperator, TokenType.Colon)
            };

            tokens.Insert(insertRemAt, new Token("bang", TokenClass.Remark));

            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _expressionEvaluator = new ExpressionEvaluator(_variableRepository, _runEnvironment);
            var programLine = new ProgramLine(10, tokens);
            _runEnvironment.CurrentLine = programLine;

            bool exceptionThrown = false;
            var sut = new Def(_runEnvironment, _expressionEvaluator);
            try
            {
                sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, message);
            Assert.AreEqual(nextToken, _runEnvironment.CurrentLine.NextToken().Text);
        }

        /// <summary>
        /// Checks DEF Throws syntax errpr on immediate mode.
        /// </summary>
        [TestMethod]
        public void DefDefinesFunctionThrowsOnBadLineNumber()
        {
            var tokens = new List<IToken>
            {
                new Token("FN", TokenClass.Statement, TokenType.Fn),
                new Token("NAME"),
                new Token("(", TokenClass.Seperator, TokenType.OpenBracket),
                new Token("A"),
                new Token(")", TokenClass.Seperator, TokenType.CloseBracket),
                new Token("=", TokenClass.Seperator, TokenType.Equal),
                new Token("A"),
                new Token("*", TokenClass.Seperator, TokenType.Multiply),
                new Token("A"),
                new Token(":", TokenClass.Seperator, TokenType.Colon)
            };

            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _expressionEvaluator = new ExpressionEvaluator(_variableRepository, _runEnvironment);
            var programLine = new ProgramLine(null, tokens);
            _runEnvironment.CurrentLine = programLine;

            bool exceptionThrown = false;
            var sut = new Def(_runEnvironment, _expressionEvaluator);
            try
            {
                sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalDirectException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
