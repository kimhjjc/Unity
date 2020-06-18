using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerEnd : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip sound;

    private void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.loop = true;
        audio.clip = sound;
        audio.volume = 0.5f;
        audio.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
