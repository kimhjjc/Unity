using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 사운드
    private AudioSource audio;
    public AudioClip deathSound;

    private AudioSource gunAudio;
    public AudioClip gunSound;

    //-----
    private Collider m_collider;


    // 에너미 스테이터스
    public float hp;
    public float maxHp;
    public float preHp;

    public bool targetOn;
    public float chaseTime;

    public GameObject attackField;
    public GameObject bullet1;
    float attackTime;
    float attackTime2;
    // 이동 관련

    Animator animator;

    Vector3 dir;
    bool goStraight;
    float timer; // 행동변경 최소 시간, 1초

    //죽음 (생존 체크)
    bool death;

    // Start is called before the first frame update
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;
        this.audio.clip = this.deathSound;

        this.gunAudio = this.gameObject.AddComponent<AudioSource>();

        m_collider = GetComponent<Collider>();

        hp = 100;
        maxHp = 100;
        if (gameObject.tag == "Police")
        {
            hp = 150;
            maxHp = 150;
        }
        preHp = maxHp;

        targetOn = false;
        chaseTime = 0;

        attackTime = 0;

        animator = GetComponent<Animator>();
        animator.SetFloat("Move", 0);
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);

        timer = 0;

        death = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_collider.isTrigger)
            return;


        Attack();
        Move();

        isDie();

    }

    void Attack()
    {
        if (!targetOn)
            return;

        Vector3 dir = GameObject.Find("Player").transform.position - transform.position;

        if (dir.magnitude < 1)
        {
            if (attackTime <= 0)
            {
                animator.SetBool("Attack1", true);
                attackTime = 0.6f;

                var field = GameObject.Instantiate(attackField) as GameObject;
                field.transform.position = transform.position;
                field.transform.SetParent(transform);
                field.transform.LookAt(transform.position + transform.forward * 50);
                field.transform.Translate(Vector3.forward * 1.5f + Vector3.up * 0.75f);
            }

        }
        else if (gameObject.tag == "Police" && attackTime2 < 0 && dir.magnitude < 20)
        {
            animator.SetBool("Attack2", true);
            attackTime = 0.3f;
            attackTime2 = 2;

            this.gunAudio.clip = this.gunSound;
            this.gunAudio.volume = 0.5f;
            this.gunAudio.Play();

            var bullet = GameObject.Instantiate(bullet1) as GameObject;
            bullet.transform.position = transform.position;
            bullet.transform.SetParent(GameObject.Find("Map").transform);

            bullet.transform.LookAt(transform.position + transform.forward * 50);
            bullet.transform.Translate(Vector3.forward * 1.2f + Vector3.up * 0.5f);
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * 5f;
        }

        attackTime -= Time.deltaTime;
        attackTime2 -= Time.deltaTime;

        if (attackTime < 0.4f)
            animator.SetBool("Attack1", false);

        if (attackTime2 < 1.7f)
            this.gunAudio.Stop();

        if (attackTime2 < 1.5f)
            animator.SetBool("Attack2", false);

    }

    void Move()
    {
        if (attackTime > 0)
            return;

        if ((chaseTime < 0 && targetOn)|| UIController.gameOver)
        {
            hp = maxHp;
            targetOn = false;
        }
        if (Random.Range(0, 10000) < 1)  // 쫒아오는 조건 1 (그냥 시비가 붙는다.)
        {
            targetOn = true;
            chaseTime = 5f;
        }
        if (hp < preHp)        // 쫒아오는 조건 2 (맞았을 때)
        {
            targetOn = true;
            chaseTime = 30;
            preHp = hp;
        }

        if (hp < maxHp )
        {
            targetOn = true;
            chaseTime = 30;

            transform.LookAt(GameObject.Find("Player").transform);
            transform.Translate(transform.forward * 0.05f, Space.World);

            chaseTime -= Time.deltaTime;
            return;
        }



        if (Random.Range(0, 100f) < 1 && timer < 0)
        {
            timer = 1;
            if (Random.Range(0, 100f) < 10)
            {
                goStraight = false;
                animator.SetFloat("Move", 0);
                return;
            }
            else
            {
                goStraight = true;
                animator.SetFloat("Move", 1);
            }

            transform.eulerAngles = new Vector3(0, Random.Range(0, 360f), 0);
            dir = transform.forward;
            transform.LookAt(transform.position + dir * 50);
        }

        if (goStraight)
            transform.Translate(dir.normalized * 0.05f, Space.World);

        if (hp < maxHp)
            HPManager();

        timer -= Time.deltaTime;
    }

    void HPManager()
    {
        //hpBar.value = hp / maxHp;
        //hpBar.transform.position = transform.position + Vector3.up * 2f;
    }

    void isDie()
    {
        if (hp > 0)
            return;
        if (!death)
        {
            this.audio.clip = this.deathSound;

            Score.score++;
            if (gameObject.tag == "Police")
            {
                Score.policeKillscore++;
                Score.score++;
            }

            gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.GetComponent<Animator>().SetBool("Death", true);
            Destroy(gameObject, 5f);
            death = true;
            this.audio.Play();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball1")
        {
            hp -= 100f;
        }
        if (other.gameObject.tag != "AbleWalk")
        {
            transform.eulerAngles = new Vector3(0, Random.Range(0, 360f), 0);
            dir = transform.forward;
            transform.LookAt(transform.position + dir * 50);
        }
    }
}
