using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PerfmonCounterSample1
{
    class Program
    {
        public static string ThisProgramName { get; } = System.IO.Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);

        public static bool keepRunning { get; private set; } = true;

        static PerfmonCounter databaseReadsDoneCounter = new PerfmonCounter(ThisProgramName, "ReadsDone", PerfmonCounter.CounterType.ItemCount);

        static PerfmonCounter databaseEventsDoneCounter = new PerfmonCounter(ThisProgramName,"EventsDone", PerfmonCounter.CounterType.ItemCount);

        public static Timer timerReadDatabase;
        public static Timer timerGetData;

        private static bool readDatabaseNeeded = true;
        private const int sampleRateInMillis = 5000;
        private const int numberofSamples = 100;
        private static int countedSamples = 0;

        private const int databaseReadTime = 0;

        static long databaseReadsDone = 0;
        static SimulatedValue lastDatabaseReads = new SimulatedValue(900, 1000);

        static long databaseEventsFound = 0;
        static SimulatedValue lastDatabaseEventsFound = new SimulatedValue(5);

        static void Main(string[] args)
        {
            if (args[1].ToLower().StartsWith("/install"))
            {
                InstallCounters();
            }
            CreateTimers();

            MainProcessing();
        }


        private static void CreateTimers()
        {
            timerReadDatabase = new Timer(ReadDatabaseHandler, null, 0, databaseReadTime);
            timerGetData = new Timer(ReadEventsHandler, null, 15, sampleRateInMillis);
        }

        private static void ReadEventsHandler(object state)
        {
            databaseEventsFound += lastDatabaseEventsFound.Value;
        }

        private static void ReadDatabaseHandler(object state)
        {   // leave flag for reads to happen at top of next loop, no need to read database async for this sample
            readDatabaseNeeded = true;
        }

        private static void MainProcessing()
        {
            do
            {
                if (readDatabaseNeeded) ReadDatabase();

                HandleNewEvents();

                UpdatePerfCounters();
                System.Threading.Thread.Sleep(sampleRateInMillis);
            } while (keepRunning);

        }

        private static void HandleNewEvents()
        {
            if (countedSamples++ > numberofSamples) keepRunning = false;    // simulator exits after this long...
        }

        private static void ReadDatabase()
        {
            databaseReadsDone += lastDatabaseReads.Value;
        }

        // counters have to be installed by Administrator
        private static void InstallCounters()
        {
            databaseReadsDoneCounter.Install();
            databaseEventsDoneCounter.Install();
        }

        private static void UpdatePerfCounters()
        {
            databaseReadsDoneCounter.Report(databaseReadsDone);
            databaseReadsDone = 0;
            databaseEventsDoneCounter.Report(databaseEventsFound);
            databaseEventsFound = 0;
        }

    }

    class SimulatedValue
    {
        private Random rand = null;
        private double multiplier;
        private double minimum;

        public SimulatedValue(double maxValue)
        {
            rand = new Random();
            multiplier = maxValue;
        }
        public SimulatedValue(double minValue, double maxValue)
        {
            rand = new Random();
            minimum = minValue;
            multiplier = (maxValue - minValue);
        }

        public long Value
        {
            get
            {
                //var num = rand.NextDouble(); // 0.0 --- 1.00
                return Convert.ToInt64((rand.NextDouble() * multiplier) + minimum);
            }
        }
    }
}
