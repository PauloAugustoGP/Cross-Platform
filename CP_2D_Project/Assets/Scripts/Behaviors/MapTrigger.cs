using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    public bool create;
    public int area;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            string filename = GameObject.FindWithTag("GameController").GetComponent<BoardManager>().filenames[area-1];

            if (create)
                GameObject.FindWithTag("GameController").GetComponent<BoardManager>().ReadFile(filename, area);
            else
                GameObject.FindWithTag("GameController").GetComponent<BoardManager>().DestroyLevel(area);
        }
    }
}
