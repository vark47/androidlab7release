using System;
using System.Linq;
using DAL;
using System.IO;

namespace SQLiteDBCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Place the Tide XML files you want to use to seed the database in the TideFiles folder at the root of this solution folder.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.Clear();

            using(var db = new SQLite.SQLiteConnection("../../../TideSQLite/Assets/tides.db"))
            {
                if (db.CreateTable<Tide>() == 0)
                {
                    db.DeleteAll<Tide>();
                }

                var tideFiles = Directory.GetFiles("../../../TideFiles/", "*.xml");

                Console.WriteLine("{0} XML files found. Parsing now...\n", tideFiles.Length);

                foreach(var tideFile in tideFiles)
                {
                    var tides = TideXMLParser.Parse(tideFile);

                    int rows = db.InsertAll(tides);
                    Console.WriteLine("{0} rows inserted from the file {1}", rows, tideFile);
                }

                Console.WriteLine("\nTesting data retrieval. If the information shown does not look correct, something did not work.\n");

                var table = db.Table<Tide>().ToList();

                var locs = table.Select(t => t.Location).Distinct();


                foreach (var loc in locs)
                {
                    var nowTicks = DateTime.Now.Ticks;
                    var nextTide = db.Table<Tide>().FirstOrDefault(t => t.TimeTicks >= nowTicks && t.Location == loc);
                    var date = new DateTime(nextTide.TimeTicks);

                    Console.Write("Information for the next tide at {1} is:{0}Date/Time: {2}{0}Tide Level: {3} feet; {4} centimeters; {5}{0}{0}", "\n", nextTide.Location, date.ToString("dd/MM/yyyy HH:MM"), nextTide.FeetHeight, nextTide.CentimeterHeight, nextTide.HighLow);
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}
