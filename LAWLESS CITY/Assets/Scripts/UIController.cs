using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip GameClearSound;


    // 미니맵
    GameObject player;
    GameObject miniMap;

    //HPBar
    public Slider playerHP;

    // 상호작용
    public Text carInteraction;
    public Text NPCInteraction;

    // NPC 대화창
    public GameObject TalkWindow;
    public Text NPCName;
    public Image NPCImage;
    public Text TalkText;
    public static int questInfo;
    public static int questTalkCount;
    public bool[] questReward;

    // 메인 목표 및 클리어 확인
    public static bool mainQuest1Clear;
    public static bool mainQuest2Clear;
    public static bool mainQuest3Clear;
    public static bool gameOver = false;
    float gameOverTime;
    public static bool gameClear;


    public static bool stage1Clear = false;
    public static bool stage2Clear = false;

    public GameObject stageClearText;

    float gameQuitTimer;


    // GameOver텍스트와 버튼 지우기
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.loop = false;

        // 미니맵
        player = GameObject.FindGameObjectWithTag("Player");
        miniMap = GameObject.FindGameObjectWithTag("MiniMap");
        miniMap.SetActive(false);

        carInteraction.enabled = false;
        NPCInteraction.enabled = false;

        TalkWindow.SetActive(false);
        questReward = new bool[10];
        for (int i = 0; i < questReward.Length; i++)
            questReward[i] = false;

        mainQuest1Clear = false;
        mainQuest2Clear = false;
        mainQuest3Clear = false;
        gameOver = false;
        gameOverTime = 4f;
        gameClear = false;

        //stage1Clear = false;
        //stage2Clear = false;

        stageClearText.SetActive(false);

        //gameQuitTimer = 5;
    }

    // GameOver텍스트와 버튼 표시
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Player.miniMapAvailable)
            miniMap.transform.GetChild(0).localPosition = new Vector3((player.transform.position.x / 3.5f) - 50, (player.transform.position.z / 3.5f) - 50, 0);

        if (Car.isAvailable)
            carInteraction.enabled = true;
        else
            carInteraction.enabled = false;

        if (NPC.isAvailable)
            NPCInteraction.enabled = true;
        else
            NPCInteraction.enabled = false;

        if (Player.NPCInteraction)
            TalkWindow.SetActive(true);
        else
            TalkWindow.SetActive(false);

        if (Player.miniMapAvailable)
            miniMap.SetActive(true);



        playerHP.value = (float)player.GetComponent<Player>().hp / (float)player.GetComponent<Player>().maxHp;


        if (gameOver)
        {
            gameOverTime -= Time.deltaTime;

            if (gameOverTime < 0)
                SceneManager.LoadScene("Fail");
            return;
        }

        //isQuestClear();
        QuestList();
        if (!stage1Clear)   // 1 스테이지에서
        {
            if (Input.GetKeyDown(KeyCode.T) && stageClearText.activeSelf)
            {
                SceneManager.LoadScene("2Stage");
                stage1Clear = true;
            }
        }

    }


    void QuestList()        // 실제 퀘스트 내용
    {
        if (!Player.NPCInteraction)
            return;

        if (!stage1Clear)   // 1 스테이지
        {
            NPCName.text = NPC.NPCTr.name;

            if (questInfo == 0)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("soldier");
                NPCImage.sprite = NPCSprite;
                switch (questTalkCount)
                {
                    case 3:
                        TalkText.text = "이 도시는 마피아에 의해서 돌아가고 법이라는 것이 존재하지 않지";
                        break;
                    case 2:
                        TalkText.text = "밤이되면 하루하루 사건 사고가 터지고...";
                        break;
                    case 1:
                        TalkText.text = "어딘가에는 마피아 보스가 숨어 있다더라고.";
                        break;
                    case 0:
                        TalkText.text = "여기는 힘이 권력이니까 어디 니가 하고싶은대로 해봐";
                        break;
                    default:
                        break;
                }

            }
            else if (questInfo == 1)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("soldier");
                NPCImage.overrideSprite = NPCSprite;
                if (Score.score < 3 && !questReward[questInfo])    // 퀘스트의 클리어 조건
                {
                    switch (questTalkCount)
                    {
                        case 1:
                            TalkText.text = "야 거기 너 운전할줄 아나?";
                            break;
                        case 0:
                            TalkText.text = "내가 가르쳐 줄께 그 대신 3코인을 가져와";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Score.score -= 3;
                        Player.carAvailable = true;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    switch (questTalkCount)
                    {
                        case 1:
                            TalkText.text = "3코인을 가져왔네";
                            break;
                        case 0:
                            TalkText.text = "여기 면허증이다 축하해 이제 운전을 해봐";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (questInfo == 2)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("girl");
                NPCImage.overrideSprite = NPCSprite;
                if (Score.score < 15 && !questReward[questInfo])    // 퀘스트의 클리어 조건
                {
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "어이, 내가 좋은거 가지고 있는데 생각있어?";
                            break;
                        case 1:
                            TalkText.text = "그러면 15코인을 가져와";
                            break;
                        case 0:
                            TalkText.text = "실망 안할꺼야";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Score.score -= 15;
                        Cannon.myWeapon[0] = true;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "15코인을 가져왔네";
                            break;
                        case 1:
                            TalkText.text = "이건 권총이다 이런거 처음보지?";
                            break;
                        case 0:
                            TalkText.text = "좋은데 써라";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (questInfo == 3)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("stranger");
                NPCImage.overrideSprite = NPCSprite;
                if (Score.policeKillscore < 5 && !questReward[questInfo])    // 퀘스트의 클리어 조건
                {
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "내가 새로운 기술을 알려주지";
                            break;
                        case 1:
                            TalkText.text = "기술 배우기전에 미션을 준다";
                            break;
                        case 0:
                            TalkText.text = "경찰 5명을 죽여서 너의 의지를 보여줘!" +
                              Score.policeKillscore.ToString() + " / 5";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Player.dashAvailable = true;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "진짜 경찰을 5명이나 죽였네";
                            break;
                        case 1:
                            TalkText.text = "이제 SHIFT를 눌르고 움직이면 훨씬더 빨리 움직일수 있을거야";
                            break;
                        case 0:
                            TalkText.text = "그럼 이만";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (questInfo == 4)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("zombie");
                NPCImage.overrideSprite = NPCSprite;
                if (Score.score < 20 && !questReward[questInfo])    // 퀘스트의 클리어 조건
                {
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "그렇게 정처 없이 어딜 가는거야";
                            break;
                        case 1:
                            TalkText.text = "내가 지도를 가지고 있기는한데...";
                            break;
                        case 0:
                            TalkText.text = "좀 비싼대... 20 코인 필요하면 말걸어";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Score.score -= 20;
                        Player.miniMapAvailable = true;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "진짜 20 코인이나 구해왔네";
                            break;
                        case 1:
                            TalkText.text = "그럼 여기 있어";
                            break;
                        case 0:
                            TalkText.text = "잘 써봐";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (questInfo == 5)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("police");
                NPCImage.overrideSprite = NPCSprite;
                switch (questTalkCount)
                {
                    case 0:
                        TalkText.text = "안녕하세요! 좋은하루 대세요!";
                        break;
                    default:
                        break;
                }

            }
            else if (questInfo == 6)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("zombie");
                NPCImage.overrideSprite = NPCSprite;
                switch (questTalkCount)
                {
                    case 0:
                        TalkText.text = "뭘봐 꺼져";
                        break;
                    default:
                        break;
                }

            }
        }
        else if (!stage2Clear)  // 2스테이지
        {
            if (questInfo == 0)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("soldier");
                NPCImage.sprite = NPCSprite;
                switch (questTalkCount)
                {
                    case 2:
                        TalkText.text = "밤이 되었군 이제 난동이 시작 될꺼야";
                        break;
                    case 1:
                        TalkText.text = "죽지말고 몸조심해";
                        break;
                    case 0:
                        TalkText.text = "그럼 여기저기 미션을 수행해서 이 상황을 해쳐나가";
                        break;
                    default:
                        break;
                }

            }
            else if (questInfo == 1)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("stranger");
                NPCImage.overrideSprite = NPCSprite;
                if (Score.score < 30 && !questReward[questInfo])    // 퀘스트의 클리어 조건
                {
                    switch (questTalkCount)
                    {
                        case 1:
                            TalkText.text = "거기 너 그런 총가지고 되겠어?";
                            break;
                        case 0:
                            TalkText.text = "30코인만 가져오면 내가 더 좋은거 줄게~";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Score.score -= 30;
                        Cannon.myWeapon[1] = true;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    switch (questTalkCount)
                    {
                        case 1:
                            TalkText.text = "30코인을 가져왔네";
                            break;
                        case 0:
                            TalkText.text = "여기 이건 샷건이다 권총하고는 비교할수도 없지";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (questInfo == 2)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("stranger");
                NPCImage.overrideSprite = NPCSprite;
                if (Score.score < 40 && !questReward[questInfo])    // 퀘스트의 클리어 조건
                {
                    switch (questTalkCount)
                    {
                        case 1:
                            TalkText.text = "그런 구닥다리 무기를 쓰다니";
                            break;
                        case 0:
                            TalkText.text = "내가 더 좋은걸 줄테니 40코인가져와";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Score.score -= 40;
                        Cannon.myWeapon[2] = true;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    switch (questTalkCount)
                    {
                        case 1:
                            TalkText.text = "어서와 기다리고 있었어";
                            break;
                        case 0:
                            TalkText.text = "여기 레이져총이야 위험하니까 조심히 써";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (questInfo == 3)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("zombie");
                NPCImage.overrideSprite = NPCSprite;
                if (Score.score < 70 && !questReward[questInfo])    // 퀘스트의 클리어 조건
                {
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "이봐 이런 지옥에서 벗어나고 싶지않아?";
                            break;
                        case 1:
                            TalkText.text = "내가 왕이 되는 방법을 알려주지";
                            break;
                        case 0:
                            TalkText.text = "그 대신 비싼 정보니까 70코인을 가져와";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Score.score -= 70;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "70코인.... 확인했고";
                            break;
                        case 1:
                            TalkText.text = "남쪽 끝으로 가면 절벽이 있어 거기 사람들한태 말을 걸어봐";
                            break;
                        case 0:
                            TalkText.text = "행운을 빌지";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (questInfo == 4)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("zombie");
                NPCImage.overrideSprite = NPCSprite;
                if (!questReward[3] && !questReward[questInfo])    // 퀘스트의 클리어 조건
                {
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "어이 거기 여기가 어디라고 오는거야?";
                            break;
                        case 1:
                            TalkText.text = "너는 아직 자격이 없는 것 같군";
                            break;
                        case 0:
                            TalkText.text = "여기를 지나가고싶으면 중요한 미션을 완료하고 와라";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Score.score -= 70;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    switch (questTalkCount)
                    {
                        case 2:
                            TalkText.text = "이제 너는 자격이 생긴거같군";
                            break;
                        case 1:
                            TalkText.text = "이시대의 새로운 왕이 되어주었으면 좋겠다";
                            break;
                        case 0:
                            TalkText.text = "행운을 빌지";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (questInfo == 5)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("stranger");
                NPCImage.sprite = NPCSprite;

                if (Score.score < 10 && !questReward[questInfo])
                {
                    TalkText.text = "어이 돈있나?";
                }
                else
                {
                    if (!questReward[questInfo])         // 퀘스트 보상
                    {
                        Score.score -= 10;
                        questReward[questInfo] = true;      // true면 지급 완료
                    }
                    TalkText.text = "어이 돈있나?\n" +
                        "(10원을 빼앗겼다.)";

                }
            }
            else if (questInfo == 6)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("zombie");
                NPCImage.sprite = NPCSprite;
                
                TalkText.text = "뭘봐 뒤질라고";
            }
            else if (questInfo == 7)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("zombie");
                NPCImage.sprite = NPCSprite;
                
                TalkText.text = "이도시는 새로운 왕이 필요해...";
            }
            else if (questInfo == 8)
            {
                Sprite NPCSprite = Resources.Load<Sprite>("zombie");
                NPCImage.sprite = NPCSprite;
                
                TalkText.text = "...";
            }
        }
    }

    public static void NPCList()    //Player가 Space바 누를 때 작용할 함수
    {
        if (!stage1Clear)   // 1 스테이지
        {
            if (NPC.NPCTr.gameObject == GameObject.Find("Explain"))
            {
                questInfo = 0; questTalkCount = 3;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Car"))
            {
                questInfo = 1; questTalkCount = 1;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Gun"))
            {
                questInfo = 2; questTalkCount = 2;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Dash"))
            {
                questInfo = 3; questTalkCount = 2;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("MiniMap"))
            {
                questInfo = 4; questTalkCount = 2;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("NPCPolice1") ||
                NPC.NPCTr.gameObject == GameObject.Find("NPCPolice2"))
            {
                questInfo = 5; questTalkCount = 0;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Shut") ||
                NPC.NPCTr.gameObject == GameObject.Find("Raser"))
            {
                questInfo = 6; questTalkCount = 0;
            }
        }
        else if (!stage2Clear)  // 2스테이지
        {
            if (NPC.NPCTr.gameObject == GameObject.Find("Explain"))
            {
                questInfo = 0; questTalkCount = 2;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Shotgun"))
            {
                questInfo = 1; questTalkCount = 1;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Raser"))
            {
                questInfo = 2; questTalkCount = 1;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Guide"))
            {
                questInfo = 3; questTalkCount = 2;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Final"))
            {
                questInfo = 4; questTalkCount = 2;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Bat"))
            {
                questInfo = 5; questTalkCount = 0;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Gang"))
            {
                questInfo = 6; questTalkCount = 0;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("Say"))
            {
                questInfo = 7; questTalkCount = 0;
            }
            else if (NPC.NPCTr.gameObject == GameObject.Find("etc"))
            {
                questInfo = 8; questTalkCount = 0;
            }
        }
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
