using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float velocity;
    
    void Start()
    {
    }
    void Update ()
    {
        Vector3 pos = gameObject.transform.position;
        Vector3 view = Camera.main.WorldToViewportPoint(pos);
        if (view.x < -0.2f || view.x > 1.2f || view.y < -0.2f || view.y > 1.2f)
        {
            Destroy(gameObject);
        }
        pos.z -= velocity * Time.deltaTime;
        gameObject.transform.position = pos;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("LaserBase"))
        {
            LaserBase l = collider.gameObject.GetComponent<LaserBase>();
            if (l.visible)
            {
                l.Die();
                Destroy(gameObject);
            }
        }
        else if(collider.CompareTag("Shield"))
        {
            Shield s = collider.gameObject.GetComponent<Shield>();
            s.Damage();
            Destroy(gameObject);
        }
    }
}
