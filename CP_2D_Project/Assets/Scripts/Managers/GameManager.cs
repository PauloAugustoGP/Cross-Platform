using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Vector3 playerSpawn;

    public GameObject ui;
    
	void Start ()
    {
        if (player)
            Instantiate(player, playerSpawn, Quaternion.identity);
        else
            Debug.Log("Failed to Instantiate the player!");

        if (ui)
            Instantiate(ui);
        else
            Debug.Log("Failed to Instantiate the UI!");
	}
}
