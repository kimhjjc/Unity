using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour
{

    public GameObject obj;
    public Transform objParent;
    public float interval = 5;
    private float time;

    void Start()
    {

    }

    void Update()
    {
        if (UIController.gameOver)
            return;

        // UIController의 gameOver가 false면 시간을 재고 interval마다 랜덤하게 enemy생성
        time += Time.deltaTime;

        if (time >= interval)
        {
            time = 0;
            GameObject enemy = Instantiate(obj) as GameObject;
            enemy.transform.SetParent(objParent);
        }
    }
}
