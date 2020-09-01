using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private bool loadTestJson;
    public TextAsset jsonFile;

    [SerializeField] private Text debugConsole;
    [SerializeField] private Text debugTextCity;
    [SerializeField] private Text debugTextTime;
    [SerializeField] private Text debugTextWeather;
    [SerializeField] private Text debugTextTemperature;
    [SerializeField] private Slider dateSlider;
    [SerializeField] private Text dataSliderText;

    bool initialized { get => forecastDict != null; }

    private List<ForecastData> forecastList;
    private List<DateTime> allDates;
    private Dictionary<DateTime, ForecastData> forecastDict;

    void Start()
    {
        if (loadTestJson)
        {
            ForecastMessage forecastMessage = JsonUtility.FromJson<ForecastMessage>(jsonFile.text);

            debugTextCity.text = forecastMessage.City;

            if (forecastMessage == null)
            {
                throw new NullReferenceException($"Forecast Message list is NULL");
            }
            forecastList = forecastMessage.ForecastDataListJson;
            if (forecastList == null)
            {
                throw new NullReferenceException($"Forecast Data List is NULL");
            }
            InitializeFromList(ref forecastList);
        }
    }

    public void SetForecastData(string data)
    {
        if (data == null)
        {
            throw new NullReferenceException("Received Messsage DATA is NULL!");
        }

        //ConsoleWrite("Received Data:");
        //ConsoleWrite(data);
        FlutterUnityPlugin.Message message = FlutterUnityPlugin.Messages.Receive(data);
        if (message == null)
        {
            throw new NullReferenceException("Received Messsage is NULL!");
        }
        ForecastMessage forecastMessage = JsonUtility.FromJson<ForecastMessage>(message.data);

        debugTextCity.text = forecastMessage.City;

        if (forecastMessage == null)
        {
            throw new NullReferenceException($"Forecast Message list is NULL");
        }
        forecastList = forecastMessage.ForecastDataListJson;
        if (forecastList == null)
        {
            throw new NullReferenceException($"Forecast Data List is NULL");
        }
        InitializeFromList(ref forecastList);
    }

    public void InitializeFromList(ref List<ForecastData> forecastList)
    {
        dateSlider.maxValue = forecastList.Count - 1;
        forecastDict = new Dictionary<DateTime, ForecastData>();
        allDates = new List<DateTime>();
        foreach (ForecastData d in forecastList)
        {
            allDates.Add(d.DateTime);
            forecastDict.Add(d.DateTime, d);
        }
        ForecastData closestForecast = FindForecastClosestToPresent();
        int index = forecastList.IndexOf(closestForecast);
        dateSlider.value = index;
        debugTextWeather.text = closestForecast.Weather;
        debugTextTemperature.text = closestForecast.Celcius;
        dataSliderText.text = closestForecast.Date;
        WeatherModelController.instance.Set(closestForecast.Weather);
        debugTextTime.text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
    }

    public void OnDataSliderChanged()
    {
        if (!initialized)
        {
            return;
        }
        int value = Mathf.RoundToInt(dateSlider.value);
        //Debug.Log($"New index: {value}");
        ForecastData selected = forecastList[value];
        debugTextWeather.text = selected.Weather;
        debugTextTemperature.text = selected.Celcius;
        dataSliderText.text = selected.Date;
        WeatherModelController.instance.Set(selected.Weather);
    }

    float counter;

    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 1)
        {
            counter = 0;
            debugTextTime.text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }

    void ConsoleWrite(string text)
    {
        if (debugConsole == null)
        {
            throw new ArgumentNullException("ConsoleWrite with NULL assigned value on the game object!");
        }
        if (String.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException("ConsoleWrite with NULL text");
        }
        debugConsole.text += $"{text}\n";
    }

    private ForecastData FindForecastClosestToPresent()
    {
        if (forecastList == null)
        {
            throw new ArgumentNullException("Reference to forecastList is null on FindForecastClosestToPresent method.");
        }
        if (forecastDict == null)
        {
            throw new ArgumentNullException("Reference to forecastDict is null on FindForecastClosestToPresent method.");
        }
        if (allDates == null)
        {
            throw new ArgumentNullException("Reference to allDates is null on FindForecastClosestToPresent method.");
        }
        DateTime closestDate = allDates
        .Where(x => x <= DateTime.Now)
        .DefaultIfEmpty()
        .Max();
        ForecastData data = forecastDict[closestDate];
        if (data == null)
        {
            throw new ArgumentNullException("Could not find closest date to present...");
        }
        /*
        double now = ConvertToUnixTimestamp(DateTime.Now);
        float diff = int.MaxValue;
        foreach (ForecastData fdata in list)
        {
            double time = ConvertToUnixTimestamp(fdata.DateTime);
            float d = Mathf.Abs(float.Parse((time - now).ToString()));
            if (d < diff){
                diff = d;
                data = fdata;
            }
        }
        data = list[UnityEngine.Random.Range(0,list.Count)];
        dataSliderText.text = $"Date: {data.Date}";
        */
        return data;
    }

    private double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalSeconds);
    }
}
