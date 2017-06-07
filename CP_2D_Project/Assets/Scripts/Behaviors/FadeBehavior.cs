using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehavior : MonoBehaviour
{
    [SerializeField]private float time;
    private float timeSpent = 0;
    
	void Update ()
    {
        timeSpent += Time.deltaTime;

        if (timeSpent >= time)
            Destroy(gameObject);
	}
}
