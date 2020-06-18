using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalInhanceEnemy : MonoBehaviour
{
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        enemy.maxHp *= 4f;
        enemy.hp *= 4f;
        //enemy.targetOn = true;

        enemy.tag = "Police";
    }

    // Update is called once per frame
    void Update()
    {
        //enemy.targetOn = true;
        //enemy.chaseTime = 30;
    }
}
