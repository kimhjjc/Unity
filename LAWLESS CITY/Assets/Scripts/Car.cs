using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    static public bool isAvailable;
    static public Transform carTr;

    // Start is called before the first frame update

    void Start()
    {
        isAvailable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Player.characterState)
            return;
        if (transform == GameObject.Find("Player").transform.GetChild(2) &&
         (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Police"))
        {
            other.gameObject.GetComponent<Enemy>().hp = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isAvailable && !Player.characterState && !NPC.isAvailable)
        {
            isAvailable = true;
            carTr = transform;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && isAvailable && !Player.characterState)
        {
            isAvailable = false;
        }
    }
    
}
