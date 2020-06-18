using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

    // 변수 선언
    private AudioSource audio;
    public AudioClip attackSound;

    public GameObject prefab;   // 공을 담아두기 위한 변수
    float power;         // 공을 날리는 힘

	void Start () {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;
        power = 10;
	}
	
	void Update () {
        if (UIController.gameOver || UIController.gameClear)
            return;

        if (Input.GetMouseButtonDown(0)) {
            this.audio.clip = this.attackSound;
            this.audio.Play();

            GameObject bullet = LoadBullet();
            Vector3 dir = transform.GetChild(0).forward;
            bullet.transform.LookAt(transform.position + dir * 50);
            bullet.transform.Translate(Vector3.forward);
            bullet.GetComponent<Rigidbody>().velocity = dir * power;
        }
    }

    // Bullet을 이용하여 공을 생성
    GameObject LoadBullet() {
		var bullet = GameObject.Instantiate(prefab) as GameObject;
		bullet.transform.position = transform.position;
        bullet.transform.SetParent(gameObject.transform);
		return bullet;
    }
}
