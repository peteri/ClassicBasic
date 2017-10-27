// <copyright file="Program.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Console
{
    using System;
    using Autofac;
    using ClassicBasic.Interpreter;

    /// <summary>
    /// Host for the basic interpreter.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main program. Passes argument one to interpreter as a run file command.
        /// </summary>
        /// <param name="args">Arguments from the command line.</param>
        public static void Main(string[] args)
        {
            string initialCommand = string.Empty;
            if (args.Length > 1)
            {
                initialCommand = $"RUN \"{args[1]}\"";
            }
            else
            {
                Console.WriteLine("Welcome to Classic BASIC, let us program like it's 1979");
            }

            var builder = new ContainerBuilder();
            builder.RegisterInstance(new GlassTeletype(initialCommand)).As<ITeletype>();
            RegisterTypes.Register(builder);
            try
            {
                var container = builder.Build();
                var interpreter = container.Resolve<IInterpreter>();
                interpreter.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($".Net exception {ex}");
                Console.ReadKey();
            }
        }
    }
}
