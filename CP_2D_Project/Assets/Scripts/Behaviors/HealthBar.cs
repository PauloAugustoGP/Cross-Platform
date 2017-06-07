using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]private GameObject parent;

	void Update ()
    {
        GetComponent<Slider>().value = (parent.GetComponent<Enemy>().health / (float)parent.GetComponent<Enemy>().maxHealth) * 100;
	}
}
