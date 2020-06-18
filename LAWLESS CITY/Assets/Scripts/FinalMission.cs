using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalMission : MonoBehaviour
{
    private AudioSource clearAudio;
    public AudioClip clearSound;

    private AudioSource deathAudio;
    public AudioClip deathSound;

    bool GameClear;
    float countTime = 0;
    float clearCounttime = 0;

    Text text;
    // Start is called before the first frame update
    void Start()
    {
        Score.policeKillscore = 0;

        clearAudio = gameObject.AddComponent<AudioSource>();
        clearAudio.clip = clearSound;
        clearAudio.loop = false;
        deathAudio = gameObject.AddComponent<AudioSource>();
        deathAudio.clip = deathSound;
        deathAudio.loop = false;

        countTime = 100f;
        clearCounttime = 7.8f;

        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "죽여야 할 갱단 : " + (Score.policeKillscore).ToString() + " / 30 \n or \n" +
            "버텨야 할 시간 : " + ((int)countTime).ToString();

        countTime -= Time.deltaTime;

        if (countTime < 0 || (GameObject.FindGameObjectsWithTag("Police").Length < 5 && countTime < 90))
        {
            countTime = 0;
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Police");

            for (int i = 0; i < Enemys.Length; i++)
            {
                Enemys[i].GetComponent<Collider>().isTrigger = true;
                Enemys[i].GetComponent<Animator>().SetBool("Death", true);
                Destroy(Enemys[i], 2f);
            }
        }



        if (countTime <= 0)
        {
            GameObject.Find("Player").GetComponent<Player>().BGMAudio.enabled = false;

            clearCounttime -= Time.deltaTime;

            if (!clearAudio.isPlaying)
                clearAudio.Play();
        }

        if (clearCounttime < 0)
            SceneManager.LoadScene("Ending");
    }
}
