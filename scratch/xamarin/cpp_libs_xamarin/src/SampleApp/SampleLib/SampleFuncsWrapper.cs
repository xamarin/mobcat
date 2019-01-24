using System;
using System.Runtime.InteropServices;

namespace SampleLib
{
    internal static class SampleFuncsWrapper
    {
#if Android
        const string DllName = "libSample.so";
#else
        const string DllName = "__Internal";
#endif

        internal delegate void GetFibonacciCallback(IntPtr numbers, int size);

        [DllImport(DllName, EntryPoint = nameof(CreateSampleFuncs))]
        internal static extern SampleFuncsSafeHandle CreateSampleFuncs();

        [DllImport(DllName, EntryPoint = nameof(ReleaseSampleFuncs))]
        internal static extern void ReleaseSampleFuncs(SampleFuncsSafeHandle handle);

        [DllImport(DllName, EntryPoint = nameof(GetFibonacci))]
        internal static extern void GetFibonacci(SampleFuncsSafeHandle handle, int n, GetFibonacciCallback callback);
    }
}