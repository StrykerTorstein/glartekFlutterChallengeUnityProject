using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    [SerializeField] private Color clearColor;
    [SerializeField] private Color cloudsColor;
    [SerializeField] private Color rainColor;

    void Awake()
    {
        if(instance != null){
            Debug.LogError("There already is an instance of "+this.GetType().ToString());
        }else{
            instance = this;
        }
        this.GetComponent<Camera>().backgroundColor = Color.white;
    }

    public void Set(string weather){
        string w = weather.ToLower();
        Color c = Color.white;
        if(w.Equals("clouds")){
            c = cloudsColor;
        }else if(w.Equals("clear")){
            c = clearColor;
        }else if(w.Equals("rain")){
            c = rainColor;
        }
        this.GetComponent<Camera>().backgroundColor = c;
    }
}
