using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip GameClearSound;

    public Text goal1;
    public Text goal2;
    public Text goal3;
    public Text gameOverText;
    public static bool gameOver;
    public static bool gameClear;
    public GameObject button;

    float gameQuitTimer;

    // GameOver텍스트와 버튼 지우기
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;

        goal1.enabled = false;
        goal2.enabled = false;
        goal3.enabled = false;
        gameOverText.enabled = false;
        gameOver = false;
        gameClear = false;
        button.SetActive(false);

        gameQuitTimer = 5;
    }

    // GameOver텍스트와 버튼 표시
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (gameOver)
        {
            goal1.enabled = false;
            goal2.enabled = false;
            goal3.enabled = false;
            gameOverText.enabled = true;
            button.SetActive(true);
            return;
        }

        if (Score.score < 5)
        {
            goal1.enabled = true;
        }
        else if (Score.score >= 5 && isEnemy())
        {
            goal1.enabled = false;
            goal2.enabled = true;
            goal2.text = "목표2 : 남아있는 적을 모두 처치해라!\n" +
                "남은 적 : " + EnemyCount();
        }
        else
        {
            goal1.enabled = false;
            goal2.enabled = false;
            goal3.enabled = true;

            if(!gameClear)
            {
                this.audio.clip = this.GameClearSound;
                this.audio.Play();
            }

            gameClear = true;
        }

        if (gameClear)
            gameQuitTimer -= Time.deltaTime;

        if(gameQuitTimer < 0)
            Application.Quit();
    }

    bool isEnemy()
    {
        if (GameObject.FindWithTag("Enemy"))
            return true;
        else
            return false;
    }

    int EnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
