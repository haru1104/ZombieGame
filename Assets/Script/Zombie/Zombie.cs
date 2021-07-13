using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Zombie : MonoBehaviour
{
    private Transform playerTr;
    private NavMeshAgent nav;
    private Animator myAni;

    // Start is called before the first frame update
    void Start()
    {
        NavStartSet();
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        myAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(playerTr.position);
        ZombieAniSet();
    }

    private void NavStartSet()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance = 1.5f;
    }
    private void ZombieAniSet()
    {
        if (nav.velocity.x != 0 && nav.velocity.z !=0 )
        {
            myAni.SetBool("Run", true);
        }
        else
        {
            myAni.SetBool("Run", false);
        }
    }
}
