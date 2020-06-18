using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackField : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip attackSound;

    bool onceAttack;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;

        this.audio.clip = this.attackSound;
        this.audio.Play();

        Destroy(gameObject, 0.5f);
        onceAttack = false;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }


    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Police") && !onceAttack && time > 0.2f)
        {
            other.gameObject.GetComponent<Enemy>().hp -= 40;
            onceAttack = true;
        }
        
        if (other.gameObject.tag == "Player" && !onceAttack && time > 0.2f && Player.hitTime < 0)
        {
            other.gameObject.GetComponent<Player>().hp -= 20;
            Player.hitTime = 1;
        }

    }
}
