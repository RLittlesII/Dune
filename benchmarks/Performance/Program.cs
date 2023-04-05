// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;

_ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);