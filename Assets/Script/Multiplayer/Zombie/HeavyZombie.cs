using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class HeavyZombie : Zombie {
    float stopDistance = 1.5f;

    public float speed = 4f;

    public override void onSpawn() {
        type = ZombieType.Normal;

        health = 200f;

        attackDamage = 15f;
        attackSpeed = 3.2f;

        moveSpeed = speed;

        ani = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;

        particle = GetComponentInChildren<ParticleSystem>();
    }
}
