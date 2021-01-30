using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInvader : MonoBehaviour
{   
    public float z;
    public float padding;
    public float velocity;
    public GameObject invader;
    public List<GameObject> invaders;
    // Start is called before the first frame update
    void Start()
    {
        z = Camera.main.WorldToScreenPoint(new Vector3(0,0,0)).z;
        padding = Camera.main.WorldToScreenPoint(new Vector3(2.0f, 0, 0)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)).x;
        velocity = 3.0f;
        invaders = new List<GameObject>();
        float width = Screen.width;
        float height = Screen.height;
        for (int i = 0; i < 11; i++) 
        {
            for (int j = 0; j < 5; j++) 
            {
                GameObject o = Instantiate(invader, Camera.main.ScreenToWorldPoint(new Vector3(padding + i * padding, height - padding - 0.75f * j * padding, z)), Quaternion.identity);
                o.GetComponent<Rigidbody>().velocity = new Vector3(velocity, 0, 0);
                invaders.Add(o);
            }
        }
    }
    public void UpdateVelocity() 
    {
        velocity = Mathf.Sign(velocity) * (30 * Mathf.Pow(1 - invaders.Count / 55.0f, 10) + 3);
        foreach (GameObject go in invaders)
        {
            go.GetComponent<Rigidbody>().velocity = new Vector3(velocity, 0, 0);
        }
    }
    void ChangeDirection() 
    {
        velocity *= -1;
        foreach (GameObject go in invaders)
        {
            go.GetComponent<Rigidbody>().velocity = new Vector3(velocity, 0, 0);
        }
    }
    void Descend() 
    {
        foreach (GameObject go in invaders)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(go.transform.position);
            pos.y -= 0.75f * padding;
            go.GetComponent<Rigidbody>().MovePosition(Camera.main.ScreenToWorldPoint(pos));
        }
    }
    void FixedUpdate()
    {
        if (invaders.Count > 0) 
        {
            if (velocity > 0)
            {
                float pos = Camera.main.WorldToScreenPoint(invaders[invaders.Count - 1].transform.position).x;
                if (pos >= Screen.width - padding) {
                    Descend();
                    ChangeDirection();
                }
            }
            else if (velocity < 0) 
            {
                float pos = Camera.main.WorldToScreenPoint(invaders[0].transform.position).x;
                if (pos <= padding) {
                    Descend();
                    ChangeDirection();
                }
            }
        }
    }
}
