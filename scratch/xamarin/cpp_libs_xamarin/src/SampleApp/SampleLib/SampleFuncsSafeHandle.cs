using Microsoft.Win32.SafeHandles;

namespace SampleLib
{
    internal class SampleFuncsSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal SampleFuncsSafeHandle() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            SampleFuncsWrapper.ReleaseSampleFuncs(this);
            return true;
        }
    }
}