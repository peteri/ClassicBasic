// <copyright file="TokeniserTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using Autofac;
    using ClassicBasic.Interpreter;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test for the tokeniser.
    /// </summary>
    [TestClass]
    public class TokeniserTests
    {
        private static ITokeniser _tokeniser;

        /// <summary>
        /// Initialises the tokeniser.
        /// </summary>
        /// <param name="context">Test context.</param>
        [ClassInitialize]
        public static void SetupSut(TestContext context)
        {
            var builder = new ContainerBuilder();
            RegisterTypes.Register(builder);
            builder.RegisterInstance(new MockTeletype()).As<ITeletype>();
            var container = builder.Build();
            _tokeniser = container.Resolve<ITokeniser>();
        }

        /// <summary>
        /// Basic smoke test that we parse code without spaces.
        /// </summary>
        [TestMethod]
        public void ValidCode()
        {
            var result = _tokeniser.Tokenise("20IFI<>10THENPRINT\"HELLO\"");
            Assert.AreEqual(20, result.LineNumber.Value);
            TokenCheck(result.NextToken(), "IF", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "I", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "<", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), ">", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "10", TokenType.ClassNumber);
            TokenCheck(result.NextToken(), "THEN", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "PRINT", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "HELLO", TokenType.ClassString);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that we find tokens at the end of the line.
        /// </summary>
        [TestMethod]
        public void FindTokensAtEnd()
        {
            var result = _tokeniser.Tokenise("LET AFOR=23");
            TokenCheck(result.NextToken(), "LET", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "A", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "FOR", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "=", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "23", TokenType.ClassNumber);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that we find tokens at the end of the line.
        /// </summary>
        [TestMethod]
        public void FindOddTokensAtEnd()
        {
            var result = _tokeniser.Tokenise("LET AFOR=ONER");
            TokenCheck(result.NextToken(), "LET", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "A", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "FOR", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "=", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "ON", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "ER", TokenType.ClassVariable);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that we convert ? to PRINT.
        /// </summary>
        [TestMethod]
        public void ConvertQuestionMarkToPrint()
        {
            var result = _tokeniser.Tokenise("?\"x\":?");
            TokenCheck(result.NextToken(), "PRINT", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "x", TokenType.ClassString);
            TokenCheck(result.NextToken(), ":", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "PRINT", TokenType.ClassStatement);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that remarks keep spaces and case correctly.
        /// </summary>
        [TestMethod]
        public void RemarksKeepSpacesAndCase()
        {
            var result = _tokeniser.Tokenise("REM   Hello World");
            TokenCheck(result.NextToken(), "REM", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "Hello World", TokenType.ClassRemark);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that DATA keep spaces and case correctly.
        /// </summary>
        [TestMethod]
        public void DataKeepSpacesAndCase()
        {
            var result = _tokeniser.Tokenise("DATA   Hello World,\"DEF GH\"");
            TokenCheck(result.NextToken(), "DATA", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "Hello World,\"DEF GH\"", TokenType.ClassData);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that DATA keep spaces, case and breaks on colon correctly.
        /// </summary>
        [TestMethod]
        public void DataKeepSpacesAndCaseAndBreaksOnColon()
        {
            var result = _tokeniser.Tokenise("DATA   Hello World, \"DEF GH\" : PRINT");
            TokenCheck(result.NextToken(), "DATA", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "Hello World, \"DEF GH\" ", TokenType.ClassData);
            TokenCheck(result.NextToken(), ":", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "PRINT", TokenType.ClassStatement);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that DATA keep spaces, case and breaks on colon correctly.
        /// </summary>
        [TestMethod]
        public void DataKeepSpacesAndCaseAndBreaksOnColonUnlessQuoted()
        {
            var result = _tokeniser.Tokenise("DATA  ab,cd\"ef,\"g:h\" : PRINT");
            TokenCheck(result.NextToken(), "DATA", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "ab,cd\"ef,\"g:h\" ", TokenType.ClassData);
            TokenCheck(result.NextToken(), ":", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "PRINT", TokenType.ClassStatement);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that blank DATA adds ClassDataToken.
        /// </summary>
        [TestMethod]
        public void BlankDataAddsClassDataTokenWithNoColon()
        {
            var result = _tokeniser.Tokenise("DATA    ");
            TokenCheck(result.NextToken(), "DATA", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), string.Empty, TokenType.ClassData);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that blank DATA adds ClassDataToken.
        /// </summary>
        [TestMethod]
        public void BlankDataAddsClassDataTokenWithColon()
        {
            var result = _tokeniser.Tokenise("DATA    : PRINT");
            TokenCheck(result.NextToken(), "DATA", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), string.Empty, TokenType.ClassData);
            TokenCheck(result.NextToken(), ":", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "PRINT", TokenType.ClassStatement);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that the tokeniser terminates strings that don't have a closing quote.
        /// </summary>
        [TestMethod]
        public void TokeniserTerminatesStrings()
        {
            var result = _tokeniser.Tokenise("PRINT \"Hello World");
            TokenCheck(result.NextToken(), "PRINT", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "Hello World", TokenType.ClassString);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Test that the tokeniser terminates strings that don't have a closing quote.
        /// </summary>
        [TestMethod]
        public void TokeniserSplitsVariablesBeforeStrings()
        {
            var result = _tokeniser.Tokenise("PRINT AB\"Hello World");
            TokenCheck(result.NextToken(), "PRINT", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "AB", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "Hello World", TokenType.ClassString);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Check we split correctly where the token is at the end.
        /// </summary>
        [TestMethod]
        public void SplitAwkard()
        {
            var result = _tokeniser.Tokenise("ACRIGHT$(C$,1)");
            TokenCheck(result.NextToken(), "AC", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "RIGHT$", TokenType.ClassFunction);
            TokenCheck(result.NextToken(), "(", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "C", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "$", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), ",", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "1", TokenType.ClassNumber);
            TokenCheck(result.NextToken(), ")", TokenType.ClassSeperator);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// CONT is tricky we initially ended up parsing it as C ON T.
        /// </summary>
        [TestMethod]
        public void SplitAwkardCont()
        {
            var result = _tokeniser.Tokenise("CON:CONT:CONX:ONX:CON");
            TokenCheck(result.NextToken(), "C", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "ON", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), ":", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "CONT", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), ":", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "C", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "ON", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "X", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), ":", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "ON", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "X", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), ":", TokenType.ClassSeperator);
            TokenCheck(result.NextToken(), "C", TokenType.ClassVariable);
            TokenCheck(result.NextToken(), "ON", TokenType.ClassStatement);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Special case
        /// </summary>
        [TestMethod]
        public void SplitAwkardCont2()
        {
            var result = _tokeniser.Tokenise("ONX");
            TokenCheck(result.NextToken(), "ON", TokenType.ClassStatement);
            TokenCheck(result.NextToken(), "X", TokenType.ClassVariable);
            Assert.IsTrue(result.EndOfLine);
        }

        /// <summary>
        /// Tokeniser throws syntax error if line number > 65535
        /// </summary>
        [TestMethod]
        public void TokeniserThrowsSyntaxErrorIfLineNumberToBig()
        {
            bool exceptionThrown = false;
            try
            {
                _tokeniser.Tokenise("65536 ONX");
            }
            catch (ClassicBasic.Interpreter.Exceptions.SyntaxErrorException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        private void TokenCheck(IToken token, string text, TokenType tokenType)
        {
            Assert.AreEqual(text, token.Text);
            Assert.AreEqual(tokenType, token.TokenClass);
        }
    }
}
