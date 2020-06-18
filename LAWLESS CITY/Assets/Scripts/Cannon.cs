using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

    // 변수 선언
    private AudioSource audio;
    public AudioClip bulletSound;
    public AudioClip shotGunSound;
    public AudioClip laserSound;

    private AudioSource gunChangeAudio;
    public AudioClip gunChangeSound;

    float bulletSoundEnd;

    public GameObject bullet1;  // 총알
    public GameObject bullet2;
    public GameObject laser;
    public LineRenderer laserRenderer;
    float laserTime;

    public static bool[] myWeapon = new bool[3] {false, false, false}; // 가지고 있는 웨폰 (true면 가지고 있는 것)

    int weaponNumber; // 무기 번호
    float coolTime; // 장전 쿨타임

	void Start () {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;

        this.gunChangeAudio = this.gameObject.AddComponent<AudioSource>();
        this.gunChangeAudio.loop = false;
        this.gunChangeAudio.clip = this.gunChangeSound;
        laserRenderer = laser.GetComponent<LineRenderer>();
        laserRenderer.enabled = false;
        laserTime = 0;

        weaponNumber = -1;
        coolTime = 0;
	}
	
	void Update () {
        if (UIController.gameOver || UIController.gameClear || Player.characterState || Player.NPCInteraction || 0 < Player.attackTime)
            return;

        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && myWeapon[0])
        {
            weaponNumber = 0;
            gunChangeAudio.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && myWeapon[1])
        {
            weaponNumber = 1;
            gunChangeAudio.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && myWeapon[2])
        {
            weaponNumber = 2;
            gunChangeAudio.Play();
            
        }


        if (this.audio.clip == this.bulletSound && bulletSoundEnd < 0)
            audio.Stop();

        if (Input.GetMouseButtonDown(1)) {

            switch(weaponNumber)
            {
                case 0:
                    LoadBullet1();
                    break;
                case 1:
                    LoadBullet2();
                    break;
                case 2:
                    LoadLaser();
                    break;
                default:
                    break;
            }

            this.audio.Play();
        }

        bulletSoundEnd -= Time.deltaTime;
        laserTime -= Time.deltaTime;

        if (laserTime < 0)
            laserRenderer.enabled = false;
    }

    void LoadBullet1()  // 기본 총
    {
        bulletSoundEnd = 0.3f;

        this.audio.clip = this.bulletSound;
        this.audio.volume = 1f;
        var bullet = GameObject.Instantiate(bullet1) as GameObject;
        bullet.transform.position = transform.position;
        bullet.transform.SetParent(GameObject.Find("Map").transform);

        Vector3 dir = transform.GetChild(1).forward;
        bullet.transform.LookAt(transform.position + dir * 50);
        bullet.transform.Translate(Vector3.forward * 1.2f + Vector3.up * 0.5f);
        bullet.GetComponent<Rigidbody>().velocity = dir * 30;
    }


    void LoadBullet2()  // 샷건 (산탄 총)
    {
        this.audio.clip = this.shotGunSound;
        this.audio.volume = 0.5f;
        for (int i = 0; i < 5; i++)
        {
            var bullet = GameObject.Instantiate(bullet2) as GameObject;
            bullet.transform.position = transform.position;
            bullet.transform.SetParent(GameObject.Find("Map").transform);

            Vector3 dir = transform.GetChild(1).forward + transform.GetChild(1).right * (-2 + i) * 0.2f;
            bullet.transform.LookAt(transform.position + dir * 50);
            bullet.transform.Translate(Vector3.forward * 1.2f + Vector3.up * 0.5f);
            bullet.GetComponent<Rigidbody>().velocity = dir * 30;
        }
        coolTime = 1f;
    }

    void LoadLaser()    // 레이져
    {
        this.audio.clip = this.laserSound;
        this.audio.volume = 0.1f;
        laserTime = 0.6f;
        laserRenderer.enabled = true;
    }
}
