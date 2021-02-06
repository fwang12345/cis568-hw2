using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillStreakTimer : MonoBehaviour
{
    Global globalObj;
    Slider killstreakSlider;
    // Use this for initialization
    void Start () {
        GameObject g = GameObject.Find("GlobalObject");
        globalObj = g.GetComponent<Global>();
        killstreakSlider = gameObject.GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update () {
        killstreakSlider.value = globalObj.killstreakTimer;
    }
}
