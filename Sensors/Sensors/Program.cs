//#define SWIPE_DB_ON_START

using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;


namespace Sensors
{


    internal class Program
    {


        const string celsius = "°C", percent = "%", microgrammpercubicmeter="µg/m3";

        static int TestMySensorID = -1;


        static void addGrobstaub(int sensorId, double T)
        {
            DbOperation.addMeasurement(sensorId, T, microgrammpercubicmeter, "Grobstaub");
            Console.WriteLine("{0} {1} added", T, microgrammpercubicmeter);
        }
        static void addFeinstaub(int sensorId,double T)
        {
            DbOperation.addMeasurement(sensorId, T, microgrammpercubicmeter,"Feinstaub");
            Console.WriteLine("{0} {1} added", T, microgrammpercubicmeter);
        }
        static void addTemperature(int sensorId, double T)
        {
            DbOperation.addMeasurement(sensorId, T, celsius,"Temperatur");
            Console.WriteLine("{0} {1} added", T, celsius);
        }
        static void addHumidity(int sensorId, double H)
        {
            DbOperation.addMeasurement(sensorId, H, percent,"Luftfeuchte");
            Console.WriteLine("{0} {1} added", H, percent);
        }



        public static void initialInsertions()
        {
            IotSensor thing = new IotSensor()
            {
                url = "http://192.168.178.21/data.json",
            };
           TestMySensorID= DbOperation.addThing(thing, "Airrohr");

        }

        private static void Main()
        {
            DbOperation.clearData();

#if SWIPE_DB_ON_START
            initialInsertions();
#else
            DbOperation.readMeasurements();
#endif

            string airrohrUrl = DbOperation.getSensorUrl(sensorId: TestMySensorID);

            Airohr climaticSensor = new Airohr(airrohrUrl);

            for (int i = 0; i < 6; i++)
            {
                if (climaticSensor.Read())
                {
                    if(climaticSensor.Temperature.validData)
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
            DbOperation.readMeasurements();
        }
    }
}