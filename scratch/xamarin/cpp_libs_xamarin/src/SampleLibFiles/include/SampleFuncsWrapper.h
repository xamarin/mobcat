#include "SampleFuncs.h"
using namespace SampleLib;

extern "C" {
    SampleFuncs* CreateSampleFuncs();
    void ReleaseSampleFuncs(SampleFuncs* ptr);
    void GetFibonacci(SampleFuncs* ptr, int n, GetFibonacciCallback callback);
}