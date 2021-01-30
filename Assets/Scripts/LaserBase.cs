using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBase : MonoBehaviour
{
    public float velocity;
    public GameObject laser;
    void Start()
    {
        velocity = 10.0f;
    }
    void Update() 
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }
    void Fire() {
        // Spawn laser
            Vector3 spawnPos = gameObject.transform.position;
            spawnPos.z += 1.1f;
            GameObject obj = Instantiate(laser, spawnPos, Quaternion.identity) as GameObject;
            Laser b = obj.GetComponent<Laser>();
    }
    void FixedUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        if (direction > 0)
        {
            //GetComponent<Rigidbody>().AddForce(new Vector3(velocity, 0, 0));
            GetComponent<Rigidbody>().velocity = new Vector3(velocity, 0, 0);
        }
        else if (direction < 0)
        {
            //GetComponent<Rigidbody>().AddForce(new Vector3(-velocity, 0, 0));
            GetComponent<Rigidbody>().velocity = new Vector3(-velocity, 0, 0);
        } else {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}
