using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
namespace haruroad.szd.singleplayer
{
    public class NormalZombie : Zombie
    {
        float stopDistance = 1.5f;

        public float speed = 5.5f;

        public override void onSpawn()
        {
            type = ZombieType.Normal;

            health = 100f;

            attackDamage = 10f;
            attackSpeed = 1.7f;

            moveSpeed = speed;

            ani = GetComponent<Animator>();

            agent = GetComponent<NavMeshAgent>();
            agent.speed = moveSpeed;
            agent.stoppingDistance = stopDistance;

            particle = GetComponentInChildren<ParticleSystem>();
        }
    }
}
