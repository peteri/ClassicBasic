// <copyright file="GetTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests the GET statement.
    /// </summary>
    [TestClass]
    public class GetTests
    {
        private IVariableRepository _variableRepository;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private Mock<ITeletypeWithPosition> _mockTeletypeWithPosition;
        private IRunEnvironment _runEnvironment;
        private Get _sut;

        /// <summary>
        /// Tests Get for string variable.
        /// </summary>
        [TestMethod]
        public void GetWorksWithStringVariable()
        {
            SetupSut();
            _mockTeletypeWithPosition.Setup(mtwp => mtwp.ReadChar()).Returns('1');
            var variableReference = _variableRepository.GetOrCreateVariable("A$", new short[] { });
            _mockExpressionEvaluator.Setup(mee => mee.GetLeftValue()).Returns(variableReference);
            _sut.Execute();
            Assert.AreEqual("1", variableReference.GetValue().ValueAsString());
        }

        /// <summary>
        /// Tests Get throws for immediate mode.
        /// </summary>
        [TestMethod]
        public void GetThrowsForImmediate()
        {
            SetupSut();
            _mockTeletypeWithPosition.Setup(mtwp => mtwp.ReadChar()).Returns('1');
            var variableReference = _variableRepository.GetOrCreateVariable("A$", new short[] { });
            _mockExpressionEvaluator.Setup(mee => mee.GetLeftValue()).Returns(variableReference);
            _runEnvironment.CurrentLine = new ProgramLine(null, new List<IToken> { });
            Test.Throws<IllegalDirectException>(_sut.Execute);
        }

        /// <summary>
        /// Tests Get throws syntax error for Alpha in numeric mode.
        /// </summary>
        /// <param name="character">Character to test.</param>
        /// <param name="throwsException">Should throw exception</param>
        [DataTestMethod]
        [DataRow('0', false)]
        [DataRow('9', false)]
        [DataRow('/', true)]
        [DataRow(':', true)]
        public void GetThrowsForAlphaInNumeric(char character, bool throwsException)
        {
            SetupSut();
            _mockTeletypeWithPosition.Setup(mtwp => mtwp.ReadChar()).Returns(character);
            var variableReference = _variableRepository.GetOrCreateVariable("A", new short[] { });
            _mockExpressionEvaluator.Setup(mee => mee.GetLeftValue()).Returns(variableReference);
            bool exceptionThrown = false;
            try
            {
                _sut.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(throwsException, exceptionThrown);
        }

        /// <summary>
        /// Tests Get for numeric variable.
        /// The plus sign, minus sign, ctrl @, E, space and the period all return a zero as the typed value.
        /// </summary>
        /// <param name="character">Character from keyboard.</param>
        /// <param name="expectedValue">Expected value.</param>
        [DataTestMethod]
        [DataRow('+', 0.0)]
        [DataRow('-', 0.0)]
        [DataRow('E', 0.0)]
        [DataRow('.', 0.0)]
        [DataRow('1', 1.0)]
        [DataRow('9', 9.0)]
        [DataRow((char)0, 0.0)]
        public void GetWorksWithNumericVariable(char character, double expectedValue)
        {
            SetupSut();
            _mockTeletypeWithPosition.Setup(mtwp => mtwp.ReadChar()).Returns(character);
            var variableReference = _variableRepository.GetOrCreateVariable("A", new short[] { });
            _mockExpressionEvaluator.Setup(mee => mee.GetLeftValue()).Returns(variableReference);
            _sut.Execute();
            Assert.AreEqual(expectedValue, variableReference.GetValue().ValueAsDouble());
        }

        private void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _mockTeletypeWithPosition = new Mock<ITeletypeWithPosition>();
            _runEnvironment.CurrentLine = new ProgramLine(10, new List<IToken> { });
            _sut = new Get(_runEnvironment, _mockExpressionEvaluator.Object, _mockTeletypeWithPosition.Object);
        }
    }
}
