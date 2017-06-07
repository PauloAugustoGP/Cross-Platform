using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject target;
    
	void Start ()
    {
        target = GameObject.FindWithTag("Player");
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10f);
	}
	
	void Update ()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10f);
    }
}
