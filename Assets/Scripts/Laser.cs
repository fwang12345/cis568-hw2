using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector3 thrust;
    
    void Start()
    {
        thrust.z = 800.0f;
        GetComponent<Rigidbody>().drag = 0;
        GetComponent<Rigidbody>().AddRelativeForce(thrust);
    }

    void OnCollisionEnter( Collision collision )
    {
        Collider collider = collision.collider;
        if( collider.CompareTag("Invader") )
        {
            Invader inv = collider.gameObject.GetComponent<Invader>();
            // let the other object handle its own death throes
            inv.Die();
        }
        else
        {
            // if we collided with something else, print to console
            // what the other thing was
            Debug.Log ("Collided with " + collider.tag);
        }
        Destroy(gameObject);
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
