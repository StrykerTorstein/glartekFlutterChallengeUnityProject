using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherModelController : MonoBehaviour
{
    public static WeatherModelController instance { get; private set; }
    [SerializeField] private GameObject clouds, clear, rain;

    void Awake()
    {
        if(instance != null){
            Debug.LogError("There already is an instance of "+this.GetType().ToString());
        }else{
            instance = this;
        }
        clouds.SetActive(false);
        clear.SetActive(false);
        rain.SetActive(false);
    }

    public void Set(string weather){
        string w = weather.ToLower();
        clouds.SetActive(w.Equals("clouds"));
        clear.SetActive(w.Equals("clear"));
        rain.SetActive(w.Equals("rain"));
        CameraController.instance.Set(weather);
    }
}
