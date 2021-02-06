using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public int score;
    public int highscore;
    public int lives;
    public int round;
    public bool lose;
    public int killstreak;
    public float killstreakTimer;
    public bool freezeKillstreak;
    public GameObject gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        highscore = PlayerPrefs.GetInt("highscore", highscore);
        lives = 3;
        round = 1;
        killstreak = 0;
        killstreakTimer = 0;
        freezeKillstreak = false;
        lose = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);
        }
        if (!freezeKillstreak) 
        {
            killstreakTimer = Mathf.Max(killstreakTimer - Time.deltaTime, 0);
            if (killstreak > 0 && killstreakTimer <= 0)
            {
                killstreak -= 1;
                killstreakTimer = 1;
            }
        }
    }
    void NewGame()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void GameOver() 
    {
        if (!lose)
        {
            lose = true;
            GameObject gameOver = Instantiate(gameOverText, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            gameOver.transform.SetParent(GameObject.Find("Canvas").transform, false);
            Invoke("NewGame", 3f);
        }
    }
    public void NewRound()
    {
        round += 1;
        killstreak = 0;
        killstreakTimer = 0;
    }
}
