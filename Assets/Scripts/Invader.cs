using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        GlobalInvader gi = GameObject.Find("GlobalInvaderObject").GetComponent<GlobalInvader>();
        // Destroy object and increase velocity
        gi.invaders.Remove(gameObject);
        gi.UpdateVelocity();
        Destroy(gameObject);
    }
}
