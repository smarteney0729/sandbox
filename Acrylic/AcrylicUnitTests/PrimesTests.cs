using Acrylic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AcrylicUnitTests
{
    public class PrimesTests
    {
        [Theory]
        [InlineData(30,new int[]{2,3,5,7,11,13,17,19,23,29})]
        public void GetPrimes_returns_prime_numbers_less_than_given(int limit, int[] primes)
        {
            var service = new PrimeNumberProvider();
            var result = service.GetPrimes(limit);

            Assert.Equal<int>(primes, result);
        }
    }
}
