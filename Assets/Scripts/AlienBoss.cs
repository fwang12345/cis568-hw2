using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBoss : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool left;
    public float velocity;
    public float velocityTimer;
    public float fireTimer;
    public float minX;
    public float maxX;
    public GameObject nuke;
    public GameObject progressBar;
    public ProgressBar pb;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = GameObject.Find("GlobalObject").GetComponent<Global>().round * 10;
        health = maxHealth;
        float padding = Camera.main.WorldToScreenPoint(new Vector3(1, 0, 0)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)).x;
        float z = z = Camera.main.WorldToScreenPoint(new Vector3(0,0,0)).z;
        minX = Camera.main.ScreenToWorldPoint(new Vector3(2 * padding, 0, z)).x;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 2 * padding, 0, z)).x;
        UpdateVelocity();
        fireTimer = Random.Range(1f, 3f);
        GameObject.Find("GlobalObject").GetComponent<Global>().freezeKillstreak = true;
        GameObject pbo = Instantiate(progressBar, new Vector3(0, 0, 0), Quaternion.identity);
        pb = pbo.GetComponent<ProgressBar>();
        pb.transform.SetParent(GameObject.Find("Canvas").transform, false);
        pb.BarValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        velocityTimer -= Time.deltaTime;
        if (velocityTimer <= 0)
        {
            UpdateVelocity();
        }
        Vector3 pos = gameObject.transform.position;
        float dx = velocity * Time.deltaTime;
        if (left)
        {
            if (pos.x - minX < dx)
            {
                UpdateVelocity();
            }
            else {
                pos.x = Mathf.Max(minX, pos.x - dx);
                gameObject.transform.position = pos;
            }
        }
        else
        {
            if (maxX - pos.x < dx)
            {
                UpdateVelocity();
            }
            else 
            {
                pos.x = Mathf.Min(maxX, pos.x + dx);
                gameObject.transform.position = pos;
            }
        }
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            Fire();
            fireTimer = Random.Range(1f, 3f);
        }
    }

    void UpdateVelocity()
    {
        left = Random.Range(0f, 1f) < 0.5f;
        velocity = Random.Range(5f, 10f);
        velocityTimer = Random.Range(0.5f, 2f);
    }

    void Fire()
    {
        // Spawn laser
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.z -= 6f;
        Instantiate(nuke, spawnPos, Quaternion.identity);
    }
    

    public void Damage()
    {
        health -= 1;
        pb.BarValue = health * 100f / maxHealth;
        if (health <= 0)
        {
            // Destroy gameobject and progress bar
            GlobalSpawn gs = GameObject.Find("GlobalSpawnObject").GetComponent<GlobalSpawn>();
            gs.bossDead = true;
            Global g = GameObject.Find("GlobalObject").GetComponent<Global>();
            g.score += maxHealth * 10;
            g.freezeKillstreak = false;
            Destroy(gameObject);
            Transform t = GameObject.Find("Canvas").transform.Find("ProgressBarUI(Clone)");
            if (t != null)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
