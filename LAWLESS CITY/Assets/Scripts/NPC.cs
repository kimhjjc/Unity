using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    static public bool isAvailable;
    static public Transform NPCTr;

    // Start is called before the first frame update

    void Start()
    {
        isAvailable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isAvailable && !Player.characterState)
        {
            isAvailable = true;
            NPCTr = transform;
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
