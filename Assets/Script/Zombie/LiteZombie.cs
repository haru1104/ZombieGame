using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class LiteZombie : Zombie {
    float stopDistance = 1.5f;

    public float speed = 6.5f;

    public override void onSpawn() {
        type = ZombieType.Normal;

        health = 70f;

        attackDamage = 20f;
        attackSpeed = 2f;

        moveSpeed = speed;

        ani = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;

        particle = GetComponentInChildren<ParticleSystem>();
    }
}
