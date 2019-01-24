#include <algorithm>
#include <cstdlib>
#include <vector>
using namespace std;

extern "C"
{
    typedef void (* GetFibonacciCallback)(int numbers[], int size); 
}

namespace SampleLib
{
    class SampleFuncs
    {
    public:
        void GetFibonacci(int n, GetFibonacciCallback callback);
    private:
        vector<int> CalculateFibonacci(int n);
    };
}