using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    public float thrust;
    public GameObject explosion;
    
    void Start()
    {
        thrust = -300f;
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
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("LaserBase"))
        {
            LaserBase l = collider.gameObject.GetComponent<LaserBase>();
            l.Die();
        }
        Die();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Shield")) 
        {
            Shield s = collider.gameObject.GetComponent<Shield>();
            s.Die();
            Die();
        }
    }
}
