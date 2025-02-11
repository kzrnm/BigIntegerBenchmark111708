// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

namespace System
{
    internal static class SR
    {
        public static string Format(string format, params object?[] args) => string.Format(format, args);

        public static string Format_TooLarge => nameof(Format_TooLarge);

        public static string Arg_HexStyleNotSupported => nameof(Arg_HexStyleNotSupported);

        public static string Argument_BadFormatSpecifier => nameof(Argument_BadFormatSpecifier);
        public static string Argument_MustBeBigInt => nameof(Argument_MustBeBigInt);
        public static string Argument_MinMaxValue => nameof(Argument_MinMaxValue);
        public static string Argument_InvalidHexStyle => nameof(Argument_InvalidHexStyle);
        public static string Argument_InvalidNumberStyles => nameof(Argument_InvalidNumberStyles);
        public static string ArgumentOutOfRange_NeedNonNegNum => nameof(ArgumentOutOfRange_NeedNonNegNum);

        public static string Overflow_BigIntInfinity => nameof(Overflow_BigIntInfinity);
        public static string Overflow_NotANumber => nameof(Overflow_NotANumber);
        public static string Overflow_UInt32 => nameof(Overflow_UInt32);
        public static string Overflow_UInt64 => nameof(Overflow_UInt64);
        public static string Overflow_UInt128 => nameof(Overflow_UInt128);
        public static string Overflow_Int32 => nameof(Overflow_Int32);
        public static string Overflow_Int64 => nameof(Overflow_Int64);
        public static string Overflow_Int128 => nameof(Overflow_Int128);
        public static string Overflow_Negative_Unsigned => nameof(Overflow_Negative_Unsigned);
        public static string Overflow_Decimal => nameof(Overflow_Decimal);
        public static string Overflow_ParseBigInteger => nameof(Overflow_ParseBigInteger);

        /*

            if ((style & InvalidNumberStyles) != 0)
            {
                e = new ArgumentException(SR.Argument_InvalidNumberStyles, nameof(style));
                return false;
            }
            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            { // Check for hex number
                if ((style & ~NumberStyles.HexNumber) != 0)
                {
                    e = new ArgumentException(SR.Argument_InvalidHexStyle, nameof(style));
         */
    }
}
