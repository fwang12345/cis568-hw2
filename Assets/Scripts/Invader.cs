using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public GameObject missile;
    public int score;
    public List<GameObject> columnList;
    public int row;
    public int column;
    public bool dead;
    public GameObject deathExplosion;
    // Start is called before the first frame update
    void Start()
    {
        dead = false;
    }

    void Update() 
    {
        Vector3 pos = gameObject.transform.position;
        Vector3 view = Camera.main.WorldToViewportPoint(pos);
        if (view.x < -0.2f || view.x > 1.2f || view.y < -0.2f || view.y > 1.2f)
        {
            Destroy(gameObject);
        }
    }

    public void Fire() {
        // Spawn missile
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.z -= 1.1f;
        GameObject obj = Instantiate(missile, spawnPos, Quaternion.identity) as GameObject;
        Missile m = obj.GetComponent<Missile>();
    }
    public void Die()
    {
        // Update globals
        if (!dead)
        {
            dead = true;
            // Update components
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            // Spawn explosion
            GameObject explosion = Instantiate(deathExplosion, gameObject.transform.position, Quaternion.identity);
            explosion.transform.parent = gameObject.transform;
            // Update score and killstreak
            Global g = GameObject.Find("GlobalObject").GetComponent<Global>();
            g.score += score;
            g.killstreak += 1;
            g.killstreakTimer = 1;
            // Update invader count and move rate
            GlobalSpawn gs = GameObject.Find("GlobalSpawnObject").GetComponent<GlobalSpawn>();
            columnList.Remove(gameObject);
            if (columnList.Count == 0)
            {
                gs.invadersList.Remove(columnList);
            }
            gs.invaders[row, column] = null;
            gs.numInvaders -= 1;
            gs.UpdateMoveRate();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!dead)
        {
            if (collider.CompareTag("Shield")) 
            {
                Shield s = collider.gameObject.GetComponent<Shield>();
                s.Die();
            }
        }
    }
}
