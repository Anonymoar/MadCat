using System;
using System.Runtime.InteropServices;

namespace NutEngine.Utilities
{
    class Hash
    {
        /// <summary>
        /// Fletcher's 16 checksum algorithm.
        /// </summary>
        public static UInt16 Fletcher16<T>(params T[] objects)
            where T : struct
        {
            UInt16 sum1 = 0;
            UInt16 sum2 = 0;

            foreach (var variable in objects) {
                Int32 size = Marshal.SizeOf(variable);
                for (Int32 nByte = 0; nByte < size; nByte++) {
                    sum1 = (UInt16)((sum1 + Marshal.ReadByte(variable, nByte)) % 255);
                    sum2 = (UInt16)((sum2 + sum1) % 255);
                }
            }

            return (UInt16)((sum2 << 8) | sum1);
        }
    }
}
