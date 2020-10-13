using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Remus.Extensions {
    /// <summary>
    /// Provides extension methods for the <see cref="ILGenerator"/> type.
    /// </summary>
    public static class ILGeneratorExtensions {
        public static void EmitStloc(this ILGenerator generator, int index) {
            if (index < 4) {
                generator.Emit(index switch {
                    0 => OpCodes.Stloc_0,
                    1 => OpCodes.Stloc_1,
                    2 => OpCodes.Stloc_2,
                    3 => OpCodes.Stloc_3,
                    _ => throw new ArgumentOutOfRangeException(nameof(index))
                });
            }

            if (index <= byte.MaxValue) {
                generator.Emit(OpCodes.Stloc_S, (byte) index);
            }
            else if (index <= short.MaxValue) {
                generator.Emit(OpCodes.Stloc, (short) index);
            }
            else {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}