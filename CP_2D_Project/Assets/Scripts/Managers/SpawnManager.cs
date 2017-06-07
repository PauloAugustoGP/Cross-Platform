using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject spawnHandler;
    private GameObject spawns;

    public List<GameObject> spawnPoints;
    
	void Start ()
    {
        spawns = Instantiate(spawnHandler);

        for (int i = 0; i < spawnPoints.Count; i++)
            Instantiate(spawnPoints[i]).transform.SetParent(spawns.transform, false);
	}
	
	void Update ()
    {
        for (int i = 1; i <= 16; i++)
        {
            if (GameObject.Find("Area" + i) != null)
                spawns.gameObject.transform.GetChild(i - 1).gameObject.SetActive(true);
            else
                spawns.gameObject.transform.GetChild(i - 1).gameObject.SetActive(false);
        }
    }
}
