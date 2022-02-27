
namespace Sensors
{
    static class DbOperation
    {
        static public void printMeasurements()
        {
            using (var db = new DataModel())
            {
                var data = from m in db.Measurements
                           join e in db.MeasurementTypes on m.measurementTypeFK equals e
                           join u in db.Units on m.unitFK equals u
                           join s in db.Sensors on m.sensorFK equals s
                           select new
                           {
                               m.measurementID,
                               m.timestamp,
                               m.measurementTypeFK.measurementType,
                               m.value,
                               m.unitFK.unit,
                               m.sensorFK.name 
                           };


                foreach (var m in data)
                    Console.WriteLine("{0}\t{1}\t{2:dd.MM.yy HH:MM:ss}\t{3}\t{4}\t{5}", m.measurementType, m.measurementID, m.timestamp, m.value, m.unit, m.name);
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
                Console.WriteLine("Inserting a new thing and sensor.");
                var sens = db.Sensors.Add(new Sensor() { name = name }).Entity;
                thing.sensorFK = sens;
                db.IotSensors.Add(thing);

                db.SaveChanges();
                
                Console.WriteLine("The inserted sensor's id={0}", sens.sensorID);

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