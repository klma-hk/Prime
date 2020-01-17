using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WPFMT
{
    public class VMAsyncDemo : INPCBase
    {
        private ObservableCollection<long> _lstPrimes = new ObservableCollection<long>();
        public ObservableCollection<long> lstPrimes
        {
            get { return _lstPrimes; }
            set
            {
                _lstPrimes = value;
                NotifyPropertyChanged();
            }
        }

        private int tasks = 8;

        [Range(2, 16, ErrorMessage = "Number of tasks must be between 2 to 16")]
        public int Tasks
        {
            get { return tasks; }
            set
            {
                if (value != tasks)
                {
                    tasks = value;
                    ValidateProperty(value);
                    NotifyPropertyChanged();
                }
            }
        }

        private long rangestart = 1;

        [Range(2, Int64.MaxValue - 1, ErrorMessage = "The value must be between 1 to maxium value of 64 bits integer - 1")]
        public long RangeStart
        {
            get { return rangestart; }
            set
            {
                if (value != rangestart)
                {
                    rangestart = value;
                    ValidateProperty(value);
                    CheckValueFromTo();
                    NotifyPropertyChanged();
                }
            }
        }

        private long rangeend = 10000;

        [Range(3, Int64.MaxValue, ErrorMessage = "The value must be between 3 to Maxium value of 64 bits integer")]
        public long RangeEnd
        {
            get { return rangeend; }
            set
            {
                if (value != rangeend)
                {
                    rangeend = value;
                    ValidateProperty(value);
                    CheckValueFromTo();
                    NotifyPropertyChanged();
                }
            }
        }

        private int numberofprimes;
        public int NumberofPrimes
        {
            get { return numberofprimes; }
            set
            {
                if (numberofprimes != value)
                {
                    numberofprimes = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string durationtime;
        public string DurationTime
        {
            get { return durationtime; }
            set
            {
                if (durationtime != value)
                {
                    durationtime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _isrunning = false;
        public bool IsRunning
        {
            get { return _isrunning; }
            set
            {
                if (_isrunning != value)
                {
                    _isrunning = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private RunType runoption;
        public RunType RunOption
        {
            get { return runoption; }
            set
            {
                if (runoption != value)
                {
                    runoption = value;
                    NotifyPropertyChanged();
                }
            }

        }
        public RelayCommand cmdDoCalculation { get; set; }

        private readonly PrimeCalculation PrimeCalculation = new PrimeCalculation();
        public VMAsyncDemo()
        {
            LoadCommand();
        }
        private void LoadCommand()
        {
            cmdDoCalculation = new RelayCommand(async () => await DoCalculation(), (obj) => { return !HasErrors; });
        }
        public async Task DoCalculation()
        {
            try
            {
                IsRunning = true;
                var primes = new List<long>();
                var starttime = DateTime.Now;
                NumberofPrimes = 0;
                DurationTime = "";
                lstPrimes.Clear();

                switch (RunOption)
                {
                    case RunType.Tasks:
                        primes = await PrimeCalculation.GetAllPrimesInTask(tasks, rangestart, rangeend);
                        break;
                    case RunType.Parallel:
                        primes = await PrimeCalculation.GetAllPrimesInParallelFor(rangestart, rangeend);
                        break;
                    case RunType.Asyn:
                        primes = await PrimeCalculation.GetAllPrimes(rangestart, rangeend);
                        break;
                }

                // In case of large volume of data returned, therefore use the multithreading approach 
                // also it can be use the parallel sorting to improve the performance
                await Task.Run(() => { primes.Sort(); });

                foreach (var prime in primes)
                {
                    lstPrimes.Add(prime);
                }

                NumberofPrimes = primes.Count();
                DurationTime = (DateTime.Now - starttime).ToString("mm':'ss':'fff");
                IsRunning = false;

            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }

        }
        private void CheckValueFromTo([CallerMemberName] string keyname = "")
        {
            if (rangeend <= rangestart)
            {
                _Errors.Remove(keyname);
                _Errors.Add(keyname, new List<string>(new string[] { "To value cannot be small than or equal to From value !" }));
                OnErrorsChanged(keyname);
            }

        }
    }
}
