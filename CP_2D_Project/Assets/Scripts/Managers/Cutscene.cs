using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public Text subtitles;

    private int indexText = -1;
    private AsyncOperation async;

    private bool loading = true;
    private bool skipped = false;
    public bool begin = false;

    public GameObject loadingScreen;
    public Slider progressBar;
    public Text progressText;

    private Animator anim;

    public List<AudioClip> lines;
    private AudioSource narrative;

    void Start()
    {
        anim = GetComponent<Animator>();
        narrative = GetComponent<AudioSource>();

        StartCoroutine("load");

        //subtitles.text = getText(++indexText);
    }

    void Update()
    {
        if(loading && begin)
        {
            progressBar.value = async.progress;
            progressText.text = (async.progress * 100) + "%";
        }

        if(Input.GetKey(KeyCode.Escape) && !skipped)
        {
            skipped = true;
            anim.Stop();
            ChangeScene();
        }
    }
    
    public void ChangeSubtitles()
    {
        subtitles.text = getText(++indexText);
    }

    string getText(int index)
    {
        narrative.clip = lines[index];

        narrative.Play();

        if (begin)
        {
            switch (index)
            {
                case 0: { return "Long time ago, Turem was the guardian of this world."; }
                case 1: { return "He became blind with his power and started to seek for more!"; }
                case 2: { return "Yrama, the angel that guide humanity warned him about the dangers, but he didn't listen her."; }
                case 3: { return "He transformed into an evil creature after being corrupted by a dark crystal."; }
                case 4: { return "He sent his monsters through the world to conquer it."; }
                case 5: { return "And now you, Hakom, is the only one capable to save him from darkness!"; }

                default: break;
            }
        }
        else
        {
            switch (index)
            {
                case 0: { return "After a long and hard battle against Turem."; }
                case 1: { return "The fierce battle finally found an end."; }
                case 2: { return "Yrama came to aid and cleanse the darkness."; }
                case 3: { return "but sadly Turem was consumed and begone togheter with it."; }
                case 4: { return "And now Hakom is the new guardian of the world!."; }

                default: break;
            }
        }

        return "";
    }

    public void ChangeScene()
    {
        async.allowSceneActivation = true;
        
        if(!async.isDone && begin)
        {
            loadingScreen.SetActive(true);
        }
        else
        {
            loading = false;
        }
    }

    private IEnumerator load()
    {
        Debug.Log("ASYNC LOAD STARTED");
        if(begin)
            async = SceneManager.LoadSceneAsync("Level1");
        else
            async = SceneManager.LoadSceneAsync("Menu");
        async.allowSceneActivation = false;
        
        yield return async;
    }
}
