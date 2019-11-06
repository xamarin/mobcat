using System;
using System.Collections.Generic;
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

        public Task<IEnumerable<int>> GetFibonacciAsync(int n) => SampleFuncsWrapper.GetFibonacciAsync(handle, n);

        public IEnumerable<int> GetFibonacci(int n) => GetFibonacciAsync(n).GetAwaiter().GetResult();

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