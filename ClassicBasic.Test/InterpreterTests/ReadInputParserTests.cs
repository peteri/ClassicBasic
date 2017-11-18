// <copyright file="ReadInputParserTests.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Test.InterpreterTests
{
    using System.Collections.Generic;
    using ClassicBasic.Interpreter;
    using ClassicBasic.Interpreter.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test the ReadInputParser
    /// </summary>
    [TestClass]
    public class ReadInputParserTests
    {
        private ReadInputParser _sut;
        private IVariableRepository _variableRepository;
        private VariableReference[] _numericVariables = new VariableReference[3];
        private VariableReference[] _stringVariables = new VariableReference[3];
        private Queue<string> _inputQueue;

        /// <summary>
        /// Read input parser, can parse across multiple lines.
        /// </summary>
        [TestMethod]
        public void ReadInputParserParsesAcrossMultipleLines()
        {
            SetupSut();
            _inputQueue.Enqueue("   A BC  ,12.3,  \"dcd,\"");
            _inputQueue.Enqueue("12.0, ABC\"DEF , 4.0");
            for (int a = 0; a < 3; a++)
            {
                _sut.ReadVariables(new List<VariableReference> { _stringVariables[a], _numericVariables[a] });
            }

            Assert.AreEqual("A BC  ", _stringVariables[0].GetValue().ValueAsString());
            Assert.AreEqual("dcd,", _stringVariables[1].GetValue().ValueAsString());
            Assert.AreEqual("ABC\"DEF ", _stringVariables[2].GetValue().ValueAsString());
            Assert.AreEqual(12.3, _numericVariables[0].GetValue().ValueAsDouble());
            Assert.AreEqual(12.0, _numericVariables[1].GetValue().ValueAsDouble());
            Assert.AreEqual(4.0, _numericVariables[2].GetValue().ValueAsDouble());
            Assert.IsFalse(_sut.HasExtraData);
        }

        /// <summary>
        /// Read input parser, skips trailing spaces on string.
        /// </summary>
        [TestMethod]
        public void ReadInputParserSkipsTrailingSpacesOfString()
        {
            SetupSut();
            _inputQueue.Enqueue("\"dcd,\"    ,10");
            _sut.ReadVariables(new List<VariableReference> { _stringVariables[0], _numericVariables[0] });

            Assert.AreEqual("dcd,", _stringVariables[0].GetValue().ValueAsString());
            Assert.AreEqual(10.0, _numericVariables[0].GetValue().ValueAsDouble());
            Assert.IsFalse(_sut.HasExtraData);
        }

        /// <summary>
        /// Read input parser, treats empty line as empty string or 0.
        /// </summary>
        [TestMethod]
        public void ReadInputParserTreatsEmptyLineAsEmptyStringOrZero()
        {
            SetupSut();
            _inputQueue.Enqueue(string.Empty);
            _inputQueue.Enqueue(string.Empty);
            _sut.ReadVariables(new List<VariableReference> { _stringVariables[0], _numericVariables[0] });

            Assert.AreEqual(string.Empty, _stringVariables[0].GetValue().ValueAsString());
            Assert.AreEqual(0.0, _numericVariables[0].GetValue().ValueAsDouble());
            Assert.IsFalse(_sut.HasExtraData);
        }

        /// <summary>
        /// Read input parser, treats empty comma as empty string or 0.
        /// </summary>
        [TestMethod]
        public void ReadInputParserTreatsEmptyCommasAsZeroOrEmptyString()
        {
            SetupSut();
            _inputQueue.Enqueue("   A BC  ,,\"dcd,\"");
            _inputQueue.Enqueue("12.0,  , 4.0");
            for (int a = 0; a < 3; a++)
            {
                _sut.ReadVariables(new List<VariableReference> { _stringVariables[a], _numericVariables[a] });
            }

            Assert.AreEqual("A BC  ", _stringVariables[0].GetValue().ValueAsString());
            Assert.AreEqual("dcd,", _stringVariables[1].GetValue().ValueAsString());
            Assert.AreEqual(string.Empty, _stringVariables[2].GetValue().ValueAsString());
            Assert.AreEqual(0.0, _numericVariables[0].GetValue().ValueAsDouble());
            Assert.AreEqual(12.0, _numericVariables[1].GetValue().ValueAsDouble());
            Assert.AreEqual(4.0, _numericVariables[2].GetValue().ValueAsDouble());
            Assert.IsFalse(_sut.HasExtraData);
        }

        /// <summary>
        /// Read input parser, extra data returns true when extra value on line.
        /// </summary>
        [TestMethod]
        public void ReadInputParserExtraDataReturnsTrueWhenExtraValueOnLine()
        {
            SetupSut();
            _inputQueue.Enqueue("ABC,3,4");
            _sut.ReadVariables(new List<VariableReference> { _stringVariables[0], _numericVariables[0] });

            Assert.AreEqual("ABC", _stringVariables[0].GetValue().ValueAsString());
            Assert.AreEqual(3.0, _numericVariables[0].GetValue().ValueAsDouble());
            Assert.IsTrue(_sut.HasExtraData);
        }

        /// <summary>
        /// Read input parser, Throws syntax error if not a number.
        /// Read input parser, Throws syntax error if quoted string has text outside of quotes.
        /// </summary>
        /// <param name="input">Input typed by user.</param>
        /// <param name="throwsException">Throws exception</param>
        [DataTestMethod]
        [DataRow("\"ABC\" ,4", false)]
        [DataRow("3,ABC,4", true)]
        [DataRow("\"ABC\" 3,4", true)]
        public void ReadInputParserThrowsSyntaxErrorIfNotANumber(string input, bool throwsException)
        {
            SetupSut();
            _inputQueue.Enqueue(input);
            Test.Throws<SyntaxErrorException>(
                () => _sut.ReadVariables(new List<VariableReference> { _stringVariables[0], _numericVariables[0] }),
                throwsException);
        }

        private void SetupSut()
        {
            _inputQueue = new Queue<string>();
            _sut = new ReadInputParser(() => _inputQueue.Dequeue());
            _sut.Clear();
            _variableRepository = new VariableRepository();
            _numericVariables[0] = _variableRepository.GetOrCreateVariable("A", new short[] { });
            _numericVariables[1] = _variableRepository.GetOrCreateVariable("B", new short[] { });
            _numericVariables[2] = _variableRepository.GetOrCreateVariable("C", new short[] { });
            _stringVariables[0] = _variableRepository.GetOrCreateVariable("A$", new short[] { });
            _stringVariables[1] = _variableRepository.GetOrCreateVariable("B$", new short[] { });
            _stringVariables[2] = _variableRepository.GetOrCreateVariable("C$", new short[] { });
        }
    }
}
