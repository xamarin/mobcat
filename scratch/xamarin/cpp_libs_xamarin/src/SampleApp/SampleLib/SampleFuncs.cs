using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SampleLib
{
    public class SampleFuncs : IDisposable
    {
        readonly SampleFuncsSafeHandle handle;
        bool disposed;

        public SampleFuncs()
        {
            handle = SampleFuncsWrapper.CreateSampleFuncs();
        }

        public Task<IEnumerable<int>> GetFibonacciAsync(int n)
        {
            TaskCompletionSource<IEnumerable<int>> tcs = new TaskCompletionSource<IEnumerable<int>>();

            Task.Run(() =>
            {
                SampleFuncsWrapper.GetFibonacci(handle, n, (numberPtr, size) => 
                {
                    int[] numbersArray = new int[size];
                    Marshal.Copy(numberPtr, numbersArray, 0, size);

                    tcs.SetResult(new List<int>(numbersArray));
                });
            });

            return tcs.Task;
        }

        public IEnumerable<int> GetFibonacci(int n)
        {
            return GetFibonacciAsync(n).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (handle != null && !handle.IsInvalid)
                handle.Dispose();

            disposed = true;
        }
    }
}