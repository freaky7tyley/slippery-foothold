# SENSORS

liest daten aus Sensoren (derzeit Airrohr siehe: https://sensor.community/de/sensors/airrohr/) in eine DB.

## Wozu das eigentlich?

Dient mir als Gehversuch in EF

### Was ist die SampleDB?

Sie beinhaltet Testdaten für offlineexperimente zb. 

### Geht das auch ohne Sensor im Netzwerk?

Ja. 
In Airrohr.cs:67 kann zum probieren die _Airrohr_response_example.json_ gelesen werden.

-------------------------------------------------
            try
            {
                GetDataFromFile();
                //await GetDataFromSensor();
-------------------------------------------------
