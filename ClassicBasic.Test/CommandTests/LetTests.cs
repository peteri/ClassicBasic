// <copyright file="LetTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests the LET command.
    /// </summary>
    [TestClass]
    public class LetTests
    {
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private IVariableRepository _variableRepository;
        private VariableReference _variableReference;
        private IRunEnvironment _runEnvironment;
        private Let _sut;

        /// <summary>
        /// Check we can assign expression to a variable
        /// </summary>
        [TestMethod]
        public void LetAssignsExpressionToVariable()
        {
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(
                10,
                new List<IToken> { new Token("=", TokenClass.Seperator, TokenType.Equal) });
            _sut.Execute();
            Assert.AreEqual(3.0, _variableReference.GetValue().ValueAsDouble());
        }

        /// <summary>
        /// Check we throw an exception when token isn't equals.
        /// </summary>
        [TestMethod]
        public void LetIsNotFollowedByEquals()
        {
            SetupSut();
            _runEnvironment.CurrentLine = new ProgramLine(
                10,
                new List<IToken> { new Token(":", TokenClass.Seperator, TokenType.Colon) });
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

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _variableRepository = new VariableRepository();
            _variableReference = _variableRepository.GetOrCreateVariable("A", new short[] { });

            _mockExpressionEvaluator.Setup(mre => mre.GetLeftValue()).Returns(_variableReference);
            _mockExpressionEvaluator.Setup(mre => mre.GetExpression()).Returns(new Accumulator(3.0));
            _sut = new Let(_runEnvironment, _mockExpressionEvaluator.Object);
        }
    }
}
