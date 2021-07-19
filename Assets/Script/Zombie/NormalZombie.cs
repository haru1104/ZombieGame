using System.Collections;
using System.Collections.Generic;

using UnityEditor.SearchService;

using UnityEngine;
using UnityEngine.AI;

public class NormalZombie : Zombie {
    float stopDistance = 1.5f;

    public float speed = 5.5f;

    private void findPlayers() {
        if (playerTr.Count != 0) {
            playerTr.Clear();
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) {
            playerTr.Add(players[i].transform);
        }
    }

    private void findBarricade() {
        if (obstacleTr.Count != 0) {
            obstacleTr.Clear();
        }

        GameObject[] barricades = GameObject.FindGameObjectsWithTag("Barricade");
        GameObject[] barrels = GameObject.FindGameObjectsWithTag("Barrel");

        for (int i = 0; i < barricades.Length; i++) {
            obstacleTr.Add(barricades[i].transform);
        }

        for (int i = 0; i < barrels.Length; i++) {
            obstacleTr.Add(barrels[i].transform);
        }
    }

    private void playZombieAnimation() {
        if ((agent.velocity.x != 0) && (agent.velocity.z != 0)) {
            ani.SetBool("Run", true);
        }
        else {
            ani.SetBool("Run", false);
        }
    }

    public override void onSpawn() {
        type = ZombieType.Normal;

        health = 100f;

        attackDamage = 2f;
        attackSpeed = 1.7f;

        moveSpeed = speed;

        ani = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;

        particle = GetComponentInChildren<ParticleSystem>();
    }

    public override void onMove() {
        if (agent.enabled && !agent.isStopped) {
            findPlayers();
            findBarricade();

            int index = 0;
            bool isObstacle = false;
            float dist = float.MaxValue;

            for (int i = 0; i < playerTr.Count; i++) {
                float tempDist = Vector3.Distance(transform.position, playerTr[i].position);

                if (tempDist <= dist) {
                    isObstacle = false;
                    index = i;
                    dist = tempDist;
                }
            }

            for (int i = 0; i < obstacleTr.Count; i++) {
                if (obstacleTr[i] != null) {
                    float tempDist = Vector3.Distance(transform.position, obstacleTr[i].position);

                    if (tempDist <= dist) {
                        isObstacle = true;
                        index = i;
                        dist = tempDist;
                    }
                }
                else {
                    obstacleTr.RemoveAt(i);
                    onMove();
                    return;
                }
            }

            if (isObstacle) {
                agent.SetDestination(obstacleTr[index].position);
            }
            else {
                agent.SetDestination(playerTr[index].position);
            }

            playZombieAnimation();
        }
    }

    public override void onAttack() {
        ani.SetTrigger("Attack");
    }
}
