using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBase : MonoBehaviour
{
    public float velocity;
    public float fireRate;
    public float fireTimer;
    public float minX;
    public float maxX;
    public bool visible;
    public GameObject laser;
    public AudioClip fireSound;
    public AudioClip deathSound;
    void Start()
    {
        velocity = 5f;
        fireRate = 0.1f;
        fireTimer = fireRate;
        visible = true;
        float padding = Camera.main.WorldToScreenPoint(new Vector3(1, 0, 0)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)).x;
        float z = z = Camera.main.WorldToScreenPoint(new Vector3(0,0,0)).z;
        minX = Camera.main.ScreenToWorldPoint(new Vector3(padding, 0, z)).x;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - padding, 0, z)).x;
    }
    void Update() 
    {
        if (visible)
        {
            fireTimer -= Time.deltaTime;
            if (Input.GetButtonDown("Fire1"))
            {
                if (fireTimer < 0)
                {
                    Fire();
                    fireTimer = fireRate;
                }
            }
            float direction = Input.GetAxisRaw("Horizontal");
            if (direction > 0)
            {
                Vector3 pos = gameObject.transform.position;
                pos.x = Mathf.Min(maxX, pos.x + velocity * Time.deltaTime);
                gameObject.transform.position = pos;
            }
            else if (direction < 0)
            {
                Vector3 pos = gameObject.transform.position;
                pos.x = Mathf.Max(minX, pos.x - velocity * Time.deltaTime);
                gameObject.transform.position = pos;
            }
        }
    }
    void Fire() {
        // Spawn laser
        AudioSource.PlayClipAtPoint(fireSound, gameObject.transform.position);
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.z += 1.1f;
        Instantiate(laser, spawnPos, Quaternion.identity);
    }
    void Dissappear()
    {
        GameObject.Find("GlobalSpawnObject").GetComponent<GlobalSpawn>().Timeout(5);
        visible = false;
        Transform child = gameObject.transform.GetChild(0);
        child.gameObject.GetComponent<MeshRenderer>().enabled = false;
        child.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    void Appear()
    {
        visible = true;
        Transform child = gameObject.transform.GetChild(0);
        child.gameObject.GetComponent<MeshRenderer>().enabled = true;
        child.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
    public void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
        Global g = GameObject.Find("GlobalObject").GetComponent<Global>();
        g.lives -= 1;
        Dissappear();
        if (g.lives <= 0) 
        {
            g.GameOver();
        }
        else 
        {
            Invoke("Appear", 4f);
        }
    }
}
