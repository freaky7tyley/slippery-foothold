
using Newtonsoft.Json;

namespace Sensors
{
    class Airohr : JsonSensorData, ISensor, IDHT22, ISDS011
    {
        //bekanntes Problem bei Airrohr, der DHT22 wird nicht immer zuverlässig ausgewertet, dann enthält die Json response KEINE value_type 'temperature' & 'humidity'

        string url;


        public ValueType Temperature { get; private set; }
        public ValueType Humidity { get; private set; }
        public ValueType PM2_5 { get; private set; }
        public ValueType PM10 { get; private set; }

        public Airohr(string url)
        {
            this.url = url;
            Temperature = new ValueType(IDHT22.temp);
            Humidity = new ValueType(IDHT22.hum);
            PM2_5 = new ValueType(ISDS011.fein);
            PM10 = new ValueType(ISDS011.grob);
        }

        protected override async void GetDataFromSensor()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            jsonString = await response.Content.ReadAsStringAsync();
        }

        protected override void ParseJsonString()
        {
            jsonData = JsonConvert.DeserializeObject<JsonDataStructure>(jsonString);
        }

        /// <summary>
        /// Reads the AirRohr
        /// </summary>
        /// <param name="printRaw">Prints the raw json string to the console</param>
        /// <returns>true if data is valid</returns>
        public bool Read(bool printRaw = false)
        {
            try
            {
                GetDataFromSensor();

                if (printRaw)
                    Console.WriteLine("this is my data: {0}",jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading sensor: {0}", ex.Message);
                return false;
            }
            try
            {
                ParseJsonString();
                getTemperatureHumidity();
                getParticels();
            }
            catch (InvalidOperationException)
            {
                //no sequence, beim fehlerhaften lesen 
                Console.WriteLine("No data");
                return false;
            }
            catch (Exception ex)
            {
                //no sequence, beim fehlerhaften lesen des angeschlossenen DHT22
                Console.WriteLine("Error parsing data: {0}",ex.Message);
                return false;
            }
            return true;
        }

        public void getTemperatureHumidity()
        {
            Temperature.parseValueFromJson(jsonData);
            Humidity.parseValueFromJson(jsonData);

            //throwt bei feherhaftem auslesen des DHT22 (kann u.U. erst nach Stunden wieder ok sein)

        }

        public void getParticels()
        {
            PM2_5.parseValueFromJson(jsonData);
            PM10.parseValueFromJson(jsonData);
        }
    }
}

