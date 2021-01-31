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
    public float moveTimer;
    public bool left;
    // paramater to spawn missile
    public float missileTimer;
    // parameter to spawn ship
    public float shipTimer;
    public GameObject invader;
    public GameObject laserBase;
    public GameObject shield;
    public GameObject ship;
    // Start is called before the first frame update
    void Start()
    {
        z = Camera.main.WorldToScreenPoint(new Vector3(0,0,0)).z;
        padding = Camera.main.WorldToScreenPoint(new Vector3(1, 0, 0)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)).x;
        width = Screen.width;
        height = Screen.height;
        NewRound();
    }
    
    void Update() 
    {
        Transform t = gameObject.transform;
        if (t.childCount > 0) 
        {
            // Check if any child has reached the bottom row
            foreach (Transform child in t) 
            {
                float childPos = Camera.main.WorldToScreenPoint(child.position).y;
                if (childPos < height - 18.5f * padding)
                {
                    GameObject.Find("GlobalObject").GetComponent<Global>().GameOver();
                }
            }
            // Spawn missile
            missileTimer -= Time.deltaTime;
            if (missileTimer < 0)
            {
                t.GetChild(Random.Range(0, t.childCount)).gameObject.GetComponent<Invader>().Fire();
                missileTimer = Random.Range(1f, 3f);
            }
            // Move Invaders
            moveTimer -= Time.deltaTime;
            if (moveTimer < 0) 
            {
                Vector3 pos = t.position;
                // Move invaders left
                if (left)
                {
                    float childPos = Camera.main.WorldToScreenPoint(t.GetChild(0).gameObject.transform.position).x;
                    if (childPos < 1.5f * padding) 
                    {
                        left = false;
                        pos.z -= 1.5f;
                    } 
                    else {
                        pos.x -= 2;
                    }
                }
                // Move invaders right
                else {
                    float childPos = Camera.main.WorldToScreenPoint(t.GetChild(t.childCount - 1).gameObject.transform.position).x;
                    if (childPos > width - 1.5f * padding) 
                    {
                        left = true;
                        pos.z -= 1.5f;
                    } 
                    else {
                        pos.x += 2;
                    }
                }
                t.position = pos;
                moveTimer = moveRate;
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
                shipTimer = Random.Range(20f, 30f);
            }
        }
    }
    // Updates move rate based on number of invaders alivee
    public void UpdateMoveRate() 
    {
        moveRate = 0.6f * Mathf.Log(gameObject.transform.childCount + 1, 10);
        if (gameObject.transform.childCount <= 1)
        {
            Global g = GameObject.Find("GlobalObject").GetComponent<Global>();
            if (!g.lose)
            {
                g.NewRound();
                Invoke("NewRound", 3f);
            }
        }
    }
    void NewRound()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Shield"))
        {
            Destroy(g);
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("LaserBase"))
        {
            Destroy(g);
        }
        left = false;
        int diff = (GameObject.Find("GlobalObject").GetComponent<Global>().round - 1) % 3;
        for (int i = 0; i < 11; i++) 
        {
            for (int j = 0; j < 5; j++) 
            {
                GameObject inv = Instantiate(invader, 
                    Camera.main.ScreenToWorldPoint(new Vector3(
                        padding + 2f * i * padding, height - 4f * padding - 1.5f * (j + diff) * padding, z)), 
                    Quaternion.identity) as GameObject;
                inv.GetComponent<Invader>().score = (6 - j) / 2 * 10;
                inv.transform.SetParent(gameObject.transform, true);
            }
        }
        UpdateMoveRate();
        moveTimer = moveRate;
        missileTimer = Random.Range(0.25f, 1f);
        // Spawn player laserbase
        Instantiate(laserBase, Camera.main.ScreenToWorldPoint(new Vector3(padding, height - 19f * padding, z)), Quaternion.identity);
        // Spawn shields
        float space = (width - 8 * padding) / 3f;
        for (int i = 0; i < 4; i++)
        {
            Instantiate(shield, Camera.main.ScreenToWorldPoint(new Vector3(4 * padding + space * i, height - 17.5f * padding, z)), Quaternion.identity);
        }
        shipTimer = Random.Range(10f, 2f);
    }
    public void Timeout(float t)
    {
        moveTimer += t;
        missileTimer += t;
        shipTimer += t;
    }
}
