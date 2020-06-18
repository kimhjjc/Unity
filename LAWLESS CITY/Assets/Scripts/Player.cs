using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // 사운드
    private AudioSource audio;
    public AudioClip gameOverSound;

    public AudioSource BGMAudio;
    public AudioClip BGMSound;

    private AudioSource driveAudio;
    public AudioClip driveSound;

    private AudioSource NPCAudio;
    public AudioClip NPCSound;

    // 이동 속도
    float moveSpeed; // 이동 속도 지정
    float realSpeed;

    public static float carSpeed;
    float carRotationSpeed;



    // 캐릭터 컨트롤러 (이동, 공격, 체력, 애니메이션)
    CharacterController characterController;
    Animator animator;
    public static float attackTime;
    float attackTime2;
    public GameObject attackField;


    public static bool carAvailable = false;
    public static bool dashAvailable = false;
    public static bool miniMapAvailable = false;

    public static bool characterState;    // 캐릭터 상태(차량 탑승)
    Vector3 carRotation;
    public static bool NPCInteraction;     // NPC 상호작용상태

    // Life 시스템

    public float hp;
    public float maxHp;
    public static float hitTime;
    float healTime;

    // Start is called before the first frame update
    void Start()
    {
        // 사운드
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;

        this.BGMAudio = this.gameObject.AddComponent<AudioSource>();
        this.BGMAudio.loop = true;
        this.BGMAudio.clip = this.BGMSound;
        this.BGMAudio.volume = 0.4f;

        this.driveAudio = this.gameObject.AddComponent<AudioSource>();
        this.driveAudio.clip = this.driveSound;

        this.NPCAudio = this.gameObject.AddComponent<AudioSource>();
        this.NPCAudio.clip = this.NPCSound;
        NPCAudio.loop = false;

        // 이동 속도
        moveSpeed = 7f;
        realSpeed = moveSpeed;

        carSpeed = 0;
        carRotationSpeed = 1f;

        // 캐릭터 컨트롤러 (이동, 공격, 애니메이션)
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("Move", 0);
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        attackTime = 0;
        attackTime2 = 0;

        characterState = false;    // 캐릭터 상태(차량 탑승)

        NPCInteraction = false;     // NPC 상호작용상태

        // Life 시스템
        //lifeObj = GameObject.Find("Life").transform;

        hp = 100;
        maxHp = 100;
        hitTime = 0;
        healTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.gameOver || UIController.gameClear)
            return;

        if (Input.GetKeyDown(KeyCode.R)) // 치트 키 돈 증가
        {
            Score.score += 10;
        }
        if (Input.GetKeyDown(KeyCode.Y)) // 치트 키 미션 완료 템 기능 모두 획득
        {
            Cannon.myWeapon[0] = true;
            Cannon.myWeapon[1] = true;
            Cannon.myWeapon[2] = true;

            dashAvailable = true;
            carAvailable = true;
            miniMapAvailable = true;
            UIController UICtr = GameObject.Find("Canvas").GetComponent<UIController>();
            for (int i = 1; i < 5; i++)
            {
                UICtr.questReward[i] = true;
            }
            if (!UIController.stage1Clear)   // 스테이지 클리어 조건 체크 ( 기능을 주는 모든 NPC의 퀘스트를 클리어함.)
            {
                UICtr.stageClearText.SetActive(true);
            }
        }

        if (this.BGMAudio.isPlaying == false) // replay 중이 아니면
        {
            this.BGMAudio.Play(); // audio clip 음원을 재생.
        }
        Interaction();
        if (NPCInteraction)
            return;

        if (!characterState)
            Move1();
        else
            Move2();

        if (hitTime < 0 && hp < maxHp)
            hp += Time.deltaTime * 15f;

        hitTime -= Time.deltaTime;

        IsAlive();
    }

    void Interaction()      //상호작용 (NPC, 차량 탑승)
    {
        GameObject modelGameObject = transform.GetChild(1).gameObject;

        if (Input.GetKeyDown(KeyCode.E) && Car.isAvailable && carAvailable) // 차량 탑승
        {
            if (modelGameObject.activeSelf)
            {
                carRotation = Car.carTr.localEulerAngles;
                Vector3 dir = Car.carTr.position - transform.position;
                modelGameObject.SetActive(false);
                Car.carTr.SetParent(transform);
                Car.carTr.localPosition = Vector3.zero;
                characterController.Move(dir);
                characterState = true;
                carSpeed = 0;
            }
            else
            {
                Transform car = transform.GetChild(2);
                Vector3 dir = transform.position;
                characterController.Move(car.right * -2f);
                car.SetParent(GameObject.Find("Cars").transform);
                car.position = dir;
                modelGameObject.SetActive(true);
                characterState = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && NPC.isAvailable) // NPC 상호작용
        {
            if (!NPCInteraction)
            {
                NPCAudio.Play();

                UIController.NPCList(); // 퀘스트 정보 받아오기

                transform.GetChild(0).localPosition = NPC.NPCTr.position - transform.position + (Vector3.up + Vector3.back) * 4f;
                transform.GetChild(0).LookAt(NPC.NPCTr);
                NPCInteraction = true;

                GameObject.Find("Canvas").gameObject.GetComponent<UIController>().questReward[5] = false;
            }
            else if (NPCInteraction && UIController.questTalkCount > 0)
            {
                UIController.questTalkCount--;
            }
            else
            {
                transform.GetChild(0).localPosition = new Vector3(0, 16.76f, 0);
                transform.GetChild(0).localEulerAngles = new Vector3(90f, 0, 0);
                NPCInteraction = false;

                UIController UICtr = GameObject.Find("Canvas").GetComponent<UIController>();
                if (!UIController.stage1Clear)   // 스테이지 클리어 조건 체크 ( 기능을 주는 모든 NPC의 퀘스트를 클리어함.)
                {

                    for (int i = 1; i < 5; i++)
                    {
                        if (!UICtr.questReward[i])
                            return;
                    }

                    UICtr.stageClearText.SetActive(true);

                }
                else if (UIController.stage1Clear && UICtr.questReward[4])
                    SceneManager.LoadScene("FinalStage");

            }
        }
    }

    void Move1()
    {
        characterController.Move(new Vector3(0, Physics.gravity.y * Time.deltaTime * 50, 0));

        Attack1();
        if (IsAttack())
            return;
        Attack2();

        Transform modelTransform = transform.GetChild(1);

        if (Input.GetKey(KeyCode.LeftShift) && dashAvailable)
        {
            realSpeed = moveSpeed * 1.5f;
        }


        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            characterController.Move(transform.forward * realSpeed * Time.deltaTime);
            modelTransform.LookAt(modelTransform.position + transform.forward * 50);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            characterController.Move(transform.forward * realSpeed * Time.deltaTime * -1);
            modelTransform.LookAt(modelTransform.position + transform.forward * 50 * -1);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            characterController.Move(transform.right * realSpeed * Time.deltaTime * -1);
            modelTransform.LookAt(modelTransform.position + transform.right * 50 * -1);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            characterController.Move(transform.right * realSpeed * Time.deltaTime);
            modelTransform.LookAt(modelTransform.position + transform.right * 50);
        }

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) // 왼앞보기
        {
            modelTransform.LookAt(modelTransform.position + (transform.forward + transform.right * -1) * 50);
        }
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))) // 오앞보기
        {
            modelTransform.LookAt(modelTransform.position + (transform.forward + transform.right) * 50);
        }
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) // 왼뒤보기
        {
            modelTransform.LookAt(modelTransform.position + (transform.forward * -1 + transform.right * -1) * 50);
        }
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))) // 오뒤보기
        {
            modelTransform.LookAt(modelTransform.position + (transform.forward * -1 + transform.right) * 50);
        }


        realSpeed = moveSpeed;

        animator.SetFloat("Move", characterController.velocity.magnitude);

        this.driveAudio.Stop();
    }

    void Move2()
    {
        characterController.Move(new Vector3(0, Physics.gravity.y * Time.deltaTime * 50, 0));

        Transform modelTransform = transform.GetChild(2);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            carSpeed += Time.deltaTime * 4f;
            if (0 < carSpeed && carSpeed < 5)
                carSpeed *= 1.3f;
            if (-5 < carSpeed && carSpeed < 0)
                carSpeed *= 0.7f;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            carSpeed -= Time.deltaTime * 4f;
            if (0 < carSpeed && carSpeed < 5)
                carSpeed *= 0.7f;
            if (-5 < carSpeed && carSpeed < 0)
                carSpeed *= 1.3f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            carRotationSpeed -= Time.deltaTime * 3f;
            if (carRotationSpeed > 0)
                carRotationSpeed -= Time.deltaTime * 2f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            carRotationSpeed += Time.deltaTime * 3f;
            if (carRotationSpeed < 0)
                carRotationSpeed += Time.deltaTime * 2f;
        }

        if (carSpeed > 0)
            carSpeed -= Time.deltaTime * 2f;
        else if (carSpeed < 0)
            carSpeed += Time.deltaTime * 2f;

        if (carSpeed >= 16)
            carSpeed = 16;
        else if (carSpeed <= -10)
            carSpeed = -10;


        if (carRotationSpeed > 0 && !IsCarHandling())
            carRotationSpeed -= Time.deltaTime * 5f;
        else if (carRotationSpeed < 0 && !IsCarHandling())
            carRotationSpeed += Time.deltaTime * 5f;

        if (carRotationSpeed >= 2)
            carRotationSpeed = 2f;
        else if (carRotationSpeed <= -2)
            carRotationSpeed = -2f;

        carRotation += new Vector3(0, carRotationSpeed * carSpeed / 13f, 0);
        modelTransform.eulerAngles = carRotation;
        characterController.Move(modelTransform.forward * carSpeed * Time.deltaTime);

        DrivingSound();
    }

    bool IsCarHandling() // 자동차 핸들링 (좌, 우)
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            return true;
        }
        else
            return false;
    }

    void DrivingSound() // 자동차 운전중 (앞, 뒤)
    {
        if (!this.driveAudio.isPlaying)
            this.driveAudio.Play();

        this.driveAudio.pitch = carSpeed * 0.13f;
        if (carSpeed < 0)
            this.driveAudio.pitch = carSpeed * -0.13f;
    }

    void Attack1()
    {
        if (attackTime < 0.4f)
            animator.SetBool("Attack1", false);
        if (Input.GetMouseButtonDown(0) && attackTime <= 0 && attackTime2 <= 0)
        {
            animator.SetBool("Attack1", true);
            attackTime = 0.6f;

            var field = GameObject.Instantiate(attackField) as GameObject;
            field.transform.position = transform.position;
            field.transform.SetParent(transform);

            Vector3 dir = transform.GetChild(1).forward;
            field.transform.LookAt(transform.position + dir * 50);
            field.transform.Translate(Vector3.forward * 1.0f + Vector3.up * 0.75f);
        }
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }

    }

    void Attack2()
    {
        if (attackTime < 0.1f)
            animator.SetBool("Attack2", false);
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Attack2", true);
            attackTime2 = 0.6f;
        }
        if (attackTime2 > 0)
        {
            attackTime2 -= Time.deltaTime;
        }
    }

    bool IsAttack()
    {
        if (attackTime > 0)
            return true;
        else
            return false;
    }

    void IsAlive()
    {
        if (hp <= 0)
        {
            this.audio.clip = this.gameOverSound;
            this.audio.loop = false;
            this.audio.Play(); // audio clip 음원을 재생.
            UIController.gameOver = true;
            transform.GetChild(1).GetComponent<Animator>().SetBool("Death", true);
            this.BGMAudio.enabled = false;

        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (UIController.gameOver || UIController.gameClear)
            return;

        if (hit.gameObject.tag == "Ball" && hitTime < 0)
        {
            hp -= 45f;
            hitTime = 1;
        }

        if (hit.gameObject.tag == "Map" && characterState)
        {
            if (carSpeed > 1)
                carSpeed = -1;
            else if (carSpeed < -1)
                carSpeed = 1;
            else
                carSpeed *= -1;
        }

    }

}
