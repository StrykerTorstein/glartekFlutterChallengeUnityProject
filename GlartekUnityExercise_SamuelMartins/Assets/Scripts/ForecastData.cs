using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ForecastData
{
    [SerializeField] private int timestamp;
    [SerializeField] private string weather;
    [SerializeField] private int temperature;

    public string Weather { get => weather.ToUpper(); }
    public string Celcius { get => $"{temperature} ºC"; }

    public DateTime DateTime
    {
        get
        {
            return new DateTime(1970, 1, 1).AddSeconds(timestamp);
        }
    }

    public string Date
    {
        get
        {
            return new DateTime(1970, 1, 1).AddSeconds(timestamp).ToString("dd-MM-yyyy HH:mm:ss");
        }
    }

    public override string ToString()
    {
        return $"Date:({Date}) weather:{weather} temperature:{temperature} timestamp:{timestamp}";
    }
}

[SerializeField]
public class ForecastMessage
{
    [SerializeField] private string city;
    [SerializeField] private string timestamp;
    [SerializeField] private List<ForecastData> forecastList;

    public string City { get => city; }

    public List<ForecastData> ForecastDataListJson { get => forecastList; }

    public override string ToString()
    {
        return $"city:{city} time:{timestamp} json:{forecastList}";
    }
}
