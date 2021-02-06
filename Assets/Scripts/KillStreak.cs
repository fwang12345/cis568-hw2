using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillStreak : MonoBehaviour
{
    Global globalObj;
    Text killstreakText;
    // Use this for initialization
    void Start () {
        GameObject g = GameObject.Find("GlobalObject");
        globalObj = g.GetComponent<Global>();
        killstreakText = gameObject.GetComponent<Text>();
    }
    // Update is called once per frame
    void Update () {
        killstreakText.text = globalObj.killstreak.ToString();
    }
}