using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public GameObject missile;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update() 
    {
    }

    public void Fire() {
        // Spawn missile
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.z -= 1.1f;
        GameObject obj = Instantiate(missile, spawnPos, Quaternion.identity) as GameObject;
        Missile m = obj.GetComponent<Missile>();
        m.velocity = Random.Range(5f, 20f);  
    }
    public void Die()
    {
        GameObject.Find("GlobalSpawnObject").GetComponent<GlobalSpawn>().UpdateMoveRate();
        GameObject.Find("GlobalObject").GetComponent<Global>().score += score;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Shield"))
        {
            Shield s = collider.gameObject.GetComponent<Shield>();
            s.Die();
        }
    }
}
