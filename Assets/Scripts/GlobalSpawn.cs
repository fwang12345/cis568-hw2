using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpawn : MonoBehaviour
{   
    // parameters to spawn objects in correct location
    public float z;
    public float padding;
    float width;
    float height;
    // parameters to move invaders
    public float moveRate;
    public float[] moveTimer;
    public bool[] descend;
    public bool[] left;
    // paramater to spawn missile
    public float missileTimer;
    // parameter to spawn ship
    public float shipTimer;
    // parameter to spawn powerup
    public float powerTimer;
    public GameObject invader;
    public GameObject laserBase;
    public GameObject shield;
    public GameObject ship;
    public GameObject platform;
    public GameObject powerup;
    public GameObject alienboss;
    public Transform[,] invaders;
    public int numInvaders;
    public bool bossDead;
    public List<List<GameObject>> invadersList;
    // Start is called before the first frame update
    void Start()
    {
        z = Camera.main.WorldToScreenPoint(new Vector3(0,0,0)).z;
        padding = Camera.main.WorldToScreenPoint(new Vector3(1, 0, 0)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)).x;
        width = Screen.width;
        height = Screen.height;
        bossDead = false;
        missileTimer = Random.Range(0.5f, 1f);
        shipTimer = Random.Range(10f, 20f);
        powerTimer = Random.Range(20f, 30f);
        NewRound();
    }
    
    void Update() 
    {
        Transform t = gameObject.transform;
        if (numInvaders > 0) 
        {
            // Check if any child has reached the bottom row
            foreach (List<GameObject> column in invadersList) 
            {
                float childPos = Camera.main.WorldToScreenPoint(column[0].transform.position).y;
                if (childPos < height - 18.5f * padding)
                {
                    GameObject.Find("GlobalObject").GetComponent<Global>().GameOver();
                }
            }
            // Spawn missile
            missileTimer -= Time.deltaTime;
            if (missileTimer < 0)
            {
                invadersList[Random.Range(0, invadersList.Count)][0].GetComponent<Invader>().Fire();
                missileTimer = Random.Range(0.5f, 1f);
            }
            // Determine if the invaders should descend
            if (left[left.Length - 1])
            {
                float childPos = Camera.main.WorldToScreenPoint(invadersList[0][0].gameObject.transform.position).x;
                if (childPos < 1.5f * padding) 
                {
                    for (int i = 0; i < descend.Length; i++)
                    {
                        descend[descend.Length - 1] = true;
                    }
                }
            }
            else
            {
                float childPos = Camera.main.WorldToScreenPoint(invadersList[invadersList.Count - 1][0].transform.position).x;
                if (childPos > width - 1.5f * padding) 
                {
                    for (int i = 0; i < descend.Length; i++)
                    {
                        descend[descend.Length - 1] = true;
                    }
                }
            }
            // Move Invaders
            for (int i = moveTimer.Length - 1; i >= 0; i--)
            {
                moveTimer[i] -= Time.deltaTime;
                if (moveTimer[i] < 0) 
                {
                    Vector3 pos = t.position;
                    // Move invaders down
                    if (descend[i]) 
                    {
                        descend[i] = false;
                        if (i > 0)
                        {
                            descend[i - 1] = true;
                        }
                        for (int j = 0; j < 11; j++) 
                        {
                            if (invaders[i, j] != null) 
                            {
                                invaders[i, j].Translate(0, 0, -1.5f);
                            }
                        }
                        left[i] = !left[i];
                    }
                    // Move invaders left
                    else if (left[i])
                    {
                        for (int j = 0; j < 11; j++) 
                        {
                            if (invaders[i, j] != null) 
                            {
                                invaders[i, j].Translate(-0.5f, 0, 0);
                            }
                        }
                    }
                    // Move invaders right
                    else {
                        for (int j = 0; j < 11; j++) 
                        {
                            if (invaders[i, j] != null) 
                            {
                                invaders[i, j].Translate(0.5f, 0, 0);
                            }
                        }
                    }
                    moveTimer[i] = moveRate;
                }
            }
        }
        else {
            if (bossDead)
            {
                bossDead = false;
                Global g = GameObject.Find("GlobalObject").GetComponent<Global>();
                if (!g.lose)
                {
                    g.NewRound();
                    Invoke("NewRound", 3f);
                }
            }
        }
        // Spawn ship
        shipTimer -= Time.deltaTime;
        if (shipTimer <= 0)
        {
            bool shipDir = (Random.Range(0f, 1f) < 0.5f);
            if (shipDir)
            {
                GameObject s = Instantiate(ship, Camera.main.ScreenToWorldPoint(new Vector3(0, height - 2.5f * padding, z)), 
                    Quaternion.identity) as GameObject;
                s.GetComponent<Ship>().velocity = Random.Range(3, 7);
            }
            else
            {
                GameObject s = Instantiate(ship, Camera.main.ScreenToWorldPoint(new Vector3(width, height - 2.5f * padding, z)), 
                    Quaternion.identity) as GameObject;
                s.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
                s.GetComponent<Ship>().velocity = Random.Range(-7, -3);
            }
            shipTimer = Random.Range(10f, 20f);
        }
        // Spawn power up
        if (GameObject.Find("PowerUp(Clone)") == null) {
            powerTimer -= Time.deltaTime;
            if (powerTimer <= 0)
            {
                Instantiate(powerup, Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(padding, width - padding), height - 19f * padding, z)), Quaternion.identity);
                powerTimer = Random.Range(20f, 30f);
            }
        }
    }
    // Updates move rate based on number of invaders alive
    public void UpdateMoveRate() 
    {
        moveRate = Mathf.Log(0.1f * numInvaders + 1, 10);
        if (numInvaders == 0)
        {
            // Spawn Alien Boss
            Invoke("SpawnBoss", 3f);
        }
    }
    void SpawnBoss()
    {
        Instantiate(alienboss, Camera.main.ScreenToWorldPoint(new Vector3(width / 2f - padding, height - 7f * padding, z)), Quaternion.Euler(new Vector3(90,0,0)));
    }
    void NewRound()
    {
        // Destroy old objects
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Shield"))
        {
            Destroy(g);
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("LaserBase"))
        {
            Destroy(g);
        }
        // Initialize variables
        numInvaders = 55;
        UpdateMoveRate();
        left = new bool[5];
        moveTimer = new float[5];
        descend = new bool[5];
        invaders = new Transform[5, 11];
        invadersList = new List<List<GameObject>>();
        for (int i = 0; i < 5; i++) 
        {
            moveTimer[i] = moveRate + moveRate * (4 - i) / 5;
        }
        // Spawn invaders
        int diff = (GameObject.Find("GlobalObject").GetComponent<Global>().round - 1) % 3;
        for (int i = 0; i < 11; i++) 
        {
            List<GameObject> column = new List<GameObject>();
            for (int j = 4; j >= 0; j--) 
            {
                GameObject inv = Instantiate(invader, 
                    Camera.main.ScreenToWorldPoint(new Vector3(
                        padding + 2f * i * padding, height - 4f * padding - 1.5f * (j + diff) * padding, z)), 
                    Quaternion.identity) as GameObject;
                Invader invComp = inv.GetComponent<Invader>();
                invComp.score = (6 - j) / 2 * 10;
                column.Add(inv);
                invComp.columnList = column;
                invComp.row = j;
                invComp.column = i;
                invaders[j, i] = inv.transform;

            }
            invadersList.Add(column);
        }
        // Spawn player laserbase
        Instantiate(laserBase, Camera.main.ScreenToWorldPoint(new Vector3(padding, height - 19f * padding, z)), Quaternion.identity);
        // Spawn shields
        float space = (width - 8 * padding) / 3f;
        for (int i = 0; i < 4; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                if (j != 0)
                {
                    Instantiate(shield, Camera.main.ScreenToWorldPoint(new Vector3(4 * padding + space * i + j * padding, height - 16.5f * padding, z)), Quaternion.identity);
                }
            }
            for (int j = -2; j <= 2; j++)
            {
                Instantiate(shield, Camera.main.ScreenToWorldPoint(new Vector3(4 * padding + space * i + j * padding, height - 15.5f * padding, z)), Quaternion.identity);
            }
        }
        GameObject plat = Instantiate(platform, Camera.main.ScreenToWorldPoint(new Vector3(width / 2f, height - 20f * padding, z)), Quaternion.identity);
        float length = Camera.main.ScreenToWorldPoint(new Vector3(width - padding, 0, z)).x - Camera.main.ScreenToWorldPoint(new Vector3(padding, 0, z)).x;
        plat.transform.localScale = new Vector3(length, 0.25f, 1);
        plat.GetComponent<MeshRenderer>().enabled = false;
    }
    public void Timeout(float t)
    {
        for (int i = 0; i < moveTimer.Length; i++) 
        {
            moveTimer[i] += t;
        }
        missileTimer += t;
        shipTimer += t;
    }
}
