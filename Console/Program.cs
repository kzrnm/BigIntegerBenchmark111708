extern alias Net8;
extern alias Net9;
extern alias Net9NoOpt;
using System.Diagnostics;
using System.Numerics;

Console.WriteLine(typeof(BigInteger).Assembly);

Run<BigInteger>();
Run<Net8::System.Numerics.BigInteger>();
Run<Net9::System.Numerics.BigInteger>();
Run<Net9NoOpt::System.Numerics.BigInteger>();

static void Run<T>() where T : IBinaryInteger<T>
{
    var sw = Stopwatch.StartNew();
    var b = (T.One << (1 << 20)).ToString("E9", null);
    sw.Stop();
    Console.WriteLine($"ToString {b}");
    Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
}