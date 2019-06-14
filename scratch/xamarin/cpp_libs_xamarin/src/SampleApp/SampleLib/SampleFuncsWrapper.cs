using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SampleLib
{
    internal static class SampleFuncsWrapper
    {
#if Android
        const string DllName = "libSample.so";
#else
        const string DllName = "__Internal";
#endif

        static ConcurrentDictionary<int, TaskCompletionSource<IEnumerable<int>>> fibonacciCallbacks = new ConcurrentDictionary<int, TaskCompletionSource<IEnumerable<int>>>();
        internal delegate void GetFibonacciCallback(IntPtr numbers, int size);

        [DllImport(DllName, EntryPoint = nameof(CreateSampleFuncs))]
        internal static extern SampleFuncsSafeHandle CreateSampleFuncs();

        [DllImport(DllName, EntryPoint = nameof(ReleaseSampleFuncs))]
        internal static extern void ReleaseSampleFuncs(SampleFuncsSafeHandle handle);

        [DllImport(DllName, EntryPoint = nameof(GetFibonacci))]
        static extern void GetFibonacci(SampleFuncsSafeHandle handle, int n, GetFibonacciCallback callback);

        internal static Task<IEnumerable<int>> GetFibonacciAsync(SampleFuncsSafeHandle handle, int n)
        {
            var tcs = fibonacciCallbacks.GetOrAdd(n, new TaskCompletionSource<IEnumerable<int>>());
            Task.Run(() => GetFibonacci(handle, n, SampleFuncsWrapper.GetFibonacciCallbackDelegate));

            return tcs.Task;
        }

#if __IOS__
        [ObjCRuntime.MonoPInvokeCallback(typeof(SampleFuncsWrapper.GetFibonacciCallback))]
#endif
        static void GetFibonacciCallbackDelegate(IntPtr numberPtr, int size)
        {
            TaskCompletionSource<IEnumerable<int>> tcs = null;

            if (!fibonacciCallbacks.TryRemove(size, out tcs))
                return;

            int[] numbersArray = new int[size];
            Marshal.Copy(numberPtr, numbersArray, 0, size);

            tcs.SetResult(new List<int>(numbersArray));
        }
    }
}