using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WPFMT
{
    public class PrimeCalculation
    {
        public async Task<List<long>> GetAllPrimesInTask(int numberoftasks, long rangestart, long rangeend)
        {
            var tresults = new List<long>();
            var tasks = new Task<List<long>>[numberoftasks];
            var rangescope = GetScopeInRange(numberoftasks, rangestart, rangeend);

            for (int i = 0; i < numberoftasks; i++)
            {
                var p1 = rangescope[i, 0];
                var p2 = rangescope[i, 1];
                tasks[i] = Task.Run<List<long>>(() => { return GetAllPrimes(p1, p2); });
            }

            var WaitforResults = Task.WhenAll(tasks);

            await Task.Run(() => { WaitforResults.Wait(); });

            if (WaitforResults.Status == TaskStatus.RanToCompletion)
            {
                for (int i = 0; i < numberoftasks; i++)
                {
                    tresults.AddRange(tasks[i].Result);
                }
            }

            return tresults;
        }


        public async Task<List<long>> GetAllPrimesInParallelFor(long rangestart, long rangeend)
        {
            var tresults = new List<long>();
            var cb = new ConcurrentBag<long>();

            await Task.Run(() =>
            {
                Parallel.For(rangestart, rangeend, i =>
                {
                    if (IsPrime(i))
                    {
                        cb.Add(i);
                    };
                });
            });

            tresults.AddRange(cb.ToArray());

            return tresults;
        }
        private long[,] GetScopeInRange(int numberoftasks, long rangestart, long rangeend)
        {
            var retval = new long[numberoftasks, 2];
            var increment = (rangeend - rangestart) / numberoftasks;

            var startnumber = rangestart;
            var endnumber = startnumber + increment;

            retval[0, 0] = startnumber;
            retval[0, 1] = endnumber;

            for (int i = 1; i < numberoftasks - 1; i++)
            {
                startnumber = endnumber + 1;
                endnumber = startnumber + increment - 1;

                retval[i, 0] = startnumber;
                retval[i, 1] = endnumber;
            }

            startnumber = endnumber + 1;
            endnumber = rangeend;

            retval[numberoftasks - 1, 0] = startnumber;
            retval[numberoftasks - 1, 1] = endnumber;

            return retval;

        }
        public async Task<List<long>> GetAllPrimes(long rangestart, long rangeend)
        {
            var RetVal = new List<long>();
            await Task.Run(() =>
            {
                for (long i = rangestart; i <= rangeend; i++)
                {
                    if (IsPrime(i))
                    {
                        RetVal.Add(i);
                    }
                }
            });
            return RetVal;
        }
        private bool IsPrime(long number)
        {
            if (number <= 1)
            {
                return false;
            }

            var boundary = (long)(Math.Sqrt(number));
            if (number % 2 != 0)
            {
                for (long i = 3; i <= boundary; i += 2)
                {
                    if (number % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return number == 2 ? true : false;
            }
        }
    }
}
