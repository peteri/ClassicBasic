// <copyright file="ListTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.CommandTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests LIST
    /// </summary>
    [TestClass]
    public class ListTests
    {
        private IProgramRepository _programRepository;
        private IRunEnvironment _runEnvironment;
        private MockTeletype _teletype;
        private List _sut;

        /// <summary>
        /// Test the list command.
        /// </summary>
        /// <param name="start">Start line number</param>
        /// <param name="token">Token to seperate numbers</param>
        /// <param name="end">End line number</param>
        /// <param name="expectedLines">Lines we expect to see.</param>
        [DataTestMethod]
        [DataRow(null, ",", 20, "10,20")]
        [DataRow(null, null, null, "10,20,30,40")]
        [DataRow(20, null, null, "20")]
        [DataRow(20, "-", null, "20,30,40")]
        [DataRow(20, "-", 30, "20,30")]
        [DataRow(50, null, null, null)]
        public void ListTestWithOptions(int? start, string token, int? end, string expectedLines)
        {
            var tokens = new List<IToken>();
            var currentLine = new ProgramLine(null, tokens);
            SetupSut();
            _runEnvironment.CurrentLine = currentLine;
            if (start.HasValue)
            {
                tokens.Add(new Token(start.Value.ToString()));
            }

            if (token != null)
            {
                tokens.Add(new Token(
                    token,
                    TokenClass.Seperator,
                    token == "-" ? TokenType.Minus : TokenType.Comma));
            }

            if (end.HasValue)
            {
                tokens.Add(new Token(end.Value.ToString()));
            }

            _sut.Setup();
            int endlessLoop = 10;
            while (!_sut.Execute() && endlessLoop > 0)
            {
                endlessLoop--;
            }

            Assert.AreNotEqual(0, endlessLoop);
            if (expectedLines == null)
            {
                Assert.AreEqual(0, _teletype.Output.Count);
            }
            else
            {
                int cnt = 0;
                var lines = expectedLines.Split(",");
                while (_teletype.Output.Count > 0)
                {
                    var output = _teletype.Output.Dequeue();
                    Assert.IsTrue(output.StartsWith(lines[cnt++]));
                    var crLf = _teletype.Output.Dequeue();
                    Assert.AreEqual(System.Environment.NewLine, crLf);
                }

                Assert.AreEqual(lines.Length, cnt);
            }
        }

        private void SetupSut()
        {
            _programRepository = new ProgramRepository();
            _runEnvironment = new RunEnvironment();
            _teletype = new MockTeletype();
            _programRepository.SetProgramLine(new ProgramLine(10, new List<IToken> { new Token("ONE") }));
            _programRepository.SetProgramLine(new ProgramLine(20, new List<IToken> { new Token("TWO") }));
            _programRepository.SetProgramLine(new ProgramLine(30, new List<IToken> { new Token("THREE") }));
            _programRepository.SetProgramLine(new ProgramLine(40, new List<IToken> { new Token("FOUR") }));
            _sut = new List(_programRepository, _teletype, _runEnvironment);
        }
    }
}
