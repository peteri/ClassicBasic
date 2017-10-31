// <copyright file="ForNextTests.cs" company="Peter Ibbotson">
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
    /// Tests FOR/NEXT loops. Not very unit test as we're testing the commands as pairs.
    /// </summary>
    [TestClass]
    public class ForNextTests
    {
        private IRunEnvironment _runEnvironment;
        private IVariableRepository _variableRepository;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;
        private IToken _toToken;
        private IToken _stepToken;
        private IToken _colonToken;
        private IToken _equalToken;

        /// <summary>
        /// For next loop can loop from 1 to 3.
        /// </summary>
        [TestMethod]
        public void ForNextTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(3.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            forCmd.Execute();
            Assert.AreEqual(1.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
            var loopBackToken = _runEnvironment.CurrentLine.CurrentToken;

            // Execute next,
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();

            // variable should be 2
            Assert.AreEqual(2.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());

            // Should be back to just after for loop.
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackToken, _runEnvironment.CurrentLine.CurrentToken);

            // Execute next, variable should be 3
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();
            Assert.AreEqual(3.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackToken, _runEnvironment.CurrentLine.CurrentToken);

            // Execute next
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();

            // So we have exited the loop
            Assert.AreEqual(20, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.IsTrue(_runEnvironment.CurrentLine.EndOfLine);

            // Variable should be 4.0
            Assert.AreEqual(4.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// For next loop can loop from 1 to 6 with a step of 2.0.
        /// </summary>
        [TestMethod]
        public void ForNextWithStepTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(6.0))
                .Returns(new Accumulator(2.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken,_stepToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            forCmd.Execute();
            Assert.AreEqual(1.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
            var loopBackToken = _runEnvironment.CurrentLine.CurrentToken;

            // Execute next,
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();

            // variable should be 3
            Assert.AreEqual(3.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());

            // Should be back to just after for loop.
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackToken, _runEnvironment.CurrentLine.CurrentToken);

            // Execute next, variable should be 5
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackToken, _runEnvironment.CurrentLine.CurrentToken);

            // Execute next
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();

            // So we have exited the loop
            Assert.AreEqual(20, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.IsTrue(_runEnvironment.CurrentLine.EndOfLine);

            // Variable should be 7.0
            Assert.AreEqual(7.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// For next loop can loop from 1 to 6 with a step of 2.0.
        /// </summary>
        [TestMethod]
        public void ForNextWithNegativeStepTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(-1.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _stepToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            forCmd.Execute();
            Assert.AreEqual(3.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
            var loopBackToken = _runEnvironment.CurrentLine.CurrentToken;

            // Execute next,
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();

            // variable should be 2
            Assert.AreEqual(2.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());

            // Should be back to just after for loop.
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackToken, _runEnvironment.CurrentLine.CurrentToken);

            // Execute next, variable should be 1
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();
            Assert.AreEqual(1.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackToken, _runEnvironment.CurrentLine.CurrentToken);

            // Execute next
            line20.CurrentToken = 0;
            _runEnvironment.CurrentLine = line20;
            nextCmd.Execute();

            // So we have exited the loop
            Assert.AreEqual(20, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.IsTrue(_runEnvironment.CurrentLine.EndOfLine);

            // Variable should be 0.0
            Assert.AreEqual(0.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// For next loop loop needs a number for the step
        /// </summary>
        [TestMethod]
        public void ForNextWithNegativeBadStep()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _stepToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            bool exceptionThrown = false;
            try
            {
                forCmd.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// For next loop throws with a bad target value.
        /// </summary>
        [TestMethod]
        public void ForNextWithBadTarget()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _stepToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            bool exceptionThrown = false;
            try
            {
                forCmd.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// For next loop throws with a missing TO.
        /// </summary>
        [TestMethod]
        public void ForNextWithMissingTo()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            bool exceptionThrown = false;
            try
            {
                forCmd.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// For next loop throws with a missing non-numeric start value.
        /// </summary>
        [TestMethod]
        public void ForNextWithMissingBadStart()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            bool exceptionThrown = false;
            try
            {
                forCmd.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// For next loop throws with a missing equals.
        /// </summary>
        [TestMethod]
        public void ForNextWithMissingEquals()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            bool exceptionThrown = false;
            try
            {
                forCmd.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// For next loop throws with not a variable.
        /// </summary>
        [TestMethod]
        public void ForNextWithNoForVariable()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1"), _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A") });

            _runEnvironment.CurrentLine = line10;

            bool exceptionThrown = false;
            try
            {
                forCmd.Execute();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        private void SetupSut()
        {
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _toToken = new Token("TO", TokenType.ClassStatement | TokenType.To);
            _colonToken = new Token(":", TokenType.ClassSeperator | TokenType.Colon);
            _equalToken = new Token("=", TokenType.ClassSeperator | TokenType.Equal);
            _stepToken = new Token("STEP", TokenType.ClassStatement | TokenType.Step);
        }

    }
}