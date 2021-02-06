using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DisableCollider", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DisableCollider()
    {
        GetComponent<SphereCollider>().enabled = false;
    }
}
