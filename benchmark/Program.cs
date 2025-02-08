extern alias Net8;
extern alias Net9;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using System.Numerics;

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
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp80));
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp90));
        SummaryStyle = SummaryStyle.Default
        .WithRatioStyle(BenchmarkDotNet.Columns.RatioStyle.Value)
        ;
    }
}


[Config(typeof(BenchmarkConfig))]
[HideColumns("Job", "Error", "StdDev", "Median", "RatioSD")]
public class Benchmark
{
    private char[] _dest = new char[1_000_000];

    BigInteger bigInteger = BigInteger.One << (1 << 20);
    [Benchmark(Baseline = true)]
    public void SystemRuntimeNumerics() => bigInteger.TryFormat(_dest, out _);

    Net8::System.Numerics.BigInteger net8BigInteger = Net8::System.Numerics.BigInteger.One << (1 << 20);
    [Benchmark]
    public void Net8Clone() => net8BigInteger.TryFormat(_dest, out _);

    Net9::System.Numerics.BigInteger net9BigInteger = Net9::System.Numerics.BigInteger.One << (1 << 20);
    [Benchmark]
    public void Net9Clone() => net9BigInteger.TryFormat(_dest, out _);
}
