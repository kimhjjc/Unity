using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour
{

    public GameObject[] obj;
    public Transform parent;
    public float interval = 5;
    private float time;
    int maxPeople;
    void Start()
    {
        maxPeople = 120;
        parent = GameObject.Find("Peoples").transform;
    }

    void Update()
    {
        if (UIController.gameOver)
            return;

        // UIController의 gameOver가 false면 시간을 재고 interval마다 랜덤하게 enemy생성
        time += Time.deltaTime;

        
            time = 0;

            int currentPeople = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("Police").Length;
            if (currentPeople < maxPeople)
            {


            
                    Vector3 randomPosition = new Vector3(Random.Range(70f, 270f), 0, Random.Range(50f, 290f));

                    int randObj = Random.Range(0, obj.Length);
                    GameObject enemy = Instantiate(obj[randObj]) as GameObject;
                    enemy.transform.SetParent(parent);
                    enemy.transform.position = randomPosition;
                
            }
        
    }
}
