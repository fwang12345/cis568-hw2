using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public float duration = 0f;
	public float amplitude = 0.5f;
	Vector3 pos;
	
	void Start()
	{
        pos = gameObject.transform.localPosition;
	}
    public void Shake()
    {
        duration = 0.5f;
    }

	void Update()
	{
		if (duration > 0)
		{
			gameObject.transform.localPosition = pos + Random.insideUnitSphere * amplitude;
			duration -= Time.deltaTime;
		}
		else
		{
			duration = 0f;
			gameObject.transform.localPosition = pos;
		}
	}
}