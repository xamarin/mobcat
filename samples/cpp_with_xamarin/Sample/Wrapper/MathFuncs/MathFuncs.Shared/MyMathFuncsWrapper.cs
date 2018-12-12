using System;
using System.Runtime.InteropServices;

namespace MathFuncs
{
    internal static class MyMathFuncsWrapper
    {
#if Android
        const string DllName = "libMathFuncs.so";
#else
        const string DllName = "MathFuncs";
#endif

        [DllImport(DllName, EntryPoint = "CreateMyMathFuncsClass")]
        internal static extern IntPtr CreateMyMathFuncs();

        [DllImport(DllName, EntryPoint = "DisposeMyMathFuncsClass")]
        internal static extern void DisposeMyMathFuncs(IntPtr ptr);

        [DllImport(DllName, EntryPoint = "MyMathFuncsAdd")]
        internal static extern double Add(IntPtr ptr, double a, double b);

        [DllImport(DllName, EntryPoint = "MyMathFuncsSubtract")]
        internal static extern double Subtract(IntPtr ptr, double a, double b);

        [DllImport(DllName, EntryPoint = "MyMathFuncsMultiply")]
        internal static extern double Multiply(IntPtr ptr, double a, double b);

        [DllImport(DllName, EntryPoint = "MyMathFuncsDivide")]
        internal static extern double Divide(IntPtr ptr, double a, double b);
    }
}