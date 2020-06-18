using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer laser;

    // Use this for initialization
    void Start()
    {
        //라인렌더러 설정
        laser = GetComponent<LineRenderer>();
        laser.SetColors(Color.red, Color.yellow);
    }

    // Update is called once per frame
    void Update()
    {
        laser.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if(hit.collider)
            {
                laser.SetPosition(1, hit.point);

                if (!laser.enabled)
                    return;

                if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Police")
                    hit.collider.gameObject.GetComponent<Enemy>().hp -= 4;
            }
        }
        else
        {
            laser.SetPosition(1, transform.forward * 5000);
        }

    }
}

