using System;
using System.Diagnostics;

namespace PerfmonCounterSample1
{
    class PerfmonCounter : IDisposable
    {

        public enum CounterType
        {
            ItemsPerSecond,
            ItemCount
        }

        private String categoryName = "Custom category";
        private String counterName = "Total Things";
        private String categoryHelp = "A category for custom performance counters";
        private String counterHelp = "Total things processed";
        private CounterType counterType = CounterType.ItemCount;
        //private const int sampleRateInMillis = 5000;
        //private const int numberofSamples = 100;

        private PerformanceCounter perfCounter = null;

        public PerfmonCounter(String newCategoryName, String newCounterName, CounterType newCounterType,
                              String newCategoryHelp = "", String newCounterHelp = "")
        {

            categoryName = newCategoryName;
            counterName = newCounterName;
            categoryHelp = newCategoryHelp;
            counterHelp = newCounterHelp;
            counterType = newCounterType;
            Install();
            Initialize();
        }


        public bool Install()
        {
            bool success = false;
            if (PerformanceCounterCategory.Exists(categoryName))
            {
                success = true;
            }
            else
            {
                CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
                CounterCreationData totalItems = new CounterCreationData
                {
                    CounterType = PerformanceCounterType.NumberOfItems64,
                    CounterName = counterName
                };
                counterCreationDataCollection.Add(totalItems);
                var type = counterType == CounterType.ItemCount ?
                                    PerformanceCounterType.NumberOfItems64 :
                                    PerformanceCounterType.AverageBase;
                PerformanceCounterCategory.Create(categoryName, categoryHelp,
                                    PerformanceCounterCategoryType.SingleInstance,
                                    counterData: counterCreationDataCollection);
                success = true;
            }
            return success;
        }

        private void Initialize()
        {
            if (null == perfCounter)
            {
                perfCounter = new PerformanceCounter(categoryName, counterName, false)
                {
                    RawValue = 0
                };
            }
        }

        internal void Report(long databaseEventsDone)
        {
            Initialize();
            perfCounter.IncrementBy(databaseEventsDone);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    perfCounter.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                perfCounter.Dispose();  // JDJohnston - not sure if need this, but...
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PerfmonCounter() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
