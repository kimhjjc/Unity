using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static int score;
    private Text text;
    public static int policeKillscore;

    // Text컴포넌트 담아두기;
    void Start () {
        text = GetComponent<Text>();
    }

    // score 갱신;
    void Update () {
        text.text = " X " + score;
    }
}
