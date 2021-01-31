using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int velocity;
    void Start()
    {
    }
    void Update() 
    {
        Vector3 pos = gameObject.transform.position;
        Vector3 view = Camera.main.WorldToViewportPoint(pos);
        if (view.x < -0.2f || view.x > 1.2f || view.y < -0.2f || view.y > 1.2f)
        {
            Destroy(gameObject);
        }
        pos.x += velocity * Time.deltaTime;
        gameObject.transform.position = pos;
    }
    public void Die()
    {
        GameObject.Find("GlobalObject").GetComponent<Global>().score += Mathf.Abs(velocity) * 30;
        Destroy(gameObject);
    }
}
