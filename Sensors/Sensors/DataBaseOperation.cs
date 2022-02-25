
namespace Sensors
{
    static class DbOperation
    {
        static public void readMeasurements()
        {
            using (var db = new DataModel())
            {
                foreach (var m in db.Measurements)
                {
                    IotSensor sensor = db.IotSensors.Where(s => s.sensorFK.sensorID == m.sensorFK.sensorID).First();

                    Console.WriteLine("{4}:{0} {1}: {2}{3}", sensor.sensorFK.name, m.timestamp, m.value, m.unitFK.unit, m.measurementID);
                }
            }
        }

        static public string getSensorUrl(int sensorId)
        {
            using (var db = new DataModel())
            {
                var sns = db.Sensors.Where(x => x.sensorID == sensorId).First();
                return db.IotSensors.Where(x => x.sensorFK.sensorID == sensorId).First().url;
            }
        }

        /// <summary>
        /// Adds an IOTSensor to database
        /// </summary>
        /// <param name="thing">IOT thing</param>
        /// <param name="name"></param>
        /// <returns>ID of Sensor</returns>
        static public int addThing(IotSensor thing, string name)
        {
            using (var db = new DataModel())
            {
                // Create
                Console.WriteLine("Inserting a new thing and sensor.");
                var sens = db.Sensors.Add(new Sensor() { name = name }).Entity;
                thing.sensorFK = sens;
                db.IotSensors.Add(thing);

                db.SaveChanges();
                
                Console.WriteLine("The inserted sensor's id={0}", sens.sensorID);
                Console.WriteLine("{0} Sensors, {1} things", db.Sensors.Count(), db.IotSensors.Count());

                return sens.sensorID;
            }
        }

        static public MeasurementType addMeasurementTypeIfNotExists(DataModel db, string text)
        {
                var iemt = db.MeasurementTypes.Where(x => x.measurementType.Equals(text));
                if (iemt.Count() > 0)
                    return iemt.First();
                
                var mt=db.MeasurementTypes.Add(new MeasurementType() { measurementType=text }).Entity;
                Console.WriteLine("MeasurementType '{0}' added ", mt.measurementType);
                db.SaveChanges();
                return mt;
        }

        static public Unit addUnitIfNotExists(DataModel db, string unit)
        {
            var ieu = db.Units.Where(x => x.unit.Equals(unit));
            if (ieu.Count() > 0)
                return ieu.First();

            var u = db.Units.Add(new Unit() { unit = unit }).Entity;
            Console.WriteLine("Unit '{0}' added ", u.unit);
            db.SaveChanges();
            return u;
        }

        static public void addMeasurement(int sensorId, double value, string unit, string measurementType)
        {
            using (var db = new DataModel())
            {
                var _unit = addUnitIfNotExists(db, unit);
                var sens = db.Sensors.Where(x => x.sensorID == sensorId).First();
                var mt = addMeasurementTypeIfNotExists(db,measurementType);

                Measurement m = new Measurement() { timestamp = DateTime.Now, sensorFK = sens, value = value, unitFK = _unit, measurementTypeFK=mt };
                
                db.Measurements.Add(m);
                db.SaveChanges();
            }
        }


        static public void clearData()
        {
            using (var db = new DataModel())
            {
                db.Measurements.RemoveRange(db.Measurements);
                db.IotSensors.RemoveRange(db.IotSensors);
                db.Units.RemoveRange(db.Units);
                db.Sensors.RemoveRange(db.Sensors);
                db.MeasurementTypes.RemoveRange(db.MeasurementTypes);
                db.SaveChanges();
            }
        }
    }
}