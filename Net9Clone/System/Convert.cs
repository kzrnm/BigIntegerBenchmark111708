using System.Buffers;

#if !NET9_0_OR_GREATER
namespace System
{
    internal static class Convert
    {
        public static OperationStatus FromHexString(ReadOnlySpan<char> source, Span<byte> destination, out int charsConsumed, out int bytesWritten)
        {
            (int quotient, int remainder) = Math.DivRem(source.Length, 2);

            if (quotient == 0)
            {
                charsConsumed = 0;
                bytesWritten = 0;

                return remainder == 1 ? OperationStatus.NeedMoreData : OperationStatus.Done;
            }

            OperationStatus result;

            if (destination.Length < quotient)
            {
                source = source.Slice(0, destination.Length * 2);
                quotient = destination.Length;
                result = OperationStatus.DestinationTooSmall;
            }
            else
            {
                if (remainder == 1)
                {
                    source = source.Slice(0, source.Length - 1);
                    result = OperationStatus.NeedMoreData;
                }
                else
                {
                    result = OperationStatus.Done;
                }

                destination = destination.Slice(0, quotient);
            }

            if (!HexConverter.TryDecodeFromUtf16(source, destination, out charsConsumed))
            {
                bytesWritten = charsConsumed / 2;
                return OperationStatus.InvalidData;
            }

            bytesWritten = quotient;
            charsConsumed = source.Length;
            return result;
        }
    }
}
#endif
