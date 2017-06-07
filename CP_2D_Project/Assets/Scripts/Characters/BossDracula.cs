using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDracula : MonoBehaviour
{
    //public GameObject canvasTalk;
    //public Text talk;

    public List<AudioClip> lines;
    private AudioSource source;

    private float timeBattle = 0;
    private int index = 0;
	
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

	void Update ()
    {
        timeBattle += Time.deltaTime;
        if(timeBattle >= 20f)
        {
            source.clip = lines[index];
            source.Play();

            timeBattle = 0;
            index++;
            if (index > 3) index = 0;
        }

        if(GetComponent<Enemy>().dead)
        {
            timeBattle = 0;
            //talk.text = "IMPOSSIBLE!";
            GameObject.Find("PlayerUI(Clone)").GetComponent<UIManager>().SendMessage("GameVictory");
        }
	}
}
