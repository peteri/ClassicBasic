// <copyright file="RegisterTypes.cs" company="Peter Ibbotson">
// (C) Copyright 2017-2024 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.IO.Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Register the types for the interpreter.
    /// </summary>
    public static class RegisterTypes
    {
        /// <summary>
        /// Adds services required for using commands in ClassicBasic.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services
            .AddSingleton<IToken, Commands.Clear>()
            .AddSingleton<IToken, Commands.Clear>()
            .AddSingleton<IToken, Commands.Cont>()
            .AddSingleton<IToken, Commands.Data>()
            .AddSingleton<IToken, Commands.Def>()
            .AddSingleton<IToken, Commands.Del>()
            .AddSingleton<IToken, Commands.Dim>()
            .AddSingleton<IToken, Commands.Edit>()
            .AddSingleton<IToken, Commands.End>()
            .AddSingleton<IToken, Commands.Else>()
            .AddSingleton<IToken, Commands.For>()
            .AddSingleton<IToken, Commands.Get>()
            .AddSingleton<IToken, Commands.Gosub>()
            .AddSingleton<IToken, Commands.Goto>()
            .AddSingleton<IToken, Commands.If>()
            .AddSingleton<IToken, Commands.Input>()
            .AddSingleton<IToken, Commands.Let>()
            .AddSingleton<IToken, Commands.List>()
            .AddSingleton<IToken, Commands.Load>()
            .AddSingleton<IToken, Commands.New>()
            .AddSingleton<IToken, Commands.Next>()
            .AddSingleton<IToken, Commands.On>()
            .AddSingleton<IToken, Commands.OnErr>()
            .AddSingleton<IToken, Commands.Pop>()
            .AddSingleton<IToken, Commands.Print>()
            .AddSingleton<IToken, Commands.Read>()
            .AddSingleton<IToken, Commands.Remark>()
            .AddSingleton<IToken, Commands.Restore>()
            .AddSingleton<IToken, Commands.Resume>()
            .AddSingleton<IToken, Commands.Return>()
            .AddSingleton<IToken, Commands.Run>()
            .AddSingleton<IToken, Commands.Save>()
            .AddSingleton<IToken, Commands.Stop>();
        }

        /// <summary>
        /// Adds services required for using functions in ClassicBasic.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddFunctions(this IServiceCollection services)
        {
            return services
                .AddSingleton<IToken, Functions.Abs>()
                .AddSingleton<IToken, Functions.Asc>()
                .AddSingleton<IToken, Functions.Atn>()
                .AddSingleton<IToken, Functions.CharDollar>()
                .AddSingleton<IToken, Functions.Cos>()
                .AddSingleton<IToken, Functions.Exp>()
                .AddSingleton<IToken, Functions.Fre>()
                .AddSingleton<IToken, Functions.Int>()
                .AddSingleton<IToken, Functions.LeftDollar>()
                .AddSingleton<IToken, Functions.Len>()
                .AddSingleton<IToken, Functions.Log>()
                .AddSingleton<IToken, Functions.MidDollar>()
                .AddSingleton<IToken, Functions.Pos>()
                .AddSingleton<IToken, Functions.RightDollar>()
                .AddSingleton<IToken, Functions.Rnd>()
                .AddSingleton<IToken, Functions.Sgn>()
                .AddSingleton<IToken, Functions.Sin>()
                .AddSingleton<IToken, Functions.Sqr>()
                .AddSingleton<IToken, Functions.StrDollar>()
                .AddSingleton<IToken, Functions.Tan>()
                .AddSingleton<IToken, Functions.Val>();
        }

        /// <summary>
        /// Registers the types contained in the interpreter.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddBasicInterpreter(this IServiceCollection services)
        {
            return services
                .AddCommands()
                .AddFunctions()
                .AddSingleton<IExecutor, Executor>()
                .AddSingleton<ITokeniser, Tokeniser>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IInterpreter, Interpreter>()
                .AddSingleton<IRunEnvironment, RunEnvironment>()
                .AddSingleton<ITokensProvider, TokensProvider>()
                .AddSingleton<IProgramRepository, ProgramRepository>()
                .AddSingleton<IVariableRepository, VariableRepository>()
                .AddSingleton<IDataStatementReader, DataStatementReader>()
                .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
                .AddSingleton<ITeletypeWithPosition, TeletypeWithPosition>();
        }
    }
}
