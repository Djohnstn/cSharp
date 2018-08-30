using System;

namespace SqlTest1Core
{
    class Program
    {
        static readonly string PROGID = "{344511C6-9DC2-4C07-A9C6-72EE1BEAC45F}";
        string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SystemAccounting;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        // "AttachDbFileName= myDbFile;"
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}

// https://stackoverflow.com/questions/35292586/creating-local-database-at-run-time-with-visual-studio


// https://stackoverflow.com/questions/10540438/what-is-the-connection-string-for-localdb-for-version-11
