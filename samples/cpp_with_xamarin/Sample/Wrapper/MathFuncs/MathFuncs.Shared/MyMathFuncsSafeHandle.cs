using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace MathFuncs
{
    internal class MyMathFuncsSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public MyMathFuncsSafeHandle() : base(true) { }

        public MyMathFuncsSafeHandle(IntPtr handle) : base(true)
        {
            SetHandle(handle);
        }

        public IntPtr Ptr => this.handle;

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            MyMathFuncsWrapper.DisposeMyMathFuncs(this);
            return true;
        }
    }
}