# Weather API 
## Introduction
This is a wrapper over the **OpenWeather API**. It requires *Country, City and an Api Key* to query the OpenWeather API and returns *weather description* such as sunny, broken clouds etc. as a response.
### Build & Run
This is a WebApi built with .Net 7 using Visual Studio. Please clone this repository and build and run the solution in Visual Studio. The APIs could be accessed using tools like Postman. 

The solution has been set to listen on https://localhost:5001. Postman requests also have the same hardcoded URL. In case this is changed, URL in Postman will have to be changed as well.

An example of a valid request would be https://localhost:5001/api/weather?country=australi&apikey=validapikey&city=melbourne
### Test
The repository contains a **Postman collection** with requests and a Test collection that can be used to test the solution.
### Working
The solution has a **GET** API **(/api/weather)** that queries the OpenWeather API for weather information.
#### Query String Parameters
1. country
2. city
3. apikey
##### - country:
This is a required parameter.

If it is missing the API will return a *400* response.

If not found, the API will return a *404* response.
##### - city:
This is a required parameter.

If it is missing the API will return a *400* response.

If not found, the API will return a *404* response.
##### - apikey:
This is a required parameter. There is a set of only *5* API Keys in the solution which can be found in *appsettings.json* file. Only 5 requests can be made every hour using an API Key.

If it is missing the API will return a *400* response.

If it is incorrect, the API will return a *401* response.

If the allowable number of requests per API Key exceeds, the API will return a *429* response.
### Requirements:
You are expected to have a valid API key for accessing OpenWeather API. This key should be added to the *appsettings.json* file.