using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SampleLib.Test
{
    [TestFixture]
    public class SampleFuncsTests
    {
        SampleFuncs _sampleFuncs = new SampleFuncs();
        List<int> _validFibonacci = new List<int>(new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 });

        [Test]
        public void GetFibonacciTest()
        {
            var sequence = _sampleFuncs.GetFibonacci(_validFibonacci.Count);
            Assert.True(sequence.SequenceEqual(_validFibonacci));
        }
    }
}