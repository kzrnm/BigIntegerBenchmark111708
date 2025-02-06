using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CoreRun;
using BenchmarkDotNet.Toolchains.CsProj;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

public class BenchmarkConfig : ManualConfig
{
    static void Main(string[] args)
    {
#if DEBUG
        BenchmarkSwitcher.FromAssembly(typeof(BenchmarkConfig).Assembly).Run(args, new DebugInProcessConfig());
#else
        _ = BenchmarkRunner.Run(typeof(Benchmark).Assembly);
#endif
    }
    public BenchmarkConfig()
    {
        AddDiagnoser(MemoryDiagnoser.Default);
        AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub);
        //AddJob(Job.ShortRun.WithToolchain(CsProjClassicNetToolchain.Net481));
        //AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp31));
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp80));
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp90));
        SummaryStyle = SummaryStyle.Default
        .WithRatioStyle(BenchmarkDotNet.Columns.RatioStyle.Value)
        //.WithTimeUnit(Perfolizer.Horology.TimeUnit.Millisecond)
        ;
    }
}


[Config(typeof(BenchmarkConfig))]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class Benchmark
{
    private char[] _dest = new char[1_000_000];

    BigInteger bigInteger = System.Numerics.BigInteger.One << (1 << 20);
    [Benchmark(Baseline = true)]
    public void SystemRuntimeNumerics() => bigInteger.TryFormat(_dest, out _);

    Net8System.Numerics.BigInteger net8BigInteger = Net8System.Numerics.BigInteger.One << (1 << 20);
    [Benchmark]
    public void Net8Clone() => net8BigInteger.TryFormat(_dest, out _);

    Net9System.Numerics.BigInteger net9BigInteger = Net9System.Numerics.BigInteger.One << (1 << 20);
    [Benchmark]
    public void Net9Clone() => net9BigInteger.TryFormat(_dest, out _);
}
