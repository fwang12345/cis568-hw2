using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Round : MonoBehaviour
{
    Global globalObj;
    Text roundText;
    // Use this for initialization
    void Start () {
        GameObject g = GameObject.Find("GlobalObject");
        globalObj = g.GetComponent<Global>();
        roundText = gameObject.GetComponent<Text>();
    }
    // Update is called once per frame
    void Update () {
        roundText.text = "Round " + globalObj.round.ToString();
    }
}