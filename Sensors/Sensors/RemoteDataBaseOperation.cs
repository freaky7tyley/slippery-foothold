using System;
using System.Linq;
namespace Sensors
{
	public static class RemoteDataBaseOperation
	{
        public static void CountMesswerte()
        {
            try
            {
                using (var db = new RaspberryDatabase())
                {
                    Console.WriteLine("Sensor2Messort contains {0} datasets",db.Sensor2Messort.Count());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void PrintMesswerte()
        {
            try
            {
                using (var db = new RaspberryDatabase())
                {
                    var data = db.Sensor2Messort.Select(x=>x).OrderByDescending(id=>id.id).Take(10).ToList();

                    foreach (var m in data)
                        Console.WriteLine("{0}\t{1:HH:mm:ss dd.MM.yy}\t{2}\t{3}\t{4}\t{5}", m.id, m.time,m.Messort, m.Sensor,  m.status, m.Wert);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

