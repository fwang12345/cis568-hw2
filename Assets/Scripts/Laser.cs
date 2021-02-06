using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float thrust;
    public bool dead;
    public float deathTimer;
    public GameObject explosion;
    void Start()
    {
        thrust = 1500f;
        dead = false;
        deathTimer = 5f;
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, thrust));
    }
    void Update ()
    {
        Vector3 pos = gameObject.transform.position;
        Vector3 view = Camera.main.WorldToViewportPoint(pos);
        if (view.x < -0.2f || view.x > 1.2f || view.y < -0.2f || view.y > 1.2f)
        {
            Destroy(gameObject);
        }
        // Disable laser after a certain amount of time
        if (!dead)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                dead = true;
            }
        }
    }
    void Die()
    {
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!dead)
        {
            Collider collider = collision.collider;
            if (collider.CompareTag("Invader"))
            {
                Invader inv = collider.gameObject.GetComponent<Invader>();
                inv.Die();
            }
            else if (collider.CompareTag("Missile"))
            {
                Missile m = collider.gameObject.GetComponent<Missile>();
                m.Die();
            }
            else if (collider.CompareTag("Ship"))
            {
                Ship s = collider.gameObject.GetComponent<Ship>();
                s.Die();
            } 
            else if (collider.CompareTag("AlienBoss"))
            {
                AlienBoss boss = collider.gameObject.GetComponent<AlienBoss>();
                boss.Damage();
                Die();
            }
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
                Die();
            }
        }
    }
}
