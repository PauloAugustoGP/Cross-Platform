using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossThrone : MonoBehaviour
{
    public List<GameObject> enemyWave;

    public List<AudioClip> lines;
    private AudioSource source;

    public ParticleSystem evolving;
    public GameObject dracula;

    private int phase;
    private float timephase;

    private bool beginFight;
    
	void Start ()
    {
        phase = 0;
        timephase = 0;
        beginFight = false;

        source = GetComponent<AudioSource>();
	}
	
	void Update ()
    {
		if(beginFight)
        {
            if(timephase == 0)
            {
                source.clip = lines[phase];
                source.Play();
            }

            timephase += Time.deltaTime;

            if(timephase >= 20f)
            {
                if(phase < 4)
                {
                    Instantiate(enemyWave[phase], new Vector3(transform.position.x + 3, transform.position.y + 3, 0f), Quaternion.identity);
                    Instantiate(enemyWave[phase], new Vector3(transform.position.x + 3, transform.position.y - 3, 0f), Quaternion.identity);
                    Instantiate(enemyWave[phase], new Vector3(transform.position.x - 3, transform.position.y + 3, 0f), Quaternion.identity);
                    Instantiate(enemyWave[phase], new Vector3(transform.position.x - 3, transform.position.y - 3, 0f), Quaternion.identity);
                }
                else if(phase == 4)
                {
                    evolving.gameObject.SetActive(true);
                }
                else
                {
                    dracula.SetActive(true);
                    gameObject.SetActive(false);
                }
                timephase = 0;
                phase++;
            }
        }
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
            beginFight = true;
    }
}
