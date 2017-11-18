// <copyright file="ForNextTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter.Exceptions;
    using Interpreter;
    using Interpreter.Commands;
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
        private IToken _commaToken;

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
            var line20 = new ProgramLine(20, new List<IToken> { new Token("A"), new Token("1") });

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

            // Did we leave behind the token.
            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("1", token.Text);

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
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _stepToken, _colonToken });
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
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _stepToken, _colonToken });

            _runEnvironment.CurrentLine = line10;
            Test.Throws<TypeMismatchException>(forCmd.Execute);
        }

        /// <summary>
        /// For next loop throws with a bad target value.
        /// </summary>
        [TestMethod]
        public void ForNextWithBadTarget()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _stepToken, _colonToken });

            _runEnvironment.CurrentLine = line10;
            Test.Throws<TypeMismatchException>(forCmd.Execute);
        }

        /// <summary>
        /// For next loop throws with a missing TO.
        /// </summary>
        [TestMethod]
        public void ForNextWithMissingTo()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _colonToken });

            _runEnvironment.CurrentLine = line10;
            Test.Throws<SyntaxErrorException>(forCmd.Execute);
        }

        /// <summary>
        /// For next loop throws with a missing non-numeric start value.
        /// </summary>
        [TestMethod]
        public void ForNextWithMissingBadStart()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _colonToken });

            _runEnvironment.CurrentLine = line10;
            Test.Throws<TypeMismatchException>(forCmd.Execute);
        }

        /// <summary>
        /// For next loop throws with a missing equals.
        /// </summary>
        [TestMethod]
        public void ForNextWithMissingEquals()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _colonToken });

            _runEnvironment.CurrentLine = line10;
            Test.Throws<SyntaxErrorException>(forCmd.Execute);
        }

        /// <summary>
        /// For next loop throws with not a variable.
        /// </summary>
        [TestMethod]
        public void ForNextWithNoForVariable()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator("A"));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("1"), _colonToken });

            _runEnvironment.CurrentLine = line10;
            Test.Throws<SyntaxErrorException>(forCmd.Execute);
        }

        /// <summary>
        /// When we create a for next loop, we search back down the stack for a loop with the same name.
        /// We stop searching at return addresses for gosubs (same as the Apple ][ BASIC)
        /// </summary>
        [TestMethod]
        public void ForNextDeletesUnneededLoopsStopsAtReturn()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(1.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });

            _runEnvironment.CurrentLine = line10;

            _variableRepository.GetOrCreateVariable("A", new short[] { });
            _variableRepository.GetOrCreateVariable("B", new short[] { });
            _variableRepository.GetOrCreateVariable("C", new short[] { });

            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 1, VariableName = "A" });
            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 2, VariableName = null });
            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 3, VariableName = "B" });
            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 4, VariableName = "C" });

            forCmd.Execute();

            Assert.AreEqual(5, _runEnvironment.ProgramStack.Count);
            var newVariableA = _runEnvironment.ProgramStack.Pop();
            var oldVariableC = _runEnvironment.ProgramStack.Pop();
            var oldVariableB = _runEnvironment.ProgramStack.Pop();
            var gosub = _runEnvironment.ProgramStack.Pop();
            var oldVariableA = _runEnvironment.ProgramStack.Pop();

            Assert.AreEqual("A", newVariableA.VariableName);
            Assert.AreEqual(10, newVariableA.Line.LineNumber.Value);

            Assert.AreEqual(null, gosub.VariableName);

            Assert.AreEqual("A", oldVariableA.VariableName);
            Assert.AreEqual(null, oldVariableA.Line);
            Assert.AreEqual("B", oldVariableB.VariableName);
            Assert.AreEqual(null, oldVariableB.Line);
            Assert.AreEqual("C", oldVariableC.VariableName);
            Assert.AreEqual(null, oldVariableC.Line);
        }

        /// <summary>
        /// When we create a for next loop, we search back down the stack for a loop with the same name.
        /// We delete newer entries to stop the stack growing forever (same as the Apple ][ BASIC)
        /// </summary>
        [TestMethod]
        public void ForNextDeletesUnneededLoopsDeletesNewerEntries()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(1.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });

            _runEnvironment.CurrentLine = line10;

            _variableRepository.GetOrCreateVariable("A", new short[] { });
            _variableRepository.GetOrCreateVariable("B", new short[] { });
            _variableRepository.GetOrCreateVariable("C", new short[] { });

            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 1, VariableName = "A" });
            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 3, VariableName = "B" });
            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 4, VariableName = "C" });

            forCmd.Execute();

            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
            var newVariableA = _runEnvironment.ProgramStack.Pop();
            Assert.AreEqual("A", newVariableA.VariableName);
            Assert.AreEqual(10, newVariableA.Line.LineNumber.Value);
        }

        /// <summary>
        /// When we create a for next loop, we search back down the stack for a loop with the same name.
        /// We delete newer entries to stop the stack growing forever (same as the Apple ][ BASIC)
        /// </summary>
        [TestMethod]
        public void ForNextWithNewNameLeavesOlderEntries()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(1.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("D"), _equalToken, _toToken, _colonToken });

            _runEnvironment.CurrentLine = line10;

            _variableRepository.GetOrCreateVariable("A", new short[] { });
            _variableRepository.GetOrCreateVariable("B", new short[] { });
            _variableRepository.GetOrCreateVariable("C", new short[] { });

            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 1, VariableName = "A" });
            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 3, VariableName = "B" });
            _runEnvironment.ProgramStack.Push(new StackEntry { LineToken = 4, VariableName = "C" });

            forCmd.Execute();

            Assert.AreEqual(4, _runEnvironment.ProgramStack.Count);
            var newVariableD = _runEnvironment.ProgramStack.Pop();
            var oldVariableC = _runEnvironment.ProgramStack.Pop();
            var oldVariableB = _runEnvironment.ProgramStack.Pop();
            var oldVariableA = _runEnvironment.ProgramStack.Pop();
            Assert.AreEqual("D", newVariableD.VariableName);
            Assert.AreEqual(10, newVariableD.Line.LineNumber.Value);
            Assert.AreEqual("A", oldVariableA.VariableName);
            Assert.AreEqual("B", oldVariableB.VariableName);
            Assert.AreEqual("C", oldVariableC.VariableName);
        }

        /// <summary>
        /// For next loop loop tests for stack overflow.
        /// </summary>
        [TestMethod]
        public void ForNextTestsForStackoverflow()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(1.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });

            _runEnvironment.CurrentLine = line10;
            for (int i = 0; i < 50; i++)
            {
                _runEnvironment.ProgramStack.Push(new StackEntry());
            }

            // Check we don't overflow just yet.
            _runEnvironment.TestForStackOverflow();
            Test.Throws<OutOfMemoryException>(forCmd.Execute);
        }

        /// <summary>
        /// For next loop can loop from 1 to 3 without a variable.
        /// </summary>
        [TestMethod]
        public void ForNextLoopWithoutVariableTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(3.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("1") });

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

            // Did we leave behind the token.
            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("1", token.Text);

            // Variable should be 4.0
            Assert.AreEqual(4.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// Nested For next loop can loop from 1 to 2 and 3 to 4 without variables.
        /// </summary>
        [TestMethod]
        public void NestedForNextLoopWithoutVariableTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(2.0))
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(4.0))
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(4.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("B"), _equalToken, _toToken, _colonToken });
            var line30 = new ProgramLine(30, new List<IToken> { new Token("2") });
            var line40 = new ProgramLine(40, new List<IToken> { new Token("1") });

            // outer loop
            _runEnvironment.CurrentLine = line10;
            forCmd.Execute();
            Assert.AreEqual(1.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
            var loopBackTokenA = _runEnvironment.CurrentLine.CurrentToken;

            // inner loop
            double expectedAValue = 1.0;

            RunnerInnerLoop(forCmd, nextCmd, line20, line30, expectedAValue,  "2");

            // variable should be A=1,B=5
            // Variable should be 5.0
            Assert.AreEqual(expectedAValue, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);

            // Execute next,
            line40.CurrentToken = 0;
            _runEnvironment.CurrentLine = line40;
            nextCmd.Execute();

            // Should be back to just after for loop.
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackTokenA, _runEnvironment.CurrentLine.CurrentToken);

            // variable should be A=2,B=5
            // Variable should be 5.0
            Assert.AreEqual(2.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1.0, _runEnvironment.ProgramStack.Count);

            // Do the inner loop.
            expectedAValue = 2.0;
            RunnerInnerLoop(forCmd, nextCmd, line20, line30, expectedAValue, "2");

            // Execute next,
            line40.CurrentToken = 0;
            _runEnvironment.CurrentLine = line40;
            nextCmd.Execute();

            // Should be have finished loops.
            Assert.AreEqual(40, _runEnvironment.CurrentLine.LineNumber.Value);

            // Did we leave behind the token.
            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("1", token.Text);

            // variable should be A=3,B=5
            // Variable should be 5.0
            Assert.AreEqual(3.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// Nested For next loop can loop from 1 to 2 and 3 to 4 with variables.
        /// </summary>
        [TestMethod]
        public void NestedForNextLoopWithVariableTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(2.0))
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(4.0))
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(4.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("B"), _equalToken, _toToken, _colonToken });
            var line30 = new ProgramLine(30, new List<IToken> { new Token("B"), new Token("2") });
            var line40 = new ProgramLine(40, new List<IToken> { new Token("A"), new Token("1") });

            // outer loop
            _runEnvironment.CurrentLine = line10;
            forCmd.Execute();
            Assert.AreEqual(1.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
            var loopBackTokenA = _runEnvironment.CurrentLine.CurrentToken;

            // inner loop
            double expectedAValue = 1.0;

            RunnerInnerLoop(forCmd, nextCmd, line20, line30, expectedAValue, "2");

            // variable should be A=1,B=5
            // Variable should be 5.0
            Assert.AreEqual(expectedAValue, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);

            // Execute next,
            line40.CurrentToken = 0;
            _runEnvironment.CurrentLine = line40;
            nextCmd.Execute();

            // Should be back to just after for loop.
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackTokenA, _runEnvironment.CurrentLine.CurrentToken);

            // variable should be A=2,B=5
            // Variable should be 5.0
            Assert.AreEqual(2.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1.0, _runEnvironment.ProgramStack.Count);

            // Do the inner loop.
            expectedAValue = 2.0;
            RunnerInnerLoop(forCmd, nextCmd, line20, line30, expectedAValue, "2");

            // Execute next,
            line40.CurrentToken = 0;
            _runEnvironment.CurrentLine = line40;
            nextCmd.Execute();

            // Should be have finished loops.
            Assert.AreEqual(40, _runEnvironment.CurrentLine.LineNumber.Value);

            // Did we leave behind the token.
            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("1", token.Text);

            // variable should be A=3,B=5
            // Variable should be 5.0
            Assert.AreEqual(3.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// Nested For next loop can loop from 1 to 2 and 3 to 4 with comma variables.
        /// </summary>
        [TestMethod]
        public void NestedForNextLoopWithCommaVariableTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(2.0))
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(4.0))
                .Returns(new Accumulator(3.0))
                .Returns(new Accumulator(4.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line20 = new ProgramLine(20, new List<IToken> { new Token("B"), _equalToken, _toToken, _colonToken });
            var line30 = new ProgramLine(30, new List<IToken> { new Token("B"), _commaToken, new Token("A"), new Token("3") });

            // outer loop
            _runEnvironment.CurrentLine = line10;
            forCmd.Execute();
            Assert.AreEqual(1.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);
            var loopBackTokenA = _runEnvironment.CurrentLine.CurrentToken;

            // inner loop
            double expectedAValue = 1.0;

            RunnerInnerLoop(forCmd, nextCmd, line20, line30, expectedAValue, ":");

            expectedAValue = 2.0;

            // variable should be A=2,B=5
            // Variable should be 5.0
            Assert.AreEqual(expectedAValue, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(1, _runEnvironment.ProgramStack.Count);

            // Should be back to just after outer for loop.
            Assert.AreEqual(10, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackTokenA, _runEnvironment.CurrentLine.CurrentToken);

            Assert.AreEqual(1.0, _runEnvironment.ProgramStack.Count);

            // Do the inner loop.
            RunnerInnerLoop(forCmd, nextCmd, line20, line30, expectedAValue, "3");

            // Should be have finished loops.
            Assert.AreEqual(30, _runEnvironment.CurrentLine.LineNumber.Value);

            // Did we leave behind the token.
            var token = _runEnvironment.CurrentLine.NextToken();
            Assert.AreEqual("3", token.Text);

            // variable should be A=3,B=5
            // Variable should be 5.0
            Assert.AreEqual(3.0, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(5.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(0, _runEnvironment.ProgramStack.Count);
        }

        /// <summary>
        /// For next loop with wrong variable name throws exception.
        /// </summary>
        [TestMethod]
        public void ForNextLoopWithWrongVariableTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(4.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line30 = new ProgramLine(30, new List<IToken> { new Token("B"), new Token("2") });

            // Execute for
            _runEnvironment.CurrentLine = line10;
            forCmd.Execute();
            line30.CurrentToken = 0;
            _runEnvironment.CurrentLine = line30;
            Test.Throws<NextWithoutForException>(nextCmd.Execute);
        }

        /// <summary>
        /// For next loop with intermediate gosub and variable name.
        /// </summary>
        [TestMethod]
        public void ForNextLoopWithIntermediateGosubThrowsErrorTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(4.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line30 = new ProgramLine(30, new List<IToken> { new Token("A"), new Token("2") });

            // Execute for
            _runEnvironment.CurrentLine = line10;
            forCmd.Execute();

            // Pretend we've done a gosub
            _runEnvironment.ProgramStack.Push(new StackEntry());
            line30.CurrentToken = 0;
            _runEnvironment.CurrentLine = line30;
            Test.Throws<NextWithoutForException>(nextCmd.Execute);
        }

        /// <summary>
        /// For next loop with intermediate gosub and no variable name.
        /// </summary>
        [TestMethod]
        public void ForNextLoopWithNoVariableAndIntermediateGosubThrowsErrorTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(4.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line30 = new ProgramLine(30, new List<IToken> { new Token("2") });

            // Execute for
            _runEnvironment.CurrentLine = line10;
            forCmd.Execute();

            // Pretend we've done a gosub
            _runEnvironment.ProgramStack.Push(new StackEntry());
            line30.CurrentToken = 0;
            _runEnvironment.CurrentLine = line30;
            Test.Throws<NextWithoutForException>(nextCmd.Execute);
        }

        /// <summary>
        /// For next loop with empty stack and no variable name.
        /// </summary>
        [TestMethod]
        public void ForNextLoopWithNoVariableAndEmptyStackThrowsErrorTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(4.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line30 = new ProgramLine(30, new List<IToken> { new Token("2") });

            _runEnvironment.CurrentLine = line30;
            Test.Throws<NextWithoutForException>(nextCmd.Execute);
        }

        /// <summary>
        /// For next loop with number instead of variable when using commas throws syntax error.
        /// </summary>
        [TestMethod]
        public void ForNextLoopWithNumberInsteadOfVariableThrowsErrorTest()
        {
            SetupSut();
            var forCmd = new For(_runEnvironment, _mockExpressionEvaluator.Object, _variableRepository);
            var nextCmd = new Next(_runEnvironment, _variableRepository);
            _mockExpressionEvaluator.SetupSequence(mee => mee.GetExpression())
                .Returns(new Accumulator(1.0))
                .Returns(new Accumulator(1.0));
            var line10 = new ProgramLine(10, new List<IToken> { new Token("A"), _equalToken, _toToken, _colonToken });
            var line30 = new ProgramLine(30, new List<IToken> { new Token("A"), _commaToken, new Token("2") });

            // Execute for
            _runEnvironment.CurrentLine = line10;
            forCmd.Execute();

            line30.CurrentToken = 0;
            _runEnvironment.CurrentLine = line30;
            Test.Throws<SyntaxErrorException>(nextCmd.Execute);
        }

        private void RunnerInnerLoop(
            For forCmd,
            Next nextCmd,
            ProgramLine line20,
            ProgramLine line30,
            double expectedAValue,
            string expectedTokenText)
        {
            _runEnvironment.CurrentLine = line20;
            line20.CurrentToken = 0;
            forCmd.Execute();
            Assert.AreEqual(3.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(2, _runEnvironment.ProgramStack.Count);
            var loopBackTokenB = _runEnvironment.CurrentLine.CurrentToken;

            // Execute next,
            line30.CurrentToken = 0;
            _runEnvironment.CurrentLine = line30;
            nextCmd.Execute();

            // variable should be A=1,B=4
            Assert.AreEqual(expectedAValue, _variableRepository.GetOrCreateVariable("A", new short[] { }).GetValue().ValueAsDouble());
            Assert.AreEqual(4.0, _variableRepository.GetOrCreateVariable("B", new short[] { }).GetValue().ValueAsDouble());

            // Should be back to just after for loop.
            Assert.AreEqual(20, _runEnvironment.CurrentLine.LineNumber.Value);
            Assert.AreEqual(loopBackTokenB, _runEnvironment.CurrentLine.CurrentToken);

            // Execute next,
            line30.CurrentToken = 0;
            _runEnvironment.CurrentLine = line30;
            nextCmd.Execute();

            // Did we leave behind the token.
            var token = _runEnvironment.CurrentLine.NextToken();
            _runEnvironment.CurrentLine.PushToken(token);
            Assert.AreEqual(expectedTokenText, token.Text);
        }

        private void SetupSut()
        {
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _toToken = new Token("TO", TokenClass.Statement, TokenType.To);
            _colonToken = new Token(":", TokenClass.Seperator, TokenType.Colon);
            _equalToken = new Token("=", TokenClass.Seperator, TokenType.Equal);
            _stepToken = new Token("STEP", TokenClass.Statement, TokenType.Step);
            _commaToken = new Token(",", TokenClass.Seperator, TokenType.Comma);
        }
    }
}