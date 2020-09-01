using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//Deprecated, JSON REST API gets are done in flutter
//So that we learn how to do it :D


public class DataTransferTest : MonoBehaviour
{
    private const string APIKEY = "511678d160e046c731f64a543e97e5a1";

    private Dictionary<string,int> cityIds = new Dictionary<string, int>();

    private ForecastData forecastData;

    private Action<string> onForecastReceived;

    private void Awake(){
        cityIds.Add("Lisboa",2267056);
        cityIds.Add("Leiria",2267094);
        cityIds.Add("Coimbra",2740636);
        cityIds.Add("Porto",2735941);
        cityIds.Add("Faro",2268337);
        onForecastReceived += OnForecastReceived;
    }

    // Start is called before the first frame update
    void Start()
    {
        int CityID = cityIds["Leiria"];
        string uri = $"http://api.openweathermap.org/data/2.5/forecast?id={CityID}&appid={APIKEY}";
        //Leiria: http://api.openweathermap.org/data/2.5/forecast?id=2267094&appid=511678d160e046c731f64a543e97e5a1
        StartCoroutine(GetRequest(uri));
    }

    private void OnForecastReceived(string data){
        Debug.Log("Incoming data...");
        try{
            forecastData = JsonUtility.FromJson<ForecastData>(data);
        }catch(Exception e){
            Debug.LogError($"Error on message received: {e.ToString()}:");
            Debug.LogError(e.Message);
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                if(onForecastReceived != null){
                    onForecastReceived(webRequest.downloadHandler.text);
                }
            }
        }
    }
}
