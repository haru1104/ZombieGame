using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class HeavyZombie : Zombie {
    float stopDistance = 1.5f;

    public float speed = 4f;

    private void findPlayers() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) {
            playerTr.Add(players[i].transform);
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

        health = 200f;

        attackDamage = 2.3f;
        attackDelay = 2.5f;
        attackCooldown = 0f;

        moveSpeed = speed;

        ani = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;

        particle = GetComponentInChildren<ParticleSystem>();
    }

    public override void onMove() {
        findPlayers();

        int index = 0;
        float dist = float.MaxValue;

        for (int i = 0; i < playerTr.Count; i++) {
            float tempDist = Vector3.Distance(transform.position, playerTr[i].position);

            if (tempDist <= dist) {
                index = i;
                dist = tempDist;
            }
        }

        agent.SetDestination(playerTr[index].position);
        playZombieAnimation();
    }

    public override void onAttack() {
        if (attackCooldown < Time.time) {
            attackCooldown = Time.time + attackDelay;

            ani.SetTrigger("Attack");
        }
    }

    public override void onDamaged() {
        particle.Play();
    }

    public override void onDeath() {

    }
}
