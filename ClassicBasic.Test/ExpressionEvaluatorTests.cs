// <copyright file="ExpressionEvaluatorTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test
{
    using Autofac;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the Expression Evaluator class.
    /// </summary>
    [TestClass]
    public class ExpressionEvaluatorTests
    {
        private static ITokeniser _tokeniser;
        private IExpressionEvaluator _expressionEvaluator;
        private IRunEnvironment _runEnvironment;
        private IVariableRepository _variableRepository;

        /// <summary>
        /// Initialises the tokeniser.
        /// </summary>
        /// <param name="context">Test context.</param>
        [ClassInitialize]
        public static void SetupTokeniser(TestContext context)
        {
            var builder = new ContainerBuilder();
            RegisterTypes.Register(builder);
            builder.RegisterInstance(new MockTeletype()).As<ITeletype>();
            var container = builder.Build();
            _tokeniser = container.Resolve<ITokeniser>();
        }

        /// <summary>
        /// Setup the expression evaluator.
        /// </summary>
        [TestInitialize]
        public void SetupSut()
        {
            _runEnvironment = new RunEnvironment();
            _variableRepository = new VariableRepository();
            _expressionEvaluator = new ExpressionEvaluator(_variableRepository, _runEnvironment);
        }

        /// <summary>
        /// For a simple case, test the evaluator doesn't skip off the end.
        /// </summary>
        [TestMethod]
        public void EvaluatorDoesNotOverParse()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 \"Hello\":A=A+1");
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual("Hello", value.ValueAsString());
            Assert.AreEqual(TokenType.Colon, _runEnvironment.CurrentLine.NextToken().Seperator);
        }

        /// <summary>
        /// Test that we work left to right.
        /// </summary>
        [TestMethod]
        public void EvaluatorWorksLeftToRight()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 7-2-1");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(4, value.ValueAsShort());
        }

        /// <summary>
        /// Test that putting brackets gives higher priority to the bracketed items.
        /// </summary>
        [TestMethod]
        public void EvaluatorBracketsRaisePriority()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 7-(2-1)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(6, value.ValueAsShort());
        }

        /// <summary>
        /// Test OR.
        /// </summary>
        /// <param name="left">left side of the OR.</param>
        /// <param name="right">right side of the OR.</param>
        /// <param name="result">result of the OR.</param>
        [DataTestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 1)]
        public void TestOr(int left, int right, int result)
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise($"10 PRINT {left} OR {right}");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(result, value.ValueAsShort());
        }

        /// <summary>
        /// Test AND.
        /// </summary>
        /// <param name="left">left side of the AND.</param>
        /// <param name="right">right side of the AND.</param>
        /// <param name="result">result of the AND.</param>
        [DataTestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(1, 1, 1)]
        public void TestAnd(int left, int right, int result)
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise($"10 PRINT {left} AND {right}");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(result, value.ValueAsShort());
        }

        /// <summary>
        /// Test COMPARISON for numbers.
        /// </summary>
        /// <param name="left">left side of the AND.</param>
        /// <param name="comparison">operation</param>
        /// <param name="right">right side of the AND.</param>
        /// <param name="result">result of the AND.</param>
        [DataTestMethod]
        [DataRow(10, "<", 11, 1)]
        [DataRow(10, ">", 11, 0)]
        [DataRow(10, "=", 11, 0)]
        [DataRow(10, "<>", 11, 1)]
        [DataRow(10, "<=", 11, 1)]
        [DataRow(10, ">=", 11, 0)]
        [DataRow(11, "<", 11, 0)]
        [DataRow(11, ">", 11, 0)]
        [DataRow(11, "=", 11, 1)]
        [DataRow(11, "<>", 11, 0)]
        [DataRow(11, "<=", 11, 1)]
        [DataRow(11, ">=", 11, 1)]
        [DataRow(12, "<", 11, 0)]
        [DataRow(12, ">", 11, 1)]
        [DataRow(12, "=", 11, 0)]
        [DataRow(12, "<>", 11, 1)]
        [DataRow(12, "<=", 11, 0)]
        [DataRow(12, ">=", 11, 1)]
        public void TestComparisonNumbers(int left, string comparison, int right, int result)
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise($"10 PRINT {left} {comparison} {right}");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(result, value.ValueAsShort(), $"expected {result} from {left} {comparison} {right}");
        }

        /// <summary>
        /// Test COMPARISON for numbers.
        /// </summary>
        /// <param name="left">left side of the AND.</param>
        /// <param name="comparison">operation</param>
        /// <param name="right">right side of the AND.</param>
        /// <param name="result">result of the AND.</param>
        [DataTestMethod]
        [DataRow("AA", "<", "AB", 1)]
        [DataRow("AA", ">", "AB", 0)]
        [DataRow("AA", "=", "AB", 0)]
        [DataRow("AA", "<>", "AB", 1)]
        [DataRow("AA", "<=", "AB", 1)]
        [DataRow("AA", ">=", "AB", 0)]
        [DataRow("AB", "<", "AB", 0)]
        [DataRow("AB", ">", "AB", 0)]
        [DataRow("AB", "=", "AB", 1)]
        [DataRow("AB", "<>", "AB", 0)]
        [DataRow("AB", "<=", "AB", 1)]
        [DataRow("AB", ">=", "AB", 1)]
        [DataRow("AC", "<", "AB", 0)]
        [DataRow("AC", ">", "AB", 1)]
        [DataRow("AC", "=", "AB", 0)]
        [DataRow("AC", "<>", "AB", 1)]
        [DataRow("AC", "<=", "AB", 0)]
        [DataRow("AC", ">=", "AB", 1)]
        public void TestComparisonString(string left, string comparison, string right, int result)
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise($"10 PRINT \"{left}\" {comparison} \"{right}\"");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(result, value.ValueAsShort(), $"expected {result} from \"{left}\" {comparison} \"{right}\"");
        }

        /// <summary>
        /// Test multiply.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestMultiply()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 3.5 * 2");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(7, value.ValueAsDouble());
        }

        /// <summary>
        /// Test divide.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestDivide()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 7 / 2");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(3.5, value.ValueAsDouble());
        }

        /// <summary>
        /// Test divide by zero.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestDivideByZero()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 7 / 0");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            try
            {
                _expressionEvaluator.GetExpression();
            }
            catch (ClassicBasic.Interpreter.Exceptions.DivisionByZeroException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test Add.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestAdd()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 7 + 2 +3");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(12, value.ValueAsDouble());
        }

        /// <summary>
        /// Test Add strings.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestAddStrings()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT \"ONE\"+\"\"+\"THREE\"");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual("ONETHREE", value.ValueAsString());
        }

        /// <summary>
        /// Test Add strings and numbers.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestAddStringsAndNumbers()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT \"ONE\"+ 2 +\"THREE\"");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            try
            {
                _expressionEvaluator.GetExpression();
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test Add numbers and strings.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestAddNumbersAndStrings()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 3 +  \"ONE\"+ 2 +\"THREE\"");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            try
            {
                _expressionEvaluator.GetExpression();
            }
            catch (ClassicBasic.Interpreter.Exceptions.TypeMismatchException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test Leading minus.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestLeadingMinus()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 10 + -2 -3");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(5, value.ValueAsDouble());
        }

        /// <summary>
        /// Test not.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestNot()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT NOT (10 < 4)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(1.0, value.ValueAsDouble());
        }

        /// <summary>
        /// Test not reversed.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestNotReversed()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT NOT (10 > 4)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(0.0, value.ValueAsDouble());
        }

        /// <summary>
        /// Test not repeated
        /// </summary>
        [TestMethod]
        public void EvaluatorTestNotNot()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT NOT NOT (10 < 4)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(0.0, value.ValueAsDouble());
        }

        /// <summary>
        /// Test power
        /// </summary>
        [TestMethod]
        public void EvaluatorPower()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 4^3");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(64, value.ValueAsDouble());
        }

        /// <summary>
        /// Test power
        /// </summary>
        [TestMethod]
        public void EvaluatorPowerRepeated()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 4^3^2");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(4096, value.ValueAsDouble());
        }

        /// <summary>
        /// Test power with a bit of a complication
        /// </summary>
        [TestMethod]
        public void EvaluatorPowerRepeatedComplicated()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 4^3^2+20*5");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var value = _expressionEvaluator.GetExpression();
            Assert.AreEqual(4196, value.ValueAsDouble());
        }

        /// <summary>
        /// Test missing close bracket
        /// </summary>
        [TestMethod]
        public void EvaluatorTestMissingCloseBracket()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT (1+2*(3+5)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            try
            {
                _expressionEvaluator.GetExpression();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test close bracket eaten correctly
        /// </summary>
        [TestMethod]
        public void EvaluatorTestCloseBracketEaten()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT (1+2*(3+5)):");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.IsFalse(_runEnvironment.CurrentLine.EndOfLine);
            Assert.AreEqual(TokenType.Colon, _runEnvironment.CurrentLine.NextToken().Seperator);
        }

        /// <summary>
        /// Test depth of 65 bracket throws too complex
        /// </summary>
        [TestMethod]
        public void EvaluatorTestComplexExpression()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT " + new string('(', 64) + "3" + new string(')', 64));
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            try
            {
                _expressionEvaluator.GetExpression();
            }
            catch (ClassicBasic.Interpreter.Exceptions.FormulaTooComplex)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Maximum brackets are ok
        /// </summary>
        [TestMethod]
        public void EvaluatorMaximumBrackets()
        {
            var cmd = "10 PRINT " + new string('(', 63) + "3" + new string(')', 63);
            cmd = cmd.Replace("(", "(1+");
            cmd = cmd.Replace(")", "+3)");
            _runEnvironment.CurrentLine = _tokeniser.Tokenise(cmd);
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.AreEqual((63.0 * 4.0) + 3.0, result.ValueAsDouble());
        }

        /// <summary>
        /// Evaluate get line number
        /// </summary>
        [TestMethod]
        public void EvaluatorGetLine()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 goto 2000");
            _runEnvironment.CurrentLine.NextToken();    // Eat the goto
            var result = _expressionEvaluator.GetLineNumber();
            Assert.AreEqual(2000, result);
        }

        /// <summary>
        /// Evaluate get line number returns null if token is not number.
        /// </summary>
        [TestMethod]
        public void EvaluatorGetLineReturnsNullAndTokenIsNotEaten()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 goto print");
            _runEnvironment.CurrentLine.NextToken();    // Eat the goto
            var result = _expressionEvaluator.GetLineNumber();
            Assert.AreEqual(null, result);
            Assert.AreEqual("PRINT", _runEnvironment.CurrentLine.NextToken().Text);
        }

        /// <summary>
        /// Evaluate get line number returns null if unparsable.
        /// </summary>
        [TestMethod]
        public void EvaluatorGetLineThrowsExceptionOnBadLineNumber()
        {
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 goto 20X00");
            _runEnvironment.CurrentLine.NextToken();    // Eat the goto
            var result = _expressionEvaluator.GetLineNumber();
            Assert.IsNull(result);
            Assert.AreEqual("20X00", _runEnvironment.CurrentLine.NextToken().Text);
        }

        /// <summary>
        /// Test evaluator throws syntax exception if bad number evaluated.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestBadNumberException()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT 12.34A");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            try
            {
                _expressionEvaluator.GetExpression();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test evaluator throws syntax exception if statement evaluated.
        /// </summary>
        [TestMethod]
        public void EvaluatorTestStatementException()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT PRINT");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            try
            {
                _expressionEvaluator.GetExpression();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test the evaluator can get a double variable
        /// </summary>
        [TestMethod]
        public void EvaluatorGetsDoubleVariable()
        {
            _variableRepository.GetOrCreateVariable("B", new short[] { }).SetValue(new Accumulator(5.5));
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT B");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.AreEqual(5.5, result.ValueAsDouble());
        }

        /// <summary>
        /// Test the evaluator can get a string variable
        /// </summary>
        [TestMethod]
        public void EvaluatorGetsStringVariable()
        {
            _variableRepository.GetOrCreateVariable("C$", new short[] { }).SetValue(new Accumulator("HELLO"));
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT C$");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.AreEqual("HELLO", result.ValueAsString());
        }

        /// <summary>
        /// Test the evaluator can get a integer variable
        /// </summary>
        [TestMethod]
        public void EvaluatorGetsIntegerVariable()
        {
            _variableRepository.GetOrCreateVariable("D%", new short[] { }).SetValue(new Accumulator(6.7));
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT D%");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.AreEqual(6, result.ValueAsShort());
        }

        /// <summary>
        /// Test the evaluator can get a double array variable
        /// </summary>
        [TestMethod]
        public void EvaluatorGetsDoubleArrayVariable()
        {
            _variableRepository.GetOrCreateVariable("E", new short[] { 3, 5 }).SetValue(new Accumulator(5.5));
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT E(3,5)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.AreEqual(5.5, result.ValueAsDouble());
        }

        /// <summary>
        /// Test the evaluator can get a string array variable
        /// </summary>
        [TestMethod]
        public void EvaluatorGetsStringArrayVariable()
        {
            _variableRepository.GetOrCreateVariable("J", new short[] { }).SetValue(new Accumulator(3.0));
            _variableRepository.GetOrCreateVariable("F$", new short[] { 5, 3 }).SetValue(new Accumulator("HELLO"));
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT F$(5,J)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.AreEqual("HELLO", result.ValueAsString());
        }

        /// <summary>
        /// Test the evaluator can get a integer array variable
        /// </summary>
        [TestMethod]
        public void EvaluatorGetsIntegerArrayVariable()
        {
            _variableRepository.GetOrCreateVariable("K", new short[] { }).SetValue(new Accumulator(7.0));
            _variableRepository.GetOrCreateVariable("G%", new short[] { 7, 1 }).SetValue(new Accumulator(6.7));
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT G%(K,1)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.AreEqual(6, result.ValueAsShort());
        }

        /// <summary>
        /// Test the evaluator can get a integer array variable with integer indexes
        /// </summary>
        [TestMethod]
        public void EvaluatorGetsIntegerArrayVariableWithIntegerIndexes()
        {
            _variableRepository.GetOrCreateVariable("M%", new short[] { }).SetValue(new Accumulator(2.0));
            _variableRepository.GetOrCreateVariable("N%", new short[] { }).SetValue(new Accumulator((short)3));
            _variableRepository.GetOrCreateVariable("G%", new short[] { 2, 3 }).SetValue(new Accumulator((short)8));
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT G%(M%,N%)");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            var result = _expressionEvaluator.GetExpression();
            Assert.AreEqual(8, result.ValueAsShort());
        }

        /// <summary>
        /// Test array elements without closing bracket throws syntax erro.
        /// </summary>
        [TestMethod]
        public void EvaluatorArrayAccessWithoutClosingBracket()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT A(0,0");
            _runEnvironment.CurrentLine.NextToken();    // Eat the print
            try
            {
                _expressionEvaluator.GetExpression();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Test array elements with statement as name throws syntax erro.
        /// </summary>
        [TestMethod]
        public void EvaluatorArrayAccessAttemptedWithStatement()
        {
            var exceptionThrown = false;
            _runEnvironment.CurrentLine = _tokeniser.Tokenise("10 PRINT (0)");
            try
            {
                _expressionEvaluator.GetLeftValue();
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
