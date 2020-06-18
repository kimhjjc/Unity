using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerFail : MonoBehaviour
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

        UIController.stage1Clear = false;
        Player.carAvailable = false; ;
        Player.dashAvailable = false;
        Player.miniMapAvailable = false;

        Cannon.myWeapon[0] = false;
        Cannon.myWeapon[1] = false;
        Cannon.myWeapon[2] = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
