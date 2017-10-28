// <copyright file="RegisterTypes.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using Autofac;

    /// <summary>
    /// Register the types for the interpreter.
    /// </summary>
    public class RegisterTypes
    {
        /// <summary>
        /// Registers the types contained in the interpreter.
        /// </summary>
        /// <param name="builder">Builder to register.</param>
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterModule(new Modules.RegisterCommands());
            builder.RegisterModule(new Modules.RegisterFunctions());

            // Other stuff we care about
            builder.RegisterType<Executor>().As<IExecutor>().SingleInstance();
            builder.RegisterType<Tokeniser>().As<ITokeniser>().SingleInstance();
            builder.RegisterType<Interpreter>().As<IInterpreter>().SingleInstance();
            builder.RegisterType<RunEnvironment>().As<IRunEnvironment>().SingleInstance();
            builder.RegisterType<TokensProvider>().As<ITokensProvider>().SingleInstance();
            builder.RegisterType<ProgramRepository>().As<IProgramRepository>().SingleInstance();
            builder.RegisterType<VariableRepository>().As<IVariableRepository>().SingleInstance();
            builder.RegisterType<ExpressionEvaluator>().As<IExpressionEvaluator>().SingleInstance();
            builder.RegisterType<TeletypeWithPosition>().As<ITeletypeWithPosition>().SingleInstance();
        }
    }
}
