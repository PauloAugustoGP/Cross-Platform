using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDracula : MonoBehaviour
{
    //public GameObject canvasTalk;
    //public Text talk;

    public List<AudioClip> lines;
    //public AudioClip deadLine;
    private AudioSource source;

    private float timeBattle = 0;
    private float timeDead = 0;
    private int index = 0;
	
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

	void Update ()
    {

        if(GetComponent<Enemy>().dead)
        {
            timeBattle = 0;
            if(timeDead == 0)
            {
                source.clip = lines[4];
                source.Play();
            }

            timeDead += Time.deltaTime;
            if (timeDead >= 3f)
                Victory();

        }

        timeBattle += Time.deltaTime;
        if(timeBattle >= 20f)
        {
            source.clip = lines[index];
            source.Play();

            timeBattle = 0;
            index++;
            if (index > 3) index = 0;
        }
	}

    void Victory()
    {
        GameObject.Find("PlayerUI(Clone)").GetComponent<UIManager>().SendMessage("GameVictory");
    }
}
