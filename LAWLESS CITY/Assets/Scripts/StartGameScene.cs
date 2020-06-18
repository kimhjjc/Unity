using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScene : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip startSound;

    bool start;
    float interval;
    // Start is called before the first frame update
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;

        start = false;
        interval = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            this.audio.clip = this.startSound;
            this.audio.Play();
            start = true;
        }

        if (start)
            interval -= Time.deltaTime;

        if(interval < 0)
            SceneManager.LoadScene("Main");
    }
}
