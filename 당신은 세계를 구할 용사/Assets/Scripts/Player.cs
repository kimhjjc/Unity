using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip hitSound;
    public AudioClip gameOverSound;

    private AudioSource BGMAudio;
    public AudioClip BGMSound;

    float moveSpeed; // 이동 속도 지정
    float realSpeed;
    float rotationSpeed; // 회전 속도 지정

    CharacterController characterController;
    Animator animator;

    Transform lifeObj;

    public static int playerLife;
    float hitTime;

    // Start is called before the first frame update
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;

        this.BGMAudio = this.gameObject.AddComponent<AudioSource>();
        this.BGMAudio.loop = true;
        this.BGMAudio.clip = this.BGMSound;

        moveSpeed = 5f;
        realSpeed = moveSpeed;
        rotationSpeed = 360f * 5.0f;

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("Move", 0);

        lifeObj = GameObject.Find("Life").transform;

        playerLife = 3;
        hitTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.gameOver || UIController.gameClear)
            return;
        if (this.BGMAudio.isPlaying == false) // replay 중이 아니면
        {
            this.BGMAudio.Play(); // audio clip 음원을 재생.
        }

        Move();
        hitTime -= Time.deltaTime;
    }

    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            realSpeed = moveSpeed * 1.3f;
        }
        if (direction.sqrMagnitude > 0.01f)
        {


            Transform modelTransform = transform.GetChild(0);
            Vector3 forward = Vector3.Slerp( // 메소드를 조합해 플레이어의 방향 변환
            modelTransform.forward,
            direction,
            rotationSpeed * Time.deltaTime / Vector3.Angle(modelTransform.forward, direction)
            );
            modelTransform.LookAt(modelTransform.position + forward * 50);

        }
        // Move()를 이용해 이동, 충돌 처리, 속도 값 얻기 가능
        direction += new Vector3(0, Physics.gravity.y * Time.deltaTime * 50, 0);
        characterController.Move(direction * realSpeed * Time.deltaTime);
        realSpeed = moveSpeed;

        animator.SetFloat("Move", characterController.velocity.magnitude);
    }

    void Attack()
    {

    }

    void onHit()
    {
        this.audio.clip = this.hitSound;
        this.audio.Play();

        playerLife--;
        for (int i = 0; i < 3; i++)
        {
            if (i < playerLife)
                lifeObj.GetChild(i).gameObject.SetActive(true);
            else
                lifeObj.GetChild(i).gameObject.SetActive(false);
        }

        if (playerLife <= 0)
        {
            this.audio.clip = this.gameOverSound;
            this.audio.Play();
            UIController.gameOver = true;
            this.BGMAudio.enabled = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (UIController.gameOver || UIController.gameClear)
            return;

        if (hit.gameObject.tag == "Enemy" && hitTime < 0)
        {
            onHit();
            hitTime = 2;
        }
    }
    
}
