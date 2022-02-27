using System;
namespace Sensors
{
    interface ISensor
    {
        Task<bool> Read(bool printRaw);
    }

    #region Airohrzeug
    /// <summary>
    /// spezifische datenstruktur des airrohrs, siehe Airrohr_response_example.json 
    /// </summary>
    class JsonDataStructure
    {
        public class ValueTupel
        {
            public string value_type { get; set; } = default!;
            public string value { get; set; } = default!;
        }

        public ValueTupel[]? sensordatavalues { get; set; }
        public string software_version { get; set; } = default!;
        public string age { get; set; } = default!;
    }

    class ValueType
    {
        public double data { get; private set; }
        public string key { get; private set; }

        public bool validData { get; private set; }

        public ValueType(string key)
        {
            this.key = key;
        }

        public void parseValueFromJson(JsonDataStructure json)
        {
            try
            {
                if (json.sensordatavalues != null)
                {
                    data = Convert.ToDouble(json.sensordatavalues.Where(t => t.value_type.Equals(key)).First().value);
                    validData = true;
                }
                else
                    validData = false;
            }
            catch (Exception)
            {
                validData = false;
            }
        }
    }

    interface IDHT22
    {
        ValueType Temperature { get; }
        ValueType Humidity { get; }

        const string temp = "temperature";
        const string hum = "humidity";

        //scheitert beim auslesen des DHT22 regelmäßig aufgrund fehlender Daten auf dem device (kann u.U. erst nach Stunden wieder ok sein)
        void getTemperatureHumidity();
    }

    interface ISDS011
    {
        ValueType PM2_5 { get; }
        ValueType PM10 { get; }

        const string fein = "SDS_P2";
        const string grob = "SDS_P1";

        void getParticels();
    }

    abstract class JsonSensorData
    {
        protected string? jsonString;
        protected JsonDataStructure? jsonData;
        abstract protected Task GetDataFromSensor();
        abstract protected void ParseJsonString();
    }
    #endregion
}

