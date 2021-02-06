using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float thrust;
    public bool dead;
    
    void Start()
    {
        thrust = Random.Range(-800f, -300f); 
        dead = false;
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
    }
    public void Die()
    {
        dead = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!dead)
        {
            Collider collider = collision.collider;
            if (collider.CompareTag("LaserBase"))
            {
                LaserBase l = collider.gameObject.GetComponent<LaserBase>();
                l.Die();
            }
            Die();
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
                Destroy(gameObject);
            }
        }
    }
}
