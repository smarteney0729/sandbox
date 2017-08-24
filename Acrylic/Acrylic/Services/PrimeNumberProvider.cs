using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acrylic.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class PrimeNumberProvider
    {
        private int limit = 10000;
        private Guid instance;
        public PrimeNumberProvider()
        {
            instance = new Guid();
        }

        /// <summary>
        /// Returns a list of prime numbers less than the provided <paramref name="maxValue"/>.
        /// A silent limit set at 10,000
        /// </summary>
        /// <param name="maxValue">The max prime number to return.</param>
        /// <returns>Returns a list of prim numbers less than or equal to <paramref name="maxValue"/></returns>
        public List<int> GetPrimes(int maxValue)
        {
            int max = Math.Min(maxValue, limit);
            bool[] primes = new bool[max];

            for (int i = 0; i < max; i++) primes[i] = true;

            for(int i = 2; i < Math.Sqrt(max); i++)
            {
                if (primes[i])
                {
                    for (int j = i * i; j < max; j += i)
                    {
                        primes[j] = false;
                    }
                }
            }

            var result = new List<int>(max);
            for(int i = 2; i < max; i++)
            {
                if (primes[i])
                {
                    result.Add(i);
                }
            }

            return result;

        }
    }
}
