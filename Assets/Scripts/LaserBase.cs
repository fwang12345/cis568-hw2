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
    public int numCollisions;
    public bool visible;
    public GameObject laser;
    public AudioSource fireSound;
    public AudioSource deathSound;
    public AudioSource powerSound;
    public GameObject deathExplosion;
    public Global globalObj;
    void Start()
    {
        velocity = 5f;
        fireRate = 0.3f;
        fireTimer = fireRate;
        numCollisions = 0;
        visible = true;
        float padding = Camera.main.WorldToScreenPoint(new Vector3(1, 0, 0)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)).x;
        float z = z = Camera.main.WorldToScreenPoint(new Vector3(0,0,0)).z;
        minX = Camera.main.ScreenToWorldPoint(new Vector3(padding, 0, z)).x;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - padding, 0, z)).x;
        globalObj = GameObject.Find("GlobalObject").GetComponent<Global>();
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
            numCollisions = Mathf.Max(numCollisions, 0);
            float factor = 0.5f * Mathf.Exp(-numCollisions) + 0.5f;
            if (direction > 0)
            {
                Vector3 pos = gameObject.transform.position;
                pos.x = Mathf.Min(maxX, pos.x + velocity * Time.deltaTime * factor);
                gameObject.transform.position = pos;
            }
            else if (direction < 0)
            {
                Vector3 pos = gameObject.transform.position;
                pos.x = Mathf.Max(minX, pos.x - velocity * Time.deltaTime * factor);
                gameObject.transform.position = pos;
            }
        }
    }
    void Fire() {
        // Spawn laser
        fireSound.Play();
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.z += 1.1f;
        int num = (int) Mathf.Sqrt(globalObj.killstreak / 10) + 1;
        float space = 0.3f;
        if (num % 2 == 0)
        {
            for (int i = 0; i < num / 2; i++)
            {
                Instantiate(laser, spawnPos + new Vector3(space / 2 + i * space, 0, 0), Quaternion.identity);
                Instantiate(laser, spawnPos + new Vector3(-space / 2 - i * space, 0, 0), Quaternion.identity);
            }
        }
        else 
        {
            Instantiate(laser, spawnPos, Quaternion.identity);
            for (int i = 0; i < num / 2; i++)
            {
                Instantiate(laser, spawnPos + new Vector3((i + 1) * space, 0, 0), Quaternion.identity);
                Instantiate(laser, spawnPos + new Vector3(-(i + 1) * space, 0, 0), Quaternion.identity);
            }
        }
    }
    void Dissappear()
    {
        GameObject.Find("GlobalSpawnObject").GetComponent<GlobalSpawn>().Timeout(5);
        visible = false;
        Transform child = gameObject.transform.GetChild(0);
        child.gameObject.GetComponent<MeshRenderer>().enabled = false;
        child.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("Main Camera").GetComponent<CameraShake>().Shake();
        GetComponent<BoxCollider>().enabled = false;
    }
    void Appear()
    {
        visible = true;
        Transform child = gameObject.transform.GetChild(0);
        child.gameObject.GetComponent<MeshRenderer>().enabled = true;
        child.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
        numCollisions = 0;
        GetComponent<BoxCollider>().enabled = true;
    }
    public void Die()
    {
        deathSound.Play();
        GameObject explosion = Instantiate(deathExplosion, gameObject.transform.position, Quaternion.identity);
        explosion.transform.parent = gameObject.transform;
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
    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Invader")) 
        {
            if (collider.gameObject.GetComponent<Invader>().dead)
            {
                numCollisions++;
            }
        }
        else if (collider.CompareTag("Missile"))
        {
            if (collider.gameObject.GetComponent<Missile>().dead)
            {
                numCollisions++;
            }
        }
        else if (collider.CompareTag("Laser"))
        {
            if (collider.gameObject.GetComponent<Laser>().dead)
            {
                numCollisions++;
            }
        }
        else if (collider.CompareTag("Ship"))
        {
            if (collider.gameObject.GetComponent<Ship>().dead)
            {
                numCollisions++;
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Invader")) 
        {
            if (collider.gameObject.GetComponent<Invader>().dead)
            {
                numCollisions--;
            }
        }
        else if (collider.CompareTag("Missile"))
        {
            if (collider.gameObject.GetComponent<Missile>().dead)
            {
                numCollisions--;
            }
        }
        else if (collider.CompareTag("Laser"))
        {
            if (collider.gameObject.GetComponent<Laser>().dead)
            {
                numCollisions--;
            }
        }
        else if (collider.CompareTag("Ship"))
        {
            if (collider.gameObject.GetComponent<Ship>().dead)
            {
                numCollisions--;
            }
        }
        
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("PowerUp")) 
        {
            Global g = GameObject.Find("GlobalObject").GetComponent<Global>();
            if (g.lives < 3) 
            {
                g.lives += 1;
            }
            powerSound.Play();
            collider.gameObject.GetComponent<PowerUp>().Die();
        } 
        else if (collider.CompareTag("Explosion")) 
        {
            Die();
        }
    }
}
