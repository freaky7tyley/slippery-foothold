using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;


namespace Sensors
{
    internal class Program
    {
        const string celsius = "°C", percent = "%", microgrammpercubicmeter = "µg/m3";

        static int TestMySensorID = -1;
        const string myAirohrUrl = "http://192.168.178.21";

        static void addGrobstaub(int sensorId, double T)
        {
            DbOperation.addMeasurement(sensorId, T, microgrammpercubicmeter, "Grobstaub");
            Console.WriteLine("{0} {1} added", T, microgrammpercubicmeter);
        }
        static void addFeinstaub(int sensorId, double T)
        {
            DbOperation.addMeasurement(sensorId, T, microgrammpercubicmeter, "Feinstaub");
            Console.WriteLine("{0} {1} added", T, microgrammpercubicmeter);
        }
        static void addTemperature(int sensorId, double T)
        {
            DbOperation.addMeasurement(sensorId, T, celsius, "Temperatur");
            Console.WriteLine("{0} {1} added", T, celsius);
        }
        static void addHumidity(int sensorId, double H)
        {
            DbOperation.addMeasurement(sensorId, H, percent, "Luftfeuchte");
            Console.WriteLine("{0} {1} added", H, percent);
        }

        public static void initialInsertions()
        {
            IotSensor thing = new IotSensor()
            {
                url = myAirohrUrl,
            };
            TestMySensorID = DbOperation.addThing(thing, "Airrohr");

        }

        private static void Main()
        {

            //// Remote DB tests



            RemoteDataBaseOperation.PrintMesswerte();

            Console.ReadKey();
            return;

            //// Local DB tests
            //initialInsertions();
            try
            {
                TestMySensorID = DbOperation.getIOTthingID(myAirohrUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Scheinbar gibts diesen Sensor noch nicht, die SampleDB kann/sollte vom User an diesen ort kopiert werden: {0}.", new DataModel().DbPath);
                return;
            }

            string airrohrUrl = DbOperation.getSensorUrl(sensorId: TestMySensorID);

            Airohr climaticSensor = new Airohr(airrohrUrl);

            for (int i = 0; i < 6; i++)
            {
                if (climaticSensor.Read().Result)
                {
                    if (climaticSensor.Temperature.validData)
                        addTemperature(TestMySensorID, climaticSensor.Temperature.data);
                    if (climaticSensor.Humidity.validData)
                        addHumidity(TestMySensorID, climaticSensor.Humidity.data);
                    if (climaticSensor.PM2_5.validData)
                        addFeinstaub(TestMySensorID, climaticSensor.PM2_5.data);
                    if (climaticSensor.PM10.validData)
                        addGrobstaub(TestMySensorID, climaticSensor.PM10.data);
                }
                Thread.Sleep(1000);
            }
            DbOperation.printMeasurements();
            Console.ReadKey();
        }
    }
}