using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float velocity;
    
    void Start()
    {
        velocity = 30f;
    }
    void Update ()
    {
        Vector3 pos = gameObject.transform.position;
        Vector3 view = Camera.main.WorldToViewportPoint(pos);
        if (view.x < -0.2f || view.x > 1.2f || view.y < -0.2f || view.y > 1.2f)
        {
            Destroy(gameObject);
        }
        pos.z += velocity * Time.deltaTime;
        gameObject.transform.position = pos;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Invader"))
        {
            Invader inv = collider.gameObject.GetComponent<Invader>();
            inv.Die();
        }
        else if (collider.CompareTag("Shield"))
        {
            Shield s = collider.gameObject.GetComponent<Shield>();
            s.Damage();
        }
        else if (collider.CompareTag("Missile"))
        {
            Destroy(collider.gameObject);
        }
        else if (collider.CompareTag("Ship"))
        {
            Ship s = collider.gameObject.GetComponent<Ship>();
            s.Die();
        }
        Destroy(gameObject);
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
