using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalControl : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip InterectionSound;

    bool[] onceSoundCheck;

    public GameObject enemySpawner;

    Renderer[] crystals;
    // Start is called before the first frame update

    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;

        crystals = GetComponentsInChildren<Renderer>();

        onceSoundCheck = new bool[5];
        for (int i = 0; i < crystals.Length; i++)
        {
            onceSoundCheck[i] = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Score.score = 0;
        for (int i = 0; i < crystals.Length; i++)
        {
            if (crystals[i].material.name == "Blue (Instance)")
            {
                if(!onceSoundCheck[i])
                {
                    this.audio.clip = this.InterectionSound;
                    this.audio.Play();
                    onceSoundCheck[i] = true;
                }

                Score.score++;
            }
        }

        // 성공할 메세지 작성
        if (Score.score >= 5)
        {
            enemySpawner.SetActive(false);
        }
    }
    
}
