
using Newtonsoft.Json;
using System.IO;

namespace Sensors
{
    class Airohr : JsonSensorData, ISensor, IDHT22, ISDS011
    {
        //bekanntes Problem bei Airrohr, der DHT22 wird nicht immer zuverlässig ausgewertet, dann enthält die Json response KEINE value_type 'temperature' & 'humidity'

        string url;
        const string jsonDataPage = "/data.json"; 

        public ValueType Temperature { get; private set; }
        public ValueType Humidity { get; private set; }
        public ValueType PM2_5 { get; private set; }
        public ValueType PM10 { get; private set; }

        public Airohr(string url)
        {
            this.url = url;
            if (!this.url.EndsWith(jsonDataPage))
                this.url += jsonDataPage;

            Temperature = new ValueType(IDHT22.temp);
            Humidity = new ValueType(IDHT22.hum);
            PM2_5 = new ValueType(ISDS011.fein);
            PM10 = new ValueType(ISDS011.grob);
        }

        protected override async Task GetDataFromSensor()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            jsonString = await response.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// zum debuggen während der fahrt ganz praktisch
        /// </summary>
        void GetDataFromFile()
        {
            using (StreamReader rd = new StreamReader("../../../Airrohr_response_example.json"))
            {
                jsonString = rd.ReadToEnd();
            }
       }

        protected override void ParseJsonString()
        {
            if (String.IsNullOrEmpty(jsonString))
                throw new Exception("jsonString was null");

            jsonData = JsonConvert.DeserializeObject<JsonDataStructure>(jsonString);
        }

        /// <summary>
        /// Reads the AirRohr
        /// </summary>
        /// <param name="printRaw">Prints the raw json string to the console</param>
        /// <returns>true if data is valid</returns>
        public async Task<bool> Read(bool printRaw = false)
        {
            try
            {
                //GetDataFromFile();
                await GetDataFromSensor();

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
            try
            {
                //throwt bei feherhaftem auslesen des DHT22 (kann u.U. erst nach Stunden wieder ok sein)
#pragma warning disable CS8604 // Possible null reference argument.
                Temperature.parseValueFromJson(jsonData);
#pragma warning restore CS8604 // Possible null reference argument.
                Humidity.parseValueFromJson(jsonData);
            }
            catch (InvalidOperationException)
            {
                //no sequence, beim fehlerhaften lesen 
                Console.WriteLine("No data");
            }
        }

        public void getParticels()
        {
            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                PM2_5.parseValueFromJson(jsonData);
#pragma warning restore CS8604 // Possible null reference argument.
                PM10.parseValueFromJson(jsonData);
            }
            catch (InvalidOperationException)
            {
                //no sequence, beim fehlerhaften lesen 
                Console.WriteLine("No data");
            }
        }
    }
}

