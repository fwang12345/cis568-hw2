using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public int score;
    public int lives;
    public int round;
    public bool lose;
    public GameObject gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        lives = 3;
        round = 1;
        lose = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        lives = 3;
    }
}
