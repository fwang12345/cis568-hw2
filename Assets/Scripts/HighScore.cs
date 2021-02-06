using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    Global globalObj;
    Text highscoreText;
    // Use this for initialization
    void Start () {
        GameObject g = GameObject.Find("GlobalObject");
        globalObj = g.GetComponent<Global>();
        highscoreText = gameObject.GetComponent<Text>();
    }
    // Update is called once per frame
    void Update () {
        highscoreText.text = "High Score: " + globalObj.highscore.ToString();
    }
}