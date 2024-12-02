// <copyright file="Run.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter.Commands
{
    /// <summary>
    /// Implements the RUN command.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Run"/> class.
    /// </remarks>
    /// <param name="runEnvironment">Run time environment.</param>
    /// <param name="programRepository">Program Repository.</param>
    /// <param name="variableRepository">Variable Repository.</param>
    /// <param name="dataStatementReader">Data statement reader.</param>
    public class Run(
        IRunEnvironment runEnvironment,
        IProgramRepository programRepository,
        IVariableRepository variableRepository,
        IDataStatementReader dataStatementReader) : Token("RUN", TokenClass.Statement), ITokeniserCommand
    {

        /// <summary>
        /// Executes the RUN command.
        /// </summary>
        /// <param name="tokeniser">Tokeniser used by the load command.</param>
        public void Execute(ITokeniser tokeniser)
        {
            var nextToken = runEnvironment.CurrentLine?.NextToken();

            if (nextToken.TokenClass == TokenClass.String)
            {
                // Since we have a tokeniser, we can just fake being the executor/interpreter
                // and create our own LOAD command and call it.
                var oldLine = runEnvironment.CurrentLine;
                runEnvironment.CurrentLine = tokeniser.Tokenise($"LOAD {nextToken}");
                var loadToken = runEnvironment.CurrentLine.NextToken() as ITokeniserCommand;
                loadToken?.Execute(tokeniser);
                runEnvironment.CurrentLine = oldLine;
            }
            else
            {
                runEnvironment.CurrentLine.PushToken(nextToken);
            }

            variableRepository.Clear();
            runEnvironment.Clear();
            dataStatementReader.RestoreToLineNumber(null);
            int? startingLineNumber = runEnvironment.CurrentLine.GetLineNumber();
            runEnvironment.CurrentLine = startingLineNumber.HasValue ?
                programRepository.GetLine(startingLineNumber.Value) :
                programRepository.GetFirstLine();
        }
    }
}
