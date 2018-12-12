using System;
using System.Runtime.InteropServices;

namespace MathFuncs
{
    public class MyMathFuncs : IDisposable
    {
        private bool disposedValue = false;
        private readonly IntPtr ptr = IntPtr.Zero;

        public MyMathFuncs()
        {
            ptr = MyMathFuncsWrapper.CreateMyMathFuncs();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                try
                {
                    MyMathFuncsWrapper.DisposeMyMathFuncs(ptr);
                }
                finally
                {
                    disposedValue = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MyMathFuncs()
        {
            Dispose(false);
        }

        public double Add(double a, double b)
        {
            return MyMathFuncsWrapper.Add(ptr, a, b);
        }

        public double Subtract(double a, double b)
        {
            return MyMathFuncsWrapper.Subtract(ptr, a, b);
        }

        public double Multiply(double a, double b)
        {
            return MyMathFuncsWrapper.Multiply(ptr, a, b);
        }

        public double Divide(double a, double b)
        {
            return MyMathFuncsWrapper.Divide(ptr, a, b);
        }
    }
}