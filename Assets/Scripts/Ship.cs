using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int velocity;
    public bool dead;
    public GameObject deathExplosion;
    void Start()
    {
        dead = false;
    }
    void Update() 
    {
        Vector3 pos = gameObject.transform.position;
        Vector3 view = Camera.main.WorldToViewportPoint(pos);
        if (view.x < -1f || view.x > 2f || view.y < -1f || view.y > 2f)
        {
            Destroy(gameObject);
        }
        if (!dead)
        {
            pos.x += velocity * Time.deltaTime; 
            gameObject.transform.position = pos;
        }
    }
    public void Die()
    {
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
            // Update score
            GameObject.Find("GlobalObject").GetComponent<Global>().score += Mathf.Abs(velocity) * 30;
        }
    }
}
