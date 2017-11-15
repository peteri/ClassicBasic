// <copyright file="InputTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System;
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the INPUT statement
    /// </summary>
    [TestClass]
    public class InputTests
    {
        private Input _sut;
        private IVariableRepository _variableRepository;
        private IExpressionEvaluator _expressionEvaluator;
        private IRunEnvironment _runEnvironment;
        private MockTeletype _teletype;
        private IToken _comma;
        private IToken _dollar;
        private IToken _semi;
        private IToken _colon;

        /// <summary>
        /// Test we can parse numbers and strings.
        /// </summary>
        [TestMethod]
        public void InputParsesNumbersAndStrings()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("A"), _comma,
                new Token("B"), _dollar, _comma,
                new Token("C"), _dollar
            };
            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            _teletype.Input.Enqueue("23,\"456,abc\",456 abc");
            _sut.Execute();
            Assert.AreEqual("?", _teletype.Output.Dequeue());
            Assert.AreEqual(23.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual("456,abc", _variableRepository.GetOrCreateVariable("B$", new short[] { }).GetValue().ValueAsString());
            Assert.AreEqual("456 abc", _variableRepository.GetOrCreateVariable("C$", new short[] { }).GetValue().ValueAsString());
        }

        /// <summary>
        /// Test we can print our prompt and loop on multi line input.
        /// </summary>
        [TestMethod]
        public void InputPrintsPromptParsesNumbersAndStringsFromIndividualLines()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("Prompt", TokenType.ClassString), _semi,
                new Token("A"), _comma,
                new Token("B"), _dollar, _comma,
                new Token("C"), _dollar
            };
            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            _teletype.Input.Enqueue("23");
            _teletype.Input.Enqueue("\"456,abc\"");
            _teletype.Input.Enqueue("  456 abc");
            _sut.Execute();
            Assert.AreEqual("Prompt", _teletype.Output.Dequeue());
            Assert.AreEqual("??", _teletype.Output.Dequeue());
            Assert.AreEqual("??", _teletype.Output.Dequeue());
            Assert.AreEqual(23.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual("456,abc", _variableRepository.GetOrCreateVariable("B$", new short[] { }).GetValue().ValueAsString());
            Assert.AreEqual("456 abc", _variableRepository.GetOrCreateVariable("C$", new short[] { }).GetValue().ValueAsString());
        }

        /// <summary>
        /// Test we can print our prompt and loop on multi line input.
        /// </summary>
        [TestMethod]
        public void InputPrintsPromptParsesNumbersAndRetriesOnError()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("Prompt", TokenType.ClassString), _semi,
                new Token("A"), _comma,
                new Token("B"), _dollar, _comma,
                new Token("C"), _dollar
            };
            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            _teletype.Input.Enqueue("23");
            _teletype.Input.Enqueue("\"456,abc\" AB");
            _teletype.Input.Enqueue("23");
            _teletype.Input.Enqueue("\"456,abc\"  ");
            _teletype.Input.Enqueue("456 abc ");
            _sut.Execute();
            Assert.AreEqual("Prompt", _teletype.Output.Dequeue());
            Assert.AreEqual("??", _teletype.Output.Dequeue());
            Assert.AreEqual("?RENTER", _teletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _teletype.Output.Dequeue());
            Assert.AreEqual("Prompt", _teletype.Output.Dequeue());
            Assert.AreEqual("??", _teletype.Output.Dequeue());
            Assert.AreEqual("??", _teletype.Output.Dequeue());
            Assert.AreEqual(23.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual("456,abc", _variableRepository.GetOrCreateVariable("B$", new short[] { }).GetValue().ValueAsString());
            Assert.AreEqual("456 abc ", _variableRepository.GetOrCreateVariable("C$", new short[] { }).GetValue().ValueAsString());
        }

        /// <summary>
        /// Test we can output extra ignored.
        /// </summary>
        [TestMethod]
        public void InputPrintsParsesNumbersAndDisplaysExtraInput()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("A"), _comma,
                new Token("B"), _dollar
            };
            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            _teletype.Input.Enqueue(string.Empty);
            _teletype.Input.Enqueue("456\"abc  , extra");
            _sut.Execute();
            Assert.AreEqual("?", _teletype.Output.Dequeue());
            Assert.AreEqual("??", _teletype.Output.Dequeue());
            Assert.AreEqual("?EXTRA IGNORED", _teletype.Output.Dequeue());
            Assert.AreEqual(Environment.NewLine, _teletype.Output.Dequeue());
            Assert.AreEqual(0.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual("456\"abc  ", _variableRepository.GetOrCreateVariable("B$", new short[] { }).GetValue().ValueAsString());
        }

        /// <summary>
        /// Test we throw syntax error on missing semicolon with prompt.
        /// </summary>
        [TestMethod]
        public void InputThrowsExceptionOnMissingSemiColon()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("Prompt", TokenType.ClassString),
                new Token("A"), _comma,
                new Token("B"), _dollar
            };

            _teletype.Input.Enqueue(string.Empty);
            _teletype.Input.Enqueue("456\"abc  , extra");
            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            bool exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test we throw syntax error on missing variable.
        /// </summary>
        [TestMethod]

        public void InputThrowsExceptionOnMissingVariable()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("1"), _comma,
                new Token("B"), _dollar
            };

            _teletype.Input.Enqueue(string.Empty);
            _teletype.Input.Enqueue("456\"abc  , extra");
            _runEnvironment.CurrentLine = new ProgramLine(10, tokens);

            bool exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test we throw illegal direct exception.
        /// </summary>
        [TestMethod]
        public void InputThrowsExceptionInDirectMode()
        {
            SetupSut();
            var tokens = new List<IToken>
            {
                new Token("1"), _comma,
                new Token("B"), _dollar
            };

            _teletype.Input.Enqueue(string.Empty);
            _teletype.Input.Enqueue("456\"abc  , extra");
            _runEnvironment.CurrentLine = new ProgramLine(null, tokens);

            bool exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.IllegalDirectException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        private void SetupSut()
        {
            _teletype = new MockTeletype();
            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _expressionEvaluator = new ExpressionEvaluator(_variableRepository, _runEnvironment);

            _semi = new Token(";", TokenType.ClassSeperator | TokenType.Semicolon);
            _colon = new Token(":", TokenType.ClassSeperator | TokenType.Colon);
            _comma = new Token(",", TokenType.ClassSeperator | TokenType.Comma);
            _dollar = new Token("$", TokenType.ClassSeperator | TokenType.Dollar);

            _sut = new Input(_runEnvironment, _expressionEvaluator, _variableRepository, _teletype);
        }
    }
}
