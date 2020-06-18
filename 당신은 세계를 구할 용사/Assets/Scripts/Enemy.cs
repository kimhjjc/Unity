using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        agent.speed = 2f;
        agent.angularSpeed = 1080f;
        agent.stoppingDistance = 1.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.gameOver || UIController.gameClear)
        {
            agent.enabled = false;
            return;
        }

        while (!agent.isOnNavMesh || (0.0f < transform.position.x && transform.position.x < 10.0f &&
               0.0f < transform.position.z && transform.position.z < 8.5f))
        {
            agent.Warp(new Vector3(Random.Range(2.0f, 48.0f), 0.0f, Random.Range(2.0f, 48.0f)));
        }
        agent.destination = target.transform.position;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
            Destroy(gameObject);
    }
}
