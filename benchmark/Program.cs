extern alias Net8;
extern alias Net9;
extern alias Net9NoOpt;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using System.Numerics;
using BigInteger8 = Net8::System.Numerics.BigInteger;
using BigInteger9 = Net9::System.Numerics.BigInteger;
using BigInteger9NoOpt = Net9NoOpt::System.Numerics.BigInteger;

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
        AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(maxDepth: 100, exportHtml: true, exportDiff: true)));
        AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub);
        AddExporter(BenchmarkDotNet.Exporters.MarkdownExporter.GitHub);
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp80));
        AddJob(Job.ShortRun.WithToolchain(CsProjCoreToolchain.NetCoreApp90));
        AddJob(Job.ShortRun.WithToolchain(BenchmarkDotNet.Toolchains.NativeAot.NativeAotToolchain.Net90));
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

    BigInteger8 net8BigInteger = BigInteger8.One << (1 << 20);
    [Benchmark]
    public void Net8Clone() => net8BigInteger.TryFormat(_dest, out _);

    BigInteger9 net9BigInteger = BigInteger9.One << (1 << 20);
    [Benchmark]
    public void Net9Clone() => net9BigInteger.TryFormat(_dest, out _);

    BigInteger9NoOpt net9NoOptBigInteger = BigInteger9NoOpt.One << (1 << 20);
    [Benchmark]
    public void Net9NoOptClone() => net9NoOptBigInteger.TryFormat(_dest, out _);
}
