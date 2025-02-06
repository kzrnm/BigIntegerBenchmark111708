// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Net8System.Numerics
{
    internal static class NumericsHelpers
    {
        private const int kcbitUint = 32;

        public static void GetDoubleParts(double dbl, out int sign, out int exp, out ulong man, out bool fFinite)
        {
            ulong bits = BitConverter.DoubleToUInt64Bits(dbl);

            sign = 1 - ((int)(bits >> 62) & 2);
            man = bits & 0x000FFFFFFFFFFFFF;
            exp = (int)(bits >> 52) & 0x7FF;
            if (exp == 0)
            {
                // Denormalized number.
                fFinite = true;
                if (man != 0)
                    exp = -1074;
            }
            else if (exp == 0x7FF)
            {
                // NaN or Infinite.
                fFinite = false;
                exp = int.MaxValue;
            }
            else
            {
                fFinite = true;
                man |= 0x0010000000000000;
                exp -= 1075;
            }
        }

        // Do an in-place two's complement. "Dangerous" because it causes
        // a mutation and needs to be used with care for immutable types.
        public static void DangerousMakeTwosComplement(Span<uint> d)
        {
            if (d.Length > 0)
            {
                d[0] = unchecked(~d[0] + 1);

                int i = 1;

                // first do complement and +1 as long as carry is needed
                for (; d[i - 1] == 0 && i < d.Length; i++)
                {
                    d[i] = unchecked(~d[i] + 1);
                }
                // now ones complement is sufficient
                for (; i < d.Length; i++)
                {
                    d[i] = ~d[i];
                }
            }
        }

        public static ulong MakeUInt64(uint uHi, uint uLo)
        {
            return ((ulong)uHi << kcbitUint) | uLo;
        }

        public static uint Abs(int a)
        {
            unchecked
            {
                uint mask = (uint)(a >> 31);
                return ((uint)a ^ mask) - mask;
            }
        }
    }
}
