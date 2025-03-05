using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    static void Main()
    {
        uint[] bits = new uint[1 << 15];
        new Random(227).NextBytes(MemoryMarshal.AsBytes<uint>(bits));

        var sw = Stopwatch.StartNew();

        // First convert to base 10^9.
        int cuSrc = bits.Length;
        // A quick conservative max length of base 10^9 representation
        // A uint contributes to no more than 10/9 of 10^9 block, +1 for ceiling of division
        int cuMax = cuSrc * (9 + 1) / 9 + 1;

        uint[] bufferToReturn = bufferToReturn = ArrayPool<uint>.Shared.Rent(cuMax);
        bufferToReturn.AsSpan().Clear();
        ReadOnlySpan<uint> base1E9Value = BitsToBase1E9(bits, bufferToReturn, cuSrc);
        sw.Stop();
        
        Console.WriteLine($"Base $10^9$ Length: {base1E9Value.Trim(0u).Length}");
        Console.WriteLine($"Elapsed: **{sw.ElapsedMilliseconds} msec**");
    }
    static ReadOnlySpan<uint> BitsToBase1E9(ReadOnlySpan<uint> bits, Span<uint> base1E9Buffer, int cuSrc)
    {
        const int TenPowMaxPartial = (int)1e9;
        int cuDst = 0;

        for (int iuSrc = cuSrc; --iuSrc >= 0;)
        {
            uint uCarry = bits[iuSrc];
            for (int iuDst = 0; iuDst < cuDst; iuDst++)
            {
                Debug.Assert(base1E9Buffer[iuDst] < TenPowMaxPartial);

                // Use X86Base.DivRem when stable
                ulong uuRes = ((ulong)base1E9Buffer[iuDst] << 32) | uCarry;
                (ulong quo, ulong rem) = Math.DivRem(uuRes, TenPowMaxPartial);
                uCarry = (uint)quo;
                base1E9Buffer[iuDst] = (uint)rem;
            }
            if (uCarry != 0)
            {
                (uCarry, base1E9Buffer[cuDst++]) = Math.DivRem(uCarry, TenPowMaxPartial);
                if (uCarry != 0)
                    base1E9Buffer[cuDst++] = uCarry;
            }
        }

        return base1E9Buffer.Slice(0, cuDst);
    }
}