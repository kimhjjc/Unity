using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {
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

    void Update () {

        if (start)
            interval -= Time.deltaTime;
        
        if (interval < 0)
            SceneManager.LoadScene("Main");
    }

    public void Loadgame()
    {
        this.audio.clip = this.startSound;
        this.audio.Play();
        start = true;

    }
}
